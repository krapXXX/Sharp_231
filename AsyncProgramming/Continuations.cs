using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpKnP321.AsyncProgramming
{
    internal class Continuations
    {
        private long startTicks;
        
        public void Run()
        {
            Console.WriteLine(
         Task.Run(GetString)
        .ContinueWith(t => Spacefy(t.Result))
        .ContinueWith(t => Capitalyze(t.Result))
        .ContinueWith(t => Slugify(t.Result))
        .Result);
        }
        public void RunOptimal()
        {
            startTicks = DateTime.Now.Ticks;

            Task<string> chain1 = Task.Run(Work1).ContinueWith(t => Work3(t.Result));
            Task<string> chain2 = Task.Run(Work2).ContinueWith(t => Work4(t.Result));

            Console.WriteLine("{1:F2} Chain1 res: {0}", chain1.Result, (DateTime.Now.Ticks - startTicks) % (long)1e8 / 1e7);
            Console.WriteLine("{1:F2} Chain2 res: {0}", chain2.Result, (DateTime.Now.Ticks - startTicks) % (long)1e8 / 1e7);
        }
        /*
         задача: оброблення рядків
        1) нормалізація пробілів - усі кратні пробіли замінити на одиночні
        2) зробити кожну першу літеру слова великою 
        3) слогіфікація - утворення безпробільних послідовностей  


        \s - space symbols 
        \S - non-space 
        \d - digit
        \D - non-digit
        \w - word symbol
        \W - non \w
        \b - boundary (, . ; " ")
         ^ - begining of the str
         $ - end for the str
         . - any symbol

        [0-9a-fA-F] - group of symbols
        [^0-9] - all except 

        ? - 1 or 0 
        + - 1 and more
        * - any amount (0)
        {2} - 2
        {2,5} - 2 - 5
        {2,} - 2 and more 
         */
        private String Spacefy(String str)
        {
            Task.Delay(1000).Wait();
            String res = Regex.Replace(str, @"\s+", " ");
            Console.WriteLine($"Spacefy: '{str}' - > '{res}'");
            return res;
        }

        private String GetString()
        {
            Task.Delay(1000).Wait();
            String str = "!The quick,\tbrown \t fox        jumps - over the lazy; dog??";
            Console.WriteLine($"GetString: '{str}'");
            return str;
        }

        private String Capitalyze(String str)
        {
            Task.Delay(1000).Wait();
            String res = String.Join(' ', str.Split(" ",StringSplitOptions.RemoveEmptyEntries)
            .Select(s => $"{s[0].ToString().ToUpper()}{s[1..]}"));
            Console.WriteLine($"Capitalyze: '{str}' - > '{res}'");
            return res;
        }

        private String Slugify(String str, String glue = "")
        {
            Task.Delay(1000).Wait();
            String res = (Regex.Replace(str, @"\W+", glue)).ToLower().Trim(glue.ToCharArray());
            Console.WriteLine($"Slugify: '{str}' - > '{res}'");
            return res;
        }
        public void RunWrong()
        {
            startTicks = DateTime.Now.Ticks;   // ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7
            // Task.Run(Work1).Wait();   // Work1()
            Task<String> task1 = Task.Run(Work1);
            Task<String> task2 = Task.Run(Work2);

            Console.WriteLine("{0:F2} Work1 result: {1}", task1.Result,((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7); 
            Task<String> task3 = Task.Run(()=>Work3(task1.Result));

            Console.WriteLine("{0:F2} Work2 result: {1}", task2.Result,((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Task<String> task4 = Task.Run(() => Work4(task2.Result));

            Console.WriteLine("{0:F2} Work3 result: {1}", task3.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Console.WriteLine("{0:F2} Work4 result: {1}", task4.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);

            Console.WriteLine("{0:F2} Demo finish", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);

        }

        private String Work1() => Worker(1, 2000);
        private String Work2() => Worker(2, 1000);
        private String Work3(String data = "") => Worker(3, 1000, data);
        private String Work4(String data = "") => Worker(4, 2000, data);

        private String Worker(int num, int delay, String data = "")
        {
            Console.WriteLine("{0:F2} Work{1} start", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7, num, data);
            Task.Delay(delay).Wait();
            Console.WriteLine("{0:F2} Work{1} finish", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7, num);
            return $"Work{num} result";
        }
    }

}
/* Task chaining, Continuations, ланцюгове (ниткове) програмування
 * Вживається коли одні задачі базуються на результатах інших, проте,
 * з третіми задачами можуть виконуватись паралельно
 * --1---==3==
 * --2-==4====
 */
