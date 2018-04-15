﻿require('pagerjs');

var index = (function () {
    require('./index.scss');
    document.getElementById("app").insertAdjacentHTML('beforeend', require('./index.html'));
    // Require each component that needs to be loaded here
    require('./utility.js');
    
    return {
        dropdownOptions: require('./dropdownOptions.js'),
        loginComponent: require('./LoginComponent/login-component.js'),
        adminComponent: require('./AdminComponent/admin-component.js'),
        accountCreationComponent: require('./AccountCreationComponent/account-creation-component.js'),
        accountDashboardComponent: require('./AccountDashboardComponent/account-dashboard-component.js'),
        logout: function () {
            window.sessionStorage.removeItem("Jwt");
            window.location = "#";
        }
    };
})();

// extend your view-model with pager.js specific data
pager.extendWithPage(index);
// apply the view-model using KnockoutJS as normal
ko.applyBindings(index);
// start pager.js
pager.start();