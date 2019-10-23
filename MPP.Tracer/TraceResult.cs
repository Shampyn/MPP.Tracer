using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MPP.Tracer
{
    [JsonObject]
    [XmlRoot("root")]
    [Serializable]
    public class TraceResult
    {
        [JsonProperty("threads")]
        [XmlElement("threads")]
        public ConcurrentSerializableDictionary<int, ThreadTraceResult> Threads { get; set; }

        public TraceResult()
        {
            Threads = new ConcurrentSerializableDictionary<int, ThreadTraceResult>();
        }

        public int GetSummOfMethodsWorkTimes(int Indexofthread)
        {
            if (this.Threads.IsEmpty || !this.Threads.Keys.Contains(Indexofthread))
            {
                return -1;
            }

            double result = 0;
            ThreadTraceResult Threadtraceresult = this.Threads[Indexofthread];

            foreach (MethodTraceResult method in Threadtraceresult.RootMethods)
            {
                result += Math.Round(method.WorkTime + GetSummOfInnerMethodsWorkTimes(method.Methods));
            }

            return (int)Math.Truncate(result);

        }

        private double GetSummOfInnerMethodsWorkTimes(List<MethodTraceResult> Methods)
        {
            if (Methods == null)
            {
                return 0;
            }

            double sum = 0;

            foreach (MethodTraceResult method in Methods)
            {
                sum += Math.Round(method.WorkTime + GetSummOfInnerMethodsWorkTimes(method.Methods));
            }

            return sum;
        }
    }
}