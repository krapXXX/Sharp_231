using Sharp_231.AsyncProgramming;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sharp_231.Networking
{
    internal class NbuRateXml
    {
        public int R030 { get; set; }
        public string Txt { get; set; } = null!;
        public double Rate { get; set; }
        public string Cc { get; set; } = null!;
        public string ExchangeDate { get; set; } = null!;

        public static List<NbuRateXml> ListFromXmlString(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            return doc.Descendants("currency")
                .Select(x => new NbuRateXml
                {
                    R030 = int.Parse(x.Element("r030")!.Value, CultureInfo.InvariantCulture),
                    Txt = x.Element("txt")!.Value,
                    Rate = double.Parse(x.Element("rate")!.Value, CultureInfo.InvariantCulture),
                    Cc = x.Element("cc")!.Value,
                    ExchangeDate = x.Element("exchangedate")!.Value
                })
                .ToList();
        }

        public override string ToString()
        {
            return $"r030={R030}, txt={Txt}, rate={Rate}, cc={Cc}, date={ExchangeDate}";
        }
    }
}
