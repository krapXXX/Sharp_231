using Dapper;
using Microsoft.Data.SqlClient;
using Sharp_231.Users.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SharpKnP321.Users.Dal
{
    internal class DataAccessor
    {
        private SqlConnection connection;

        public DataAccessor()
        {
            String settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                throw new Exception("Не знайдено файл конфігурації. Прочитайте Readme");
            }
            var settings = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(settingsFilename)
            );
            String userDb;
            try
            {
                var csSection = settings.GetProperty("ConnectionStrings");
                userDb = csSection.GetProperty("UserDb").GetString()!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка визначення конфігурації: {ex.Message}");
            }
            connection = new(userDb);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка підключення БД: {ex.Message}");
            }
        }
        public void SignUp(UserData userData)
        {
            if (userData.UserId == default)
            {
                userData.UserId = Guid.NewGuid();
            }

            userData.UserEmailCode = string.Join("",
                Enumerable.Range(0, 6)
                          .Select(_ => Random.Shared.Next(0, 10).ToString())
            );

            // подальша логіка збереження користувача...
        }
        public void Install(bool isHard = false)
        { 
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS UserData;");
            }
            connection.Execute(@"CREATE TABLE UserData (
                UserId        UNIQUEIDENTIFIER PRIMARY KEY,
                UserName      NVARCHAR(128)    NOT NULL,
                UserEmail     NVARCHAR(256)    NOT NULL,
                UserEmailCode VARCHAR(16)          NULL,
                UserDelAt     DATETIME2            NULL
           ) ");
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS UserAccess;");
            }
            connection.Execute(@"CREATE TABLE UserAccess (
                AccessId      UNIQUEIDENTIFIER PRIMARY KEY,
                UserId        UNIQUEIDENTIFIER NOT NULL,
                RoleId        UNIQUEIDENTIFIER     NULL,
                AccessLogin   NVARCHAR(64)     NOT NULL,
                AccessSalt    CHAR(16)         NOT NULL,
                AccessDk      CHAR(32)             NULL
            )");
            if (isHard)
            {
                connection.Execute("DROP TABLE IF EXISTS AccessToken;");
            }
            connection.Execute(@"
CREATE TABLE AccessToken (
    TokenId UNIQUEIDENTIFIER PRIMARY KEY,
    AccessId UNIQUEIDENTIFIER NOT NULL,
    TokenIat DATETIME2 NOT NULL,
    TokenExp DATETIME2 NOT NULL
)");
           
        }
    }
}
