const paymentControllerRoot = "/api/payment";

ko.components.register('payment-plan-selection-page-component', {
    viewModel: function (params) {
        let vm = this;

        //observables passed in from higher level ko component
        vm.PLAN_TYPE_VALUES = params.PLAN_TYPE_VALUES;
        vm.paymentTypeSelection = params.paymentTypeSelection;
        vm.user = params.user;

        //rates calculated to be displayed on radio buttons
        vm.yearlyRate = ko.observable(0);
        vm.monthlyRate = ko.observable(0);
        vm.semesterlyRate = ko.observable(0);

        vm.calcRates = function () {
            let userTemplate = vm.user();

            userTemplate.Plan = vm.PLAN_TYPE_VALUES["YEARLY"];
            calculatePeriodicPayment(userTemplate).done(function (data) {
                vm.yearlyRate(data.formatAsMoney());
            }).fail(function (jqXHR) {
                window.alert("Could not calculate yearly rate.");
            });

            userTemplate.Plan = vm.PLAN_TYPE_VALUES["SEMESTERLY"];
            calculatePeriodicPayment(userTemplate).done(function (data) {
                vm.semesterlyRate(data.formatAsMoney());
            }).fail(function (jqXHR) {
                window.alert("Could not calculate semesterly rate.");
            });

            userTemplate.Plan = vm.PLAN_TYPE_VALUES["MONTHLY"];
            calculatePeriodicPayment(userTemplate).done(function (data) {
                vm.monthlyRate(data.formatAsMoney());
            }).fail(function (jqXHR) {
                window.alert("Could not calculate monthly rate.");
            });
        };

        $('#page-4').on('show', vm.calcRates);

        return vm;
    },

    template: require('./payment-plan-selection-page-component.html')
});

//calculate the user's periodic payment
function calculatePeriodicPayment(user) {
    let userData = JSON.stringify(user);
    return $.ajax(`${paymentControllerRoot}/CalculatePeriodicPayment`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: userData
    });
}
