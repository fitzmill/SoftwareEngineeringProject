require('./account-creation-component.scss');
require('../assets/background-image.scss');

ko.components.register('account-creation-component', {
    viewModel: function (params) {
        let vm = this;

        vm.firstName = ko.observable();
        vm.lastName = ko.observable();

        return vm;
    },

    template: require('./account-creation-component.html')
});