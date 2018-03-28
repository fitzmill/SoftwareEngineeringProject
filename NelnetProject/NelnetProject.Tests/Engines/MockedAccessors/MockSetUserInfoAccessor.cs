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
        public List<User> mockUserTable;
        public List<Student> mockStudentTable;

        public MockSetUserInfoAccessor(List<User> mockUserTable, List<Student> mockStudentTable)
        {
            this.mockUserTable = mockUserTable;
            this.mockStudentTable = mockStudentTable;
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
