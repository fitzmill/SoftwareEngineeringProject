ko.components.register('transaction-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.transactions = params.transactions;

        $("#processStateInformation").tooltip();
        //$("#transaction-table").DataTable();

        return vm;
    },

    template: require('./transaction-information-component.html')
});