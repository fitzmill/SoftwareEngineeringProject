require('./login-component.scss');

ko.components.register('login-component', {
    viewModel: function (params) {
        var vm = this;

        vm.email = ko.observable();
        vm.password = ko.observable();

        vm.login = function () {
            //TODO actually implement this
            if (vm.email() === 'sfitzmill@gmail.com' && vm.password() === 'software') {
                window.location = '#report';
            } else {
                $('#label-invalid-info').show();
            }
        }

        $(document).keypress(function (e) {
            //If the user presses enter, it will click the login button
            if (e.which == 13) {
                $('#btn-login').focus();
                $("#btn-login").click();
            }
        });

        return vm;
    },
    template: require('./login-component.html')
});