require('./report-component.scss');

var adminAPIURL = "/api/admin";
var reportAPIURL = "/api/report";

function parseDate(dateString) {
    let dateArray = dateString.split('-');
    return {
        Year: dateArray[0],
        Month: dateArray[1],
        Day: dateArray[2]
    };
}

function getReports() {
    let result = [];
    $.ajax(reportAPIURL, {
        method: "GET",
        success: function (data) {
            result = data;
        },
        error: function () {
            window.alert("Could not get report history, please try refreshing the page.");
            result = [];
        }
    });
    return result;
}

ko.components.register('report-component', {
    viewModel: function (params) {
        this.generateStartDate = ko.observable();
        this.generateEndDate = ko.observable();

        this.generateReport = function () {
            if (!this.generateStartDate() || !this.generateEndDate()) {
                window.alert("Please enter valid dates.");
            }

            let startDate = parseDate(this.generateStartDate());
            let endDate = parseDate(this.generateEndDate());

            $.ajax(reportAPIURL, {
                method: "POST",
                data: {
                    StartDate: startDate,
                    EndDate: endDate
                },
                success: function () {
                    vm.reports.push({

                    });
                    window.alert("Report created");
                },
                error: function (jqXHR) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Could not generate report: ".concat(errorMessage));
                }
            });
        }

        this.reports = ko.observableArray(getReports());
        this.text = ko.observable('hi');
    },
    template: require('./report-component.html')
});