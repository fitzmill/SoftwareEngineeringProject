using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accessors
{
    class SetUserInfoAccessor : ISetUserInfoAccessor
    {
        public void InsertPersonalInfo(User user)
        {
            //add the personal record from the user to the database
            //store the auto-generated database id in the user model
            throw new NotImplementedException();
        }

        public void UpdatePersonalInfo(User user)
        {
            //use the id from the user model to update the record asociated with the user
            throw new NotImplementedException();
        }

        public void DeletePersonalInfo(int userID)
        {
            //remove all personal data from the database associated with the userID
            throw new NotImplementedException();
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            //add a new student to the database associated with the userID
            throw new NotImplementedException();
        }

        public void UpdateStudentInfo(Student student)
        {
            //update a student in the database with the information provided
            throw new NotImplementedException();
        }

        public void DeleteStudentInfo(int studentID)
        {
            //remove data associated with the particular studentID
            throw new NotImplementedException();
        }
    }
}
