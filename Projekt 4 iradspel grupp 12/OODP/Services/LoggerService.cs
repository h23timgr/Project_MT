using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODP
{
    internal class LoggerService : Interfaces.ILoggerService // Implementerar gränssnittet ILoggerService
    {
        public void Log(string message)
        {   
            Console.WriteLine($"Log: {message}"); // Logik för loggning, till en fil, en databas eller konsol
        }
    }
}