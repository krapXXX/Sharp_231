using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data.Dto
{
    internal class SaleModel
    {
        public string ManagerName { get; set; } = null!;
        public int Sales { get; set; }

        public override string ToString()
        {
            return $"{ManagerName} ---- {Sales}";
        }
    }

}
