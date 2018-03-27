require('./account-creation-component.scss');

ko.components.register('account-creation-component', {
    viewModel: function (params) {
        let vm = this;

        vm.text = ko.observable("hi");

        return vm;
    },

    template: require('./account-creation-component.html')
});