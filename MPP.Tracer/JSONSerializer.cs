using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MPP.Tracer
{
    public class JSONSerializer:ISerializer
    {
        public string Serialize(object obj)
        {
            string result = "";
            result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return result;
        }
    }
}
