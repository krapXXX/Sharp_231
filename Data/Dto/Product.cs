using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data.Dto
{
    internal class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }


        public static Product FromReader(SqlDataReader reader)
        {
            return new()
            {
                Id = reader.GetGuid(0),  // using System.Data
                Name = reader.GetString(1),
                Price = Convert.ToDouble(reader.GetDecimal(2))
            };
        }
        public override string ToString()
        {
           return $"{Id.ToString()[..3]}...{Id.ToString()[^3..]} - {Name}  {Price:F2}";
        }

    }
}
/*
DTO - Data transfer Object(Entity) - об'єкти (класи) для представлення даних
 Відображення рядка таблиці БД (Products)

 */