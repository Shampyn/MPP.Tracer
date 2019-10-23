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
        public static void TestMe(Tracer tracer)
        {
            tracer.StartTrace();
            int z = 100;         
                for (int i = 1; i < 10000; i++)
                {
                    i.ToString();
                    z = z - i;
                }
            
            tracer.StopTrace();
        }

        public static void TestMe1(Tracer tracer)
        {
            tracer.StartTrace();
            int z = 100;
            for (int i = 1; i < 10000; i++)
            {
                i.ToString();
                z = z - i;
            }
            tracer.StopTrace();
        }


        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            tracer.StartTrace();
            TestMe(tracer);
            TestMe(tracer);
            Thread thread1 = new Thread(() => { TestMe1(tracer); });
            thread1.Start();
            Thread.Sleep(100);
            tracer.StopTrace();

            FileTraceRusultWriter fw = new FileTraceRusultWriter("Result.xml");


            XMLSerializer xml = new XMLSerializer();
            fw.WriteResult(xml.Serialize(tracer.GetTraceResult()));

           

            ConsoleTraceResultWriter rw = new ConsoleTraceResultWriter();


            XMLSerializer xml1 = new XMLSerializer();
            rw.WriteResult(xml.Serialize(tracer.GetTraceResult()));

            JSONSerializer json = new JSONSerializer();
            rw.WriteResult(json.Serialize(tracer.GetTraceResult()));
            Console.ReadLine();

        }
    }
}
