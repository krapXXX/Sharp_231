using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class Threading
    {
        public void Run()
        {
            Thread t10;
            CancellationTokenSource cts = new();
            try
            {
                t10 = new Thread(ThreadActivity);
                    t10.Start(new ThreadData(10, cts.Token)); 
                new Thread(ThreadActivity).Start("Str");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press s key");
            Console.ReadKey();
            cts.Cancel();//подати сигнал скасування на всі токени данного джерела
                         //сам сигнал не зупиняє потоки, лише перневодить токен до
                         //скасованого стану. потоки мають переврти це в середині себе
        }

        private void ThreadActivity(Object? arg)
        {
            try
            {
                if (arg is ThreadData data) // Pattern matching
                {
                    string res = "";
                    StringBuilder sb = new();

                    for (int i = 0; i < data.N; i += 1)
                    {
                        Thread.Sleep(1000); // імітація тривалих розрахунків

                        // res += i; // Поєднання рядків у циклі
                        sb.Append(i);
                        Console.WriteLine("proceeded " + i);

                        // контрольована перевірка скасування потоку
                        data.CancellationToken.ThrowIfCancellationRequested();
                    }

                    res = sb.ToString();
                    Console.WriteLine(res);
                }
                else
                {
                    //throw new ArgumentException("arg should be int");
                    Console.WriteLine("Wrong, arg should be int");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    internal record ThreadData(int N, CancellationToken CancellationToken) { }

}


/*
 багатопоточність: управління виконанням 
                                поза catch
---Run----try{}catch{}---          [|]
           \                        |
            \------thread(10)-cw-|  |
             \------thread("str")-throw-|

 Винятки існують у межах одного потоку
 до інших потоків не передаються

 * Скасування потоків
 * Старий підхід (на зараз не підтримується)
 * thread.Cancel() — виклик якого створював у потоці виняток
 * Закладено традиції — потокові функції/методи повністю оточувати try-catch
 
 */
