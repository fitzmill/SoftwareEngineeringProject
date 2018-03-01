(function () {
    require('./index.scss');
    document.getElementById("app").insertAdjacentHTML('beforeend', require('./index.html'));
    // Require each component that needs to be loaded here
    require('./HelloWorldComponent/hello-world-component.js');
    require('./ReportComponent/report-component.js');

    //function PagerViewModel() {
    //    var self = this;
    //}

    //var vm = new PagerViewModel();
    //// use HTML5 history
    //pager.useHTML5history = true;
    //// use History instead of history
    //pager.Href5.history = History;
    //// extend your view-model with pager.js specific data
    //pager.extendWithPage(vm);
    //// apply the view-model using KnockoutJS as normal
    //ko.applyBindings(vm);
    //// start pager.js
    //pager.startHistoryJs();
    ko.applyBindings();
})();