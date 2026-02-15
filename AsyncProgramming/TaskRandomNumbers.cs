using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class TaskRandomNumbers
    {
        private readonly object _numbersLock = new object();

        public void Run()
        {
            Console.Write("введіть кількість чисел для генерування: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Некоректне число!");
                return;
            }

            var numbers = new List<int>(n);
            var tasks = new Task[n];
            int finished = 0;

            for (int i = 0; i < n; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    int value = RandomNumberService(); 

                    lock (_numbersLock)
                    {
                        numbers.Add(value);
                        Console.WriteLine("[" + string.Join(", ", numbers) + "]");
                    }

                    if (Interlocked.Increment(ref finished) == n)
                    {
                        lock (_numbersLock)
                        {
                            Console.WriteLine("Результат: [" + string.Join(", ", numbers) + "]");
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        private int RandomNumberService()
        {
            Task.Delay(1000).Wait(); 
            return Random.Shared.Next(1, 100);
        }
    }
}
