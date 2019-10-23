using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP.Tracer
{
    class ConsoleTraceResultWriter:IResultWriter
    {
        public void WriteResult(string result)
        {
            System.Console.WriteLine(result);
        }
    }
}
