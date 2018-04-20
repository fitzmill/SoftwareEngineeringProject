ko.components.register('payment-information-page-component', {
    viewModel: function (params) {
        let vm = this;

        vm.cardNumber = params.cardNumber;
        vm.cardFirstName = params.cardFirstName;
        vm.cardLastName = params.cardLastName;
        vm.month = params.month;
        vm.year = params.year;
        vm.address1 = params.address1;
        vm.address2 = params.address2;
        vm.city = params.city;
        vm.state = params.state;
        vm.zip = params.zip;

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