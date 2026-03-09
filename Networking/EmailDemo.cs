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
        private string? server, email, password;
        private int port;
        private bool isSsl;
        private bool isLoaded = false;

        public void Run()
        {
            Console.WriteLine("Working with E-mail. SMTP");

            MailMessage mailMessage = new()
            {
                From = new MailAddress(GetEmail()),
                IsBodyHtml = true,
                Subject = "Message from Sharp",
                Body = "<html><h1>Dear user!</h1><p>" +
                "Be healthy and happy!<p><a style='background:maroon;" +
                "color:snow;border-radius:10px;padding:7px 12px' " +
                "href='https://itstep.org'>Study</a></html>",
            };
            mailMessage.To.Add(new MailAddress("min.arrrsss@gmail.com"));

            try
            {
                SendEmail(mailMessage);
                Console.WriteLine("Message sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-mail sending error: {ex.Message}");
            }
        }

        private void SendEmail(MailMessage mailMessage)
        {
            LoadConfig();

            using SmtpClient smtpClient = new()
            {
                Host = server!,
                Port = port,
                EnableSsl = isSsl,
                Credentials = new NetworkCredential(email, password)
            };

            smtpClient.Send(mailMessage);
        }

        private void LoadConfig()
        {
            if (isLoaded) return;

            string settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                throw new Exception("Configuration file not found. Check README");
            }

            var settings = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(settingsFilename)
            );

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
                throw new Exception($"Configuration error: {ex.Message}");
            }

            isLoaded = true;
        }

        private string GetEmail()
        {
            LoadConfig();
            return email!;
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