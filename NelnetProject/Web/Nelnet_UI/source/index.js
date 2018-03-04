﻿(function () {
    require('./index.scss');
    document.getElementById("app").insertAdjacentHTML('beforeend', require('./index.html'));
    // Require each component that needs to be loaded here
    require('./HelloWorldComponent/hello-world-component.js');
    require('./ReportComponent/report-component.js');

    //some helper functions
    Array.prototype.sumProperty = function (prop) {
        let total = 0;
        for (let i = 0; i < this.length; i++) {
            total += this[i][prop];
        }
        return total;
    };

    Array.prototype.createCSVString = function () {
        let keys = Object.keys(this[0]);

        let result = keys.join(',');
        result += "\n";

        for (let i = 0; i < this.length; i++) {
            for (let j = 0; j < keys.length; j++) {
                //separate it from last value
                if (j > 0) result += ",";

                let val = this[i][keys[j]];
                if (typeof val === "string") {
                    //gets rid of commas that would mess up csv
                    result += val.replace(/,/, '');
                } else {
                    result += val;
                }
            }
            result += "\n";
        }
        return result;
    };

    String.prototype.downloadCSV = function (filename) {
        let csv = this;
        if (!csv.match(/^data:text\/csv/i)) {
            csv = 'data:text/csv;charset=utf-8,' + csv;
        }

        let data = encodeURI(csv);

        let link = document.createElement('a');
        link.setAttribute('href', data);
        link.setAttribute('download', filename);
        link.click();
    };


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