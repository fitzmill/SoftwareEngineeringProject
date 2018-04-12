exports.Months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

exports.Years = (function () {
    let years = [];
    let startYear = (new Date()).getFullYear();
    for (let i = 0; i < 99; i++) {
        years.push(startYear + i);
    }
    return years;
})();

exports.GradeNumbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

exporst.GradeTexts = ["Kindergarten", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th", "11th", "12th"];
