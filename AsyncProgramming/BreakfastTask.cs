using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class BreakfastTask
    {
        public void Run()
        {
            Console.WriteLine("{0:F2} Breakfast start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Task makeToast = Task.Run(MakeToast);
            Task roastBeacon = Task.Run(RoastBacon);
            Task makeCoffee = Task.Run(MakeCoffee);

            Task<String> toastString = Task.Run(ToastStringAction);
            Task<String> baconString = Task.Run(BaconStringAction);
            Task<String> coffeeString = Task.Run(CoffeeStringAction);

            Console.WriteLine($"Toast {toastString.Result}");
            Console.WriteLine($"Bacon {baconString.Result}");
            Console.WriteLine($"Coffee {coffeeString.Result}");

            Console.WriteLine("{0:F2} Breakfast finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }


        private void MakeToast()
        {
            Console.WriteLine("{0:F2} MakeToast Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Task.Delay(1000).Wait();
            Console.WriteLine("{0:F2} MakeToast Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }
        private void RoastBacon()
        {
            Console.WriteLine("{0:F2} RoastBacon Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Task.Delay(1000).Wait();
            Console.WriteLine("{0:F2} RoastBacon Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }
        private void MakeCoffee()
        {
            Console.WriteLine("{0:F2} MakeCoffee Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Task.Delay(1000).Wait();
            Console.WriteLine("{0:F2} MakeCoffee Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private String ToastStringAction()
        {
            Console.WriteLine("ToastStringAction start");
            Task.Delay(300).Wait();
            return "ready";
        } 
        private String BaconStringAction()
        {
            Console.WriteLine("BaconStringAction start");
            Task.Delay(800).Wait();
            return "ready";
        } 
        private String CoffeeStringAction()
        {
            Console.WriteLine("CoffeeStringAction start");
            Task.Delay(1800).Wait();
            return "ready";
        }
    }
}
