require('./payment-status-component.scss');

const utility = require('../../utility.js');

ko.components.register('payment-status-component', {
    viewModel: function (params) {
        var vm = this;

        vm.paymentPlan = params.paymentPlan;
        vm.nextPaymentDate = params.nextPaymentDate;
        vm.nextPaymentCost = params.nextPaymentCost;
        vm.paymentStatus = params.paymentStatus;

        return vm;
    },

    template: require('./payment-status-component.html')
});