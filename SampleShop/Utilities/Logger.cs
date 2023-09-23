using System;
using SampleShop.Interfaces;

namespace SampleShop.Utilities
{
    public class Logger : ILogger
    {
        public Logger()
        {
        }

        public void WriteToLog(string log)
        {
            Console.Write(log);
        }
    }
}
