using System;
using SampleShop.Interfaces;
using Xunit;

namespace SampleShop.Tests.Mocks
{
    public class MockLogger: ILogger
    {
        string testCheck;
        public MockLogger(string check)
        {
            testCheck = check;
        }

        public void WriteToLog(string log)
        {
            Assert.Equal(testCheck, log);
        }
    }
}
