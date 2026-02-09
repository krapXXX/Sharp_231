using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class ThreadJoin
    {
        public void Run()
        {
            Console.WriteLine("{0:F2} Breakfast start", (DateTime.Now.Ticks % (long)1e8) / 1e7);

            Thread makeToast = new(MakeToast);
            Thread roastBeacon = new(RoastBeacon);
            Thread makeCoffee = new(MakeCoffee);

            makeCoffee.Start();   // Порядок запуску — спочатку найбільш тривалі
            roastBeacon.Start();
            makeToast.Start();

            makeToast.Join();     // очікування потоку — блокування потоку виклику (Run) до завершення роботи
            roastBeacon.Join();   // порядок очікування — за логікою
            makeCoffee.Join();

            Console.WriteLine("{0:F2} Breakfast finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }


        private void MakeToast()
        {
            Console.WriteLine("{0:F2} MakeToast Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(100);
            Console.WriteLine("{0:F2} MakeToast Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private void RoastBeacon()
        {
            Console.WriteLine("{0:F2} RoastBeacon Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(300);
            Console.WriteLine("{0:F2} RoastBeacon Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private void MakeCoffee()
        {
            Console.WriteLine("{0:F2} MakeCoffee Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(1000);
            Console.WriteLine("{0:F2} MakeCoffee Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

    }
}
