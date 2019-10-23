using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPP.Tracer
{
   public class Tracer : ITracer
    {
        public TraceResult Traceresult;
        public bool Calculated { get; private set; }

        public Tracer()
        {
            Traceresult = new TraceResult();
        }        

        public void StartTrace()
        {
            Calculated = false;
            int threadId = Thread.CurrentThread.ManagedThreadId;
            var thread = Traceresult.Threads.GetOrAdd(threadId, (_) => new ThreadTraceResult((uint)threadId));
            StackTrace Stacktrace = new StackTrace();
            MethodBase method = Stacktrace.GetFrame(1).GetMethod();
            MethodTraceResult Newmethod = new MethodTraceResult(method.Name, method.ReflectedType.Name);
            Newmethod.StartTrace();

            if (thread.LastStackMethods.Count() != 0)
            {
                thread.LastStackMethods.Peek().AddChildMethod(Newmethod);
            }
            else
            {
                thread.Methods.Add(Newmethod);
            }
            thread.LastStackMethods.Push(Newmethod);
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodTraceResult lastMethod = Traceresult.Threads[threadId].LastStackMethods.Pop();
            lastMethod.StopTrace();
        }

        public TraceResult GetTraceResult()
        {
            if (!Calculated)
            {
                CalculateThreadsTime();
                Calculated = true;
            }
            return Traceresult;
        }

        private void CalculateThreadsTime()
        {
            foreach (KeyValuePair<int, ThreadTraceResult> thread in Traceresult.Threads)
            {
                thread.Value.CalculateFullTime();
            }
        }
    }
}
