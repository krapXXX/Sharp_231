using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class NetworkingDemo
    {
        public async Task Run()
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
            Task<String> contentTask = response.Content.ReadAsStringAsync();

            Console.WriteLine(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");

            Console.WriteLine($"HTTP/{response.Version} {((int)response.StatusCode)} {response.ReasonPhrase}");
            foreach (var header in response.Headers)
            {
                Console.WriteLine("{0}: {1}", header.Key, String.Join(", ", header.Value));
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
            String requestBody = getRequest.Result;
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
