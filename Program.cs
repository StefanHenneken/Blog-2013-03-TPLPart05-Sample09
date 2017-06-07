using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample09
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Task task1 = null;
            Task task2 = null;
            Task task3 = null;
            try
            {
                Console.WriteLine("Start Run");
                task1 = Task.Run(() => DoWork01());
                task2 = task1.ContinueWith(task => DoWork02(task));
                task3 = task2.ContinueWith(task => DoWork03(task));
                Task.WaitAll(task1, task2, task3);
            }
            catch (AggregateException aex)
            {
                Console.WriteLine("\nAggregateException in Run: " + aex.Message);
                aex.Flatten();
                foreach (Exception ex in aex.InnerExceptions)
                    Console.WriteLine("  Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\ntask1.Status: " + task1.Status);
                Console.WriteLine("task2.Status: " + task2.Status);
                Console.WriteLine("task3.Status: " + task3.Status);
                Console.WriteLine("End Run");
                Console.ReadLine();
            }
        }
        private void DoWork01()
        {
            Console.WriteLine("Start DoWork01");
            throw new DivideByZeroException();
        }
        private void DoWork02(Task task)
        {
            Console.WriteLine("Start DoWork02");
            if (task.Exception != null)
            {
                Console.WriteLine("\nAggregateException in DoWork02: " + task.Exception.Message);
                task.Exception.Flatten();
                foreach (Exception ex in task.Exception.InnerExceptions)
                    Console.WriteLine("  Exception: " + ex.Message);
            }
        }
        private void DoWork03(Task task)
        {
            Console.WriteLine("Start DoWork03");
            throw new IndexOutOfRangeException();
        }
    }
}
