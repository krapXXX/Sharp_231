using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Networking
{
    internal class EmailDemo
    {
        public void Run()
        {
            Console.WriteLine("Робота з електронною поштою. SMTP");
            string settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                Console.WriteLine("Не знайдено файл конфігурації. Перегляньте README");
                return;
            }
            var settings = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(settingsFilename)
                );
            string server, email, password;
            int port;
            bool isSsl;
            try
            {
                var emailSection = settings.GetProperty("Emails");
                var gmailSection = emailSection.GetProperty("Gmail");

                server = gmailSection.GetProperty("Server").GetString()!;
                email = gmailSection.GetProperty("Username").GetString()!;
                password = gmailSection.GetProperty("Password").GetString()!;
                port = gmailSection.GetProperty("Port").GetInt32();
                isSsl = gmailSection.GetProperty("Ssl").GetBoolean();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка визначення конфігурації: {ex.Message}");
                return;
            }
            using SmtpClient smtpClient = new()
            {
                Host = server,
                Port = port,
                EnableSsl = isSsl,
                Credentials = new NetworkCredential(email, password)
            };
            //базова форма - текстові повідомлення
            // smtpClient.Send(email, "min.arrrsss@gmail.com", "Message from Sharp", "Hello, User!");

            //розширена форма
            MailMessage mailMessage = new()
            {
                From = new MailAddress(email),
                IsBodyHtml = true,
                Subject = "Message from Sharp",
                Body = "<html><h1>Шановний користувач!</h1><p>" +
                "Шоби ви були здорові!<p><a style='background:maroon;" +
                "color:snow;border-radius:10px;padding:7px 12px' " +
                "href='https://itstep.org'>Вчитись</a></html>",
            };
            mailMessage.To.Add(new MailAddress("min.arrrsss@gmail.com"));

            smtpClient.Send(mailMessage);
        }
    }
}

/* Робота з електронною поштою. SMTP

 * Організація збереження паролних даних.
 * При роботі з мережевими сервісами доволі часто потрібні паролі, ключі тощо.
 * Основні проблеми виникають при публікації проекту у репозиторії, особливо,
 * відкритого типу (public).
 * Одне з рішень — зміни оточення, проте, це ускладнює поширення проекту.
 * Інше рішення — файли конфігурації: один з паролями та вилучений з репозиторію,
 * другий — зразковий з правильними ключами, але видаленими паролями.
 * - визначаємось з назвою файлу: appsettings.json
 * - вносимо до .gitignore відповідний запис (до створення файлу), зберігаємо зміни
 * - створюємо сам файл, переконуємось, що він не фіксується у змінах репозиторію
 * - заповнюємо файл даними
 * - створюємо копію appsettings_sample.json, у якій видаляємо паролі
 *   (залишаються на *** чи шаблон)
 *   
 *   додаємо до репозиторію інструкцію із встановлення (інсталяції) - README.MD
 *   
 *   
 *   Program            Gmail(server)           UkrNet(Client, box)
 *              SMTP
 *   Send --------------->          --------------->
 *          to: ukr.net                to: ukr.net
 *          from: gmail
 *   
 */