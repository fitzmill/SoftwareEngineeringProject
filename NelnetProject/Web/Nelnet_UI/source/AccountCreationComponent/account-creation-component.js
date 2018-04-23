require('./account-creation-component.scss');
require('../assets/background-image.scss');

require('./WelcomePageComponent/welcome-page-component.js');
const personalInformationPage = require('./PersonalInformationPageComponent/personal-information-page-component.js');
const paymentInformationPage = require('./PaymentInformationPageComponent/payment-information-page-component.js');
const studentInformationPage = require('./StudentInformationPageComponent/student-information-page-component.js');
require('./PaymentPlanSelectionPage/payment-plan-selection-page-component.js');

//paging constants
const START_PAGE = 0;
const WELCOME_PAGE = 0;
const PERSONAL_INFORMATION_PAGE = 1;
const PAYMENT_INFORMATION_PAGE = 2;
const STUDENT_INFORMATION_PAGE = 3;
const END_PAGE = 4;

//prefixes for updating the correct pages, and the progress bar
const PAGE_ID_PREFIX = "#page-";
const DOT_ID_PREFIX = "#dot-page-";
const RECTANGLE_ID_PREFIX = "#rectangle-";

//api url constants
const userInfoControllerRoot = "/api/userinfo";

ko.components.register('account-creation-component', {
    viewModel: function (params) {
        let vm = this;

        //first page
        vm.personalInformation = ko.observable();

        //second page
        vm.paymentInformation = ko.observable();

        //third page
        vm.students = ko.observable();

        //fourth page
        vm.paymentTypeSelection = ko.observable();
        vm.accountCreationInformation = ko.observable();

        //errors
        vm.personalInputErrorMessage = ko.observable();
        vm.paymentInputErrorMessage = ko.observable();
        vm.studentInputErrorMessage = ko.observable();

        //keep track of page state
        vm.currentPage = START_PAGE;

        //create function to update progress bar
        vm.updateProgressBar = function () {
            for (let i = START_PAGE + 1; i < END_PAGE; i++) {
                if (i >= vm.currentPage) {
                    $(RECTANGLE_ID_PREFIX + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $(RECTANGLE_ID_PREFIX + i)[0].style.backgroundColor = "#007bff";
                }
            }

            for (let i = START_PAGE + 1; i <= END_PAGE; i++) {
                if (i > vm.currentPage) {
                    $(DOT_ID_PREFIX + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $(DOT_ID_PREFIX + i)[0].style.backgroundColor = "#007bff";
                }
            }

            if (vm.currentPage === WELCOME_PAGE) {
                $("#progress-bar").hide();
            } else {
                $("#progress-bar").show();
            }
        };

        //create function to update buttons
        vm.updateButtons = function () {
            //let's get started button
            if (vm.currentPage === WELCOME_PAGE) {
                $("#btn-start").show();
            } else {
                $("#btn-start").hide();
            }
            //back and cancel buttons
            if (vm.currentPage === WELCOME_PAGE) {
                $("#btn-cancel").hide();
                $("#btn-back").hide();
            } else if (vm.currentPage === PERSONAL_INFORMATION_PAGE) {
                $("#btn-cancel").show();
                $("#btn-back").hide();
            } else {
                $("#btn-cancel").hide();
                $("#btn-back").show();
            }
            //next and done buttons
            if (vm.currentPage === WELCOME_PAGE) {
                $("#btn-done").hide();
                $("#btn-next").hide();
            }else if (vm.currentPage === END_PAGE) {
                $("#btn-done").show();
                $("#btn-next").hide();
            } else {
                $("#btn-done").hide();
                $("#btn-next").show();
            }
        };

        //exit account creation
        vm.cancel = function () {
            window.location = '#';
        };

        //move to the previous page
        vm.back = function () {
            $(PAGE_ID_PREFIX + vm.currentPage).hide();
            vm.currentPage--;
            $(PAGE_ID_PREFIX + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        };

        //advance to the next page
        vm.advancePage = function () {
            $(PAGE_ID_PREFIX + vm.currentPage).hide();
            vm.currentPage++;
            $(PAGE_ID_PREFIX + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        }

        //move to the next page
        vm.next = function () {
            if (vm.currentPage === WELCOME_PAGE){
                vm.advancePage();
            } else if (vm.currentPage === PERSONAL_INFORMATION_PAGE) {
                personalInformationPage.advancePageIfValid(vm.advancePage, vm.personalInformation().email());
            } else if (vm.currentPage === PAYMENT_INFORMATION_PAGE) {
                paymentInformationPage.advancePageIfValid(vm.advancePage);
            } else if (vm.currentPage === STUDENT_INFORMATION_PAGE) {
                studentInformationPage.advancePageIfValid(vm.advancePage);
            }
        };

        //finish and create account
        vm.done = function () {
            if (!$("#form-payment-plan-selection-page").valid()) {
                window.alert("Please select a payment plan");
                return;
            }

            $("#btn-done").attr("disabled", "disabled");
            $("#btn-back").attr("disabled", "disabled");

            createUser(vm.accountCreationInformation()).done(function (data) {
                window.sessionStorage.setItem("Jwt", data);
                window.location = "#account-dashboard";
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Could not create account: ".concat(errorMessage));
                }
            }).always(function () {
                $("#btn-done").removeAttr("disabled");
                $("#btn-back").removeAttr("disabled");
            });
        };

        //If the user presses enter, it will click the login button
        $("account-creation-component").keyup(function (e) {
            if (e.which === 13) {
                if ($("#btn-start").is(":visible")) {
                    vm.next();
                } else if ($("#btn-next").is(":visible")) {
                    vm.next();
                } else if ($("#btn-done").is(":visible")) {
                    vm.done();
                }
            }
        });

        //show current page and correct buttons
        for (let i = START_PAGE; i <= END_PAGE; i++) {
            if (i === vm.currentPage) {
                $(PAGE_ID_PREFIX + i).show();
            } else {
                $(PAGE_ID_PREFIX + i).hide();
            }
        }
        vm.updateButtons();
        vm.updateProgressBar();

        return vm;
    },

    template: require('./account-creation-component.html')
});

//create the user in the database
function createUser(accountCreationInformation) {
    let accountCreationInformationData = JSON.stringify(accountCreationInformation);
    return $.ajax(`${userInfoControllerRoot}/InsertUser`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: accountCreationInformationData
    });
}

