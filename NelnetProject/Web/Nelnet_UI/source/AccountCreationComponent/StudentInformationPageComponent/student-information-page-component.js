ko.components.register('student-information-page-component', {
    viewModel: function (params) {
        let vm = this;
        
        vm.students = params.students;

        vm.addStudent = function () {
            vm.students.push({
                studentID: 0,
                studentFirstName: ko.observable(),
                studentLastName: ko.observable(),
                studentGrade: ko.observable()
            });
            if (vm.students().length > 1) {
                $(".btn-remove-student").attr("disabled", false);
            }
        };

        vm.removeStudent = function (student) {
            if (vm.students().length > 1) {
                let removeIndex = vm.students().findIndex(s => {
                    return s.studentFirstName === student.studentFirstName &&
                        s.studentLastName === student.studentLastName &&
                        s.studentGrade === student.studentGrade;
                });
                vm.students.splice(removeIndex, 1);
            }
            if (vm.students().length <= 1) {
                $(".btn-remove-student").attr("disabled", true);
            }
        };

        $(".btn-remove-student").attr("disabled", true);

        return vm;
    },

    template: require('./student-information-page-component.html')
});

//runs the validation on this page and advances to the next if all information is properly validated
module.exports.advancePageIfValid = function (advancePage) {
    if ($("#form-student-information-page").valid()) {
        advancePage();
    }
}