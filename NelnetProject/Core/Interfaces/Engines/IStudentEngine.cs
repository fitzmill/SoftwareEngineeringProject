using System.Collections.Generic;

namespace Core.Interfaces.Engines
{
    /// <summary>
    /// Business logic surrounding the Student object
    /// </summary>
    public interface IStudentEngine
    {
        /// <summary>
        /// Gets a student's info by their ID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        Student GetStudentInfoByID(int studentID);

        /// <summary>
        /// Gets all students associated with a user
        /// </summary>
        /// <param name="userID">The userID associated with students</param>
        /// <returns></returns>
        IEnumerable<Student> GetStudentInfoByUserID(int userID);

        /// <summary>
        /// Insert new student records into the database
        /// </summary>
        /// <param name="userID">The user to add the student records too</param>
        /// <param name="students">The student records to be inserted</param>
        void InsertStudentInfo(int userID, IEnumerable<Student> students);

        /// <summary>
        /// Update student records in the database
        /// </summary>
        /// <param name="students">The student records to update</param>
        void UpdateStudentInfo(IEnumerable<Student> students);

        /// <summary>
        /// Delete the student from the database
        /// </summary>
        /// <param name="studentIDs">The ids of the students to delete</param>
        void DeleteStudentInfo(IEnumerable<int> studentIDs);
    }
}
