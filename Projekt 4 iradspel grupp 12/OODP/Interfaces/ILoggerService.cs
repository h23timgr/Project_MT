using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODP.Interfaces
{
    public interface ILoggerService // Gränssnitt för loggfunktioner
    {
        void Log(string message);
    }
}