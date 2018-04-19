require('./account-creation-component.scss');
require('../assets/background-image.scss');

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
const paymentControllerRoot = "/api/payment";

//user type
const GENERAL_USER = 1;

ko.components.register('account-creation-component', {
    viewModel: function (params) {
        let vm = this;

        //first page
        vm.firstName = ko.observable();
        vm.lastName = ko.observable();
        vm.email = ko.observable();
        vm.reenterEmail = ko.observable();
        vm.password = ko.observable();
        vm.reenterPassword = ko.observable();

        //second page
        vm.cardNumber = ko.observable();
        vm.csc = ko.observable();
        vm.cardFirstName = ko.observable();
        vm.cardLastName = ko.observable();
        vm.month = ko.observable();
        vm.year = ko.observable();
        vm.address1 = ko.observable();
        vm.address2 = ko.observable();
        vm.city = ko.observable();
        vm.state = ko.observable();
        vm.zip = ko.observable();

        //third page
        vm.students = ko.observableArray([{
            studentID: 0,
            studentFirstName: ko.observable(),
            studentLastName: ko.observable(),
            studentGrade: ko.observable()
        }]);
        $(".btn-remove-student").attr("disabled", true);

        vm.addStudent = function () {
            vm.students.push({
                studentID: 0,
                studentFirstName: ko.observable(),
                studentLastName: ko.observable(),
                studentGrade: ko.observable()
            });
            if (vm.students().length > 1) {
                $(".btn-remove-student").attr("disabled", false);
            }
        };

        vm.removeStudent = function (student) {
            if (vm.students().length > 1) {
                let removeIndex = vm.students().findIndex(s => {
                    return s.studentFirstName === student.studentFirstName &&
                        s.studentLastName === student.studentLastName &&
                        s.studentGrade === student.studentGrade;
                });
                vm.students.splice(removeIndex, 1);
            }
            if (vm.students().length <= 1) {
                $(".btn-remove-student").attr("disabled", true);
            }
        };

        //fourth page
        vm.paymentType = ko.observable();
        vm.calcRates = function () {
            let students = vm.students().map(s => {
                return {
                    StudentID: 0, 
                    FirstName: s.studentFirstName(),
                    LastName: s.studentLastName(),
                    Grade: s.studentGrade()
                };
            });

            let user = {
                UserID: 0,
                FirstName: vm.firstName(),
                LastName: vm.lastName(),
                Email: vm.email(),
                Plan: 0,
                UserType: GENERAL_USER,
                Students: students
            };

            user.Plan = vm.PLAN_TYPE_VALUES["YEARLY"];
            calculatePeriodicPayment(user).done(function (data) {
                vm.yearlyRate(Number(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }).fail(function (jqXHR) {
                window.alert("Could not calculate yearly rate.");
            });

            user.Plan = vm.PLAN_TYPE_VALUES["SEMESTERLY"];
            calculatePeriodicPayment(user).done(function (data) {
                vm.semesterlyRate(Number(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }).fail(function (jqXHR) {
                window.alert("Could not calculate semesterly rate.");
            });

            user.Plan = vm.PLAN_TYPE_VALUES["MONTHLY"];
            calculatePeriodicPayment(user).done(function (data) {
                vm.monthlyRate(Number(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }).fail(function (jqXHR) {
                window.alert("Could not calculate monthly rate.");
            });
        };
        vm.yearlyRate = ko.observable(0);
        vm.monthlyRate = ko.observable(0);
        vm.semesterlyRate = ko.observable(0);

        //errors
        vm.personalInputErrorMessage = ko.observable();
        vm.paymentInputErrorMessage = ko.observable();
        vm.studentInputErrorMessage = ko.observable();

        //payment plan types
        vm.PLAN_TYPE_VALUES = {}; 
        vm.PLAN_TYPE_VALUES["MONTHLY"] = 1;
        vm.PLAN_TYPE_VALUES["SEMESTERLY"] = 2;
        vm.PLAN_TYPE_VALUES["YEARLY"] = 3;
        
        vm.getPaymentPlanType = function () {
            return vm.PLAN_TYPE_VALUES[vm.paymentType()];
        };

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
            //radio buttons
            if (vm.currentPage === END_PAGE) {
                vm.calcRates();
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

        //move to the next page
        vm.next = function () {
            if (vm.currentPage === WELCOME_PAGE){
                $(PAGE_ID_PREFIX + vm.currentPage).hide();
                vm.currentPage++;
                $(PAGE_ID_PREFIX + vm.currentPage).show();
                vm.updateButtons();
                vm.updateProgressBar();
            } else if (vm.currentPage === PERSONAL_INFORMATION_PAGE) {
                let validator = $("#form-page-1").validate();
                validator.resetForm();
                if ($("#form-page-1").valid()) {
                    emailExists(vm.email()).done(function (data) {
                        if (data) {
                            validator.showErrors({
                                accountCreationEmail: "Email already exists"
                            });
                        } else {
                            $(PAGE_ID_PREFIX + vm.currentPage).hide();
                            vm.currentPage++;
                            $(PAGE_ID_PREFIX + vm.currentPage).show();
                            vm.updateButtons();
                            vm.updateProgressBar();
                        }
                    }).fail(function (jqXHR) {
                        if (jqXHR.status !== 401) {
                            let errorMessage = JSON.parse(jqXHR.responseText).Message;
                            window.alert("Could not save information: ".concat(errorMessage));
                        }
                    });
                }
            } else if (vm.currentPage === PAYMENT_INFORMATION_PAGE) {
                if ($("#form-page-2").valid()) {
                    $(PAGE_ID_PREFIX + vm.currentPage).hide();
                    vm.currentPage++;
                    $(PAGE_ID_PREFIX + vm.currentPage).show();
                    vm.updateButtons();
                    vm.updateProgressBar();
                }
            } else if (vm.currentPage === STUDENT_INFORMATION_PAGE) {
                if ($("#form-page-3").valid()) {
                    $(PAGE_ID_PREFIX + vm.currentPage).hide();
                    vm.currentPage++;
                    $(PAGE_ID_PREFIX + vm.currentPage).show();
                    vm.updateButtons();
                    vm.updateProgressBar();
                }
            }
        };

        $("account-creation-component").keypress(function (e) {
            //If the user presses enter, it will click the login button
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

        //finish and create account
        vm.done = function () {
            if (!$("#form-page-4").valid()) {
                window.alert("Please select a payment plan");
                return;
            }

            let students = vm.students().map(s => {
                return {
                    StudentID: 0, //should be set when the student is inserted into the database
                    FirstName: s.studentFirstName(),
                    LastName: s.studentLastName(),
                    Grade: s.studentGrade()
                };
            });

            let accountCreationInformation = {
                FirstName: vm.firstName(),
                LastName: vm.lastName(),
                Email: vm.email(),
                Password: vm.password(),
                Plan: vm.getPaymentPlanType(),
                UserType: GENERAL_USER,
                Students: students,
                CardholderFirstName: vm.cardFirstName(),
                CardholderLastName: vm.cardLastName(),
                StreetAddress1: vm.address1(),
                StreetAddress2: vm.address2(),
                City: vm.city(),
                State: vm.state(),
                Zip: vm.zip(),
                CardNumber: vm.cardNumber(),
                ExpirationYear: vm.year(),
                ExpirationMonth: vm.month()
            };

            $("#btn-done").attr("disabled", "disabled");
            $("#btn-back").attr("disabled", "disabled");

            createUser(accountCreationInformation).done(function (data) {
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

//calculate the user's periodic payment
function calculatePeriodicPayment(user) {
    let userData = JSON.stringify(user);
    return $.ajax(`${paymentControllerRoot}/CalculatePeriodicPayment`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: userData
    });
}

//create the user in the database
function createUser(accountCreationInformation) {
    let accountCreationInformationData = JSON.stringify(accountCreationInformation);
    return $.ajax(`${userInfoControllerRoot}/InsertUser`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: accountCreationInformationData
    });
}

//POSTs to see if an email is used in the database
function emailExists(email) {
    return $.ajax(`${userInfoControllerRoot}/EmailExists`, {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(email)
    });
}