using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Dict
{
    internal class DictDemo
    {
        private const String filename = "dictionary.json";
        private Dictionary<String, String> dictionary;
        class MenuItem
        {
            public String Title { get; set; } = null;
            public char Key { get; set; }
            public Action Action { get; set; } = null!;
            public override string ToString() => $"{Key}. {Title}";
        }
        private MenuItem[] menuItems;
        public DictDemo()
        {
            try
            {
                dictionary = JsonSerializer.Deserialize<Dictionary<String, String>>(
                    File.ReadAllText(filename))!;
                Console.Write("Downloaded:");
            }
            catch
            {
                dictionary = new()
            {     //Key    //Value
                { "apple", "яблуко" },//Pair
                { "pear",  "груша"  },
                { "plum",  "слива"  },
                { "peach", "персик" },
                { "grape", "виноград" }
            };
                Console.Write("Added:");
            }
            Console.WriteLine(dictionary.Count + " words");
            menuItems = [
                new(){Key ='0',Title = "Вихід з програми", Action = ()=>throw new Exception() },
            new(){Key ='1', Title = "Переклад слова з української до англійської", Action = Uk2En },
            new(){Key = '2', Title = "Переклад слова з англійської до української", Action = En2Uk },
            new(){Key = '3', Title = "Додати слово до словника", Action = AddWord },
            new(){Key = '4', Title = "Вивести все", Action = PrintDict },
            new(){Key = '5', Title = "Редагувати існуюче слово", Action = EditWord },
            new(){Key = '6', Title = "Видалити слово за англійським словом", Action = DeleteByEnglish },
new(){Key = '7', Title = "Видалити слово за українським словом", Action = DeleteByUkrainian }

            ];
        }
        private void SaveDictionary()
        {
            File.WriteAllText(filename,
                JsonSerializer.Serialize(dictionary,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                }));
        }
        public void Run()
        {
            try
            {
                while (true)
                {
                    Menu();
                }
            }
            catch { }
            finally
            {
                SaveDictionary();
            }
        }
        public void Menu()
        {
            ConsoleKeyInfo key;
            MenuItem? selectedItem;
            do
            {
                Console.WriteLine("Словник");
                foreach (MenuItem item in menuItems)
                {
                    Console.WriteLine(item);
                }
                key = Console.ReadKey();
                selectedItem = menuItems
                .FirstOrDefault(item => item.Key == key.KeyChar);
                Console.WriteLine();

                if (selectedItem == null)
                {
                    Console.WriteLine(" - Невірний вибір, спробуйте ще раз.");
                }
                else
                {
                    selectedItem!.Action();
                }
            }
            while (selectedItem == null);
        }
        private void AddWord()
        {
            Console.Write("Введіть слово англійською: ");
            String en = Console.ReadLine()!.Trim();
            Console.Write("Введіть слово українською: ");
            String uk = Console.ReadLine()!.Trim();
            try
            {
                dictionary.Add(en, uk);
                Console.WriteLine("Слово додано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Виникла помилка: " + ex.Message);
            }
        }
        private void Uk2En()
        {
            Console.Write("Введіть слово українською: ");
            String? word;
            do
            {
                word = Console.ReadLine();
            } while (String.IsNullOrEmpty(word));
            var tr = dictionary
                 .Where(pair => pair.Value == word);
            if (tr.Any())
            {
                foreach (var pair in tr)
                {
                    Console.WriteLine("{0} -- {1}", word, pair.Key);
                }
            }
            else
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                SuggestClosest(word);
            }
        }
        private void En2Uk()
        {
            Console.Write("Введіть слово англійською: ");
            String? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (String.IsNullOrEmpty(word));
            if (dictionary.ContainsKey(word))
            {
                Console.WriteLine("{0} -- {1}", word, dictionary[word]);
            }
            else
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                SuggestClosest(word);
            }
        }
        private void PrintDict()
        {
            const int pageSize = 2;
            int total = dictionary.Count;
            int index = 0;

            while (index < total)
            {
                Console.Clear();
                Console.WriteLine("Use → arrow for next page, ← for previous, ESC to exit.");

                foreach (var pair in dictionary.Skip(index).Take(pageSize))
                {
                    Console.WriteLine("{0} -- {1}", pair.Key, pair.Value);
                }

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.RightArrow)
                {
                    if (index + pageSize < total)
                        index += pageSize;
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    if (index - pageSize >= 0)
                        index -= pageSize;
                }
                else if (key == ConsoleKey.Escape)
                {
                    break;
                }

            }
        }
        private void EditWord()
        {
            Console.Write("Введіть англійською слово для зміни: ");
            String? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (String.IsNullOrEmpty(word));

            if (!dictionary.ContainsKey(word))
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            Console.WriteLine("Поточний переклад: {0}", dictionary[word]);
            Console.Write("Введіть новий переклад: ");

            String? newWord;
            do
            {
                newWord = Console.ReadLine()?.Trim();
            } while (String.IsNullOrEmpty(newWord));

            dictionary[word] = newWord;
            Console.WriteLine("Запис оновлено: {0} -- {1}", word, newWord);
        }
        private void DeleteByEnglish()
        {
            Console.Write("Введіть слово для видалення англійською: ");
            String? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (String.IsNullOrEmpty(word));

            if (!dictionary.ContainsKey(word))
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            dictionary.Remove(word);
            Console.WriteLine("Слово '{0}' успішно видалено!", word);
        }
        private void DeleteByUkrainian()
        {
            Console.Write("Введіть слово для видалення українською: ");
            String? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (String.IsNullOrEmpty(word));

            var matches = dictionary
                .Where(pair => pair.Value == word)
                .ToList();

            if (!matches.Any())
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            foreach (var pair in matches)
            {
                dictionary.Remove(pair.Key);
                Console.WriteLine("Видалено: {0} -- {1}", pair.Key, pair.Value);
            }
        }
        private void SuggestClosest(string word)
        {
            Console.WriteLine("Можливо, ви мали на увазі:");
            var closest = dictionary
                .Select(pair => new
                {
                    Word = pair.Key,
                    LD = LevenshteinDistance(word, pair.Key)
                })
                .OrderBy(x => x.LD)
                .Take(3);

            foreach (var item in closest)
            {
                Console.WriteLine(" - {0}  (distance {1})", item.Word, item.LD);
            }
        }

        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; i++)
                d[i, 0] = i;

            for (int j = 0; j <= m; j++)
                d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,       
                            d[i, j - 1] + 1),      
                        d[i - 1, j - 1] + cost);   
                }
            }
            return d[n, m];
        }


    }
}
