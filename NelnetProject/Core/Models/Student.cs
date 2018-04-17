using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    /// <summary>
    /// Model for individual students
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Student's ID
        /// </summary>
        [Range(0, int.MaxValue)]
        public int StudentID { get; set; }

        /// <summary>
        /// Student's First Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        /// <summary>
        /// Student's Last Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        /// <summary>
        /// Students Grade Level (0 is Kindergarten)
        /// </summary>
        [Range(0, 12)]
        public int Grade { get; set; }
    }
}
