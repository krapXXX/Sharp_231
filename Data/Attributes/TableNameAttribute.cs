using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Data.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    internal class TableNameAttribute(String Value) : Attribute
    {
        public string Value { get; init; } = Value;
    }
}
