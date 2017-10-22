using System;

namespace LoriaNET
{
    public class ConsoleLog : ILog
    {
        public void Info(string message) => Console.WriteLine($"info> {message}");
        public void Warning(string message) => Console.WriteLine($"warn> {message}");
        public void Error(string message) => Console.WriteLine($"error> {message}");
    }
}
