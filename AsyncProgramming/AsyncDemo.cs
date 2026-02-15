using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncDemo
    {
        public void Run()
        {
            Console.WriteLine("AsyncDemo start");

            // для переходу від синхронних до async методів —
            // необхідний один "не цукровий" варіант
            RunAsync().Wait();

            Console.WriteLine("AsyncDemo finish");
        }

        // за традиціями, імена async-методів завершуються на Async
        private async Task RunAsync()
        {
            Console.WriteLine(await GetStringAsync(10));
            Console.WriteLine(await GetStringAsync(50, 500));
            Console.WriteLine(await GetStringAsync(70, 700));
        }

        // явно зазначаємо "обгортку" Task<string>
        private async Task<string> GetStringAsync(int length, int delay = 1000)
        {
            // Task.Delay(1000).Wait();
            await Task.Delay(delay);   // await – аналог .Result, але неблокуючий

            // ... проте повертаємо просто String
            return $"The string of length {length}";
        }
    }
}
