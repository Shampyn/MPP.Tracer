using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MPP.Tracer
{
    public class ThreadTraceResult
    {
        public uint Id { get; set; }

        public List<MethodTraceResult> Methods { get; set; }

        public ThreadTraceResult(uint id)
            : this()
        {
            Id = id;
        }

        private ThreadTraceResult()
        {
            Methods = new List<MethodTraceResult>();
            Id = uint.MaxValue;
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