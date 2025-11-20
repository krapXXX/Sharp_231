using Microsoft.Data.SqlClient;
using Sharp_231.Data.Dto;
using Sharp_231.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\arina\source\repos\Sharp_231\Data\Database1.mdf;Integrated Security=True";
            SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
            DataAccessor dataAccessor = new();
            // dataAccessor.Install();
            //dataAccessor.Seed();
            List<Product> products = dataAccessor.GetProducts();

            //products.ForEach(Console.WriteLine);
            dataAccessor.PrintNP();
            Console.WriteLine("====================");
            dataAccessor.PrintDesc();
            Console.WriteLine("====================");
            dataAccessor.PrintAlphabet();
            Console.WriteLine("=========TOP========");

            dataAccessor.PrintTop();
            Console.WriteLine("========LOSER=======");
            dataAccessor.PrintLoser();

            Console.WriteLine("=======RANDOM=======");
            dataAccessor.PrintRandom();



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
            catch(SqlException ex)
            {
                Console.WriteLine("Connection failed: "+ex.Message);
            }

            //2 - формування та виконання команди
            String sql = "select CURRENT_TIMESTAMP";
            using SqlCommand cmd = new(sql, connection);//using у данномму контексті виконує autodisposable
            object scalar;
            try
            {
               scalar = cmd.ExecuteScalar();
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Connection failed {0}\n{1}: "+ex.Message,sql);
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