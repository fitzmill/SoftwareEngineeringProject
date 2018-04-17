require('./personal-information-component.scss');

const utility = require('../../utility.js');

const accountDashboardAPIURL = "/api/account";

var personalInfo = undefined;

ko.components.register('personal-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.UserFirstName = ko.observable();
        vm.UserLastName = ko.observable();
        vm.Email = ko.observable();

        personalInfo = params.personalInfo;

        vm.setUIPersonalInformation = function () {
            vm.UserFirstName(personalInfo().FirstName);
            vm.UserLastName(personalInfo().LastName);
            vm.Email(personalInfo().Email);
        }

        personalInfo.subscribe(vm.setUIPersonalInformation);

        //Changes the user info in database and ui to what the user entered.
        vm.updatePersonalInfo = function (data, event) {
            if ($("#edit-personal-form").valid()) {

                //disable save and cancel buttons
                $("#btn-save-edit-personal").prop("disabled", true);
                $("#btn-cancel-edit-personal").prop("disabled", true);

                //Check if email already exists in database
                emailExists(vm.Email()).done(function (data) {
                    emailInUse = data;
                    if (emailInUse && vm.Email() !== personalInfo().Email) {
                        vm.personalInputErrorMessage("Email is already used by another user");
                        $("#edit-personal-input-error").show();
                    } else {
                        let changedUserInfo = personalInfo();
                        changedUserInfo.FirstName = vm.UserFirstName();
                        changedUserInfo.LastName = vm.UserLastName();
                        changedUserInfo.Email = vm.Email();

                        updatePersonalInfo(changedUserInfo).done(function (newToken) {
                            //update user in local storage in the case of page reload
                            window.sessionStorage.setItem("Jwt", newToken);
                            personalInfo(changedUserInfo);
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
                    }
                }).fail(function (jqXHR) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Couldn't check if email has been used: ".concat(errorMessage));

                    //re-enable buttons if fail
                    $("#btn-save-edit-personal").removeAttr("disabled");
                    $("#btn-cancel-edit-personal").removeAttr("disabled");
                });
            }
        };
    },

    template: require('./personal-information-component.html')
});

function emailExists(email) {
    return $.ajax(accountDashboardAPIURL + "/EmailExists", {
        method: "POST",
        contentType: "application/JSON; charset=utf-8",
        data: JSON.stringify(email)
    });
}

//POSTs any changes to the user
function updatePersonalInfo(userInfo) {
    return $.ajax(accountDashboardAPIURL + "/UpdatePersonalInfo", {
        method: "POST",
        data: userInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}


