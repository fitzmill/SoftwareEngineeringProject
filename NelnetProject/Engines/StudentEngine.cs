using Core;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (int id in studentIDs)
            {
                _studentAccessor.DeleteStudentInfoByStudentID(id);
            }
        }

        public Student GetStudentInfoByID(int studentID)
        {
            return _studentAccessor.GetStudentInfoByID(studentID);
        }

        public IEnumerable<Student> GetStudentInfoByUserID(int userID)
        {
            return _studentAccessor.GetStudentInfoByUserID(userID);
        }

        public void InsertStudentInfo(int userID, IEnumerable<Student> students)
        {
            foreach(Student student in students)
            {
                _studentAccessor.InsertStudentInfo(userID, student);
            }
        }

        public void UpdateStudentInfo(IEnumerable<Student> students)
        {
            foreach(Student student in students)
            {
                _studentAccessor.UpdateStudentInfo(student);
            }
        }
    }
}
