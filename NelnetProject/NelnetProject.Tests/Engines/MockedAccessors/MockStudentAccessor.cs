using Core;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockStudentAccessor : IStudentAccessor
    {
        public List<Student> MockDb;

        public MockStudentAccessor(List<Student> students)
        {
            MockDb = students;
        }

        public void DeleteStudentInfoByStudentID(int studentID)
        {
            MockDb.RemoveAll(x => x.StudentID == studentID);
        }

        public void DeleteStudentInfoByUserID(int userID)
        {
            MockDb.RemoveAll(x => x.UserID == userID);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return MockDb;
        }

        public Student GetStudentInfoByID(int studentID)
        {
            return MockDb.FirstOrDefault(x => x.StudentID == studentID);
        }

        public IEnumerable<Student> GetStudentInfoByUserID(int userID)
        {
            return MockDb.Where(x => x.UserID == userID);
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            student.UserID = userID;
            student.StudentID = MockDb.Count > 0 ? MockDb.Select(x => x.StudentID).Max() + 1 : 1;
            MockDb.Add(student);
        }

        public void UpdateStudentInfo(Student student)
        {
            var index = MockDb.IndexOf(student);
            MockDb.RemoveAll(x => x.StudentID == student.StudentID);
            MockDb.Insert(index, student);
        }
    }
}
