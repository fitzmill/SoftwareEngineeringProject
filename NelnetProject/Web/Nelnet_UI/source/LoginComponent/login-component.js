require('./login-component.scss');
require('../assets/background-image.scss');

const loginAPIURL = "/api/login";
const accountAPIURL = "/api/account";

ko.components.register('login-component', {
    viewModel: function (params) {
        var vm = this;

        vm.email = ko.observable();
        vm.password = ko.observable();

        vm.login = function () {
            if (!vm.email().emailMeetsRequirements()) {
                $("#label-invalid-info").show();
                return;
            } else if (!vm.password().passwordMeetsRequirements()) {
                $("#label-invalid-info").show();
                return;
            }

            validateLoginInfo(vm.email(), vm.password()).done(function (validLogin) {
                if (validLogin) {
                    getUserInfoByEmail(vm.email()).done(function (user) {
                        vm.email("");
                        vm.password("");
                        if (user.UserType === "GENERAL") {
                            window.localStorage.setItem("user", JSON.stringify(user));
                            window.location = "#account-dashboard";
                        } else if (user.UserType === "ADMIN") {
                            window.localStorage.setItem("user", JSON.stringify(user));
                            window.location = "#admin";
                        }
                    }).fail(function (jqXHR) {
                        let errorMessage = JSON.parse(jqXHR.responseText).Message;
                        window.alert(errorMessage);
                    });
                } else {
                    $("#label-invalid-info").show();
                }
            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert(errorMessage);
            });
        }

        $("#card-login").keypress(function (e) {
            //If the user presses enter, it will click the login button
            if (e.which === 13) {
                $('#btn-login').focus();
                $("#btn-login").click();
            }
        });

        return vm;
    },
    template: require('./login-component.html')
});

function validateLoginInfo(email, password) {
    return $.ajax(loginAPIURL + "/ValidateLoginInfo", {
        method: "POST",
        data: {
            Email: email,
            Password: password
        }
    });
}

function getUserInfoByEmail(email) {
    return $.ajax(accountAPIURL + "/GetUserInfoByEmail", {
        method: "POST",
        contentType: "application/JSON; charset=utf-8",
        data: JSON.stringify(email)
    });
}