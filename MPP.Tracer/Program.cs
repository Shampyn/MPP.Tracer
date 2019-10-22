using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPP.Tracer
{
    class Program
    {
        public static void TestMe()
        {
            int z = 100;
            for (int i = 1; i < 100; i++)
            {
                z = z - i;
            }
        }


        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            tracer.StartTrace();
            TestMe();
            Thread.Sleep(100);
            tracer.StopTrace();
    
        }
    }
}
