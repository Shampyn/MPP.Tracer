using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MPP.Tracer
{
    [JsonObject]
    [Serializable]
    [XmlRoot("thread")]
    public class ThreadTraceResult
    {
        [JsonProperty("id")]
        [XmlAttribute("id")]
        public uint Id { get; set; }

        [JsonProperty("methods")]
        [XmlArray("methods")]
        public List<MethodTraceResult> RootMethods { get; set; }//Methods

        [JsonIgnore]
        [XmlIgnore]
        public Stack<MethodTraceResult> InnerMethods { get; set; }//LastMethods

        [JsonIgnore]
        [XmlIgnore]
        public int MethodsCount { get; private set; }

        [JsonIgnore]
        [XmlIgnore]
        public double WorkTime { get; set; }

        [JsonProperty("time")]
        [XmlAttribute("time")]
        public string WorkTimeStr
        {
            get
            {
                return Math.Round(WorkTime) + "ms";
            }
            set
            {
                WorkTimeStr = value;
            }
        }
       

        public ThreadTraceResult(uint id)
            : this()
        {
            Id = id;
        }

        private ThreadTraceResult()
        {
            InnerMethods = new Stack<MethodTraceResult>();
            InnerMethods.Push(MethodTraceResult.StackTop);
            RootMethods = new List<MethodTraceResult>();
            Id = uint.MaxValue;
            WorkTime = 0;
        }

        public double CalculateFullTime()
        {
            this.MethodsCount = 0;
            WorkTime = SummAllMethodsTimes(RootMethods);

            return WorkTime;
        }

        private double SummAllMethodsTimes(List<MethodTraceResult> methods)
        {
            double summ = 0;
            foreach (MethodTraceResult method in methods)
            {
                this.MethodsCount++;
                summ += Math.Round(method.GetWorkTime() + SummAllMethodsTimes(method.Methods));
            }

            this.WorkTime = summ;
            return summ;
        }

        public static bool IsExist(int threadId, ConcurrentDictionary<int, ThreadTraceResult> threads)
        {
            foreach (KeyValuePair<int, ThreadTraceResult> thread in threads)
            {
                if (threadId == thread.Key)
                {
                    return true;
                }
            }

            return false;
        }
    }
}