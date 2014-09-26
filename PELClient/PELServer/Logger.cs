using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELServer
{
    public static class Logger
    {

        public enum LogLevel
        {
            Inform,
            Warn,
            Error,
        }


        static Logger()
        {

        }
       

        public static void Log(LogLevel logLevel, string message)
        {
            Console.WriteLine(string.Format("[{0}]: {1}", logLevel.ToString(), message));
        }

        public static void Log(string message)
        {
            Log(LogLevel.Inform, message);
        }

        public static void Warn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public static void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

    }
}
