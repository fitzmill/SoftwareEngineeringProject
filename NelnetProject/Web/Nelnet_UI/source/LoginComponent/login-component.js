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
            if ($("#form-page-2").valid()) {
                validateLoginInfo(vm.email(), vm.password()).done(function (loginInfo) {
                    if (!loginInfo) {
                        $("label-invalid-info").show();
                        return;
                    }
                    window.sessionStorage.setItem('Jwt', loginInfo.JwtToken);
                    if (loginInfo.UserType === "GENERAL") {
                        window.location = "#account-dashboard";
                    } else if (loginInfo.UserType === "ADMIN") {
                        window.location = "#admin";
                    }
                }).fail(function (jqXHR, responseText, errorThrown) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert(errorMessage);
                });
                }).fail(function (jqXHR) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert(errorMessage);
                });
            }
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
        method: "GET",
        beforeSend: function (jqXHR) {
            jqXHR.setRequestHeader("Authorization", "Basic " + btoa(email + ':' + password));
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