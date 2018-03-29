require('./account-dashboard-component.scss');

const accountDashboardAPIURL = "/api/account-dashboard";
const user = "Need To Implement";

//gets a user's payment spring information
function getPaymentSpringInfo(userID) {
    let result = undefined;
    let userIDString = userID.toString();

    $.ajax(accountDashboardAPIURL + "GetPaymentInfoForUser/" + userIDString, {
        method: "GET",
        async: false,
        success: function (data) {
            result = data;
        },
        error: function (jqXHR) {
            window.alert("Could not generate all user information. Please refresh the page.");
        }
    });
    return result;
}

ko.components.register('account-dashboard-component', {
    viewModel: function (params) {
        let accountDashboardVM = this;

        accountDashboardVM.Company = ko.observable();
        accountDashboardVM.FirstName = ko.observable();
        accountDashboardVM.LastName = ko.observable();
        accountDashboardVM.StreetAddress1 = ko.observable();
        accountDashboardVM.StreetAddress2 = ko.observable();
        accountDashboardVM.City = ko.observable();
        accountDashboardVM.State = ko.observable();
        accountDashboardVM.Zip = ko.observable();
        accountDashboardVM.CardNumber = ko.observable();
        accountDashboardVM.ExpirationYear = ko.observable();
        accountDashboardVM.ExpirationMonth = ko.observable();
        accountDashboardVM.CardType = ko.observable();

        return accountDashboardVM;
    },

    template: require('./account-dashboard-component.html')
});