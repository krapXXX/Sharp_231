using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Users.Dal.Entities
{
    internal class AccessToken
    {
        public Guid TokenId { get; set; }
        public Guid AccessId { get; set; }
        public DateTime TokenIat { get; set; }
        public DateTime? TokenExp { get; set; }

    }
}
