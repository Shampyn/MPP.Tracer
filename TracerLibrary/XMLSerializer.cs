using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TracerLibrary
{
   public class XMLSerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            StringWriter strWriter = null;

            try
            {
                strWriter = new StringWriter();
                serializer.Serialize(strWriter, obj);
            }
            finally
            {
                if (strWriter != null)
                {
                    strWriter.Dispose();
                }
            }
            return strWriter.ToString();
        }
    }
}
