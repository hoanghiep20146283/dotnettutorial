using System.Diagnostics;

class CancelableTaskExample
{
    static readonly CancellationTokenSource s_cts = new CancellationTokenSource();

    static readonly HttpClient s_client = new HttpClient
    {
        MaxResponseContentBufferSize = 1_000_000
    };

    public static async Task Main()
    {
        Console.WriteLine("Application started.");
        Console.WriteLine("Press the ENTER key to cancel...\n");

        Task cancelTask = Task.Run(() =>
        {
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press the ENTER key to cancel...");
            }

            Console.WriteLine("\nENTER key pressed: cancelling get course info...\n");
            s_cts.Cancel();
        });

        Task getCourseInfoTask = GetCourseInfoAsync();

        Task finishedTask = await Task.WhenAny(new[] { cancelTask, getCourseInfoTask });
        if (finishedTask == cancelTask)
        {
            // wait for the cancellation to take place:
            try
            {
                await getCourseInfoTask;
                Console.WriteLine("Get course info completed before cancel request was processed.");
            }
            catch (AggregateException aggrEx)
            {
                Console.Error.WriteLine($"AggregateException: {aggrEx.Message}");
                Console.Error.WriteLine($"BaseException: {aggrEx.GetBaseException().Message}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Download task has been cancelled.");
            }
        }

        Console.WriteLine("Application ending.");
    }

    static async Task GetCourseInfoAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        int total = 0;
        int contentLength = await ProcessUrlAsync("http://localhost:4000/courses/66cc289e-6de9-49b2-9ca7-8b4f409d6467", s_client, s_cts.Token);
        total += contentLength;

        stopwatch.Stop();

        Console.WriteLine($"\nTotal bytes returned:  {total:#,#}");
        Console.WriteLine($"Elapsed time:          {stopwatch.Elapsed}\n");
    }

    static async Task<int> ProcessUrlAsync(string url, HttpClient client, CancellationToken token)
    {
        HttpResponseMessage response = await client.GetAsync(url, token);
        byte[] content = await response.Content.ReadAsByteArrayAsync(token);
        Console.WriteLine($"Course Info Response: {System.Text.Encoding.UTF8.GetString(content)}");
        Console.WriteLine($"{url,-60} {content.Length,10:#,#}");

        return content.Length;
    }
}