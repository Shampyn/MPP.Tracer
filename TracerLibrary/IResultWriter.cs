using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerLibrary
{
  public interface IResultWriter
    {
        void WriteResult(string result);
    }
}

