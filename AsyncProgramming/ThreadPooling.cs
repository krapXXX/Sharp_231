using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class ThreadPooling
    {
        public void Run()
        {
            ThreadPool.SetMaxThreads(1,1);
            Console.WriteLine("ThreadPooling start");
            ThreadPool.QueueUserWorkItem(ThreadAction, 300);
            ThreadPool.QueueUserWorkItem(ThreadAction, 1000);
            ThreadPool.QueueUserWorkItem(ThreadAction, 500);
            Thread.Sleep(500);
            Console.WriteLine($"ThreadPooling finish: {ThreadPool.CompletedWorkItemCount} done, {ThreadPool.PendingWorkItemCount} terminated");
        }

        private void ThreadAction(Object? timeout)
        {
            Console.WriteLine($"ThreadAction {timeout} start");
            Thread.Sleep((int)timeout!);
            Console.WriteLine($"ThreadAction {timeout} finish");
        }
    }
}
/*
 Thread Pool - пул потоків - середовище виконання потоків з фоновим пріоритетом
 */
