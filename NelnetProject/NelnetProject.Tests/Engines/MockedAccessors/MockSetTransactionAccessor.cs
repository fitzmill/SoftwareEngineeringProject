using Core;
using Core.Interfaces;

namespace NelnetProject.Tests.Engines
{
    internal class MockSetTransactionAccessor : ISetTransactionAccessor
    {
        public void AddTransaction(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}