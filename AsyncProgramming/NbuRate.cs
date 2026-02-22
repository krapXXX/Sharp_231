using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sharp_231.AsyncProgramming
{
    internal class NbuRate
    {
        public int R030 { get; set; }

        public string Txt { get; set; } = null!;

        public double Rate { get; set; }

        public string Cc { get; set; } = null!;

        public string? Special { get; set; }

        public DateOnly Exchangedate { get; set; }

        static NbuRate FromJson(JsonElement jsonElement)
        {
            return new()
            {
                R030 = jsonElement.GetProperty("r030").GetInt32(),
                Txt = jsonElement.GetProperty("txt").GetString()!,
                Rate = jsonElement.GetProperty("rate").GetDouble(),
                Cc = jsonElement.GetProperty("cc").GetString()!,
                Exchangedate = DateOnly.Parse(jsonElement.GetProperty("exchangedate").GetString()),
                Special = jsonElement.GetProperty("special").GetString()
            };
        }
        public static List<NbuRate> ListFromJsonString(string json)
        {
            return
            [
                .. JsonSerializer.Deserialize<JsonElement>(json)
            .EnumerateArray()
            .Select(jsonElement => NbuRate.FromJson(jsonElement))
            ];
        }
        public override string ToString()
        {
            return $"r030={R030}, txt={Txt}, rate={Rate}, cc={Cc}";
        }
    }
}
