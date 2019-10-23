using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MPP.Tracer
{
    [JsonObject]
    [Serializable]
    [XmlRoot("method")]
    public class MethodTraceResult
    {
        [JsonIgnore]
        [XmlIgnore]
        public static MethodTraceResult StackTop { get; }

        [JsonProperty("methods")]
        [XmlArray("methods")]
        public List<MethodTraceResult> Methods { get; }

        [JsonIgnore]
        [XmlIgnore]
        public long WorkTime { get; set; }

        [JsonProperty("class")]
        [XmlAttribute("class")]
        public string ClassName { get; set; }

        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string MethodName { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        private Stopwatch _stopwatch;

        public MethodTraceResult(string methodName, string className)
            : this()
        {
            MethodName = methodName;
            ClassName = className;
        }

        private MethodTraceResult()
        {
            _stopwatch = new Stopwatch();
            Methods = new List<MethodTraceResult>();
        }

        [JsonProperty("time")]
        [XmlAttribute("time")]
        public string WorkTimeStr
        {
            get
            {
                return Math.Round((double)WorkTime) + "ms";
            }
            set
            {
                WorkTimeStr = value;
            }
        }

        public void StartTrace()
        {
            _stopwatch.Start();
        }

        public long GetWorkTime()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
            WorkTime = _stopwatch.ElapsedMilliseconds;
        }

        public static bool AddNestedMethod(MethodTraceResult parent, MethodTraceResult child)
        {
            if (child == null || parent == null)
                return false;

            parent.Methods.Add(child);

            return true;
        }

    }
}
