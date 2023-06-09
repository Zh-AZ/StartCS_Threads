using System;
using System.Threading;


internal class Program
{
    static bool stopThread = false;

    private static async Task Main(string[] args)
    {
        void AsyncThread()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Асинхронный поток {i}\n");
                Thread.Sleep(500);
            }
        }

        void SecondThread(object? name)
        {
            Console.WriteLine($"{name} начал работу");
            
            while (true)
            {
                if (stopThread == true)
                {
                    Console.WriteLine($"{name} завершен");
                    break;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"* | {name}\n");
                Thread.Sleep(600);
            }
        }

        async Task MainThread()
        {
            Thread secondThread = new Thread(new ParameterizedThreadStart(SecondThread));
            secondThread.Name = "Второй поток";
            secondThread.Start(secondThread.Name);
            secondThread.IsBackground = true;
            
            await Task.Run(() => AsyncThread());

            var mainThread = Thread.CurrentThread;
            mainThread.Name = "Главный поток";

            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{mainThread.Name}: {i}\n");
                Thread.Sleep(1000);
            }

            Console.WriteLine($"{mainThread.Name} завершен");
            stopThread = true;

            Console.ReadKey();
        }

        await MainThread();
    }
}