require('./billing-information-component.scss');

const utility = require('../utility.js');

const accountDashboardAPIURL = "/api/account";

var billingInfo = undefined;

ko.components.register("billing-information-component", {
    viewModel = function (params) {
        var vm = this;

        billingInfo = params.billingInfo;

        accountDashboardVM.CardFirstName = ko.observable();
        accountDashboardVM.CardLastName = ko.observable();
        accountDashboardVM.StreetAddress1 = ko.observable();
        accountDashboardVM.StreetAddress2 = ko.observable();
        accountDashboardVM.City = ko.observable();
        accountDashboardVM.State = ko.observable();
        accountDashboardVM.Zip = ko.observable();

        accountDashboardVM.setUIBillingInfo = function () {
            accountDashboardVM.CardFirstName(userPaymentInfo.FirstName);
            accountDashboardVM.CardLastName(userPaymentInfo.LastName);
            accountDashboardVM.StreetAddress1(userPaymentInfo.StreetAddress1);
            accountDashboardVM.StreetAddress2(userPaymentInfo.StreetAddress2);
            accountDashboardVM.City(userPaymentInfo.City);
            accountDashboardVM.State(userPaymentInfo.State);
            accountDashboardVM.Zip(userPaymentInfo.Zip);
        };

        accountDashboardVM.updateBillingInfo = function (data, event) {
            if ($("#edit-billing-form").valid()) {

                //disable cancel and save buttons while request loads
                $("#btn-save-edit-billing").attr('disabled', 'disabled');
                $("#btn-cancel-edit-billing").attr('disabled', 'disabled');

                let changedBillingInfo = {
                    CustomerID: billingInfo.CustomerID,
                    FirstName: accountDashboardVM.CardFirstName(),
                    LastName: accountDashboardVM.CardLastName(),
                    StreetAddress1: accountDashboardVM.StreetAddress1(),
                    StreetAddress2: accountDashboardVM.StreetAddress2(),
                    City: accountDashboardVM.City(),
                    State: accountDashboardVM.State(),
                    Zip: accountDashboardVM.Zip()
                };

                updatePaymentBillingInfo(changedBillingInfo).done(function () {
                    userPaymentInfo.FirstName = changedBillingInfo.FirstName;
                    userPaymentInfo.LastName = changedBillingInfo.LastName;
                    userPaymentInfo.StreetAddress1 = changedBillingInfo.StreetAddress1;
                    userPaymentInfo.StreetAddress2 = changedBillingInfo.StreetAddress2;
                    userPaymentInfo.City = changedBillingInfo.City;
                    userPaymentInfo.State = changedBillingInfo.State;
                    userPaymentInfo.Zip = changedBillingInfo.Zip;

                    //UI will be updated here
                    accountDashboardVM.stopEditing(data, event);
                }).fail(function (jqXHR) {
                    if (jqXHR.status !== 401) {
                        let errorMessage = JSON.parse(jqXHR.responseText).Message;
                        window.alert("Could not save information: ".concat(errorMessage));
                    }
                }).always(function () {
                    //re-enable buttons
                    $("#btn-save-edit-billing").removeAttr('disabled');
                    $("#btn-cancel-edit-billing").removeAttr('disabled');
                });
            }
        }

        return vm;
    },

    template: require('./billing-information-component.html')
});

//POSTs any changes to the user's billing address
function updatePaymentBillingInfo(paymentBillingInfo) {
    return $.ajax(accountDashboardAPIURL + "/UpdatePaymentBillingInfo", {
        method: "POST",
        data: paymentBillingInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}
