require('./report-component.scss');

var adminAPIURL = "/api/admin";
var reportAPIURL = "/api/report";

ko.components.register('report-component', {
    viewModel: function (params) {
        var vm = this;
        vm.reports = ko.observableArray();

        $.ajax(reportAPIURL, {
            method: "GET",
            success: function (data) {
                vm.reports = ko.observableArray(data);
            },
            error: function () {
                window.alert("Could not get report history, please try refreshing the page.");
            }
        });
        vm.text = "hi";

        return vm;
    },
    template: require('./report-component.html')
});