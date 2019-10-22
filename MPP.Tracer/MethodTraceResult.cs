using System;
using System.Diagnostics;

namespace MPP.Tracer
{
    internal class MethodTraceResult
    {

        private Stopwatch _stopwatch;

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

        public long WorkTime { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }

        public MethodTraceResult(string methodName, string className)
            : this()
        {
            MethodName = methodName;
            ClassName = className;
        }

        private MethodTraceResult()
        {
            _stopwatch = new Stopwatch();
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

    }
}
