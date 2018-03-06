using Core;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Engines
{
    public class SetUserInfoEngine : ISetUserInfoEngine
    {
        ISetUserInfoAccessor setUserInfoAccessor;
        ISetPaymentInfoAccessor setPaymentInfoAccessor;

        public SetUserInfoEngine(ISetUserInfoAccessor setUserInfoAccessor, ISetPaymentInfoAccessor setPaymentInfoAccessor)
        {
            this.setUserInfoAccessor = setUserInfoAccessor;
            this.setPaymentInfoAccessor = setPaymentInfoAccessor;
        }

        //make a call to the SetPaymentInfoAccessor to add the payment info to paymentSpring and return the generated customerID
        public string InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            return setPaymentInfoAccessor.CreateCustomer(userPaymentInfo);
        }

        //make a call to the SetPaymentInfoAccessor to update the payment info associated with the customerID
        public void UpdatePaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            setPaymentInfoAccessor.UpdateCustomer(userPaymentInfo);
        }

        //make a call to the SetPaymentInfoAccessor to delete the payment info associated with the customerID
        public void DeletePaymentInfo(string customerID)
        {
            setPaymentInfoAccessor.DeleteCustomer(customerID);
        }

        //make a call to the SetUserInfoAccessor to add a new record to the database
        public void InsertPersonalInfo(User user)
        {
            setUserInfoAccessor.InsertPersonalInfo(user);
            InsertStudentInfo(user.UserID, user.Students);
        }

        //make a call to the SetUserInfoAccessor that will update the database record associated with the user
        public void UpdatePersonalInfo(User user)
        {
            setUserInfoAccessor.UpdatePersonalInfo(user);
            this.UpdateStudentInfo(user.Students);
        }

        //make a call to the SetUserInfoAccessor to delete the personal information associated with the userID including students
        public void DeletePersonalInfo(int userID, string customerID)
        {
            setUserInfoAccessor.DeleteStudentInfoByUserID(userID);
            setUserInfoAccessor.DeletePersonalInfo(userID);
            setPaymentInfoAccessor.DeleteCustomer(customerID);
        }

        //make a call to the SetUserInfoAccessor to insert new students into the database to be associated with the userID
        public void InsertStudentInfo(int userID, IList<Student> students)
        {
            foreach (Student s in students)
            {
                setUserInfoAccessor.InsertStudentInfo(userID, s);
            }
        }

        //make a call to the SetUserInfoAccessor to update the student records associated with the list of students
        public void UpdateStudentInfo(IList<Student> students)
        {
            foreach (Student s in students)
            {
                setUserInfoAccessor.UpdateStudentInfo(s);
            }
        }

        //make a call to the SetUserInfoAcessor to delete the student information associated with the studentIDs
        public void DeleteStudentInfo(IList<int> studentIDs)
        { 
            foreach (int id in studentIDs)
            {
                setUserInfoAccessor.DeleteStudentInfoByStudentID(id);
            }
        }
    }
}
