using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Sharp_231.Data.Attributes;


namespace Sharp_231.Data.Dto
{
    [TableName("Accesses")]

    internal class Access
        {
            public Guid Id { get; set; }
            public Guid ManagerId { get; set; }
            public string Login { get; set; } = null!;
            public string Salt { get; set; } = null!;
            public string Dk { get; set; } = null!;

            public static Access FromReader(SqlDataReader reader)
            {
                return new()
                {
                    Id = reader.GetGuid(0),
                    ManagerId = reader.GetGuid(1),
                    Login = reader.GetString(2),
                    Salt = reader.GetString(3),
                    Dk = reader.GetString(4),
                };
            }

            public override string ToString()
                => $"{Login} ({Id.ToString()[..4]}...)";
        }
    }

