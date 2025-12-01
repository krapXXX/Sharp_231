using Microsoft.Data.SqlClient;
using Sharp_231.Data.Attributes;
using Sharp_231.Data.Dto;
using Sharp_231.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data
{
    public enum CompareMode
    {
        ByChecks,
        ByQuantity,
        ByMoney
    }
    internal class DataAccessor
    {
        public readonly SqlConnection connection;
         
        #region About Properties

        public int Prop0 { get; set; }  // автозгенеровані аксесори
        // тільки для читання – розрахункові дані на кшталт довжини вектора чи поточного часу
        public int Prop1 { get => 10; }
        // тільки для запису – сідуювання, виведення (запуск процесів через присвоєння)
        private int _prop2;
        public int Prop2 { set { _prop2 = value; } }
        // з різними аксесорами
        public int Prop3 { get; private set; }
        public int Prop4 { get; init; }
        private int _prop5;
        public int Prop5
        {
            get { return _prop5; }
            set
            {
                if (value != _prop5)   // Prop5 = 10  --> set(value = 10)
                {
                    _prop5 = value;
                }
            }
        }


        #endregion


        public DataAccessor()
        {
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\arina\source\repos\Sharp_231\Data\Database1.mdf;Integrated Security=True";
            this.connection = new(connectionString);
            try
            {
                this.connection.Open();   // підключення необхідно відкривати окремою командою
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }
        public T FromReader<T>(SqlDataReader reader)
        {
            // Generic – узагальнене програмування / на заміну шаблонному програмуванню (template)
            var t = typeof(T);                                 // Дані про тип, серед яких конструктори, властивості тощо
            var ctr = t.GetConstructor(Type.EmptyTypes);       // Знаходимо конструктор без параметрів ([])

            T res = (T)ctr!.Invoke(null);                      // Викликаємо конструктор, будуємо об'єкт

            foreach (var prop in t.GetProperties())            // Перебираємо усі властивості об'єкту (типу)
            {                                                  // та намагаємось зчитати з даних (reader)
                try                                            // таке ж ім'я поля
                {
                    object data = reader.GetValue(reader.GetOrdinal(prop.Name));
                    // Зчитуємо дані, якщо їх немає — буде виняток

                    if (data.GetType() == typeof(decimal))     // Якщо тип даних decimal, то перетворюємо
                    {                                           // до double (для внутрішньої сумісності)
                        prop.SetValue(res, Convert.ToDouble(data));
                    }
                    else
                    {
                        prop.SetValue(res, data);               // Для інших випадків — переносимо дані до
                    }                                           // підсумкового об'єкту res
                }
                catch                                           // Ігноруємо помилки читання полів, які не збігаються
                {
                }
            }

            return res;                                         // Повертаємо побудований об'єкт
        }
        public T ExecuteScalar<T>(string sql, Dictionary<string, object>? sqlParams = null)
        {
            using SqlCommand cmd = new(sql, connection);
            foreach (var param in sqlParams ?? [])
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();     // Reader – ресурс для передачі даних
                reader.Read();                                        // Читаємо перший рядок
                return FromReader<T>(reader);                         // Конвертуємо через generic mapper
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }
        private List<T> ExecuteList<T>(string sql, Dictionary<string, object>? sqlParams = null)
        {
            List<T> res = new();                                   // Порожній список для результатів

            using SqlCommand cmd = new(sql, connection);
            foreach (var param in sqlParams ?? [])
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();  // Reader – ресурс для передачі даних

                while (reader.Read())                              // читаємо по одному рядку, доки є результати
                {
                    res.Add(FromReader<T>(reader));                // перетворюємо через generic mapper
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }

            return res;                                            // Повертаємо зібрані результати
        }
        

        public List<Product> GetProducts()
        {
            return ExecuteList<Product>("SELECT * FROM Products");
        }
        public List<Department> GetDepartments()
        {
            return ExecuteList<Department>("SELECT * FROM Departments");
        }
        public List<Manager> GetManagers()
        {
            return ExecuteList<Manager>("SELECT * FROM Managers");
        }
        public List<News> GetNews()
        {
            return ExecuteList<News>("SELECT * FROM News");
        }
        //варіант з генератором
        public IEnumerable<Department> EnumDepartments() => EnumAll<Department>();
        public IEnumerable<Product> EnumProducts() => EnumAll<Product>();
        public IEnumerable<Manager> EnumManagers() => EnumAll<Manager>();
        public IEnumerable<Sale> EnumSales(int limit = 100)
        {
            String sql = "SELECT * FROM Sales";
            using SqlCommand cmd = new(sql, connection);
            SqlDataReader? reader;
            try { reader = cmd.ExecuteReader(); }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
            try
            {
                while (reader.Read())   // читаємо по одному рядку доки є результати
                {
                    yield return FromReader<Sale>(reader);

                    // код відновлюється з місця, на якому був зупинений, тобто після yield return
                    limit -= 1;
                    if (limit == 0)
                    {
                        yield break;   // оператор переривання (зупинки) генератора
                    }
                }
            }
            finally
            {
                reader.Dispose();
            }

        }
        public IEnumerable<News> EnumNews() => EnumAll<News>();
        public List<T> GetAll<T>()
        {
            var t = typeof(T);
            string tableName = "";
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            if (attr != null)
            {
                tableName = attr.Value;
            }
            else
            {
                tableName = t.Name + "s";
            }
            return ExecuteList<T>($"SELECT * FROM {tableName}");
        }
        public IEnumerable<T> EnumAll<T>()
        {
            var t = typeof(T);

            // визначаємо назву таблиці
            string tableName;
            var attr = t.GetCustomAttribute<TableNameAttribute>();
            if (attr != null)
                tableName = attr.Value;
            else
                tableName = t.Name + "s";

            string sql = $"SELECT * FROM {tableName}";
            using SqlCommand cmd = new(sql, connection);

            SqlDataReader? reader;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }

            while (reader.Read())
            {
                yield return FromReader<T>(reader);
            }

            reader.Dispose();
        }



        public void Install()
        {
            InstallProducts();
            InstallDepartments();
            InstallSales();
            InstallNews();
            InstallAccess();
        }
        private void InstallSales()
        {
            String sql = "CREATE TABLE Sales(" +
                "Id           UNIQUEIDENTIFIER PRIMARY KEY," +
                "ManagerId UNIQUEIDENTIFIER NOT NULL," +
                "ProductId UNIQUEIDENTIFIER NOT NULL," +
                "Quantity INT NOT NULL DEFAULT 1," +
                "Moment    DATETIME2        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallDepartments()
        {
            String sql = "CREATE TABLE Departments(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        private void InstallProducts()
        {
            String sql = "CREATE TABLE Products(" +
                "Id    UNIQUEIDENTIFIER PRIMARY KEY," +
                "Name  NVARCHAR(64)     NOT NULL," +
                "Price DECIMAL(14,2)    NOT NULL)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();   // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        public void InstallNews()
        {
            String sql = "CREATE TABLE News(" +
    "Id        UNIQUEIDENTIFIER PRIMARY KEY," +
    "AuthorId  UNIQUEIDENTIFIER NOT NULL," +
    "Title     NVARCHAR(256) NOT NULL," +
    "Content   NVARCHAR(MAX) NOT NULL," +
    "Moment    DATETIME2 NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();    // без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}: ", ex.Message, sql);
            }

        }
        public void InstallAccess()
        {
            string sql = @"
        create table Accesses(
            Id uniqueidentifier primary key,
            ManagerId uniqueidentifier not null,
            Login nvarchar(64) not null unique,
            Salt nvarchar(64) not null,
            Dk nvarchar(128) not null,
            foreign key (ManagerId) references Managers(Id)
        )";

            using SqlCommand cmd = new(sql, connection);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Accesses table created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create Accesses: " + ex.Message);
            }
        }


        public void Seed()
        {
            SeedProducts();
            SeedDepartments();
            SeedManagers();
            SeedNews();
            SeedAccesses();
        }
        public void SeedProducts()
        {
            String sql = "Insert into Products values" +
                "('1E7F21A1-237B-427B-BDED-1B1B32639E48',N'Samsung galaxy s24 Ultra',20000.00)," +
                "('ADE91C53-C314-4CFC-8B4A-D24F19E35646',N'iPhone 15 pro',30000.00)," +
                "('BC1FA982-51DD-41E9-8130-D7F0C08E07B7',N'Samsung s26',45000.00)," +
                "('E3AD30F6-5FD7-4F58-8EAD-1B5E964750DA',N'OpePlus Pro',12000.00)," +
                "('80AE1DB7-097B-4DE1-BB23-36A08E49A825',N'Google Pixel 8 Pro',8000.00)," +
                "('D4277097-7E80-4C56-9F75-202F014930A0',N'iPhone 17 Air',33000.00)," +
                "('DB3B716C-2AFA-4990-9D3A-D87540E8C574',N'Poco Plus Max',25000.00)," +
                "('1BF31902-7C21-4612-A91F-9FBAC75A1B27',N'Asus Rog Phone 3',17000.00)," +
                "('F16E0815-F613-44E3-8B52-297E1F6F9780',N'Motorola 2020',27000.00)," +
                "('E2146763-0B08-435F-93B5-D204123AC93C',N'Honor Magic 7 Pro',19000.00)," +
                "('B4B2B7BD-FD5A-4AC9-9DB6-4297212DA89F',N'Xiaomi Mi 14 Ultra',28000.00)," +
                "('C090762F-E6C4-4EBC-8D43-2962944131C8',N'Nokia X90 Max',9000.00)," +
                "('B10F5B9C-0025-4C34-B910-833EABC65D9B',N'Sony Xperia Z9 Premium',22000.00)," +
                "('FB8F4579-73B5-45BB-A3A1-7D60740BCE4E',N'Huawei P70 Pro',26000.00)," +
                "('609E794E-2913-4377-A325-EB996B5FDEB3',N'Realme GT 7 Neo',15000.00)";

            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void SeedDepartments()
        {
            String sql = "Insert into Departments values" +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD',N'Marketing')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650',N'Advertisement')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A',N'Sales')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45',N'IT')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B',N'Security')";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void SeedManagers()
        {
            String sql = "Insert into Managers(DepartmentID, ID,Name,WorksFrom) values" +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','57D1DD41-5B5E-47D8-BB2C-3433D4CD9BE8',N'Daniel Harris','2001-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','FE2D64B3-C943-43B3-90BD-9806C09756C9',N'Sofia Martinez','2002-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','EAE8E1B0-5D14-4542-B7E0-9473ABAE29F0',N'Lucas Bennett','2004-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','2D2B009E-66BA-4802-AEA3-52BAEAB4D459',N'Emma Novak','2005-01-01')," +
                "('25F34D1F-23DD-4FDE-BC7B-AA5823F373DD','03A876E6-B5F8-46EF-A3D8-07EC5A28C31A',N'Matteo Ricci','2006-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','46392AD2-86FF-4AD2-BD8E-9D4395C623D4',N'Aisha Khan','2003-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','2D47C786-04EC-4069-B9C5-FF4DFB8A6994',N'Oliver Stein','2001-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','0117882E-CFF2-4B22-A515-DE49EC3C60DE',N'Mila Petrova','2013-01-01')," +
                "('674DBCB8-4F90-4ED5-AA2A-898D35A05650','C210A5EE-B065-41B4-BBF1-64B27AF55733',N'Ethan Clarke','2011-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','307B9B90-7BFF-4D0D-9E38-424C70281C8A',N'Isabella Rossi','2007-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','E8200526-27ED-40FA-8E56-19B2C54F076B',N'Noah Williams','2019-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','3D2160E8-DC3C-46DE-B883-2CE19FB1E138',N'Chloe Lefevre','2018-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','31AD41B5-17D5-4248-8F09-6890C90D705D',N'Liam O’Connor','2017-01-01')," +
                "('89E33CC6-92F3-4ED5-B2B4-2B41F395F47A','860921D6-284E-46AF-ABA6-AD5539AE7FBC',N'Hana Suzuki','2016-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','985DEF3B-2F40-4A06-B8DD-FE1C46DD4266',N'Gabriel Costa','2015-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','9B88E497-88E0-44DC-B805-9A0A901BD3AC',N'Amira Hassan','2014-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','F0D5D6D6-FC59-4DF4-AE11-CE4F2324499A',N'Viktor Kovalenko','2013-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','58B557D6-56E1-4FCF-B34A-A48B2AC096F2',N'Sara Johansson','2020-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','99A36B74-04F2-4F3C-9CCD-E6431E8AE413',N'Adam Chen','2005-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','0CD7F24D-CAC6-4A82-8CBC-40D9C062385A',N'Layla Carter','2008-01-01')," +
                "('80A345E3-BB65-4282-A564-619A1F627D45','8C62C335-C744-4110-B1E5-71BEDAA554CD',N'Julian Weber','2009-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','3D91306F-2706-4C90-B61B-BC4827DA1677',N'Daria Sokolova','2011-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','3282A168-0FF9-4EEC-8A77-1589AF76F8A2',N'Marco Almeida','2010-01-01')," +
                "('2D4995BC-0C44-4A0F-BB65-5DD0EDC3680B','2C28822B-C9EF-48E1-B53D-F0E1A0D2DA8E',N'Nora Schultz','2021-01-01')";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void SeedNews()
        {
            String sql = "INSERT INTO News VALUES" +
            "('ADE86F29-93AE-404E-B91C-FBA275DAA5A3','2D47C786-04EC-4069-B9C5-FF4DFB8A6994', N'What happens to your body if you only eat fruit?',   N'When it comes to dieting, there is no shortage of options to try, from plant-based to keto. Occasionally, people try the fruitarian diet, which involves eating primarily fruits. Even Apple founder Steve Jobs dabbled with this way of eating. But what happens to your body if you only eat fruit? While only eating whole, natural foods from the earth sounds healthy, this diet can cause many health problems.', '2025-11-22')," +
            "('A4A74778-6683-40EA-923A-9EC85ED8FC34','0117882E-CFF2-4B22-A515-DE49EC3C60DE', N'Wife wanted!',                                       N'Aristocrat, 79, launches bid to find a Lady who is 20 years younger and can fire a gun', '2025-11-23')," +
            "('93AF9319-A335-4384-B4AC-61F06BC78B36','C210A5EE-B065-41B4-BBF1-64B27AF55733', N'The most beautiful cars ever made',                  N'We now have more than 120 years’ worth of cars to enjoy now – some memorably ugly ones too – and that, very obviously, makes it ever harder for new models to break into this list which we produce every few years', '2025-11-24')";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed: {0}\n{1}", ex.Message, sql);
            }
        }
        public void SeedAccesses()
        {
            string sql =
                "INSERT INTO Accesses VALUES" +
                "('62B1B041-D9CE-411F-8552-F5E0CA63F28A','57D1DD41-5B5E-47D8-BB2C-3433D4CD9BE8',N'Daniel111',N'salt1',N'hash1')," +
                "('47FDBEDE-DBEC-42CC-A1FB-FE1447AAD8C1','FE2D64B3-C943-43B3-90BD-9806C09756C9',N'Sofia111',N'salt2',N'hash2')," +
                "('917ECF95-4A80-42A4-9D2F-79BA75169621','EAE8E1B0-5D14-4542-B7E0-9473ABAE29F0',N'Lucas111',N'salt3',N'hash3')," +
                "('5F996A29-4BFF-4290-8DFB-192BD9C4EA4B','2D2B009E-66BA-4802-AEA3-52BAEAB4D459',N'Emma111',N'salt4',N'hash4')," +
                "('C190EA00-B498-4EB6-AD48-0A487FB355BD','03A876E6-B5F8-46EF-A3D8-07EC5A28C31A',N'Matteoty111',N'salt5',N'hash5')";

            using SqlCommand cmd = new(sql, connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }
        public void FillSales()
        {
            String sql = "Insert into Sales(id, managerId,productId, quantity, moment)values" +
                "(Newid()," +
                "(select top 1 id from managers order by newid())," +
                "(select top 1 id from products order by newid())," +
                "(select 1+abs(CHECKSUM (newid()))% 10)," +
                "(select dateadd(minute, abs(CHECKSUM (newid()))% 525600, '2024-01-01'))" +
                ")";


            using SqlCommand cmd = new(sql, connection);
            try
            {
                for (int i = 0; i < 1e5; i++)
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: ", ex.Message, sql);
            }
        }


        public void PrintNP()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Price))
            {
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
            }
        }
        public void PrintDesc()
        {
            foreach (var p in GetProducts().OrderByDescending(p => p.Price))
            {
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
            }
        }
        public void PrintAlphabet()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Name))
            {
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
            }
        }
        public void PrintLoser()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Price).Take(3))
            {
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
            }
        }
        public void PrintTop()
        {
            foreach (var p in GetProducts().OrderByDescending(p => p.Price).Take(3))
            {
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
            }
        }
        public void PrintRandom()
        {
            var rnd = new Random();

            var randomProducts = GetProducts()
                .OrderBy(p => rnd.Next())
                .Take(3)
                .ToList();

            foreach (var p in randomProducts)
            {
                Console.WriteLine("{0} -- {1:F2}", p.Name, p.Price);
            }
        }
        public Product RandomProduct()
        {
            return ExecuteScalar<Product>(
                "SELECT TOP 1 * FROM Products ORDER BY NEWID()"
            );
        }
        public Department RandomDepartment()
        {
            return ExecuteScalar<Department>(
                "SELECT TOP 1 * FROM Departments ORDER BY NEWID()"
            );
        }
        public Manager RandomManager()
        {
            return ExecuteScalar<Manager>(
                "SELECT TOP 1 * FROM Managers ORDER BY NEWID()"
            );
        }


        public int GetSalesByMonth(int month, int year = 2025)
        {
            //параметризовані запити - з відокремленням паарметрів від sql тексту
            String sql = $"select count(*) from sales where moment between @date and dateadd(month, 1, @date)";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(year, month, 1));
            try
            {
                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch
            {
                throw;
            }
        }
        public void GetYearStatistics(int year)
        {
            string? sql = @$"
            select 
            year(s.moment),
            month(s.moment),
            count (*) 
            from sales s 
            where year(s.moment) = {year}
            group by 
             month(s.moment),
             year(s.moment)
            order by 1,2
            ";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0}, {1}, {2}", reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed {0}\n{1}: " + ex.Message, sql);
            }
        }
        public void MonthlySalesByManagersSql(int month, int year = 2025)
        {

            string sql = $@"
            select
                MAX(M.Name),        
                COUNT(S.Id)        
            from Sales S
            join Managers M on S.ManagerId = M.Id
            where Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
            group by 
                M.Id
            order by 
                2 desc
";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(year, month, 1));

            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0} -- {1}", reader[0], reader[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}\n{1}", ex.Message, sql);
                throw;
            }
        }
        public List<SaleModel> MonthlySalesByManagersOrm(int month, int year = 2025)
        {
            return ExecuteList<SaleModel>($@"
            SELECT
                MAX(M.Name) AS [{nameof(SaleModel.ManagerName)}],
                COUNT(S.Id) AS [{nameof(SaleModel.Sales)}]
            FROM 
                Sales S
                JOIN Managers M ON S.ManagerId = M.Id
            WHERE
                Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
            GROUP BY
                M.Id
            ORDER BY
                2 DESC
            ",
            new()
            {
                ["@date"] = new DateTime(year, month, 1)
            });
        }
        public (int current, int previous) GetSalesInfoByMonth(int month)
        {
            int currentYear = DateTime.Now.Year;
            int previousYear = currentYear - 1;

            string sql = @"
            select count(*) 
            from sales 
            where moment between @date and dateadd(month, 1, @date)";

            int GetCount(int y)
            {
                using SqlCommand cmd = new(sql, connection);
                cmd.Parameters.AddWithValue("@date", new DateTime(y, month, 1));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }

            try
            {
                int curr = GetCount(currentYear);
                int prev = GetCount(previousYear);
                return (curr, prev);
            }
            catch
            {
                throw;
            }
        }
        public List<(string ProductName, int Sales)> MonthlySalesByProductsOrm(int month, int year = 2025)
        {
            var result = new List<(string, int)>();

            string sql = @"
            SELECT
                MAX(P.Name),
                COUNT(S.Id)
            FROM 
                Sales S
                JOIN Products P ON S.ProductId = P.Id
            WHERE
                Moment BETWEEN @date AND DATEADD(MONTH, 1, @date)
            GROUP BY
                P.Id
            ORDER BY
                2 DESC
            ";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@date", new DateTime(year, month, 1));

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int sales = reader.GetInt32(1);
                result.Add((name, sales));
            }

            return result;
        }
        public IEnumerable<ProdSaleModel> Top3DailyProducts(CompareMode compareMode)
        {
            return null;
        }


        /*
         orm - перетворення з необ'єктного в об'єктне представлення(json)
         linq - набір інмтрументів у вигляді методів розширення, для колекцій(написання query на шарпі)
         */
    }

}