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

//turns c# datetime object into a more readable format
String.prototype.parseDateTimeString = function () {
    let dateArray = this.split('-');
    let year = dateArray[0];
    let month = dateArray[1];
    let day = dateArray[2].substring(0, 2);
    return month + "/" + day + "/" + year;
}