require('pagerjs');

var index = (function () {
    require('./index.scss');
    document.getElementById("app").insertAdjacentHTML('beforeend', require('./index.html'));
    // Require each component that needs to be loaded here
    require('./utility.js');
    
    return {
        helloWorldComponent: require('./HelloWorldComponent/hello-world-component.js'),
        reportComponent: require('./ReportComponent/report-component.js')
    };
})();

// extend your view-model with pager.js specific data
pager.extendWithPage(index);
// apply the view-model using KnockoutJS as normal
ko.applyBindings(index);
// start pager.js
pager.start('hello');