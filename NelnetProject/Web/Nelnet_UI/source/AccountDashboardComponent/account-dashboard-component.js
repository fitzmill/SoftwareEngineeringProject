require('./account-dashboard-component.scss');
require('../assets/background-image.scss');

const utility = require('../utility.js');

//api url constants
const userInfoControllerRoot = "/api/userinfo";
const paymentControllerRoot = "/api/payment";
const billingControllerRoot = "/api/billing";

require('./PersonalInformationComponent/personal-information-component.js');
require('./PaymentInformationComponent/payment-information-component.js');
require('./StudentInformationComponent/student-information-component.js');
require('./BillingInformationComponent/billing-information-component.js');
require('./TransactionInformationComponent/transaction-information-component.js');

var user = undefined;

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

        accountDashboardVM.personalInfo = ko.observable();
        accountDashboardVM.paymentInfo = ko.observable();
        accountDashboardVM.billingInfo = ko.observable();
        accountDashboardVM.studentInfo = ko.observable();
        accountDashboardVM.transactionInfo = ko.observable();

        accountDashboardVM.paymentPlan = ko.observable();

        accountDashboardVM.nextPaymentDate = ko.observable();
        accountDashboardVM.nextPaymentCost = ko.observable();

        accountDashboardVM.studentInfo.subscribe(function () {
            //gets the next payment details for the user
            getNextTransactionForUser().done(function (data) {
                accountDashboardVM.nextPaymentDate(data.DateDue.parseDateTimeString());
                accountDashboardVM.nextPaymentCost(data.AmountCharged.formatAsMoney());
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get your next transaction information, please try refreshing the page.");
                }
            });
        });

        accountDashboardVM.transactions = ko.observableArray([]);

        accountDashboardVM.confirmModalData = ko.observable();

        accountDashboardVM.loadUserInformation = function () {
            //Gets users info
            getUserInfo().done(function (data) {
                user = data;
                accountDashboardVM.personalInfo(data);
                accountDashboardVM.studentInfo(data.Students);
                accountDashboardVM.paymentPlan(data.Plan);
            }).fail(function (jqXHR) {
                if (jqXHR.status !== 401) {
                    window.alert("Could not get user information, please try refreshing the page");
                }
            });

            //Get all needed information from database
            getPaymentSpringInfo().done(function (data) {
                userPaymentInfo = data;
                accountDashboardVM.billingInfo(data);
                accountDashboardVM.paymentInfo(data);
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
                accountDashboardVM.transactions(data.map(function (transaction) {
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
        };

        accountDashboardVM.deleteUser = function () {
            accountDashboardVM.openConfirmModal(user, "Deleting your last student will result in deleting your account. Are you absolutely sure?", function (data) {
                deleteUser(data).done(function () {
                    localStorage.removeItem("user");
                    window.location = "#";
                }).fail(function (jqXHR) {
                    let errorMessage = JSON.parse(jqXHR.responseText).Message;
                    window.alert("There was an error deleting your account: ".conat(errorMessage));
                });
            });
        }

        //hides label objects and edit buttons to show save and cancel buttons with text boxes
        accountDashboardVM.startEditing = function (data, event) {
            let senderElementID = event.target.id;

            let informationSection = senderElementID.replace("btn-", "");

            $(`.${informationSection}-active`).show();
            $(`.${informationSection}-inactive`).hide();
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

            $(`.${informationSection}-active`).hide();
            $(`.${informationSection}-inactive`).show();

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

        return accountDashboardVM;
    },

    template: require('./account-dashboard-component.html')
});

//Gets a user's info
function getUserInfo() {
    return $.ajax(`${userInfoControllerRoot}/GetUserInfo`, {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//Gets a user's next transaction details
function getNextTransactionForUser() {
    return $.ajax(`${paymentControllerRoot}/GetNextPaymentForUser`, {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//Gets a user's transaction details
function getAllTransactionsForUser() {
    return $.ajax(`${paymentControllerRoot}/GetAllTransactionsForUser`, {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//GETs a user's payment spring information
function getPaymentSpringInfo() {
    return $.ajax(`${billingControllerRoot}/GetPaymentInfoForUser`, {
        method: "GET",
        beforeSend: utility.attachJwtTokenToRequest
    });
}

//POSTs a user to be deleted
function deleteUser(user) {
    return $.ajax(`${userInfoControllerRoot}/DeleteUser`, {
        method: "POST",
        data: user,
        beforeSend: utility.attachJwtTokenToRequest
    });
}
