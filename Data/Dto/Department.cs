using Microsoft.Data.SqlClient;
using System;

namespace Sharp_231.Data.Dto
{
    internal class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public static Department FromReader(SqlDataReader reader)
        {
            return new()
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
            };
        }

        public override string ToString()
        {
            return $"{Id.ToString()[..3]}...{Id.ToString()[^3..]} - {Name}";
        }
    }
}

/*
 DTO – Data Transfer Object  
 Представлення одного рядка таблиці Departments
*/
