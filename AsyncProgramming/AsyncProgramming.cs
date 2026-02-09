using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncProgramming
    {
        private readonly object _sumLock = new object();
        private double sum;

        public void Run()
        {
            Console.WriteLine(Directory.GetCurrentDirectory);
            ConsoleKeyInfo keyInfo;

            do
            {
                Console.WriteLine("Async Programing: select an action");
                Console.WriteLine("1. Processes list");
                Console.WriteLine("2. Start notepad");
                Console.WriteLine("3. Edit demo file");
                Console.WriteLine("4. Threads demo");
                Console.WriteLine("5. Multi thread");
                Console.WriteLine("6. Open browser (URL)");
                Console.WriteLine("0. Exit program");

                keyInfo = Console.ReadKey();
                Console.WriteLine();

                switch (keyInfo.KeyChar)
                {
                    case '0':
                        return;
                    case '1':
                        ProcessesDemo();
                        break;
                    case '2':
                        ProcessControlDemo();
                        break;
                    case '3':
                        ProcessWithParam();
                        break;   
                    case '4':
                        ThreadsDemo();
                        break;
                    case '5':
                        MultiThread();
                        break;
                    case '6':
                        OpenBrowserUrl("https://www.google.com/search?q=async+await+c%23");
                        break;
                    default:
                        Console.WriteLine("Wrong choice!");
                        break;
                }
            }
            while (true);
        }
        private void MultiThread()
        {
            sum = 100.0;
            Console.WriteLine($"Start sum = {sum}");

            for (int i = 0; i < 12; i++)
            {
                new Thread(CalcMonth).Start(i + 1);
            }
        }
        private void CalcMonth(object? month)
        {
            int m = (int)month!;
            double res = sum;

            Console.WriteLine($"Request sent for month {m}");
            Thread.Sleep(1000); // імітація API-запиту

            double percent = 10;
            lock (_sumLock)
            {
                sum *= (1.0 + percent / 100.0);
                Console.WriteLine($"Response got for month {m}. Sum = {sum}");
            }
        }
        private void ThreadsDemo()
        {
            // Потоки — системні об'єкти, складові частини процесів
            // Процес має принаймні один потік (основний), завершення
            // усіх потоків процесу = завершення процесу
            // Потік обнується на методі (функції)
            Console.WriteLine("Threads created");

            var t = new Thread(ThreadsActivity);
            Console.WriteLine("Threads start");

            t.Start();
        }
        private void ThreadsActivity()
        {
            Console.WriteLine("Threads activity start");
            Thread.Sleep(1000);
            Console.WriteLine("Threads activity stop");

        }
        private void ProcessWithParam()
        {
            // запуск програми з параметрами вимагає правильного розрізнення
            // імен програми та параметрів
            try
            {
                string filePath = Path.Combine(".", "AsyncProgramming", "demo.txt");
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = filePath,
                    UseShellExecute = true
                });

                Console.WriteLine("Press a key");
                Console.ReadKey();
                Console.WriteLine();

                if (p != null && !p.HasExited)
                {
                    p.CloseMainWindow();
                    if (!p.WaitForExit(1000))
                        p.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void ProcessControlDemo()
        {
            try
            {
                Console.WriteLine("Press a key to start Notepad");
                Console.ReadKey();

                Process process = Process.Start("notepad.exe");
                Console.WriteLine("Press a key to stop Notepad");
                Console.ReadKey();
               if(!process.HasExited) process.Kill();
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private void ProcessesDemo()
        {
            Process[] processes = Process.GetProcesses();
            Dictionary<String, int> proc = [];
            foreach (var process in processes)
            {
                try
                {
                    if (proc.ContainsKey(process.ProcessName))
                    {
                        proc[process.ProcessName]++;
                    }
                    else
                    {
                        proc[process.ProcessName] = 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            foreach (var pair in proc.OrderByDescending(p=>p.Value).ThenBy(p=>p.Key)) 
            {
                Console.WriteLine($"{pair.Key}({pair.Value})");
            }
        }
        private void OpenBrowserUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                Console.WriteLine($"Opened: {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening browser: {ex.Message}");
            }
        }
    }
}
/*
 Розвиток техніки супроводжується зільшенням кількості процесорів
що вимагає перегляду принципів програмування з
розподілом виконавців по різних процсорах

Синхронне виконання коду - послідовне у часі, один за іншим
------- - робота1
======= = робота2
синхронне виконання: ----=====


Асинхронність - будь-яке відхилення від синхронності 
--------- - - -     -----   -----
=========   = = =      ======


Способи реалізації асинхронності 
- мережеві технології 
    = network - розподіл роботи по вузлах мережі
    = grid - суперкомп'ютери, складені з дискретних виконавців 
- багатопроцесність 
- багатопоточність
- багатозадачність 

Процес - термін операційної системи - виконання програми 
 іншими словами: запустити програму = створити процес 
процес обслуговується процесором 

Потік(thread, потік коду)[НЕ stream - потік даних]
 частини процесів, обслуговуються ядрами процесора 

Задача - поняття рівня мови програмування(платформи) 
 зазвичай об'єкт, що відповідає за асинхронне виконання коду 





 */