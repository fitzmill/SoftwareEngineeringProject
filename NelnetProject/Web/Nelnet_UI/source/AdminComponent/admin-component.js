require('./admin-component.scss');

const utility = require('../utility.js');

const adminAPIURL = "/api/admin";

exports.adminDashboardBeforeShow = function () {
    //user not logged in
    if (!window.sessionStorage.getItem("Jwt")) {
        window.location = '#';
        return;
    }
}

ko.components.register('admin-component', {
    viewModel: function (params) {
        var vm = this;

        vm.generateStartDate = ko.observable();
        vm.generateEndDate = ko.observable();
        vm.reports = ko.observableArray([]);

        vm.unsettledTransactions = ko.observableArray([]);
        vm.allTransactions = ko.observableArray([]);
        vm.amountCharged = ko.observable();
        vm.amountPaid = ko.observable();
        vm.amountOutstanding = ko.observable();
        vm.reportRange = ko.observable();

        vm.loadAdminInformation = function () {
            getReports().done((data) => {
                vm.reports(data.map((report) => parseReportModel(report)));
            }).fail((jqXHR) => {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get reports, please try refreshing the page");
                } 
            });
        }

        vm.generateReport = function () {
            generateReport(vm.generateStartDate(), vm.generateEndDate()).done(function (data) {
                vm.reports.unshift(parseReportModel(data));
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Could not generate report: ".concat(errorMessage));
                }
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
                let amountPaid = data.filter(t => t.ProcessState === "SUCCESSFUL" || t.ProcessState === "DEFERRED").sumProperty('AmountCharged');
                let amountOutstanding = amountCharged - amountPaid;

                //convert to currency
                amountCharged = Number(amountCharged).toLocaleString('en');
                amountPaid = Number(amountPaid).toLocaleString('en');
                amountOutstanding = Number(amountOutstanding).toLocaleString('en');

                //filter all transactions to just get unsettled ones
                //makes a deep copy of the array
                let unsettledTransactions = JSON.parse(JSON.stringify(charged.filter(t => t.ProcessState !== "SUCCESSFUL" && t.ProcessState !== "DEFERRED")));
                unsettledTransactions.forEach((t, index, array) => {
                    array[index].DateDue = t.DateDue.parseDateTimeString();
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
                if (jqXHR.status !== 401) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Could not get report details: ".concat(errorMessage));
                }

                $("#modalViewReport").modal("hide");
            });

        };

        vm.downloadReportDetails = function () {
            let csv = vm.allTransactions().createCSVString();

            csv.downloadCSV("Transactions.csv");
        };

        if ($("admin-component").is(":visible") && window.sessionStorage.getItem("Jwt")) {
            vm.loadAdminInformation();
        }

        $("#processStateInfo").tooltip();

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
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
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
        },
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//POSTs a startDate and endDate object to the admin api and computes report details
function getReportDetails(startDate, endDate) {
    return $.ajax(adminAPIURL + "/GetTransactionsForDateRange", {
        method: "POST",
        data: {
            StartDate: startDate,
            EndDate: endDate
        },
        beforeSend: utility.attachJwtTokenToRequest
    });
}