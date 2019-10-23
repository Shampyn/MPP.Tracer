using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP.Tracer
{
    class FileTraceRusultWriter : IResultWriter
    {
        public string FileName { get; }
        public FileTraceRusultWriter(string Filename)
        {
            this.FileName = Filename;
        }

        public void WriteResult(string result)
        {
            File.WriteAllText(FileName, result);
        }
    }
}
