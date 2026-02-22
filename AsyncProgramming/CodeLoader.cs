using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class CodeLoader
    {
        public async Task Run()
        {
            Console.Write("Insert the url: ");
            string? url = Console.ReadLine();

            using HttpClient client = new HttpClient();
            Uri uri = new Uri(url!);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            Task<HttpResponseMessage> responseTask = client.SendAsync(request);

            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" request start");

            Task openBrowserTask = OpenBrowserAsync(uri.AbsoluteUri);

            HttpResponseMessage response = await responseTask;
            Task<string> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" request finish");

            Console.WriteLine($"HTTP/{response.Version} {((int)response.StatusCode)} {response.ReasonPhrase}");
            Console.WriteLine();

            string html = await contentTask;

            Console.WriteLine("===== START =====");
            Console.WriteLine(html);
            Console.WriteLine("===== END =====");

            await openBrowserTask;
        }

        private Task OpenBrowserAsync(string url)
        {
            return Task.Run(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });

                    Console.WriteLine($"Opened: {url}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}