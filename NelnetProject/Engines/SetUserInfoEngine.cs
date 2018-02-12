using Core;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Engines
{
    public class SetUserInfoEngine : ISetUserInfoEngine
    {
        public string InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            //make a call to the SetPaymentInfoAccessor to add the payment info to paymentSpring
            //return the generated customerID
            throw new NotImplementedException();
        }

        public void UpdatePaymentInfo(string customerID, UserPaymentInfoDTO userPaymentInfo)
        {
            //make a call to the SetPaymentInfoAccessor to update the payment info associated with the customerID
            throw new NotImplementedException();
        }

        public void DeletePaymentInfo(string customerID)
        {
            //make a call to the SetPaymentInfoAccessor to delete the payment info associated with the customerID
            throw new NotImplementedException();
        }

        public void InsertPersonalInfo(User user)
        {
            //make a call to the SetUserInfoAccessor to add a new record to the database
            throw new NotImplementedException();
        }

        public void UpdatePersonalInfo(User user)
        {
            //make a call to the SetUserInfoAccessor that will update the database record associated with the user
            throw new NotImplementedException();
        }

        public void DeletePersonalInfo(int userID)
        {
            //make a call to the SetUserInfoAccessor to delete the personal information associated with the userID
            throw new NotImplementedException();
        }

        public void InsertStudentInfo(int userID, List<Student> students)
        {
            //make a call to the SetUserInfoAccessor to insert new students into the database to be associated with the userID
            throw new NotImplementedException();
        }

        public void UpdateStudentInfo(List<Student> students)
        {
            //make a call to the SetUserInfoAccessor to update the student records associated with the list of students
            throw new NotImplementedException();
        }

        public void DeleteStudentInfo(List<int> studentIDs)
        {
            //make a call to the SetUserInfoAcessor to delete the student information associated with the studentIDs
            throw new NotImplementedException();
        }
    }
}
