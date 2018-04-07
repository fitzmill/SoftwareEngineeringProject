﻿require('./account-dashboard-component.scss');
require('../assets/background-image.scss');

const accountDashboardAPIURL = "/api/account";
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

exports.loadUserInformation = function () {
    user = JSON.parse(window.localStorage.getItem("user"));

    //if not logged in
    if (!user) {
        window.location = "#";
    }

    //this will only be true when redirected to this page from login, since the component will be binded
    if ($("account-dashboard-component").children().length > 0) {
        ko.dataFor($('account-dashboard-component').get(0).firstChild).setUser();
    }


    //getPaymentSpringInfo(user.UserID).done(function (data) {
    //    accountDashboardVM.CardFirstName = data.FirstName;
    //    accountDashboardVM.CardLastName = data.LastName;
    //    accountDashboardVM.StreetAddress1 = data.StreetAddress1;
    //    accountDashboardVM.StreetAddress2 = data.StreetAddress2;
    //    accountDashboardVM.City = data.City;
    //    accountDashboardVM.State = data.State;
    //    accountDashboardVM.Zip = data.Zip;
    //    accountDashboardVM.CardNumber = data.CardNumber;
    //    accountDashboardVM.ExpirationYear = data.ExpirationYear;
    //    accountDashboardVM.ExpirationMonth = data.ExpirationMonth;
    //    accountDashboardVM.CardType = data.CardType;
    //}).fail(function (jqXHR) {
    //    window.alert("Could not get payment information, please try refreshing the page.");
    //});
}

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

        accountDashboardVM.Transactions = ko.observableArray([]);

        accountDashboardVM.NextPaymentDate = ko.observable();
        accountDashboardVM.NextPaymentCost = ko.observable();

        accountDashboardVM.confirmModalData = ko.observable();

        //gets all transactions for a user.
        getAllTransactionsForUser(user.UserID).done(function (data) {
            accountDashboardVM.Transactions(data);
        }).fail(function (jqXHR) {
            window.alert("Could not get transaction information, please try refreshing the page.");
        });

        //Sets ko components from saved user.accountDashboardVM.setUser = function () {
            accountDashboardVM.UserFirstName(user.FirstName);
            accountDashboardVM.UserLastName(user.LastName);
            accountDashboardVM.Email(user.Email);
            accountDashboardVM.PaymentPlan(JSON.stringify(user.Plan));
            accountDashboardVM.Students(user.Students.map(student => {
                return {
                    StudentID: student.StudentID,
                    FirstName: ko.observable(student.FirstName),
                    LastName: ko.observable(student.LastName),
                    Grade: ko.observable(student.Grade)
                }
            }));
        };

        //Changes the user info in database and ui to what the user entered.
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
                accountDashboardVM.setUser();
                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                window.alert("Could not save information: ".concat(errorMessage));
            });
        };


        accountDashboardVM.addStudent = function () {
            accountDashboardVM.Students.push({
                StudentID: undefined,
                FirstName: ko.observable(),
                LastName: ko.observable(),
                Grade: ko.observable()
            });
            //by default the new student will show with labels and not text boxes
            $(".edit-student-active").show();
            $(".edit-student-inactive").hide();
        }

        accountDashboardVM.deleteStudent = function (student) {
            accountDashboardVM.Students(accountDashboardVM.Students().filter((s) => s.StudentID !== student.StudentID));
        }

        //Changes the payment info in payment spring and ui to what the user entered.
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
            } else if (!accountDashboardVM.State() || !accountDashboardVM.State().match(regexLettersOnlyCheck) || accountDashboardVM.State().length != 2) {
                return;
            } else if (!accountDashboardVM.Zip() || !accountDashboardVM.Zip().match(regexZipCheck)) {
                return;
            } else if (!accountDashboardVM.CardNumber() || !accountDashboardVM.CardNumber().match(regexNumCheck) || accountDashboardVM.CardNumber().length < 15) {
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
                userPaymentInfo.ExpirationYear = changedPaymentInfo.ExpirationYear;
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

        //Sets the ko variables to the saved payment spring information
        accountDashboardVM.setUIPaymentSpringInfo = function () {
            accountDashboardVM.CardFirstName(userPaymentInfo.FirstName);
            accountDashboardVM.CardLastName(userPaymentInfo.LastName);
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

        //gets the next payment details for the user
        getNextTransactionForUser(user.UserID).done(function (data) {
            accountDashboardVM.NextPaymentDate(JSON.stringify(data.DateDue).parseDateTimeString());
            accountDashboardVM.NextPaymentCost(data.AmountCharged);
        }).fail(function (jqXHR) {
            window.alert("Could not get your next transaction information, please try refreshing the page.");
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

        accountDashboardVM.openConfirmModal = function (data, message, confirmAction) {
            accountDashboardVM.confirmModalData({
                data: data,
                warningMessage: message,
                confirmAction: function (params) {
                    $("#confirmModal").modal("hide");
                    confirmAction(params);
                }
            });

            $("#confirmModal").modal("show");
        }

        //make sure user has logged in properly
        if (user) {
            accountDashboardVM.setUser();
        }

        return accountDashboardVM;
    },

    template: require('./account-dashboard-component.html')
});

//Gets a user's next transaction details
function getNextTransactionForUser(userID) {
    let userIDString = userID.toString();
    return $.ajax(accountDashboardAPIURL + "/GetNextTransactionForUser/" + userIDString, {
        method: "GET"
    });
}

//Gets a user's transaction details
function getAllTransactionsForUser(userID) {
    let userIDString = userID.toString();
    return $.ajax(accountDashboardAPIURL + "/GetAllTransactionsForUser/" + userIDString, {
        method: "GET"
    });
}

//GETs a user's payment spring information
function getPaymentSpringInfo(userID) {
    let userIDString = userID.toString();
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