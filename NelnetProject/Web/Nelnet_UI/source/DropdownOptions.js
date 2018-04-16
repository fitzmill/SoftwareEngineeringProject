exports.Months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

exports.Years = (function () {
    let years = [];
    let startYear = (new Date()).getFullYear();
    for (let i = 0; i < 99; i++) {
        years.push(startYear + i);
    }
    return years;
})();

exports.Grades = [{ num: 0, text: "Kindergarten" },
    { num: 1, text: "1st" },
    { num: 2, text: "2nd" },
    { num: 3, text: "3rd" },
    { num: 4, text: "4th" },
    { num: 5, text: "5th" },
    { num: 6, text: "6th" },
    { num: 7, text: "7th" },
    { num: 8, text: "8th" },
    { num: 9, text: "9th" },
    { num: 10, text: "10th" },
    { num: 11, text: "11th" },
    { num: 12, text: "12th" }];

exports.States = ["AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI",
    "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA",
    "WA", "WV", "WI", "WY", "AS", "DC", "FM", "GU", "MH", "MP", "PW", "PR", "VI"];
