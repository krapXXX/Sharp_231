using Microsoft.Data.SqlClient;
using Sharp_231.Data.Dto;
using Sharp_231.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            DataAccessor dataAccessor = new();
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByMoney))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("===================");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByQuantity))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("===================");
            foreach (var m in dataAccessor.Top3DailyProducts(CompareMode.ByChecks))
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("===================");
            foreach (var s in dataAccessor.EnumSales(10))
            {
                Console.WriteLine(s);
            }
           List<Sale>sales= [.. dataAccessor.EnumSales(10)];

        }

        public void Run3()
        {
            DataAccessor dataAccessor = new();

            //foreach (var dep in dataAccessor.GetDepartments())
            //{
            //    Console.WriteLine(dep);

            //    String sql = $"SELECT * FROM Managers M WHERE M.DepartmentId = '{dep.Id}'";
            //    using SqlCommand cmd = new(sql, dataAccessor.connection);
            //    using SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        Console.WriteLine(dataAccessor.FromReader<Manager>(reader));
            //    }
            //}
            Console.WriteLine("=== Departments ===");
            foreach (var p in dataAccessor.EnumAll<Department>())
            {
                Console.WriteLine(p);
            }
            Console.WriteLine("=== Products ===");
            foreach (var p in dataAccessor.EnumAll<Product>())
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("=== Managers ===");
            foreach (var m in dataAccessor.EnumAll<Manager>())
            {
                Console.WriteLine(m);
            }
            Console.WriteLine("=== News ===");
            foreach (var m in dataAccessor.EnumAll<News>())
            {
                Console.WriteLine(m);
            }



        }
        private long Fact(uint n)
        {
            if (n < 2) return 1;
            uint m = n - 1;
            return n * Fact(m);
        }
        public void Run2()
        {

            DataAccessor dataAccessor = new();

            List<Manager> managers = dataAccessor.GetAll<Manager>();
            List<Access> accesses = dataAccessor.GetAll<Access>();

            foreach (var item in managers
    .Join(
        accesses,
        m => m.Id,
        a => a.ManagerId,
        (m, a) => new
        {
            ManagerName = m.Name,
            Login = a.Login
        }))
            {
                Console.WriteLine($"{item.ManagerName} -- {item.Login}");
            }


            //var list = dataAccessor.MonthlySalesByProductsOrm(1);
            //foreach (var item in list)
            //{
            //    Console.WriteLine($"{item.ProductName} -- {item.Sales}");
            //}


            //var (m1, m2) = dataAccessor.GetSalesInfoByMonth(1);
            //Console.WriteLine($"Поточний рік: {m1}, Попередній рік: {m2}");


            // List<Department> departments = dataAccessor.GetAll<Department>();
            // List<Manager> managers = dataAccessor.GetAll<Manager>();

            //foreach (var item in managers
            //  .Join(
            //      departments,
            //      m => m.DepartmentID,
            //      d => d.Id,
            //      (m, d) => new
            //      {
            //          ManagerName = m.Name,
            //          DepartmentName = d.Name,
            //      }
            //  ))
            // {
            //     Console.WriteLine($"{item.ManagerName} -- {item.DepartmentName}");
            // }
            // Console.WriteLine("====================");

            // Console.WriteLine(
            // String.Join("\n ", departments
            //    .GroupJoin(
            //     managers,
            //     d => d.Id,
            //     m => m.DepartmentID,
            //     (d, mans) => new
            //     {
            //         d.Name,
            //         Cnt = mans.Count(),
            //         Employee = String.Join("; ", mans.Select(m => m.Name))
            //     }
            //     )
            //     .OrderByDescending(item => item.Cnt)
            //     .Select(item => String.Format("{0} - ({1} employees): {2} ", item.Name, item.Cnt, item.Employee))
            //     ));

            // вивести назву відділу, кількість співробітників та їх імена через кому
            // Вивести: Імя менеджера -- назва відділу у якому він працює

            //dataAccessor.Install();
            //dataAccessor.Seed();
            // dataAccessor.FillSales();
            //List<Product> products = dataAccessor.GetProducts();

            //Console.WriteLine(dataAccessor.RandomProduct());
            //Console.WriteLine(dataAccessor.RandomDepartment());
            //Console.WriteLine(dataAccessor.RandomManager());

            //Console.WriteLine("\n============PRODUCTS===========");
            //dataAccessor.GetAll<Product>().ForEach(Console.WriteLine);
            //Console.WriteLine("\n==========DEPARTMENTS==========");
            //dataAccessor.GetAll<Department>().ForEach(Console.WriteLine);
            //Console.WriteLine("\n============MANAGERS===========");
            //dataAccessor.GetAll<Manager>().ForEach(Console.WriteLine);
            //Console.WriteLine("\n=============NEWS==============");
            //dataAccessor.GetAll<News>().ForEach(Console.WriteLine);


            //products.ForEach(Console.WriteLine);
            //dataAccessor.PrintNP();
            //Console.WriteLine("====================");
            //dataAccessor.PrintDesc();
            //Console.WriteLine("====================");
            //dataAccessor.PrintAlphabet();
            //Console.WriteLine("=========TOP========");

            //dataAccessor.PrintTop();
            //Console.WriteLine("========LOSER=======");
            //dataAccessor.PrintLoser();

            //Console.WriteLine("=======RANDOM=======");
            //dataAccessor.PrintRandom();

            //String input;

            //Console.WriteLine("Enter the month: ");
            //input = Console.ReadLine();

            //if(int.TryParse(input, out int value))//side effect - змінаЄоточенняЄ - змінна поза иілом функції
            //{
            //    Console.WriteLine(dataAccessor.GetSalesByMonth(value));
            //}
            //else{
            //    Console.WriteLine("Not a number");
            //}

            //Console.WriteLine("Enter the year: ");
            //input = Console.ReadLine();
            //if (int.TryParse(input,out int value))
            //{
            //    dataAccessor.GetYearStatistics(value);
            //}
            //else
            //{
            //    Console.WriteLine("Not a number");
            //}

            //dataAccessor.MonthlySalesByManagersSql(1);
            //Console.WriteLine("====================");
            //dataAccessor.MonthlySalesByManagersOrm(1).ForEach(Console.WriteLine);

        }
        public void Run1()
        {
            Console.WriteLine("Data Demo");
            //Робота з БД проводиться у кілька етапів

            //1 - підключення            raw string
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\arina\source\repos\Sharp_231\Data\Database1.mdf;Integrated Security=True";

            // ADO.NET - інструментарій(технологія) доступуу до даних у .NET
            SqlConnection connection = new(connectionString);
            //Особливість - уторення об'єкту не відкриває підключення
            try
            {
                connection.Open();//підключення необхідно відкривати окремою командою
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }

            //2 - формування та виконання команди
            String sql = "select CURRENT_TIMESTAMP";
            using SqlCommand cmd = new(sql, connection);//using у данномму контексті виконує autodisposable
            object scalar;
            try
            {
                scalar = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed {0}\n{1}: " + ex.Message, sql);
                return;
            }

            //3 - передача та оброблення даних від БД
            DateTime timestamp;
            timestamp = Convert.ToDateTime(scalar);
            Console.WriteLine("Res: {0}", timestamp);


            //закриття підключення, перевірка, що всі дані передані
            connection.Close();
        }
    }
}
/*
 робота з даними на прикладі БД
БД зазвичай є відокремленим від проєкту сервісом, що мимагає окремого
підключення та спесифічної взаємодії.

LocalDB New Item - Sercice DB - Create
знаходимо рдок підключення до БД через її властивості у Server Explorer 

NuGet - система управлінння підключенними додатковими модулями (бібліотеками)
проєкт  C#.NET:Tools -- NuGet Package Manager - manage 
Microsoft.Data.sqlClient - додаткові інструменти
для взаємодії з субд MS SQLServer у т.ч LocacDB

ORM - Object Relation Mapping - відображення даних та їх зв'язків на об'єкти(мови програмування) та їх зв'язки
DTO - Data transfer Object(Entity) - об'єкти (класи) для представлення даних
DAO - Data Access Object - об'єкти (класи) для оперування з DTO
 */

/*
 Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\arina\source\repos\Sharp_231\Data\Database1.mdf;Integrated Security=True
 */