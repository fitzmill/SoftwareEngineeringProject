const userInfoControllerRoot = "/api/userinfo";

module.exports.advancePageIfValid = function (advancePage, email) {
    let validator = $("#form-personal-information-page").validate();
    validator.resetForm();
    if ($("#form-personal-information-page").valid()) {
        emailExists(email).done(function (data) {
            if (data) {
                validator.showErrors({
                    accountCreationEmail: "Email already exists"
                });
            } else {
                advancePage();
            }
        }).fail(function (jqXHR) {
            if (jqXHR.status !== 401) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not verify information: ".concat(errorMessage));
            }
        });
    }
}

ko.components.register('personal-information-page-component', {
    viewModel: function (params) {
        let vm = this;

        vm.firstName = ko.observable();
        vm.lastName = ko.observable();
        vm.email = ko.observable();
        vm.reenterEmail = ko.observable();
        vm.password = ko.observable();
        vm.reenterPassword = ko.observable();

        params.personalInformation({
            firstName: vm.firstName,
            lastName: vm.lastName,
            email: vm.email,
            password: vm.password
        });

        return vm;
    },

    template: require('./personal-information-page-component.html')
});

//POSTs to see if an email is used in the database
function emailExists(email) {
    return $.ajax(`${userInfoControllerRoot}/EmailExists`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(email)
    });
}