using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sharp_231.Networking.Orm
{
    internal class MoonPhase
    {
        [JsonPropertyName("phaseName")]
        public string PhaseName { get; set; } = null;
        [JsonPropertyName("isPhaseLimit")]
        public dynamic IsPhaseLimit { get; set; } = null;
        [JsonPropertyName("lighting")]
        public double Lighting { get; set; }  
        [JsonPropertyName("svg")]
        public string Svg { get; set; } = null;
        [JsonPropertyName("svgMini")]
        public dynamic SvgMini { get; set; } = null;
        [JsonPropertyName("dis")]
        public double Distance { get; set; } 
        [JsonPropertyName("dayWeek")]
        public int DayWeek { get; set; }
        [JsonPropertyName("npWidget")]
        public string NpWidget { get; set; } = null;  
        
    }
}
