using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Student
    {
        public int StudentID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //0: Kindergarten
        public int Grade { get; set; }

        public override bool Equals(object obj)
        {
            var student = obj as Student;
            return student != null &&
                   StudentID == student.StudentID &&
                   FirstName == student.FirstName &&
                   LastName == student.LastName &&
                   Grade == student.Grade;
        }
    }
}
