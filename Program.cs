namespace async
{
    class Program
    {
        static void writeToConsole(int iterations, string msg, ConsoleColor color)
        {
            for (int i = 0; i < iterations; i++)
            {
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine($"Thread: {Thread.CurrentThread.Name, 10} {msg,5} {i,2}");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                }
            }
        }

        static async Task writeToConsoleAsync(string taskName, int iterations, ConsoleColor color)
        {
            await Task.Run(() => writeToConsole(iterations, taskName, color));
            Console.WriteLine($"{taskName} has finished");
        }

        static void asyncAwaitExample()
        {
            Task t1 = writeToConsoleAsync("T1", 3, ConsoleColor.Green);
            Task t2 = writeToConsoleAsync("T2", 3, ConsoleColor.Yellow);
            Task t3 = writeToConsoleAsync("T3", 3, ConsoleColor.Cyan);
            Task.WaitAll(t1, t2, t3);
            Console.WriteLine("All tasks has finished");
        }

        static void writeRandomColorAsync()
        {
            Task<string> t1 = new Task<string>((object obj) => {
                string taskName = (string)obj;
                writeToConsole(10, taskName, ConsoleColor.Green);
                return "Result from Task 1";
            }, "T1");

            Task t2 = new Task((object obj) => {
                string taskName = (string)obj;
                writeToConsole(10, taskName, ConsoleColor.Yellow);
            }, "T2");

            Task t3 = new Task((object obj) => {
                string taskName = (string)obj;
                writeToConsole(10, taskName, ConsoleColor.Cyan);
            }, "T3");

            t1.Start();
            t2.Start();
            t3.Start();

            Thread.Sleep(5000);
            Task.WaitAll(t1, t2, t3);
            var result = t1.Result;
            Console.WriteLine("Press any key");
            Console.WriteLine($"Result returned from task1: {result}");
            Console.ReadKey();
        }

        static async Task<string> fetchDataAsync()
        {
                using (var httpClient = new HttpClient())
                {
                    string result = await httpClient.GetStringAsync("http://localhost:4000/courses/66cc289e-6de9-49b2-9ca7-8b4f409d6467");
                    Console.Write($"Result from API: {result}");
                    return result;
                }
        }

        static void fetchData()
        {
            Task fetchDataTask = fetchDataAsync();
            try
            {
                Task.WaitAll(fetchDataTask);
            }
            catch (AggregateException aggrEx)
            {
                Console.Error.WriteLine($"AggregateException: {aggrEx.Message}");
                Console.Error.WriteLine($"BaseException: {aggrEx.GetBaseException().Message}");
            }
        }


        static void Main1(string[] args)
        {
            // writeRandomColorAsync();
            // Synchronized.writeColor();
            // asyncAwaitExample();
            fetchData();
        }
    }
}