//some helper functions

//Sums up one property of every object in a list
Array.prototype.sumProperty = function (prop) {
    let total = 0;
    for (let i = 0; i < this.length; i++) {
        total += this[i][prop];
    }
    return total;
};

//creates a csv string that contains every object in the array with the objects' properties names as the column headers
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

//downloads the string as a CSV file
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

String.prototype.parseDateTimeString = function () {
    let dateArray = this.split('-');
    let year = dateArray[0];
    let month = dateArray[1];
    let day = dateArray[2].substring(0, 2);
    return month + "/" + day + "/" + year;
}

Number.prototype.formatAsMoney = function () {
    return this.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

//add the option to bind events to the show and hide events
//from http://viralpatel.net/blogs/jquery-trigger-custom-event-show-hide-element/
$.each(['show', 'hide'], function (i, ev) {
    var el = $.fn[ev];
    $.fn[ev] = function () {
        this.trigger(ev);
        return el.apply(this, arguments);
    };
});

$.validator.addMethod("password", function (value) {
    return value.match(/^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-\.])(?!.*?[;'"]).{8,32}$/);
}, "Password must be between 8 and 32 characters, contain lowercase and uppercase letters, a number, and a special character.");
$.validator.addMethod("no-code-chars", function (value) {
    return value.match(/^((?![<>\\\/{};\[\]]).)*$/);
}, "Contains an invalid character.");

// https://jqueryvalidation.org/creditcard-method/
// based on https://en.wikipedia.org/wiki/Luhn_algorithm
$.validator.addMethod("creditcard", function (value, element) {
    if (this.optional(element)) {
        return "dependency-mismatch";
    }

    // Accept only spaces, digits and dashes
    if (/[^0-9 \-]+/.test(value)) {
        return false;
    }

    var nCheck = 0,
        nDigit = 0,
        bEven = false,
        n, cDigit;

    value = value.replace(/\D/g, "");

    // Basing min and max length on
    // https://dev.ean.com/general-info/valid-card-types/
    if (value.length < 13 || value.length > 19) {
        return false;
    }

    for (n = value.length - 1; n >= 0; n--) {
        cDigit = value.charAt(n);
        nDigit = parseInt(cDigit, 10);
        if (bEven) {
            if ((nDigit *= 2) > 9) {
                nDigit -= 9;
            }
        }

        nCheck += nDigit;
        bEven = !bEven;
    }

    return (nCheck % 10) === 0;
}, "Please enter a valid credit card number.");

module.exports.attachJwtTokenToRequest = function(jqXHR) {
    jqXHR.setRequestHeader("Authorization", "Bearer " + window.sessionStorage.getItem("Jwt"));
};