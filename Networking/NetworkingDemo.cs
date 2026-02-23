using Sharp_231.AsyncProgramming;
using Sharp_231.Networking.Api;
using Sharp_231.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Networking
{
    internal class NetworkingDemo
    {
        public async Task Run()
        {
            Console.WriteLine("Working with API");

            MoonApi moonApi = new();

            Console.Write("Enter date: ");
            string? input = Console.ReadLine();

            if (!DateOnly.TryParseExact(
                    input,
                    "dd.MM.yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateOnly date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            MoonPhase phase = moonApi.PhaseByDateAsync(date).Result;

            Console.WriteLine("{0} ({1})", phase.PhaseName, phase.Lighting);
        }
        public async Task RunXml()
        {
            Console.WriteLine("NBU exchange rates for a selected date (XML)");

            DateOnly date = ReadPastDateFromConsole();

            string url =
                $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={date:yyyyMMdd}";

            using HttpClient client = new HttpClient();
            string xmlString = await client.GetStringAsync(url);

            List<NbuRateXml> rates = NbuRateXml.ListFromXmlString(xmlString);

            Console.WriteLine();
            Console.WriteLine($"Date: {date:dd.MM.yyyy}");
            Console.WriteLine($"Currencies count: {rates.Count}");
            Console.WriteLine("====================================");

            foreach (NbuRateXml rate in rates)
            {
                Console.WriteLine(rate);
            }
        }

        public async Task RunUrl()
        {
            Console.WriteLine("NBU exchange rates for a selected date");

            DateOnly date = ReadPastDateFromConsole();

            string url =
                $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={date:yyyyMMdd}&json";

            using HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" request start");

            HttpResponseMessage response = await client.SendAsync(request);

            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" request finish");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");
                return;
            }

            string jsonString = await response.Content.ReadAsStringAsync();

            List<NbuRate> rates = NbuRate.ListFromJsonString(jsonString);

            Console.WriteLine();
            Console.WriteLine($"Date: {date:dd.MM.yyyy}");
            Console.WriteLine($"Currencies count: {rates.Count}");
            Console.WriteLine("====================================");

            foreach (NbuRate rate in rates)
            {
                Console.WriteLine(rate);
            }
        }

        private static DateOnly ReadPastDateFromConsole()
        {
            while (true)
            {
                Console.Write("Enter date: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Empty input. Try again.");
                    continue;
                }

                string[] formats = { "dd.MM.yyyy", "yyyy-MM-dd" };

                if (!DateTime.TryParseExact(input.Trim(), formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime dt))
                {
                    Console.WriteLine("Invalid date format.");
                    continue;
                }

                DateOnly date = DateOnly.FromDateTime(dt);
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                if (date >= today)
                {
                    Console.WriteLine("Date must belong to the past.");
                    continue;
                }

                return date;
            }
        }
        public async Task Run2()
        {
            using HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json")
            };
            Task<HttpResponseMessage> responseTask =
     client.SendAsync(request);

            Console.WriteLine("Курси валют НБУ, робота з JSON");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<string> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine("  request finish");
            string jsonString = await contentTask;

            List<NbuRate> rates = NbuRate.ListFromJsonString(jsonString);

            foreach (NbuRate rate in rates)
            {
                Console.WriteLine(rate);
            }

        }
        public async Task RunStep()
        {
            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://itstep.org/"),
            };

            Task<HttpResponseMessage> responseTask = client.SendAsync(request);

            Console.WriteLine("HTTP requests and responses");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<string> contentTask = response.Content.ReadAsStringAsync();

            Console.WriteLine(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");

            Console.WriteLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");
            foreach (var header in response.Headers)
            {
                Console.WriteLine("{0}: {1}", header.Key, string.Join(", ", header.Value));
            }
            Console.WriteLine();
            Console.WriteLine(await contentTask);

        }
        public void RunBody()
        {
            HttpClient client = new();
            Task<string> getRequest = client.GetStringAsync("https://itstep.org/");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" Get Start");
            string requestBody = getRequest.Result;
            Console.WriteLine(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(requestBody);
        }
    }
}
/*
Робота мережею інтернет 
Мережа - сукупність вузлів та зв'яків між ними (каналів зв'язку)
Вузол (node) - активний учасник, що перетворює дані(пк, принтер, телефон, виконавчий пристрій тощо)
вузол ц мережі відрізняється адресою та/або іменем 
Зв'язок - засіб передачі данних між вузлами (дріт. оптоволокно, радіоканал тощо)

HTTP - текстовий транспортний протокол
запит                   відповідь
метод шлях             стасус-код фраза
заголовки - пари ключ: значення\r\n
тіло(довільна інформація), зокрема JSON - текстовий протокол передачі даних

 * CONNECT  — службові
 * TRACE
 *
 * HEAD     — технологічні
 * OPTIONS
 *
 * GET      — одержання даних (читання, Read) — без модифікації системи (без змін)
 * POST     — створення нових елементів (Create)
 * DELETE   
 * PUT      — заміна наявних даних на передані
 * PATCH    — оновлення частини наявних даних
 * 
 * 
 * 
 * 
 */
