require('./account-creation-component.scss');
require('../assets/background-image.scss');

//constants
const START_PAGE = 1;
const END_PAGE = 4;

ko.components.register('account-creation-component', {
    viewModel: function (params) {
        let vm = this;

        //first page
        vm.firstName = ko.observable();
        vm.lastName = ko.observable();
        vm.email = ko.observable();
        vm.reenterEmail = ko.observable();
        vm.password = ko.observable();
        vm.reenterPassword = ko.observable();

        //second page
        vm.cardNumber = ko.observable();
        vm.csc = ko.observable();
        vm.cardFirstName = ko.observable();
        vm.cardLastName = ko.observable();
        vm.month = ko.observable();
        vm.year = ko.observable();
        vm.address1 = ko.observable();
        vm.address2 = ko.observable();
        vm.city = ko.observable();
        vm.state = ko.observable();
        vm.zip = ko.observable();

        //third page
        vm.students = ko.observableArray([{
            studentFirstName: ko.observable(),
            studentLastName: ko.observable(),
            studentGrade: ko.observable()
        }]);

        vm.addStudent = function () {
            vm.students.push({
                studentID: 0,
                studentFirstName: ko.observable(),
                studentLastName: ko.observable(),
                studentGrade: ko.observable()
            });
        };

        //fourth page
        vm.paymentType = ko.observable();
        vm.calcYearlyRate = function () {
            return 2640;
        };

        //keep track of page state
        vm.currentPage = START_PAGE;

        //create function to update progress bar
        vm.updateProgressBar = function () {
            for (let i = START_PAGE; i < END_PAGE; i++) {
                if (i >= vm.currentPage) {
                    $('#rectangle-' + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $('#rectangle-' + i)[0].style.backgroundColor = "#007bff";
                }
            }

            for (let i = START_PAGE; i <= END_PAGE; i++) {
                if (i > vm.currentPage) {
                    $('#dot-page-' + i)[0].style.backgroundColor = "#afafaf";
                } else {
                    $('#dot-page-' + i)[0].style.backgroundColor = "#007bff";
                }
            }
        };

        //create function to update buttons
        vm.updateButtons = function () {
            if (vm.currentPage === START_PAGE) {
                $("#btn-cancel").show();
                $("#btn-back").hide();
            } else {
                $("#btn-cancel").hide();
                $("#btn-back").show();
            }
            if (vm.currentPage === END_PAGE) {
                $("#btn-done").show();
                $("#btn-next").hide();
            } else {
                $("#btn-done").hide();
                $("#btn-next").show();
            }
        };

        //exit account creation
        vm.cancel = function () {
            window.location = '#';
        };

        //move to the previous page
        vm.back = function () {
            $("#info-page-" + vm.currentPage).hide();
            vm.currentPage--;
            $("#info-page-" + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        };

        //move to the next page
        vm.next = function () {
            $("#info-page-" + vm.currentPage).hide();
            vm.currentPage++;
            $("#info-page-" + vm.currentPage).show();
            vm.updateButtons();
            vm.updateProgressBar();
        };

        //finish and create account
        vm.done = function () {
            //TODO
        };

        //show current page and correct buttons
        for (let i = START_PAGE; i <= END_PAGE; i++) {
            if (i === vm.currentPage) {
                $("#info-page-" + i).show();
            } else {
                $("#info-page-" + i).hide();
            }
        }
        vm.updateButtons();
        vm.updateProgressBar();

        return vm;
    },

    template: require('./account-creation-component.html')
});