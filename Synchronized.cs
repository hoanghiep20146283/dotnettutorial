namespace async
{
    class Synchronized
    {
        static object lockObj = new Object();

        static void writeToConsole(int iterations, string msg, ConsoleColor color)
        {
            for (int i = 0; i < iterations; i++)
            {
                lock (lockObj)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine($"{msg,5} {i,2}");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                }
            }
        }

        public static void writeColor()
        {
            Thread t1 = new Thread(() => {
                writeToConsole(5, "Task 1", ConsoleColor.Green);
            });

            Thread t2 = new Thread(() => {
                writeToConsole(5, "Task 2", ConsoleColor.Cyan);
            });

            Thread t3 = new Thread(() => {
                writeToConsole(5, "Task 3", ConsoleColor.Yellow);
            });

            t1.Start();
            t2.Start();
            t3.Start();

            Thread.Sleep(5000);
        }
    }
}