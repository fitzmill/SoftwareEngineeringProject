﻿require('./admin-component.scss');

const adminAPIURL = "/api/admin";

ko.components.register('admin-component', {
    viewModel: function (params) {
        var vm = this;
        vm.generateStartDate = ko.observable();
        vm.generateEndDate = ko.observable();

        vm.unsettledTransactions = ko.observableArray([]);
        vm.allTransactions = ko.observableArray([]);
        vm.amountCharged = ko.observable();
        vm.amountPaid = ko.observable();
        vm.amountOutstanding = ko.observable();
        vm.reportRange = ko.observable();

        vm.generateReport = function () {
            generateReport(vm.generateStartDate(), vm.generateEndDate()).done(function (data) {
                vm.reports.unshift(parseReportModel(data));
            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not generate report: ".concat(errorMessage));
            });

        };

        vm.viewReport = function (report) {
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
                //TODO: Implement support for Joe's eventual 'DEFERRED' process state
                let unsettledTransactions = JSON.parse(JSON.stringify(charged.filter(t => t.ProcessState !== "SUCCESSFUL")));
                unsettledTransactions.forEach((t, index, array) => {
                    array[index].DateDue = parseDateTimeString(t.DateDue);
                    array[index].AmountCharged = Number(t.AmountCharged).toLocaleString("en");
                });

                //assign data to components
                vm.amountCharged(amountCharged);
                vm.amountPaid(amountPaid);
                vm.amountOutstanding(amountOutstanding);
                vm.unsettledTransactions(unsettledTransactions);
                vm.allTransactions(data);
                vm.reportRange(report.StartDate + " - " + report.EndDate);

                $("#headerLoadingModal").hide();

            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not get report details: ".concat(errorMessage));

                $("#modalViewReport").modal("hide");
            });

        };

        vm.downloadReportDetails = function () {
            let csv = vm.allTransactions().createCSVString();

            csv.downloadCSV("Transactions.csv");
        };

        vm.reports = ko.observableArray([]);

        getReports().done(function (data) {
            data.forEach(function (report) {
                vm.reports.push(parseReportModel(report));
            });
        }).fail(function (jqXHR) {
            window.alert("Could not get report history, please try refreshing the page.");
        });

        return vm;
    },
    template: require('./admin-component.html')
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