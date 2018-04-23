const paymentControllerRoot = "/api/payment";

//user type
const GENERAL_USER = 1;

ko.components.register('payment-plan-selection-page-component', {
    viewModel: function (params) {
        let vm = this;

        //observables passed in from higher level ko component
        vm.personalInformation = params.personalInformation;
        vm.paymentInformation = params.paymentInformation;
        vm.students = params.students;
        vm.PLAN_TYPE_VALUES = params.PLAN_TYPE_VALUES;
        vm.paymentTypeSelection = params.paymentTypeSelection;

        //rates calculated to be displayed on radio buttons
        vm.yearlyRate = ko.observable(0);
        vm.monthlyRate = ko.observable(0);
        vm.semesterlyRate = ko.observable(0);

        //payment plan types
        vm.PLAN_TYPE_VALUES = {};
        vm.PLAN_TYPE_VALUES["MONTHLY"] = 1;
        vm.PLAN_TYPE_VALUES["SEMESTERLY"] = 2;
        vm.PLAN_TYPE_VALUES["YEARLY"] = 3;

        //computed observables
        vm.getPaymentPlanType = function () {
            return vm.PLAN_TYPE_VALUES[vm.paymentTypeSelection()];
        };

        //the information needed to calculate rates and create an account
        vm.user = ko.computed(function () {
            return {
                UserID: 0,
                FirstName: vm.personalInformation().firstName(),
                LastName: vm.personalInformation().lastName(),
                Email: vm.personalInformation().email(),
                Plan: 0,
                UserType: GENERAL_USER,
                Students: vm.students()
            };
        });

        //calculate the rates for each plan on this page showing
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

        //the information needed to create an account
        vm.accountCreationInformation = ko.computed(function () {
            info = {
                FirstName: vm.personalInformation().firstName(),
                LastName: vm.personalInformation().lastName(),
                Email: vm.personalInformation().email(),
                Password: vm.personalInformation().password(),
                Plan: vm.getPaymentPlanType(),
                UserType: GENERAL_USER,
                Students: vm.students(),
                CardholderFirstName: vm.paymentInformation().cardFirstName(),
                CardholderLastName: vm.paymentInformation().cardLastName(),
                StreetAddress1: vm.paymentInformation().address1(),
                StreetAddress2: vm.paymentInformation().address2(),
                City: vm.paymentInformation().city(),
                State: vm.paymentInformation().state(),
                Zip: vm.paymentInformation().zip(),
                CardNumber: vm.paymentInformation().cardNumber(),
                ExpirationYear: vm.paymentInformation().year(),
                ExpirationMonth: vm.paymentInformation().month()
            };
            params.accountCreationInformation(info);
            return info;
        });

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
