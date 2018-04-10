using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Handles the storage of all user information
    /// </summary>
    public interface ISetUserInfoEngine
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
        /// <param name="userID">The ID of the user in the database to delete</param>
        /// <param name="customerID">The ID of the customer in payment spring</param>
        void DeletePersonalInfo(int userID, string customerID);

        /// <summary>
        /// Insert new student records into the database
        /// </summary>
        /// <param name="userID">The user to add the student records too</param>
        /// <param name="students">The student records to be inserted</param>
        void InsertStudentInfo(int userID, IList<Student> students);

        /// <summary>
        /// Update student records in the database
        /// </summary>
        /// <param name="students">The student records to update</param>
        void UpdateStudentInfo(IList<Student> students);

        /// <summary>
        /// Delete the student from the database
        /// </summary>
        /// <param name="studentIDs">The ids of the students to delete</param>
        void DeleteStudentInfo(IList<int> studentIDs);

        /// <summary>
        /// Insert new payment info into paymentSpring
        /// </summary>
        /// <param name="userPaymentInfo">The payment info to be stored in paymentSpring</param>
        /// <returns></returns>
        string InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo);

        /// <summary>
        /// Updates a user's name and address information on paymentSpring
        /// </summary>
        /// <param name="paymentAddressInfo">The information to update in paymentSpring</param>
        void UpdatePaymentBillingInfo(PaymentAddressDTO paymentAddressInfo);

        /// <summary>
        /// Updates a user's card information on paymentSpring
        /// </summary>
        /// <param name="paymentCardInfo">The information to update in paymentSpring</param>
        void UpdatePaymentCardInfo(PaymentCardDTO paymentCardInfo);

        /// <summary>
        /// Delete the payment information from paymentSpring
        /// </summary>
        /// <param name="customerID">The id of the customer to be deleted</param>
        void DeletePaymentInfo(string customerID);

    }
}
