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
        public List<User> _mockUserTable;
        public List<Student> _mockStudentTable;

        public MockSetUserInfoAccessor(List<User> mockUserTable, List<Student> mockStudentTable)
        {
            _mockUserTable = mockUserTable;
            _mockStudentTable = mockStudentTable;
        }

        public void DeletePersonalInfo(int userID)
        {
            _mockUserTable.RemoveAll(u => u.UserID == userID);
        }

        public void DeleteStudentInfoByStudentID(int studentID)
        {
            _mockStudentTable.RemoveAll(s => s.StudentID == studentID);
        }

        public void DeleteStudentInfoByUserID(int userID)
        {
            User user = _mockUserTable.SingleOrDefault(u => u.UserID == userID);
            _mockStudentTable.RemoveAll(s => user.Students.Contains(s));
        }

        public void InsertPersonalInfo(User user)
        {
            try
            {
                int maxID = _mockUserTable.Select(u => u.UserID).ToList().Max();
                user.UserID = maxID + 1;
            }
            catch (InvalidOperationException)
            {
                user.UserID = 1;
            }
            _mockUserTable.Add(user);
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            try
            {
                int maxID = _mockStudentTable.Select(s => s.StudentID).ToList().Max();
                student.StudentID = maxID + 1;
            }
            catch (InvalidOperationException)
            {
                student.StudentID = 1;
            }
            _mockStudentTable.Add(student);
        }

        public void UpdatePersonalInfo(User user)
        {
            _mockUserTable.RemoveAll(u => u.UserID == user.UserID);
            _mockUserTable.Add(user);
        }

        public void UpdateStudentInfo(Student student)
        {
            _mockStudentTable.RemoveAll(s => s.StudentID == student.StudentID);
            _mockStudentTable.Add(student);
        }
    }
}
