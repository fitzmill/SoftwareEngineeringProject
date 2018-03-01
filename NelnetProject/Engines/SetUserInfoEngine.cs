using Core;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Engines
{
    public class SetUserInfoEngine : ISetUserInfoEngine
    {
        //make a call to the SetPaymentInfoAccessor to add the payment info to paymentSpring and return the generated customerID
        public string InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetPaymentInfoAccessor to update the payment info associated with the customerID
        public void UpdatePaymentInfo(string customerID, UserPaymentInfoDTO userPaymentInfo)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetPaymentInfoAccessor to delete the payment info associated with the customerID
        public void DeletePaymentInfo(string customerID)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAccessor to add a new record to the database
        public void InsertPersonalInfo(User user)
        { 
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAccessor that will update the database record associated with the user
        public void UpdatePersonalInfo(User user)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAccessor to delete the personal information associated with the userID
        public void DeletePersonalInfo(int userID)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAccessor to insert new students into the database to be associated with the userID
        public void InsertStudentInfo(int userID, List<Student> students)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAccessor to update the student records associated with the list of students
        public void UpdateStudentInfo(List<Student> students)
        {
            throw new NotImplementedException();
        }

        //make a call to the SetUserInfoAcessor to delete the student information associated with the studentIDs
        public void DeleteStudentInfo(List<int> studentIDs)
        { 
            throw new NotImplementedException();
        }
    }
}
