ko.components.register('payment-information-page-component', {
    viewModel: function (params) {
        let vm = this;

        vm.cardNumber = ko.observable();
        vm.cardFirstName = ko.observable();
        vm.cardLastName = ko.observable();
        vm.month = ko.observable();
        vm.year = ko.observable();
        vm.address1 = ko.observable();
        vm.address2 = ko.observable();
        vm.city = ko.observable();
        vm.state = ko.observable();
        vm.zip = ko.observable();

        params.paymentInformation({
            cardNumber: vm.cardNumber,
            cardFirstName: vm.cardFirstName,
            cardLastName: vm.cardLastName,
            month: vm.month,
            year: vm.year,
            address1: vm.address1,
            address2: vm.address2,
            city: vm.city,
            state: vm.state,
            zip: vm.zip
        });

        return vm;
    },

    template: require('./payment-information-page-component.html')
});

//runs the validation on this page and advances to the next if all information is properly validated
module.exports.advancePageIfValid = function (advancePage) {
    if ($("#form-payment-information-page").valid()) {
        advancePage();
    }
}