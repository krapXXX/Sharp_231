using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Newspaper: Literature, IPeriodic
    {
        public DateOnly Date { get; set; }

        public override string GetCard()
        {
            return $"{base.Title} - {Date} - {base.Publisher}";
        }

        public string GetPeriod()
        {
            return "Day";
        }
    }
}
