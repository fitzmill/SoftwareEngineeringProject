require('./report-component.scss');

const adminAPIURL = "/api/admin";

ko.components.register('report-component', {
    viewModel: function (params) {
        var reportComponentVM = this;
        reportComponentVM.generateStartDate = ko.observable();
        reportComponentVM.generateEndDate = ko.observable();

        reportComponentVM.unsettledTransactions = ko.observableArray([]);
        reportComponentVM.allTransactions = ko.observableArray([]);
        reportComponentVM.amountCharged = ko.observable();
        reportComponentVM.amountPaid = ko.observable();
        reportComponentVM.amountOutstanding = ko.observable();
        reportComponentVM.reportRange = ko.observable();

        reportComponentVM.generateReport = function () {
            generateReport(reportComponentVM.generateStartDate(), reportComponentVM.generateEndDate()).done(function (data) {
                reportComponentVM.reports.unshift(parseReportModel(data));
            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not generate report: ".concat(errorMessage));
            });

        };

        reportComponentVM.viewReport = function (report) {
            $("#modalViewReport").modal("show");
            $("#modalViewReport").focus();
            $("#headerLoadingModal").show();

            let startDate = splitViewDate(report.StartDate);
            let endDate = splitViewDate(report.EndDate);

            getReportDetails(startDate, endDate).done(function (data) {
                let charged = data.filter(t => t.ProcessState !== "NOT_YET_CHARGED");

                //add up values
                let amountCharged = charged.sumProperty('AmountCharged');
                let amountPaid = data.filter(t => t.ProcessState === "SUCCESSFUL").sumProperty('AmountCharged');
                let amountOutstanding = amountCharged - amountPaid;

                //convert to currency
                amountCharged = Number(amountCharged).toLocaleString('en');
                amountPaid = Number(amountPaid).toLocaleString('en');
                amountOutstanding = Number(amountOutstanding).toLocaleString('en');

                //filter all transactions to just get unsettled ones
                //makes a deep copy of the array
                let unsettledTransactions = JSON.parse(JSON.stringify(charged.filter(t => t.ProcessState !== "SUCCESSFUL")));
                unsettledTransactions.forEach((t, index, array) => {
                    array[index].DateDue = t.DateDue.parseDateTimeString();
                    array[index].AmountCharged = Number(t.AmountCharged).toLocaleString("en");
                });

                //assign data to components
                reportComponentVM.amountCharged(amountCharged);
                reportComponentVM.amountPaid(amountPaid);
                reportComponentVM.amountOutstanding(amountOutstanding);
                reportComponentVM.unsettledTransactions(unsettledTransactions);
                reportComponentVM.allTransactions(data);
                reportComponentVM.reportRange(report.StartDate + " - " + report.EndDate);

                $("#headerLoadingModal").hide();

            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not get report details: ".concat(errorMessage));

                $("#modalViewReport").modal("hide");
            });

        };

        reportComponentVM.downloadReportDetails = function () {
            let csv = reportComponentVM.allTransactions().createCSVString();

            csv.downloadCSV("Transactions.csv");
        };

        reportComponentVM.reports = ko.observableArray([]);

        getReports().done(function (data) {
            data.forEach(function (report) {
                reportComponentVM.reports.push(parseReportModel(report));
            });
        }).fail(function (jqXHR) {
            window.alert("Could not get report history, please try refreshing the page.");
        });

        return reportComponentVM;
    },
    template: require('./report-component.html')
});

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

//turn report into a readable format
function parseReportModel(report) {
    return {
        ReportID: report.ReportID,
        DateCreated: report.DateCreated.parseDateTimeString(),
        StartDate: report.StartDate.parseDateTimeString(),
        EndDate: report.EndDate.parseDateTimeString()
    };
}


//fetches all reports from the report api
function getReports() {
    return $.ajax(adminAPIURL + "/GetAllReports", {
        method: "GET"
    });
}

//POSTs a report object to the report api and pushes it to the reports array
function generateReport(startDate, endDate) {
    if (!startDate || !endDate) {
        window.alert("Please enter valid dates.");
    }

    let parsedStartDate = splitGenerateDate(startDate);
    let parsedEndDate = splitGenerateDate(endDate);

    return $.ajax(adminAPIURL + "/InsertReport", {
        method: "POST",
        data: {
            StartDate: parsedStartDate,
            EndDate: parsedEndDate
        }
    });
}

//POSTs a startDate and endDate object to the admin api and computes report details
function getReportDetails(startDate, endDate) {
    return $.ajax(adminAPIURL + "/GetTransactionsForDateRange", {
        method: "POST",
        data: {
            StartDate: startDate,
            EndDate: endDate
        }
    });
}