require('./account-dashboard-component.scss');
require('../assets/background-image.scss');

const utility = require('../utility.js');

const accountDashboardAPIURL = "/api/account";

const regexSemicolonCheck = /^(?!.*?[;'"]).{0,}$/;
const regexZipCheck = /^\d{5}(?:[-\s]\d{4})?$/;
const regexLettersOnlyCheck = /(?!.*[^a-zA-Z]).{0,}/;

var studentsList = undefined;
var user = undefined;
var userPaymentInfo = undefined;

//add function to exports so index.js can see it
exports.accountDashboardBeforeShow = function () {
    //if not logged in
    if (!window.sessionStorage.getItem("Jwt")) {
        window.location = "#";
        return;
    }
};

ko.components.register('account-dashboard-component', {
    viewModel: function (params) {
        var accountDashboardVM = this;

        accountDashboardVM.billingInfo = ko.observable();

        accountDashboardVM.CardNumber = ko.observable();
        accountDashboardVM.ExpirationYear = ko.observable();
        accountDashboardVM.ExpirationMonth = ko.observable();
        accountDashboardVM.CardType = ko.observable();

        accountDashboardVM.UserFirstName = ko.observable();
        accountDashboardVM.UserLastName = ko.observable();
        accountDashboardVM.Email = ko.observable();
        accountDashboardVM.PaymentPlan = ko.observable();
        accountDashboardVM.Students = ko.observableArray([]);

        accountDashboardVM.Transactions = ko.observableArray([]);

        accountDashboardVM.NextPaymentDate = ko.observable();
        accountDashboardVM.NextPaymentCost = ko.observable();

        accountDashboardVM.personalInputErrorMessage = ko.observable();
        accountDashboardVM.paymentInputErrorMessage = ko.observable();
        accountDashboardVM.billingInputErrorMessage = ko.observable();
        accountDashboardVM.studentInputErrorMessage = ko.observable();

        accountDashboardVM.confirmModalData = ko.observable();

        accountDashboardVM.loadUserInformation = function () {
            //Gets users info
            getUserInfo().done(function (data) {
                user = data;
                accountDashboardVM.setUser();
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get user information, please try refreshing the page");
                }
            });

            //Get all needed information from database
            getPaymentSpringInfo().done(function (data) {
                userPaymentInfo = data;
                accountDashboardVM.billingInfo(data);
                require('./BillingInformationComponent/billing-information-component.js');
                //updates payment display info
                accountDashboardVM.setUIPaymentSpringInfo();
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get payment information, please try refreshing the page.");
                }
            });

            //gets all transactions for a user.
            getAllTransactionsForUser().done(function (data) {
                //sort so most recent is at top
                data.sort((a, b) => b.TransactionID - a.TransactionID);

                containsUnresolvedTransaction = data.find((transaction) => transaction.ProcessState === "RETRYING" || transaction.ProcessState === "FAILED");
                if (containsUnresolvedTransaction) {
                    $("#retryingTransactionError").show();
                }

                //make it display friendly
                accountDashboardVM.Transactions(data.map(function (transaction) {
                    return {
                        DateDue: transaction.DateDue.parseDateTimeString(),
                        AmountCharged: Number(transaction.AmountCharged).toLocaleString('en'),
                        ProcessState: transaction.ProcessState,
                        ReasonFailed: transaction.ReasonFailed
                    };
                }));
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get transaction information, please try refreshing the page.");
                }
            });

            //gets the next payment details for the user
            getNextTransactionForUser().done(function (data) {
                accountDashboardVM.NextPaymentDate(data.DateDue.parseDateTimeString());
                accountDashboardVM.NextPaymentCost(Number(data.AmountCharged).toLocaleString('en'));
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get your next transaction information, please try refreshing the page.");
                }
            });
        };

        //Sets ko components from saved user
        accountDashboardVM.setUser = function () {
            accountDashboardVM.UserFirstName(user.FirstName);
            accountDashboardVM.UserLastName(user.LastName);
            accountDashboardVM.Email(user.Email);
            accountDashboardVM.PaymentPlan(user.Plan);
            accountDashboardVM.Students(user.Students.map(student => {
                return {
                    StudentID: student.StudentID,
                    FirstName: ko.observable(student.FirstName),
                    LastName: ko.observable(student.LastName),
                    Grade: ko.observable(student.Grade)
                };
            }));
        };

        //Changes the user info in database and ui to what the user entered.
        accountDashboardVM.updateUser = function (data, event) {
            if ($("#edit-personal-form").valid()) {

                //disable save and cancel buttons
                $("#btn-save-edit-personal").attr("disabled", "disabled");
                $("#btn-cancel-edit-personal").attr("disabled", "disabled");

                //Check if email already exists in database
                emailExists(accountDashboardVM.Email()).done(function (data) {
                    emailInUse = data;
                    if (emailInUse && accountDashboardVM.Email() !== user.Email) {
                        accountDashboardVM.personalInputErrorMessage("Email is already used by another user");
                        $("#edit-personal-input-error").show();
                    } else {
                        let changedUserInfo = user;
                        changedUserInfo.FirstName = accountDashboardVM.UserFirstName();
                        changedUserInfo.LastName = accountDashboardVM.UserLastName();
                        changedUserInfo.Email = accountDashboardVM.Email();

                        updatePersonalInfo(changedUserInfo).done(function (newToken) {
                            //update user in local storage in the case of page reload
                            window.sessionStorage.setItem("Jwt", newToken);
                            user = changedUserInfo;
                            accountDashboardVM.stopEditing(data, event);
                        }).fail(function (jqXHR) {
                            if (jqXHR.status !== 401) {
                                let errorMessage = JSON.parse(jqXHR.responseText).Message;
                                window.alert("Could not save information: ".concat(errorMessage));
                            }
                        });
                    }
                }).fail(function (jqXHR) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("Couldn't check if email has been used: ".concat(errorMessage));
                }).always(function () {
                    //re-enable buttons
                    $("#btn-save-edit-personal").removeAttr("disabled");
                    $("#btn-cancel-edit-personal").removeAttr("disabled");
                });
            }
        };

        //Changes student info in database and ui to what user entered
        accountDashboardVM.updateStudents = function (data, event) {
            if ($("#edit-students-form").valid()) {

                //disable save and edit buttons
                $("#btn-save-edit-student").attr("disabled", "disabled");
                $("#btn-cancel-edit-student").attr("disabled", "disabled");

                //to return to normal data instead of observables
                let inputStudents = accountDashboardVM.Students().map(student => {
                    return {
                        StudentID: student.StudentID,
                        FirstName: student.FirstName(),
                        LastName: student.LastName(),
                        Grade: student.Grade()
                    };
                });

                //new students will have an undefined StudentID
                let newStudents = inputStudents.filter((s) => !s.StudentID);
                //deleted students will be in the user object but not in inputStudents
                let originalStudentIDs = user.Students.map((s) => s.StudentID);
                let inputStudentIDs = inputStudents.map((s) => s.StudentID);

                let deletedStudentIDs = originalStudentIDs.filter((id) => !inputStudentIDs.includes(id));
                //filter out new students
                let updatedStudents = inputStudents.filter((s) => s.StudentID);

                updateStudentInfo(user.UserID, updatedStudents, deletedStudentIDs, newStudents).done(function () {
                    //update user in local storage in the case of page reload
                    user.Students = inputStudents;
                    accountDashboardVM.stopEditing(data, event);
                    //gets the price of the next payment for the user based on the new students added
                    getNextTransactionForUser().done(function (data) {
                        accountDashboardVM.NextPaymentDate(data.DateDue.parseDateTimeString());
                        accountDashboardVM.NextPaymentCost(Number(data.AmountCharged).toLocaleString('en'));
                    }).fail(function (jqXHR) {
                        if (jqXHR.status !== 401) {
                            window.alert("Could not get your next transaction information, please try refreshing the page.");
                        }
                    });
                }).fail(function (jqXHR) {
                    if (jqXHR.status !== 401) {
                        let errorMessage = JSON.parse(jqXHR.responseText).Message;
                        window.alert("Could not save information: ".concat(errorMessage));
                    }
                }).always(function () {
                    //re-enable buttons
                    $("#btn-save-edit-student").removeAttr("disabled");
                    $("#btn-cancel-edit-student").removeAttr("disabled");
                });
            }
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
        };

        accountDashboardVM.deleteStudent = function (student) {
            if (accountDashboardVM.Students().length === 1) {
                accountDashboardVM.openConfirmModal(user, "Deleting your last student will result in deleting your account. Are you absolutely sure?", function (data) {
                    deleteUser(data).done(function () {
                        localStorage.removeItem("user");
                        window.location = "#";
                    }).fail(function (jqXHR) {
                        let errorMessage = JSON.parse(jqXHR.responseText).Message;
                        window.alert("There was an error deleting your account: ".conat(errorMessage));
                    });
                });
            } else {
                accountDashboardVM.Students(accountDashboardVM.Students().filter((s) => {
                    return s.StudentID !== student.StudentID || s.FirstName() !== student.FirstName() || s.LastName() !== student.LastName();
                }));
            }
        };

        //Changes the payment info in payment spring and ui to what the user entered.
        accountDashboardVM.updatePaymentInfo = function (data, event) {
            if ($("#edit-payment-form").valid()) {

                 //disable cancel and save buttons while request loads
                 $("#btn-save-edit-payment").attr('disabled', 'disabled');
                 $("#btn-cancel-edit-payment").attr('disabled', 'disabled');

                 let changedCardInfo = {
                     CustomerID: userPaymentInfo.CustomerID,
                     CardNumber: accountDashboardVM.CardNumber(),
                     ExpirationYear: accountDashboardVM.ExpirationYear(),
                     ExpirationMonth: accountDashboardVM.ExpirationMonth()
                 };

                 updatePaymentCardInfo(changedCardInfo).done(function () {
                     //reset it to last few digits
                     userPaymentInfo.CardNumber = changedCardInfo.CardNumber.substring(changedCardInfo.CardNumber.length - 4).replace(/[0]*/, "");
                     userPaymentInfo.ExpirationYear = changedCardInfo.ExpirationYear;
                     userPaymentInfo.ExpirationMonth = changedCardInfo.ExpirationMonth;

                     //UI will be updated here
                     accountDashboardVM.stopEditing(data, event);
                 }).fail(function (jqXHR) {
                     if (jqXHR.status !== 401) {
                         changedPaymentInfo = undefined;
                         let errorMessage = JSON.parse(jqXHR.responseText).Message;
                         window.alert("Could not save information: ".concat(errorMessage));
                     }
                 }).always(function () {
                     //re-enable buttons
                     $("#btn-save-edit-payment").removeAttr('disabled');
                     $("#btn-cancel-edit-payment").removeAttr('disabled');
                 });
             }
        };

        //Sets the ko variables to the saved payment spring information
        accountDashboardVM.setUIPaymentSpringInfo = function () {
            accountDashboardVM.CardNumber(userPaymentInfo.CardNumber);
            accountDashboardVM.ExpirationYear(userPaymentInfo.ExpirationYear);
            accountDashboardVM.ExpirationMonth(userPaymentInfo.ExpirationMonth);
            accountDashboardVM.CardType(userPaymentInfo.CardType);
        };

        //hides label objects and edit buttons to show save and cancel buttons with text boxes
        accountDashboardVM.startEditing = function (data, event) {
            let senderElementID = event.target.id;

            let informationSection = senderElementID.replace("btn-", "");

            $("." + informationSection + "-active").show();
            $("." + informationSection + "-inactive").hide();
        };

        //hides save and cancel buttons along with text boxes and shows labels and edit button
        accountDashboardVM.stopEditing = function (data, event) {
            let senderElementID = event.target.id;

            let informationSection = "";
            if (senderElementID.includes("cancel")) {
                informationSection = senderElementID.replace("btn-cancel-", "");
            } else if (senderElementID.includes("save")) {
                informationSection = senderElementID.replace("btn-save-", "");
            }
            
            accountDashboardVM.setUser();
            accountDashboardVM.setUIPaymentSpringInfo();

            $("." + informationSection + "-active").hide();
            $("." + informationSection + "-inactive").show();

            //hide error message if it's shown
            $("#" + informationSection + "-input-error").hide();
        };

        accountDashboardVM.openConfirmModal = function (data, message, confirmAction) {
            $("#confirmModal").finish();
            accountDashboardVM.confirmModalData({
                data: data,
                warningMessage: message,
                confirmAction: function (params) {
                    //hiding a modal is instantaneous, while showing it has a fade
                    $("#confirmModal").removeClass("fade");
                    $("#confirmModal").modal("hide");
                    $("#confirmModal").addClass("fade");
                    confirmAction(params);
                }
            });

            $("#confirmModal").modal("show");
        };

        //make sure user has logged in properly
        if ($("account-dashboard-component").is(":visible") && window.sessionStorage.getItem("Jwt")) {
            accountDashboardVM.loadUserInformation();
        }

        $("#processStateInformation").tooltip();

        return accountDashboardVM;
    },

    template: require('./account-dashboard-component.html')
});

//Gets a user's info
function getUserInfo() {
    return $.ajax(accountDashboardAPIURL + "/GetUserInfo", {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//Gets a user's next transaction details
function getNextTransactionForUser() {
    return $.ajax(accountDashboardAPIURL + "/GetNextTransactionForUser", {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//Gets a user's transaction details
function getAllTransactionsForUser() {
    return $.ajax(accountDashboardAPIURL + "/GetAllTransactionsForUser", {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//GETs a user's payment spring information
function getPaymentSpringInfo() {
    return $.ajax(accountDashboardAPIURL + "/GetPaymentInfoForUser", {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
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

//POSTs any updates to a user's list of students
function updateStudentInfo(userID, updatedStudents, deletedStudentIDs, newStudents) {
    //JSON is used here because empty arrays get cast to undefined if not using JSON
    //undefined is obviously very different from an empty array, so JSON is used to avoid that
    let jsonData = JSON.stringify({
        UserID: userID,
        UpdatedStudents: updatedStudents,
        DeletedStudentIDs: deletedStudentIDs,
        AddedStudents: newStudents
    });
    return $.ajax(accountDashboardAPIURL + "/UpdateStudentInfo", {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData,
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//POSTs a user to be deleted
function deleteUser(user) {
    return $.ajax(accountDashboardAPIURL + "/DeleteUser", {
        method: "POST",
        data: user,
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//POSTs any changes to the credit card info
function updatePaymentCardInfo(paymentCardInfo) {
    return $.ajax(accountDashboardAPIURL + "/UpdatePaymentCardInfo", {
        method: "POST",
        data: paymentCardInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}

function emailExists(email) {
    return $.ajax(accountDashboardAPIURL + "/EmailExists", {
        method: "POST",
        contentType: "application/JSON; charset=utf-8",
        data: JSON.stringify(email)
    });
}