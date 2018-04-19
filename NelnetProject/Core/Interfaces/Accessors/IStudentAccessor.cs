using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Accessors
{
    /// <summary>
    /// Accessor for performing CRUD operations with Student table
    /// </summary>
    public interface IStudentAccessor
    {
        /// <summary>
        /// Gets a student's info by their ID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        Student GetStudentInfoByID(int studentID);

        /// <summary>
        /// Gets all students who are associated with a userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<Student> GetStudentInfoByUserID(int userID);

        /// <summary>
        /// Gets all students in the database
        /// </summary>
        /// <returns>A list of all students in the database</returns>
        IEnumerable<Student> GetAllStudents();

        /// <summary>
        /// Insert new student record into the database
        /// </summary>
        /// <param name="userID">The id of the user with which the students will be associated</param>
        /// <param name="students">The student record to be inserted</param>
        void InsertStudentInfo(int userID, Student student);

        /// <summary>
        /// Update student record in the database
        /// </summary>
        /// <param name="students">The student record to update</param>
        void UpdateStudentInfo(Student student);

        /// <summary>
        /// Delete the student from the database
        /// </summary>
        /// <param name="studentIDs">The id of the student to delete</param>
        void DeleteStudentInfoByStudentID(int studentID);

        /// <summary>
        /// Delete the students belonging to the particular user from the database
        /// </summary>
        /// <param name="userID">The id of the user whose students to delete</param>
        void DeleteStudentInfoByUserID(int userID);
    }
}
