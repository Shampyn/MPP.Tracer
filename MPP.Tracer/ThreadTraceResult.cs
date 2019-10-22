using System;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace MPP.Tracer
{
    public class ThreadTraceResult
    {
        public uint Id { get; set; }

        public List<MethodTraceResult> RootMethods { get; set; }//Methods
        public Stack<MethodTraceResult> InnerMethods { get; set; }//LastMethods

        public int MethodsCount { get; private set; }

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

        public double WorkTime { get; set; }

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