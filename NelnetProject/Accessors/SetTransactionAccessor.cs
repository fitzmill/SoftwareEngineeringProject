using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace Accessors
{
    public class SetTransactionAccessor : ISetTransactionAccessor
    {
        //Executes a stored procedure to insert a transaction with all of the Transaction object's properties as parameters
        public void AddTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure to update a transaction with all of the Transaction object's properties as parameters
        public void UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
