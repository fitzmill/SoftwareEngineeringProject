using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Handles the storage of all user information
    /// </summary>
    interface ISetUserInfoEngine
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
        /// Insert new student records into the database
        /// </summary>
        /// <param name="userID">The id of the user that the students will be associated with</param>
        /// <param name="students">The student records to be inserted</param>
        void InsertStudentInfo(int userID, List<Student> students);

        /// <summary>
        /// Update student records in the database
        /// </summary>
        /// <param name="userID">The id of the user associated with the students</param>
        /// <param name="students">The student records to update</param>
        void UpdateStudentInfo(int userID, List<Student> students);

        /// <summary>
        /// Delete the student from the database
        /// </summary>
        /// <param name="userID">The id of the user associated with these students</param>
        /// <param name="studentIDs">The ids of the students to delete</param>
        void DeleteStudentInfo(int userID, List<int> studentIDs);

        /// <summary>
        /// Insert new payment info into paymentSpring
        /// </summary>
        /// <param name="customerID">Id of the customer in paymentSpring</param>
        /// <param name="cardNumber"></param>
        /// <param name="expirationYear"></param>
        /// <param name="expirationMonth"></param>
        /// <param name="CSC"></param>
        void InsertPaymentInfo(string customerID, string cardNumber, string expirationYear, string expirationMonth, string CSC);

        /// <summary>
        /// Update the payment info associated with the customerID in paymentSpring
        /// </summary>
        /// <param name="customerID">Id of the customer in paymentSpring</param>
        /// <param name="cardNumber"></param>
        /// <param name="expirationYear"></param>
        /// <param name="expirationMonth"></param>
        /// <param name="CSC"></param>
        void UpdatePaymentInfo(string customerID, string cardNumber, string expirationYear, string expirationMonth, string CSC);

        /// <summary>
        /// Delete the payment information from paymentSpring
        /// </summary>
        /// <param name="customerID">The id of the customer to be deleted</param>
        void DeletePaymentInfo(string customerID);

    }
}
