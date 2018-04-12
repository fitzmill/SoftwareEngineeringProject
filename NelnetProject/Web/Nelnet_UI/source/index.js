﻿require('pagerjs');

var index = (function () {
    require('./index.scss');
    document.getElementById("app").insertAdjacentHTML('beforeend', require('./index.html'));
    // Require each component that needs to be loaded here
    require('./utility.js');
    
    return {
        loginComponent: require('./LoginComponent/login-component.js'),
        adminComponent: require('./AdminComponent/admin-component.js'),
        accountCreationComponent: require('./AccountCreationComponent/account-creation-component.js'),
        accountDashboardComponent: require('./AccountDashboardComponent/account-dashboard-component.js'),
        dropdownOptions: require('./DropdownOptions.js')
    };
})();

// extend your view-model with pager.js specific data
pager.extendWithPage(index);
// apply the view-model using KnockoutJS as normal
ko.applyBindings(index);
// start pager.js
pager.start();