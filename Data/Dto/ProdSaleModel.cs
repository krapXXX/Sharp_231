using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data.Dto
{
    internal class ProdSaleModel
    {
        public Product Product { get; set; } = null!;
        public int Checks { get; set; }
        public int Quantity { get; set; }
        public double Money { get; set; }
        public override string ToString()
        {
            return $"{Product.Name} - checks: {Checks}, qty: {Quantity}, money: {Money:F2}";
        }


    }
}
