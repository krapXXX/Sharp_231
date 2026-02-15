using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class TaskDemo
    {
        public void Run()
        {
            Console.WriteLine("TaskDemo start");
            Task task = Task.Run(TaskAction); //запускає задачу асинхронно та повертає її об'єкт
           
            Task<String> taskString =                   //задачі дозволяють повертати значення з обгорткою Task<>
                //Task.Run(TaskStringAction);           //варіант для методу без параметра
                Task.Run(()=>TaskStringAction(2000));   //з метою передачі параметрів вживає лямбда
           
            
            Task.Delay(500).Wait();
            Console.WriteLine("TaskDemo Wait finish");
            task.Wait();

            Console.WriteLine($"taskString.Result = {taskString.Result}");
            try
            {
                Console.WriteLine(Task.Run(() => TaskStringAction(-1)).Result);   

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            Task.Delay(1000).Wait();

            Console.WriteLine("TaskDemo finish");
        }

        private void TaskAction()
        {
            Console.WriteLine("TaskAction start");
            Task.Delay(1000).Wait();
            Console.WriteLine("TaskAction finish");
        }

        private String TaskStringAction(int timeout)
        {
            if(timeout<=0)
            {
                throw new ArgumentOutOfRangeException("timeout should be positive");
            }
            Console.WriteLine($"TaskStringAction {timeout} start");
            Task.Delay(timeout).Wait();
            Console.WriteLine($"TaskStringAction {timeout} finish");
            return "TaskStringAction";
        }

    }
}
/*
Задачі - інструменти асинхронності, реалізовані на керованому рівні мови/платформи

 */