using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class Student
    {
        [Range(0, int.MaxValue)]
        public int StudentID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        //0: Kindergarten
        [Range(0, 12)]
        public int Grade { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   StudentID == student.StudentID &&
                   FirstName == student.FirstName &&
                   LastName == student.LastName &&
                   Grade == student.Grade;
        }

        public override int GetHashCode()
        {
            var hashCode = 1234312133;
            hashCode = hashCode * -1521134295 + StudentID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + Grade.GetHashCode();
            return hashCode;
        }
    }
}
