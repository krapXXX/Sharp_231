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
        public void Run()
        {
            //ProcessesDemo();
            ProcessControlDemo();
        }
        private void ProcessControlDemo()
        {
            try
            {
                Console.WriteLine("Press a key");
                Console.ReadKey();

                Process process = Process.Start("notepad.exe");
                Console.WriteLine("Press a key");
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