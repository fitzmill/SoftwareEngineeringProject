require('./student-information-component.scss');

const utility = require('../../utility.js');

//api url constants
const userInfoControllerRoot = "/api/userinfo";
const paymentControllerRoot = "/api/payment";
const billingControllerRoot = "/api/billing";

var localStudents = undefined;

ko.components.register("student-information-component", {
    viewModel: function (params) {
        var vm = this;

        vm.students = ko.observableArray([]);

        localStudents = params.students;

        vm.setUIStudentInfo = function () {
            vm.students(localStudents().map((student) => {
                return {
                    studentID: student.StudentID,
                    firstName: ko.observable(student.FirstName),
                    lastName: ko.observable(student.LastName),
                    grade: ko.observable(student.Grade)
                };
            }));
        }

        localStudents.subscribe(vm.setUIStudentInfo);

        //Changes student info in database and ui to what user entered
        vm.updateStudents = function (data, event) {
            if ($("#edit-students-form").valid()) {

                //disable save and edit buttons
                $("#btn-save-edit-student").attr("disabled", "disabled");
                $("#btn-cancel-edit-student").attr("disabled", "disabled");

                //to return to normal data instead of observables
                let inputStudents = vm.students().map(student => {
                    return {
                        StudentID: student.studentID,
                        FirstName: student.firstName(),
                        LastName: student.lastName(),
                        Grade: student.grade()
                    };
                });

                //new students will have an undefined StudentID
                let newStudents = inputStudents.filter((s) => !s.StudentID);
                //deleted students will be in the user object but not in inputStudents
                let originalStudentIDs = localStudents().map((s) => s.StudentID);
                let inputStudentIDs = inputStudents.map((s) => s.StudentID);

                let deletedStudentIDs = originalStudentIDs.filter((id) => !inputStudentIDs.includes(id));
                //filter out new students
                let updatedStudents = inputStudents.filter((s) => s.StudentID);

                updateStudentInfo(updatedStudents, deletedStudentIDs, newStudents).done(function () {
                    //update user in local storage in the case of page reload
                    localStudents(inputStudents);
                    params.stopEditing(data, event);
                }).fail(function (jqXHR) {
                    if (jqXHR.status !== 401) {
                        let errorMessage = JSON.parse(jqXHR.responseText).Message;
                        window.alert("Could not save information: ".concat(errorMessage));
                    }
                }).always(function () {
                    //re-enable buttons
                    $("#btn-save-edit-student").removeAttr("disabled");
                    $("#btn-cancel-edit-student").removeAttr("disabled");
                });
            }
        };

        vm.addStudent = function () {
            vm.students.push({
                studentID: undefined,
                firstName: ko.observable(),
                lastName: ko.observable(),
                grade: ko.observable()
            });
            //by default the new student will show with labels and not text boxes
            $(".edit-student-active").show();
            $(".edit-student-inactive").hide();
        };

        vm.deleteStudent = function (student) {
            if (vm.students().length === 1) {
                params.deleteUser();
            } else {
                vm.students(vm.students().filter((s) => {
                    return s.studentID !== student.studentID || s.firstName() !== student.firstName() || s.lastName() !== student.lastName();
                }));
            }
        };
    },

    template: require('./student-information-component.html')
});

//POSTs any updates to a user's list of students
function updateStudentInfo(updatedStudents, deletedStudentIDs, newStudents) {
    //JSON is used here because empty arrays get cast to undefined if not using JSON
    //undefined is obviously very different from an empty array, so JSON is used to avoid that
    let jsonData = JSON.stringify({
        UpdatedStudents: updatedStudents,
        DeletedStudentIDs: deletedStudentIDs,
        AddedStudents: newStudents
    });
    return $.ajax(accountDashboardAPIURL + "/UpdateStudentInfo", {
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData,
        beforeSend: utility.attachJwtTokenToRequest
    });
}
