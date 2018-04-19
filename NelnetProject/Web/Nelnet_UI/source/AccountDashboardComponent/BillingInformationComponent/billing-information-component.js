require('./billing-information-component.scss');

const utility = require('../../utility.js');

const billingControllerRoot = "/api/billing";

ko.components.register('billing-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.billingInfo = params.billingInfo;

        vm.CardFirstName = ko.observable();
        vm.CardLastName = ko.observable();
        vm.StreetAddress1 = ko.observable();
        vm.StreetAddress2 = ko.observable();
        vm.City = ko.observable();
        vm.State = ko.observable();
        vm.Zip = ko.observable();

        vm.setUIBillingInfo = function () {
            vm.CardFirstName(vm.billingInfo().FirstName);
            vm.CardLastName(vm.billingInfo().LastName);
            vm.StreetAddress1(vm.billingInfo().StreetAddress1);
            vm.StreetAddress2(vm.billingInfo().StreetAddress2);
            vm.City(vm.billingInfo().City);
            vm.State(vm.billingInfo().State);
            vm.Zip(vm.billingInfo().Zip);
        };

        vm.billingInfo.subscribe(vm.setUIBillingInfo);

        vm.updateBillingInfo = function (data, event) {
            if ($("#edit-billing-form").valid()) {

                //disable cancel and save buttons while request loads
                $("#btn-save-edit-billing").attr('disabled', 'disabled');
                $("#btn-cancel-edit-billing").attr('disabled', 'disabled');

                let changedBillingInfo = {
                    CustomerID: vm.billingInfo().CustomerID,
                    FirstName: vm.CardFirstName(),
                    LastName: vm.CardLastName(),
                    StreetAddress1: vm.StreetAddress1(),
                    StreetAddress2: vm.StreetAddress2(),
                    City: vm.City(),
                    State: vm.State(),
                    Zip: vm.Zip()
                };

                updatePaymentBillingInfo(changedBillingInfo).done(function () {
                    vm.billingInfo().FirstName = changedBillingInfo.FirstName;
                    vm.billingInfo().LastName = changedBillingInfo.LastName;
                    vm.billingInfo().StreetAddress1 = changedBillingInfo.StreetAddress1;
                    vm.billingInfo().StreetAddress2 = changedBillingInfo.StreetAddress2;
                    vm.billingInfo().City = changedBillingInfo.City;
                    vm.billingInfo().State = changedBillingInfo.State;
                    vm.billingInfo().Zip = changedBillingInfo.Zip;

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
    return $.ajax(`${billingControllerRoot}/UpdatePaymentBillingInfo`, {
        method: "POST",
        data: paymentBillingInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}
