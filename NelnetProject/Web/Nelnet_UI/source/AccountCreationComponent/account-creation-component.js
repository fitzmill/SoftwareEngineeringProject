require('./account-creation-component.scss');
require('../assets/background-image.scss');

//paging constants
const START_PAGE = 1;
const PERSONAL_INFORMATION_PAGE = 1;
const PAYMENT_INFORMATION_PAGE = 2;
const STUDENT_INFORMATION_PAGE = 3;
const END_PAGE = 4;

//information validation through regex
const regexSemicolonCheck = /^(?!.*?[;'"]).{0,}$/;
const regexZipCheck = /^\d{5}(?:[-\s]\d{4})?$/; //regexNumCheck but also allows for hyphen(-)
const regexLettersOnlyCheck = /(?!.*[^a-zA-Z]).{0,}/;

//api url constants
const accountCreationAPIURL = "/api/account";

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

        vm.addStudent = function () {
            vm.students.push({
                studentID: 0,
                studentFirstName: ko.observable(),
                studentLastName: ko.observable(),
                studentGrade: ko.observable()
            });
        };

        //fourth page
        vm.paymentType = ko.observable();
        vm.calcRates = function () {
            let students = vm.students().map(s => {
                return {
                    Grade: s.studentGrade()
                };
            });

            calculatePeriodicPayment("YEARLY", students).done(function (data) {
                vm.yearlyRate(Number(data).toLocaleString('en'));
            }).fail(function (jqXHR) {
                window.alert("Could not calculate yearly rate.");
            });

            calculatePeriodicPayment("SEMESTERLY", students).done(function (data) {
                vm.semesterlyRate(Number(data).toLocaleString('en'));
            }).fail(function (jqXHR) {
                window.alert("Could not calculate semesterly rate.");
            });

            calculatePeriodicPayment("MONTHLY", students).done(function (data) {
                vm.monthlyRate(Number(data).toLocaleString('en'));
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
            for (let i = START_PAGE; i < END_PAGE; i++) {
                if (i >= vm.currentPage) {
                    $('#rectangle-' + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $('#rectangle-' + i)[0].style.backgroundColor = "#007bff";
                }
            }

            for (let i = START_PAGE; i <= END_PAGE; i++) {
                if (i > vm.currentPage) {
                    $('#dot-page-' + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $('#dot-page-' + i)[0].style.backgroundColor = "#007bff";
                }
            }
        };

        //create function to update buttons
        vm.updateButtons = function () {
            if (vm.currentPage === START_PAGE) {
                $("#btn-cancel").show();
                $("#btn-back").hide();
            } else {
                $("#btn-cancel").hide();
                $("#btn-back").show();
            }
            if (vm.currentPage === END_PAGE) {
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
            $("#info-page-" + vm.currentPage).hide();
            vm.currentPage--;
            $("#info-page-" + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        };

        //move to the next page
        vm.next = function () {
            if (vm.currentPage === PERSONAL_INFORMATION_PAGE) {
                if (!vm.firstName() || !vm.firstName().match(regexSemicolonCheck)) {
                    vm.personalInputErrorMessage("Invalid first name");
                    return;
                } else if (!vm.lastName() || !vm.lastName().match(regexSemicolonCheck)) {
                    vm.personalInputErrorMessage("Invalid last name");
                    return;
                } else if (!vm.email() || !vm.email().emailMeetsRequirements()) {
                    vm.personalInputErrorMessage("Invalid email");
                    return;
                } else if (vm.reenterEmail() !== vm.email()) {
                    vm.personalInputErrorMessage("Emails don't match");
                    return;
                } else if (!vm.password() || !vm.password().passwordMeetsRequirements()) {
                    vm.personalInputErrorMessage("Passwords don't match");
                    return;
                }
            } else if (vm.currentPage === PAYMENT_INFORMATION_PAGE) {
                if (!vm.cardNumber() || vm.cardNumber().toString().length < 15 || vm.cardNumber().toString().length > 19) {
                    vm.paymentInputErrorMessage("Invalid Card Number");
                    return;
                } else if (!vm.cardFirstName() || !vm.cardFirstName().match(regexSemicolonCheck)) {
                    vm.paymentInputErrorMessage("Invalid Card First Name");
                    return;
                } else if (!vm.cardLastName() || !vm.cardLastName().match(regexSemicolonCheck)) {
                    vm.paymentInputErrorMessage("Invalid Card Last Name");
                    return;
                } else if (!vm.month() || vm.month() < 1 || vm.month() > 12) {
                    vm.paymentInputErrorMessage("Invalid Month");
                    return;
                } else if (!vm.year() || vm.year() < 2018) {
                    vm.paymentInputErrorMessage("Invalid Year");
                    return;
                } else if (!vm.address1() || !vm.address1().match(regexSemicolonCheck)) {
                    vm.paymentInputErrorMessage("Invalid Street Address 1");
                    return;
                } else if (vm.address2() && !vm.address2().match(regexSemicolonCheck)) {
                    vm.paymentInputErrorMessage("Invalid Street Address 2");
                    return;
                } else if (!vm.city() || !vm.city().match(regexSemicolonCheck)) {
                    vm.paymentInputErrorMessage("Invalid City");
                    return;
                } else if (!vm.state() || !vm.state().match(regexLettersOnlyCheck) || !vm.state().length === 2) {
                    vm.paymentInputErrorMessage("Invalid State");
                    return;
                } else if (!vm.zip() || !vm.zip().match(regexZipCheck)) {
                    vm.paymentInputErrorMessage("Invalid Zip Code")
                    return;
                }
            } else if (vm.currentPage === STUDENT_INFORMATION_PAGE  && !checkValidStudents(vm.students())) {
                vm.studentInputErrorMessage("Invalid student information");
                return;
            }
            $("#info-page-" + vm.currentPage).hide();
            vm.currentPage++;
            $("#info-page-" + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        };

        //finish and create account
        vm.done = function () {
            let students = vm.students().map(s => {
                return {
                    StudentID: 0, //should be set when the student is inserted into the database
                    FirstName: s.studentFirstName(),
                    LastName: s.studentLastName(),
                    Grade: s.studentGrade()
                };
            });

            console.log(students);

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

            console.log(accountCreationInformation);

            createUser(accountCreationInformation).done(function (data) {
                //TODO: move on to their dashboard
            }).fail(function (jqXHR) {
                window.alert("Could not create account, please try again later.");
            });
        };

        //show current page and correct buttons
        for (let i = START_PAGE; i <= END_PAGE; i++) {
            if (i === vm.currentPage) {
                $("#info-page-" + i).show();
            } else {
                $("#info-page-" + i).hide();
            }
        }
        vm.updateButtons();
        vm.updateProgressBar();

        return vm;
    },

    template: require('./account-creation-component.html')
});

//calculate the user's payment info
function calculatePeriodicPayment(userPaymentPlan, students) {
    let userData = JSON.stringify({
        Plan: userPaymentPlan,
        Students: students
    });
    return $.ajax(accountCreationAPIURL + "/CalculatePeriodicPayment", {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: userData
    });
}

//create the user in the database
function createUser(accountCreationInformation) {
    let accountCreationInformationData = JSON.stringify(accountCreationInformation);
    return $.ajax(accountCreationAPIURL + "/InsertUser", {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: accountCreationInformationData
    });
}

//Checks that student entries are valid
function checkValidStudents(students) {
    let result = true;
    students.forEach(function (student) {
        if (!student.studentFirstName() || !student.studentFirstName().match(regexSemicolonCheck)) {
            result = false;
        } else if (!student.studentLastName() || !student.studentLastName().match(regexSemicolonCheck)) {
            result = false;
        } else if (!student.studentGrade() || student.studentGrade() < 0 || student.studentGrade() > 12) {
            result = false;
        }
    });
    return result;
}

Number.prototype.formatMoney = function (decimals) {
    let number = this;
    sign = number < 0 ? "-" : "";
    number = Math.abs(Number(number) || 0).toFixed(decimals);
    i = String(parseInt(number));
    j = (j = i.length) > 3 ? j % 3 : 0;
    return sign + (j ? i.substr(0, j) + "," : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + ",") + (decimals ? "." + Math.abs(n - i).toFixed(decimals).slice(2) : "");
};