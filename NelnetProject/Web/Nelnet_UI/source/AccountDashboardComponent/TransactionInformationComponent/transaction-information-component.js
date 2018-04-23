ko.components.register('transaction-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.transactions = params.transactions;

        $("#processStateInformation").tooltip();

        var transactionTable = $("#transaction-table").DataTable({
            columns: [
                { data: "DateDue" },
                { data: "AmountCharged" },
                { data: "ProcessState" },
                { data: "ReasonFailed" }
            ]
        });

        vm.transactions.subscribe(function () {
            transactionTable.rows.add(vm.transactions()).draw();
        });

        return vm;
    },

    template: require('./transaction-information-component.html')
});