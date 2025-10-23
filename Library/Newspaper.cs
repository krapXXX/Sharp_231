using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Newspaper: Literature
    {
        public String Date { get; set; } = null;

        public override string GetCard()
        {
            return $"{base.Title} - {Date} - {base.Publisher}";
        }
    }
}
