require('./account-dashboard-component.scss');
require('../assets/background-image.scss');

const accountDashboardAPIURL = "/api/account-dashboard";
const regexSemicolonCheck = /[^;]/;
const regexNumCheck = /[^\d]/;
const regexZipCheck = /[^\d-]/;
const regexLettersOnlyCheck = /[^a-zA-Z]/;
var studentsList = [{ FirstName: "Joey", LastName: "Jimson", Grade: 1 }, { FirstName: "Jimbo", LastName: "Jimson", Grade: 5 }];
var user = { UserID: 1, FirstName: "Jim", LastName: "Jimson", Email: "jimjimson@jimmail.jim", Students: studentsList };
var userPaymentInfo = {
    CustomerID: undefined,
    FirstName: undefined,
    LastName: undefined,
    StreetAddress1: undefined,
    StreetAddress2: undefined,
    City: undefined,
    State: undefined,
    Zip: undefined,
    CardNumber: undefined,
    ExpirationYear: undefined,
    ExpirationMonth: undefined,
    CardType: undefined,
};

ko.components.register('account-dashboard-component', {
    viewModel: function (params) {
        var accountDashboardVM = this;

        accountDashboardVM.CardFirstName = ko.observable();
        accountDashboardVM.CardLastName = ko.observable();
        accountDashboardVM.StreetAddress1 = ko.observable();
        accountDashboardVM.StreetAddress2 = ko.observable();
        accountDashboardVM.City = ko.observable();
        accountDashboardVM.State = ko.observable();
        accountDashboardVM.Zip = ko.observable();
        accountDashboardVM.CardNumber = ko.observable();
        accountDashboardVM.ExpirationYear = ko.observable();
        accountDashboardVM.ExpirationMonth = ko.observable();
        accountDashboardVM.CardType = ko.observable();
        accountDashboardVM.CSC = ko.observable();

        accountDashboardVM.UserFirstName = ko.observable();
        accountDashboardVM.UserLastName = ko.observable();
        accountDashboardVM.Email = ko.observable();
        accountDashboardVM.PaymentPlan = ko.observable();
        accountDashboardVM.Students = ko.observableArray([]);

        accountDashboardVM.NextPaymentDate = ko.observable();
        accountDashboardVM.NextPaymentCost = ko.observable();

        accountDashboardVM.setUser = function () {
            accountDashboardVM.UserFirstName(user.FirstName);
            accountDashboardVM.UserLastName(user.LastName);
            accountDashboardVM.Email(user.Email);
            accountDashboardVM.PaymentPlan(user.Plan.toString());
            accountDashboardVM.Students(user.Students.map(student => {
                return {
                    FirstName: ko.observable(student.FirstName),
                    LastName: ko.observable(student.LastName),
                    Grade: ko.observable(student.Grade)
                }
            }));
        };

        accountDashboardVM.updateUser = function () {
            if (!accountDashboardVM.UserFirstName() || !accountDashboardVM.UserFirstName().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.UserLastName() || !accountDashboardVM.UserLastName().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.Email() || !accountDashboardVM.Email().emailMeetsRequirements()) {
                return;
            } else if (!checkValidStudents(accountDashboardVM.Students())) {
                return;
            }
            user.FirstName = accountDashboardVM.UserFirstName();
            user.LastName = accountDashboardVM.UserLastName();
            user.Email = accountDashboardVM.Email();
            user.Students = [];
            accountDashboardVM.Students().forEach(function (student) {
                user.Students.push(student);
            });

            updatePersonalAndStudentInfo().done({

            }).fail(function (jqXHR) {
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not save information: ".concat(errorMessage));
            });
        };

        accountDashboardVM.updatePaymentInfo = function () {
            if (!accountDashboardVM.CardFirstName() || !accountDashboardVM.CardFirstName().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.CardLastName() || !accountDashboardVM.CardUserLastName().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.StreetAddress1() || !accountDashboardVM.StreetAddress1().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.StreetAddress2().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.City() || !accountDashboardVM.City().match(regexSemicolonCheck)) {
                return;
            } else if (!accountDashboardVM.State() || !accountDashboardVM.State().match(regexLettersOnlyCheck)) {
                return;
            } else if (!accountDashboardVM.Zip() || !accountDashboardVM.Zip().match(regexZipCheck)) {
                return;
            } else if (!accountDashboardVM.CardNumber() || !accountDashboardVM.CardNumber().match(regexNumCheck) || accountDashboardVM.CardNumber().length < 15 || accountDashboardVM().CardNumber.length > 19) {
                return;
            } else if (!accountDashboardVM.ExpirationYear() || !accountDashboardVM.ExpirationYear().match(regexNumCheck) || accountDashboardVM.ExpirationYear().length != 2) {
                return;
            } else if (!accountDashboardVM.ExpirationMonth() || !accountDashboardVM.ExpirationMonth().match(regexNumCheck) || accountDashboardVM.ExpirationMonth().length != 2) {
                return;
            } else if (!accountDashboardVM.CardType() || !accountDashboardVM.CardType().match(regexLettersOnlyCheck)) {
                return;
            } else if (!accountDashboardVM.CSC() || !accountDashboardVM.CSC().match(regexNumCheck)) {
                return;
            }
            var changedPaymentInfo = {
                CustomerID: userPaymentInfo.CustomerID,
                FirstName: accountDashboardVM.CardFirstName(),
                LastName: accountDashboardVM.CardLastName(),
                StreetAddress1: accountDashboardVM.StreetAddress1(),
                StreetAddress2: accountDashboardVM.StreetAddress2(),
                City: accountDashboardVM.City(),
                State: accountDashboardVM.State(),
                Zip: accountDashboardVM.Zip(),
                CardNumber: accountDashboardVM.CardNumber(),
                ExpirationYear: accountDashboardVM.ExpirationYear(),
                ExpirationMonth: accountDashboardVM.ExpirationMonth(),
                CardType: accountDashboardVM.CardType(),
                CSC: accountDashboardVM.CSC()
            };

            updatePaymentInfo(changedPaymentInfo).done( function () {
                userPaymentInfo.CustomerID = changedPaymentInfo.CustomerID;
                userPaymentInfo.FirstName = changedPaymentInfo.FirstName;
                userPaymentInfo.LastName = changedPaymentInfo.LastName;
                userPaymentInfo.StreetAddress1 = changedPaymentInfo.StreetAddress1;
                userPaymentInfo.StreetAddress2 = changedPaymentInfo.StreetAddress2;
                userPaymentInfo.City = changedPaymentInfo.City;
                userPaymentInfo.State = changedPaymentInfo.State;
                userPaymentInfo.Zip = changedPaymentInfo.Zip;
                userPaymentInfo.CardNumber = changedPaymentInfo.CardNumber.substr(changedPaymentInfo.CardNumber.length - 4);
                auserPaymentInfo.ExpirationYear = changedPaymentInfo.ExpirationYear;
                userPaymentInfo.ExpirationMonth = changedPaymentInfo.ExpirationMonth;
                userPaymentInfo.CardType = changedPaymentInfo.CardType;
                changedPaymentInfo = undefined;
                accountDashboardVM.CSC(undefined);
                accountDashboardVM.CardNumber(undefined);
            }).fail(function (jqXHR) {
                changedPaymentInfo = undefined;
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not save information: ".concat(errorMessage));
                });
        }

        accountDashboardVM.setUIPaymentSpringInfo = function () {
            accountDashboardVM.FirstName(userPaymentInfo.FirstName);
            accountDashboardVM.LastName(userPaymentInfo.LastName);
            accountDashboardVM.StreetAddress1(userPaymentInfo.StreetAddress1);
            accountDashboardVM.StreetAddress2(userPaymentInfo.StreetAddress2);
            accountDashboardVM.City(userPaymentInfo.City);
            accountDashboardVM.State(userPaymentInfo.State);
            accountDashboardVM.Zip(userPaymentInfo.Zip);
            accountDashboardVM.CardNumber(userPaymentInfo.CardNumber);
            accountDashboardVM.ExpirationYear(userPaymentInfo.ExpirationYear);
            accountDashboardVM.ExpirationMonth(userPaymentInfo.ExpirationMonth);
            accountDashboardVM.CardType(userPaymentInfo.CardType);
        }

        getPaymentSpringInfo(user.UserID).done(function (data) {
            userPaymentInfo.CustomerID = data.CustomerID;
            userPaymentInfo.FirstName = data.FirstName;
            userPaymentInfo.LastName = data.LastName;
            userPaymentInfo.StreetAddress1 = data.StreetAddress1;
            userPaymentInfo.StreetAddress2 = data.StreetAddress2;
            userPaymentInfo.City = data.City;
            userPaymentInfo.State = data.State;
            userPaymentInfo.Zip = data.Zip;
            userPaymentInfo.CardNumber = data.CardNumber;
            userPaymentInfo.ExpirationYear = data.ExpirationYear;
            userPaymentInfo.ExpirationMonth = data.ExpirationMonth;
            userPaymentInfo.CardType = data.CardType;
        }).fail(function (jqXHR) {
            window.alert("Could not get payment information, please try refreshing the page.");
        });

        //hides label objects and edit buttons to show save and cancel buttons with text boxes
        accountDashboardVM.startEditing = function (data, event) {
            let senderElementID = event.target.id;

            let informationSection = senderElementID.replace("btn-", "");

            $("." + informationSection + "-active").show();
            $("." + informationSection + "-inactive").hide();
        }

        //hides save and cancel buttons along with text boxes and shows labels and edit button
        accountDashboardVM.stopEditing = function (data, event) {
            let senderElementID = event.target.id;

            let informationSection = senderElementID.replace("btn-cancel-", "");
            accountDashboardVM.setUser()
            accountDashboardVM.setUIPaymentSpringInfo();

            $("." + informationSection + "-active").hide();
            $("." + informationSection + "-inactive").show();
        }

        accountDashboardVM.setUser();
        getPaymentSpringInfo(user.UserID);
        accountDashboardVM.setUIPaymentSpringInfo();

        return accountDashboardVM;
    },

    template: require('./account-dashboard-component.html')
});

//GETs a user's payment spring information
function getPaymentSpringInfo(userID) {
    let userIDString = user.UserID.toString();
    return $.ajax(accountDashboardAPIURL + "/GetPaymentInfoForUser/" + userIDString, {
        method: "GET"
    });
}

//POSTs any changes to the user
function updatePersonalAndStudentInfo() {
    return $.ajax(accountDashboardAPIURL + "/UpdatePersonalAndStudentInfo", {
        method: "POST",
        data: user
    });
}

//POSTs any changes to the payment info
function updatePaymentInfo(paymentInfo) {
    return $.ajax(accountDashboardAPIURL + "/UpdatePaymentInfo", {
        method: "POST",
        data: paymentInfo
    });
}

//Checks that student entries are valid
function checkValidStudents(students) {
    students.forEach(function (student) {
        if (!student.FirstName || !student.LastName.match(regexSemicolonCheck)) {
            return false;
        } else if (!student.LastName || !student.LastName.match(regexSemicolonCheck)) {
            return false;
        } else if (!student.Grade || student.Grade.match(regexNumCheck)) {
            return false;
        }
    });
    return true;
}