using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncAwaitHw
    {
        private readonly object _numbersLock = new object();

        public async Task Run()
        {
            Console.Write("введіть кількість чисел для генерування: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Некоректне число!");
                return;
            }

            var numbers = new List<int>(n);
            Task[] tasks = new Task[n];
            int finished = 0;

            for (int i = 0; i < n; i++)
            {
                tasks[i] = GenerateAndAddAsync(numbers, n, () =>
                {
                    if (Interlocked.Increment(ref finished) == n)
                    {
                        lock (_numbersLock)
                        {
                            Console.WriteLine("Результат: [" + string.Join(", ", numbers) + "]");
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
        }

        private async Task GenerateAndAddAsync(List<int> numbers, int total, Action onFinish)
        {
            int value = await RandomNumberServiceAsync(); 

            lock (_numbersLock)
            {
                numbers.Add(value);
                Console.WriteLine("[" + string.Join(", ", numbers) + "]");
            }

            onFinish();
        }

        private async Task<int> RandomNumberServiceAsync()
        {
            await Task.Delay(Random.Shared.Next(200, 1200)); 
            return Random.Shared.Next(1, 100);
        }
    }
}