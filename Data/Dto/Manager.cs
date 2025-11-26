using Microsoft.Data.SqlClient;
using System;

namespace Sharp_231.Data.Dto
{
    internal class Manager
    {
        public Guid DepartmentID { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime WorksFrom { get; set; }

        public static Manager FromReader(SqlDataReader reader)
        {
            return new()
            {
                DepartmentID = reader.GetGuid(0),
                Id = reader.GetGuid(1),
                Name = reader.GetString(2),
                WorksFrom = reader.GetDateTime(3)
            };
        }

        public override string ToString()
        {
            return $"{Id.ToString()[..3]}...{Id.ToString()[^3..]} - {Name}  [{WorksFrom:yyyy:MM}]";
        }
    }
}
