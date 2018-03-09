using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    class MockSetUserInfoAccessor : ISetUserInfoAccessor
    {
        public List<Student> mockStudentTable = new List<Student>()
        {
            new Student()
            {
                StudentID = 1,
                FirstName = "Lucas",
                LastName = "Hall",
                Grade = 9
            },

            new Student()
            {
                StudentID = 2,
                FirstName = "Joe",
                LastName = "Cowboy",
                Grade = 4
            }
        };

        public List<User> mockUserTable = new List<User>()
        {
            new User()
            {
                UserID = 1,
                FirstName = "George",
                LastName = "Curious",
                Email = "curious.george@gmail.com",
                Hashed = "asdfjkl",
                Salt = "salt",
                PaymentPlan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "abc123",
                Students = new List<Student>()
            }
        };

        public MockSetUserInfoAccessor()
        {
            mockUserTable.ElementAt(0).Students.Add(mockStudentTable.ElementAt(0));
            mockUserTable.ElementAt(0).Students.Add(mockStudentTable.ElementAt(1));
        }

        public void DeletePersonalInfo(int userID)
        {
            mockUserTable.RemoveAll(u => u.UserID == userID);
        }

        public void DeleteStudentInfoByStudentID(int studentID)
        {
            mockStudentTable.RemoveAll(s => s.StudentID == studentID);
        }

        public void DeleteStudentInfoByUserID(int userID)
        {
            User user = mockUserTable.SingleOrDefault(u => u.UserID == userID);
            mockStudentTable.RemoveAll(s => user.Students.Contains(s));
        }

        public void InsertPersonalInfo(User user)
        {
            try
            {
                int maxID = mockUserTable.Select(u => u.UserID).ToList().Max();
                user.UserID = maxID + 1;
            }
            catch (InvalidOperationException)
            {
                user.UserID = 1;
            }
            mockUserTable.Add(user);
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            try
            {
                int maxID = mockStudentTable.Select(s => s.StudentID).ToList().Max();
                student.StudentID = maxID + 1;
            }
            catch (InvalidOperationException)
            {
                student.StudentID = 1;
            }
            mockStudentTable.Add(student);
        }

        public void UpdatePersonalInfo(User user)
        {
            mockUserTable.RemoveAll(u => u.UserID == user.UserID);
            mockUserTable.Add(user);
        }

        public void UpdateStudentInfo(Student student)
        {
            mockStudentTable.RemoveAll(s => s.StudentID == student.StudentID);
            mockStudentTable.Add(student);
        }
    }
}
