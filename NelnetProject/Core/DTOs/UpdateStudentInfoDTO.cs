using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    /// <summary>
    /// Contains information about what student objects need to be updated
    /// </summary>
    public class UpdateStudentInfoDTO
    {
        /// <summary>
        /// The UserID that the students are associated with
        /// </summary>
        [Range(0, int.MaxValue)]
        public int UserID { get; set; }

        /// <summary>
        /// List of students that have only had their properties updated
        /// </summary>
        [Required]
        public List<Student> UpdatedStudents { get; set; }

        /// <summary>
        /// List of StudentIDs that have been deleted by the user
        /// </summary>
        [Required]
        public List<int> DeletedStudentIDs { get; set; }

        /// <summary>
        /// New students that have been added to the user
        /// </summary>
        [Required]
        public List<Student> AddedStudents { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as UpdateStudentInfoDTO;
            return dTO != null &&
                   UserID == dTO.UserID &&
                   EqualityComparer<List<Student>>.Default.Equals(UpdatedStudents, dTO.UpdatedStudents) &&
                   EqualityComparer<List<int>>.Default.Equals(DeletedStudentIDs, dTO.DeletedStudentIDs) &&
                   EqualityComparer<List<Student>>.Default.Equals(AddedStudents, dTO.AddedStudents);
        }

        public override int GetHashCode()
        {
            var hashCode = 553243157;
            hashCode = hashCode * -1521134295 + UserID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Student>>.Default.GetHashCode(UpdatedStudents);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(DeletedStudentIDs);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Student>>.Default.GetHashCode(AddedStudents);
            return hashCode;
        }
    }
}
