using Microsoft.Data.SqlClient;
using Sharp_231.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data
{
    internal class DataAccessor
    {
        private readonly SqlConnection connection;

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
        public List<Product> GetProducts()
        {
            List<Product> products = [];

            string sql = "select *from products ";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(Product.FromReader(reader));
                   // Console.WriteLine(" {1}, {2}", reader.GetString(1), reader.GetDecimal(2));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed {0}\n{1}: " + ex.Message, sql);
            }
            return products;
        }

        public void Install()
        {
            InstallProducts();
            InstallDepartments();
            InstallManagers();
        }
        private void InstallManagers()
        {
            String sql = "CREATE TABLE Managers(" +
                "Id           UNIQUEIDENTIFIER PRIMARY KEY," +
                "DepartmentId UNIQUEIDENTIFIER NOT NULL," +
                "Name         NVARCHAR(64)     NOT NULL," +
                "WorksFrom    DATETIME2        NOT NULL  DEFAULT CURRENT_TIMESTAMP)";
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

        public void Seed()
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
                "('F16E0815-F613-44E3-8B52-297E1F6F9780',N'Motorola 2020',27000.00)";
            using SqlCommand cmd = new(sql, connection);
            try
            {
                cmd.ExecuteNonQuery();//без зворотнього результату
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command failed {0}\n{1}: " + ex.Message, sql);
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

    }
}
