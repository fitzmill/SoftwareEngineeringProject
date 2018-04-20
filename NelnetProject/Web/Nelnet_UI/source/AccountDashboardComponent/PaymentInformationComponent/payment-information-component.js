var utility = require('../../utility.js');

const billingControllerRoot = "/api/billing";

ko.components.register('payment-information-component', {
    viewModel: function (params) {
        var vm = this;

        vm.cardNumber = ko.observable();
        vm.expirationYear = ko.observable();
        vm.expirationMonth = ko.observable();
        vm.cardType = ko.observable();

        vm.paymentInfo = params.paymentInfo;

        vm.paymentPlan = params.paymentPlan;

        vm.nextPaymentDate = params.nextPaymentDate;
        vm.nextPaymentCost = params.nextPaymentCost;

        vm.setUIPaymentInfo = function () {
            vm.cardNumber(vm.paymentInfo().CardNumber);
            vm.expirationYear(vm.paymentInfo().ExpirationYear);
            vm.expirationMonth(vm.paymentInfo().ExpirationMonth);
            vm.cardType(vm.paymentInfo().CardType);
        }

        vm.paymentInfo.subscribe(vm.setUIPaymentInfo);

        //Changes the payment info in payment spring and ui to what the user entered.
        vm.updatePaymentInfo = function (data, event) {
            if ($("#edit-payment-form").valid()) {

                //disable cancel and save buttons while request loads
                $("#btn-save-edit-payment").attr('disabled', 'disabled');
                $("#btn-cancel-edit-payment").attr('disabled', 'disabled');

                let changedCardInfo = {
                    CustomerID: vm.paymentInfo().CustomerID,
                    CardNumber: vm.cardNumber(),
                    ExpirationYear: vm.expirationYear(),
                    ExpirationMonth: vm.expirationMonth()
                };

                updatePaymentCardInfo(changedCardInfo).done(function () {
                    //reset it to last few digits
                    vm.paymentInfo().CardNumber = changedCardInfo.CardNumber.substring(changedCardInfo.CardNumber.length - 4).replace(/[0]*/, "");
                    vm.paymentInfo().ExpirationYear = changedCardInfo.ExpirationYear;
                    vm.paymentInfo().ExpirationMonth = changedCardInfo.ExpirationMonth;

                    vm.cardNumber(vm.paymentInfo().CardNumber);

                    //UI will be updated here
                    params.stopEditing(data, event);
                }).fail(function (jqXHR) {
                    if (jqXHR.status !== 401) {
                        changedvm.paymentInfo = undefined;
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

        return vm;
    },

    template: require('./payment-information-component.html')
});

//POSTs any changes to the credit card info
function updatePaymentCardInfo(paymentCardInfo) {
    return $.ajax(`${billingControllerRoot}/UpdatePaymentCardInfo`, {
        method: "POST",
        data: paymentCardInfo,
        beforeSend: utility.attachJwtTokenToRequest
    });
}
