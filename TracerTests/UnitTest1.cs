using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TracerLibrary;

namespace TracerTests
{
    public class A
    {
        private Tracer _tracer;
        private B _innerClassB;

        public A(Tracer tracer)
        {
            this._tracer = tracer;
            _innerClassB = new B(_tracer);
        }

        public void MethodA()
        {
            _tracer.StartTrace();

            _innerClassB.MethodBWithCycle();

            _tracer.StopTrace();
        }

        public void MultiThreadedMethodA()
        {
            _tracer.StartTrace();
            B b = new B(_tracer);

            Thread thread1 = new Thread(() => { b.MethodBWithCycle(); });

            thread1.Start();

            _tracer.StopTrace();
        }
    }

    public class B
    {
        private Tracer _tracer;
        private List<int> TestInt;

        public B(Tracer tracer)
        {
            this._tracer = tracer;
            TestInt = new List<int>();
        }

        public void MethodBWithCycle()
        {
            _tracer.StartTrace();
            for (int i = 0; i < 1000; i++)
            {
                TestInt.Add(i * 4);
            }
            _tracer.StopTrace();
        }
    }

    public class C
    {
        private Tracer _tracer;
        private List<int> TestInt;
        private D _innerClassD;

        public C(Tracer tracer)
        {
            this._tracer = tracer;
            TestInt = new List<int>();
            _innerClassD = new D(_tracer);
        }

        public void MultiThreadedMethodC()
        {
            _tracer.StartTrace();
            D d = new D(_tracer);
            Thread thread1 = new Thread(() => { d.MethodDWithCycle(); });
            Thread thread2 = new Thread(() => { MethodCWithCycle(); });

            thread1.Start();
            thread2.Start();
            MethodCWithCycle();
            for (int i = 1; i < 1000; i++)
            {
                TestInt.Add(i);
            }

            _tracer.StopTrace();
        }

        public void MethodCWithCycle()
        {
            _tracer.StartTrace();
            for (int i = 0; i < 1000; i++)
            {
                i.ToString();
            }
            _tracer.StopTrace();
        }
    }

    public class D
    {
        private Tracer _tracer;
        private List<int> TestInt;

        public D(Tracer tracer)
        {
            this._tracer = tracer;
            TestInt = new List<int>();
        }
        public void MethodDWithCycle()
        {
            _tracer.StartTrace();
            for (int i = 0; i < 1000; i++)
            {
                TestInt.Add(i * 4);
            }
            InnerMethodD();
            _tracer.StopTrace();
        }
        public void InnerMethodD()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ShouldTraceOneThread()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MethodA();
            Assert.AreEqual(1, tracer.GetTraceResult().Threads.Count);
        }

        [TestMethod]
        public void ShouldContainCorrectThreadID()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MethodA();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Assert.AreEqual(true, tracer.GetTraceResult().Threads.Keys.Contains(threadId));
        }

        [TestMethod]
        public void ShouldTraceTwoMethods()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MethodA();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Assert.AreEqual(2, tracer.GetTraceResult().Threads.Values.ElementAt(0).MethodsCount);
        }

        [TestMethod]
        public void ShouldReturnFilledDictionary()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MultiThreadedMethodA();
            Assert.AreNotEqual(0, tracer.GetTraceResult().Threads.Count);
        }

        [TestMethod]
        public void ShouldReturnEqualTimeForMultipleThreadsAndAllTheirMethods()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MultiThreadedMethodA();

            foreach (KeyValuePair<int, ThreadTraceResult> thread in tracer.GetTraceResult().Threads)
            {
                Assert.AreEqual((int)Math.Floor(thread.Value.WorkTime), tracer.GetTraceResult().GetSummOfMethodsWorkTimes(thread.Key));
            }
        }

        [TestMethod]
        public void ShouldReturnEqualTimeForThreadAndAllItsMethods()
        {
            Tracer tracer = new Tracer();
            A a = new A(tracer);
            a.MethodA();

            foreach (KeyValuePair<int, ThreadTraceResult> thread in tracer.GetTraceResult().Threads)
            {
                Assert.AreEqual((int)Math.Floor(thread.Value.WorkTime), tracer.GetTraceResult().GetSummOfMethodsWorkTimes(thread.Key));
            }
        }

        [TestMethod]
        public void ShouldTraceOneInnerMethod()
        {
            Tracer tracer = new Tracer();
            C c = new C(tracer);
            c.MultiThreadedMethodC();
            Assert.AreEqual(1, tracer.GetTraceResult().Threads.Values.ElementAt(0).Methods.Count);
        }

        [TestMethod]
        public void ShouldTraceThreeThreads()
        {
            Tracer tracer = new Tracer();
            C c = new C(tracer);
            c.MultiThreadedMethodC();
            Assert.AreEqual(3, tracer.GetTraceResult().Threads.Count);
        }
    }
}
