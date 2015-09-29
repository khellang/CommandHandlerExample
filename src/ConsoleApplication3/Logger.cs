using System;

namespace ConsoleApplication3
{
    public interface ILogger
    {
        void Info(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string message) => Console.WriteLine(message);
    }
}