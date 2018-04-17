require('./billing-information-component.scss');

const utility = require('../../utility.js');

const accountDashboardAPIURL = "/api/account";

var billingInfo = undefined;

ko.components.register('billing-information-component', {
    viewModel: function (params) {
        var vm = this;

        billingInfo = params.billingInfo;

        vm.CardFirstName = ko.observable();
        vm.CardLastName = ko.observable();
        vm.StreetAddress1 = ko.observable();
        vm.StreetAddress2 = ko.observable();
        vm.City = ko.observable();
        vm.State = ko.observable();
        vm.Zip = ko.observable();

        vm.setUIBillingInfo = function () {
            vm.CardFirstName(billingInfo().FirstName);
            vm.CardLastName(billingInfo().LastName);
            vm.StreetAddress1(billingInfo().StreetAddress1);
            vm.StreetAddress2(billingInfo().StreetAddress2);
            vm.City(billingInfo().City);
            vm.State(billingInfo().State);
            vm.Zip(billingInfo().Zip);
        };

        billingInfo.subscribe(vm.setUIBillingInfo);

        vm.updateBillingInfo = function (data, event) {
            if ($("#edit-billing-form").valid()) {

                //disable cancel and save buttons while request loads
                $("#btn-save-edit-billing").attr('disabled', 'disabled');
                $("#btn-cancel-edit-billing").attr('disabled', 'disabled');

                let changedBillingInfo = {
                    CustomerID: billingInfo().CustomerID,
                    FirstName: vm.CardFirstName(),
                    LastName: vm.CardLastName(),
                    StreetAddress1: vm.StreetAddress1(),
                    StreetAddress2: vm.StreetAddress2(),
                    City: vm.City(),
                    State: vm.State(),
                    Zip: vm.Zip()
                };

                updatePaymentBillingInfo(changedBillingInfo).done(function () {
                    billingInfo().FirstName = changedBillingInfo.FirstName;
                    billingInfo().LastName = changedBillingInfo.LastName;
                    billingInfo().StreetAddress1 = changedBillingInfo.StreetAddress1;
                    billingInfo().StreetAddress2 = changedBillingInfo.StreetAddress2;
                    billingInfo().City = changedBillingInfo.City;
                    billingInfo().State = changedBillingInfo.State;
                    billingInfo().Zip = changedBillingInfo.Zip;

                    //UI will be updated here
                    params.stopEditing(data, event);
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
