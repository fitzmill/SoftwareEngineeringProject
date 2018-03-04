require('./report-component.scss');

const adminAPIURL = "/api/admin";
const reportAPIURL = "/api/report";

//splits a date string thats in YYYY/MM/DD format into parts
function splitGenerateDate(dateString) {
    let dateArray = dateString.split('-');
    return {
        Year: dateArray[0],
        Month: dateArray[1],
        Day: dateArray[2].substring(0, 2)
    };
}

//splits a string that's in MM/DD/YYYY format into parts
function splitViewDate(dateString) {
    let dateArray = dateString.split('/');
    return {
        Month: dateArray[0],
        Day: dateArray[1],
        Year: dateArray[2]
    };
}

//turns c# datetime object into a more readable format
function parseDateTimeString(dateTime) {
    let dateArray = dateTime.split('-');
    let year = dateArray[0];
    let month = dateArray[1];
    let day = dateArray[2].substring(0, 2);
    return month + "/" + day + "/" + year;
}

//turn report into a readable format
function parseReportModel(report) {
    return {
        ReportID: report.ReportID,
        DateCreated: parseDateTimeString(report.DateCreated),
        StartDate: parseDateTimeString(report.StartDate),
        EndDate: parseDateTimeString(report.EndDate)
    };
}


//fetches all reports from the report api
function getReports() {
    let result = [];
    $.ajax(reportAPIURL, {
        method: "GET",
        async: false,
        success: function (data) {
            data.forEach((report) => {
                result.push(parseReportModel(report));
            });
        },
        error: function () {
            window.alert("Could not get report history, please try refreshing the page.");
            result = [];
        }
    });
    return result;
}

//POSTs a report object to the report api and pushes it to the reports array
function generateReport(startDate, endDate) {
    if (!startDate || !endDate) {
        window.alert("Please enter valid dates.");
    }

    let parsedStartDate = splitGenerateDate(startDate);
    let parsedEndDate = splitGenerateDate(endDate);

    let result = undefined;

    $.ajax(reportAPIURL, {
        method: "POST",
        async: false,
        data: {
            StartDate: parsedStartDate,
            EndDate: parsedEndDate
        },
        success: function (data) {
            result = parseReportModel(data);
        },
        error: function (jqXHR) {
            let errorMessage = JSON.parse(jqXHR.responseText).Message;
            window.alert("Could not generate report: ".concat(errorMessage));
        }
    });
    return result;
}

//POSTs a startDate and endDate object to the admin api and computes report details
function getReportDetails(startDate, endDate) {
    let result = null;

    $.ajax(adminAPIURL + "/GetTransactionsForDateRange", {
        method: "POST",
        async: false,
        data: {
            StartDate: startDate,
            EndDate: endDate
        },
        success: function (data) {
            let notYetCharged = data.filter(t => t.ProcessState !== "NOT_YET_CHARGED");

            //add up values
            let amountCharged = notYetCharged.sumProperty('AmountCharged');
            let amountPaid = data.filter(t => t.ProcessState === "SUCCESSFUL").sumProperty('AmountCharged');
            let amountOutstanding = amountCharged - amountPaid;

            //convert to currency
            amountCharged = Number(amountCharged).toLocaleString('en');
            amountPaid = Number(amountPaid).toLocaleString('en');
            amountOutstanding = Number(amountOutstanding).toLocaleString('en');

            //filter all transactions to just get unsettled ones
            let unsettledTransactions = notYetCharged.filter(t => t.ProcessState !== "SUCCESSFUL");
            unsettledTransactions.forEach((t, index, array) => {
                array[index].DateDue = parseDateTimeString(t.DateDue);
                array[index].AmountCharged = Number(t.AmountCharged).toLocaleString("en");
            });

            result = {
                amountCharged: amountCharged,
                amountPaid: amountPaid,
                amountOutstanding: amountOutstanding,
                unsettledTransactions: unsettledTransactions
            };
        },
        error: function (jqXHR) {
            let errorMessage = JSON.parse(jqXHR.responseText).Message;
            window.alert("Could not get report details: ".concat(errorMessage));
        }
    });

    return result;
}

ko.components.register('report-component', {
    viewModel: function (params) {
        var reportComponentVM = this;
        reportComponentVM.generateStartDate = ko.observable();
        reportComponentVM.generateEndDate = ko.observable();
        
        reportComponentVM.unsettledTransactions = ko.observableArray([]);
        reportComponentVM.amountCharged = ko.observable();
        reportComponentVM.amountPaid = ko.observable();
        reportComponentVM.amountOutstanding = ko.observable();
        
        reportComponentVM.generateReport = function () {
            reportComponentVM.reports.unshift(generateReport(reportComponentVM.generateStartDate(), reportComponentVM.generateEndDate()));
        };

        reportComponentVM.viewReport = function (report) {
            $("#modalViewReport").modal("show");
            $("#modalViewReport").focus();
            $("#headerLoadingModal").show();

            let startDate = splitViewDate(report.StartDate);
            let endDate = splitViewDate(report.EndDate);

            let result = getReportDetails(startDate, endDate);

            if (result) {
                //assign to knockout components
                reportComponentVM.amountCharged(result.amountCharged);
                reportComponentVM.amountPaid(result.amountPaid);
                reportComponentVM.amountOutstanding(result.amountOutstanding);
                reportComponentVM.unsettledTransactions(result.unsettledTransactions);

                $("#headerLoadingModal").hide();
            } else {
                $("#modalViewReport").modal("hide");
            }
            
        };

        reportComponentVM.downloadReportDetails = function () {
            let csv = reportComponentVM.unsettledTransactions().createCSVString();

            csv.downloadCSV("UnsettledTransactions.csv");
        };

        reportComponentVM.reports = ko.observableArray(getReports());

        return reportComponentVM;
    },
    template: require('./report-component.html')
});