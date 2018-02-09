using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Stores information about users in the database.
    /// </summary>
    interface ISetUserInfoAccessor
    {
        /// <summary>
        /// Inserts a new user record into the database with the information contained in the user model
        /// </summary>
        /// <param name="user">The user model to insert</param>
        void InsertPersonalInfo(User user);

        /// <summary>
        /// Updates the user record in the database specified by the userID in the user model
        /// </summary>
        /// <param name="user">The user model to update</param>
        void UpdatePersonalInfo(User user);

        /// <summary>
        /// Delete a user record from the database specified by the userID in the user model
        /// </summary>
        /// <param name="userID"></param>
        void DeletePersonalInfo(int userID);

        /// <summary>
        /// Insert new student record into the database
        /// </summary>
        /// <param name="userID">The id of the user that the students will be associated with</param>
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
        void DeleteStudentInfo(int studentID);

    }
}
