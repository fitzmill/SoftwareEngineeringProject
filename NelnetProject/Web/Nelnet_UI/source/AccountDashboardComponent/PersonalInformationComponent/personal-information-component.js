require('./personal-information-component.scss');

const utility = require('../../utility.js');

//api url constants
const userInfoControllerRoot = "/api/userinfo";

ko.components.register('personal-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.UserFirstName = ko.observable();
        vm.UserLastName = ko.observable();
        vm.Email = ko.observable();

        vm.personalInfo = params.personalInfo;

        vm.setUIPersonalInformation = function () {
            vm.UserFirstName(vm.personalInfo().FirstName);
            vm.UserLastName(vm.personalInfo().LastName);
            vm.Email(vm.personalInfo().Email);
        }

        vm.personalInfo.subscribe(vm.setUIPersonalInformation);

        //Changes the user info in database and ui to what the user entered.
        vm.updatePersonalInfo = function (data, event) {
            let validator = $("#edit-personal-form").validate();
            if (vm.Email() == vm.personalInfo().Email) {
                if ($("#edit-personal-form").valid()) {
                    vm.updateUserValidated(data, event);
                }
            } else {
                if ($("#edit-personal-form").valid()) {
                    emailExists(vm.Email()).done(function (data) {
                        if (data) {
                            validator.showErrors({
                                dashboardEmail: "Email already exists"
                            });
                        } else {
                            vm.updateUserValidated(data, event);
                        }
                    }).fail(function (jqXHR) {
                        if (jqXHR.status !== 401) {
                            let errorMessage = JSON.parse(jqXHR.responseText).Message;
                            window.alert("Could not save information: ".concat(errorMessage));
                        }
                    });
                }
            }
        };

        vm.updateUserValidated = function (data, event) {
            //disable save and cancel buttons
            $("#btn-save-edit-personal").attr("disabled", "disabled");
            $("#btn-cancel-edit-personal").attr("disabled", "disabled");
            let changedUserInfo = vm.personalInfo();
            changedUserInfo.FirstName = vm.UserFirstName();
            changedUserInfo.LastName = vm.UserLastName();
            changedUserInfo.Email = vm.Email();

            updatePersonalInfo(changedUserInfo).done(function (newToken) {
                //update user in local storage in the case of page reload
                window.sessionStorage.setItem("Jwt", newToken);
                vm.personalInfo(changedUserInfo);
                params.stopEditing(data, event);
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Could not save information: ".concat(errorMessage));
                }
            }).always(function () {
                //re-enable buttons
                $("#btn-save-edit-personal").removeAttr("disabled");
                $("#btn-cancel-edit-personal").removeAttr("disabled");
            });
        };
    },

    template: require('./personal-information-component.html')
});

function emailExists(email) {
    return $.ajax(`${userInfoControllerRoot}/EmailExists`, {
        method: "POST",
        contentType: "application/JSON; charset=utf-8",
        data: JSON.stringify(email)
    });
}

//POSTs any changes to the user
function updatePersonalInfo(userInfo) {
    return $.ajax(`${userInfoControllerRoot}/UpdatePersonalInfo`, {
        method: "POST",
        data: userInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}


