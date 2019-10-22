using System.Collections.Concurrent;

namespace MPP.Tracer
{
    public class TraceResult
    {
        public ConcurrentDictionary<int, ThreadTraceResult> Threads { get; set; }

        public TraceResult()
        {
            Threads = new ConcurrentDictionary<int, ThreadTraceResult>();
        }
    }
}