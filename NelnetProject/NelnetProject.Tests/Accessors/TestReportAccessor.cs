using Accessors;
using Core.Interfaces.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestReportAccessor
    {
        private readonly IReportAccessor _reportAccessor;

        public TestReportAccessor()
        {
            _reportAccessor = new ReportAccessor();
        }

        [TestMethod]
        public void TestGetAllReports()
        {
            var result = _reportAccessor.GetAllReports();

            Assert.IsNotNull(result);
        }
    }
}
