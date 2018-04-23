using Core;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Engines.Utils;
using System.Collections.Generic;

namespace Engines
{
    public class StudentEngine : IStudentEngine
    {
        private readonly IStudentAccessor _studentAccessor;

        public StudentEngine(IStudentAccessor studentAccessor)
        {
            _studentAccessor = studentAccessor;
        }

        public void DeleteStudentInfo(IEnumerable<int> studentIDs)
        {
            EngineArgumentValidation.ArgumentIsNotNull(studentIDs, "Students");
            foreach (int id in studentIDs)
            {
                //Validate all ids before deleting any of them
                EngineArgumentValidation.ArgumentIsNonNegative(id, "StudentID");
            }

            foreach (int id in studentIDs)
            {
                _studentAccessor.DeleteStudentInfoByStudentID(id);
            }
        }

        public Student GetStudentInfoByID(int studentID)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(studentID, "StudentID");
            return _studentAccessor.GetStudentInfoByID(studentID);
        }

        public IEnumerable<Student> GetStudentInfoByUserID(int userID)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(userID, "UserID");
            return _studentAccessor.GetStudentInfoByUserID(userID);
        }

        public void InsertStudentInfo(int userID, IEnumerable<Student> students)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(userID, "UserID");
            EngineArgumentValidation.ArgumentIsNotNull(students, "Students");
            foreach (Student student in students)
            {
                EngineArgumentValidation.ArgumentIsNotNull(student, "Student");
            }

            foreach (Student student in students)
            {
                _studentAccessor.InsertStudentInfo(userID, student);
            }
        }

        public void UpdateStudentInfo(IEnumerable<Student> students)
        {
            EngineArgumentValidation.ArgumentIsNotNull(students, "Students");
            foreach (Student student in students)
            {
                EngineArgumentValidation.ArgumentIsNotNull(student, "Student");
            }

            foreach (Student student in students)
            {
                _studentAccessor.UpdateStudentInfo(student);
            }
        }
    }
}
