using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    public abstract class Literature
    {
        public String Publisher { get; set; } = String.Empty;
        public String Title { get; set; } = null!;

        public abstract String GetCard();

    }

}
