using Sharp_231.Users.Dal.Entities;
using SharpKnP321.Users.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sharp_231.Users
{
    internal record MenuItem(char Key, string Title, Action? action)
    {
        public override string ToString()
        {
            return $"{Key} - {Title}";
        }
    };
    internal class UsersDemo
    {
        private DataAccessor _accessor;
        private MenuItem[] menu => [
    new MenuItem('i', "Інсталювати таблиці БД", () => _accessor.Install()),
    new MenuItem('h', "Переінсталювати таблиці БД", () => _accessor.Install(isHard: true)),
    new MenuItem('1', "Реєстрація нового користувача", SignUp),
    new MenuItem('2', "Вхід до системи (автентифікація)", SignIn),
    new MenuItem('0', "Вихід", null),
];
        public void Run()
        {
            try
            {
                _accessor = new();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            MenuItem? selectedItem;

            do
            {
                foreach (var item in menu)
                {
                    Console.WriteLine(item);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();

                selectedItem = menu.FirstOrDefault(item => item.Key == keyInfo.KeyChar);

                if (selectedItem == null)
                {
                    Console.WriteLine("Нерозпізнаний вибір");
                }
                else
                {
                    selectedItem.action?.Invoke();
                }

            } while (selectedItem == null || selectedItem.action != null);
        }
        private void SignUp()
        {
            UserData userData = new();
            bool isEntryCorrect;
            String? userPassword;

            Console.WriteLine("Реєстрація нового користувача (порожній ввід - вихід)");

            // --- UserName ---
            do
            {
                Console.Write("Повне Ім'я: ");
                userData.UserName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userData.UserName))
                    return;

                userData.UserName = userData.UserName.Trim();
                isEntryCorrect = userData.UserName.Length >= 2;

                if (!isEntryCorrect)
                {
                    Console.WriteLine("Занадто коротке, відкоригуйте");
                }

            } while (!isEntryCorrect);

            // --- UserEmail ---
            do
            {
                Console.Write("E-mail: ");
                userData.UserEmail = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userData.UserEmail))
                    return;

                userData.UserEmail = userData.UserEmail.Trim();

                isEntryCorrect = Regex.IsMatch(
                    userData.UserEmail,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                );

                if (!isEntryCorrect)
                {
                    Console.WriteLine("Не відповідає формату, відкоригуйте");
                }

            } while (!isEntryCorrect);

            do
            {
                Console.Write("Пароль: ");
                userPassword = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userPassword))
                    return;

                userPassword = userPassword.Trim();

                isEntryCorrect = ValidatePassword(userPassword);

            } while (!isEntryCorrect);

            Console.WriteLine("Реєстрацію завершено");
        }
        private void SignIn()
        {
            Console.WriteLine("SignIn");
        }
        private bool ValidatePassword(String password)
        {
            bool isValid = true;

            if (password.Length < 6)
            {
                Console.WriteLine("Пароль повинен містити щонайменше 6 символів");
                isValid = false;
            }

            if (!password.Any(Char.IsDigit))
            {
                Console.WriteLine("Пароль повинен містити щонайменше одну цифру");
                isValid = false;
            }

            if (!password.Any(Char.IsLower))
            {
                Console.WriteLine("Пароль повинен містити щонайменше одну малу літеру");
                isValid = false;
            }

            if (!password.Any(Char.IsUpper))
            {
                Console.WriteLine("Пароль повинен містити щонайменше одну велику літеру");
                isValid = false;
            }

            if (!password.Any(c => !Char.IsLetterOrDigit(c)))
            {
                Console.WriteLine("Пароль повинен містити щонайменше один спецсимвол");
                isValid = false;
            }

            return isValid;
        }
    }
}
/* Робота з користувачами: реєстрація, автентифікація, авторизація

 * UserData        UserAccess        AccessTokens
 * --------        ----------        ------------
 * UserId          AccessId          TokenId
 * UserName        AccessLogin       AccesId
 * UserEmail       AccessSalt        TokenIat
 * UserSalt        AccessDk          TokenExp
 *
 */