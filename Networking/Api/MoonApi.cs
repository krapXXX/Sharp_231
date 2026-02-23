using Sharp_231.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Networking.Api
{
    internal class MoonApi
    {
        public async Task<MoonPhase> TodayPhaseAsync()
        {
            int day = DateTime.Now.Day;

            var moonApiResponse =await FetchDataAsync(DateTime.Now.Year, DateTime.Now.Month, day);
            return moonApiResponse.Phase[day.ToString()];
        }

        public async Task<MoonPhase> PhaseByDateAsync(DateOnly date)
        {
            int day = date.Day;
            var moonApiResponse = await FetchDataAsync(date.Year, date.Month, day);
            return moonApiResponse.Phase[day.ToString()];
        }

        private async Task<MoonApiResponse> FetchDataAsync(int year, int month, int day)
        {
            using HttpClient httpClient = new();
            string href = $"https://www.icalendar37.net/lunar/api/?year={year}&month={month}&day={day}&shadeColor=1&size=150&lightColor=rgb(255,255,210)&texturize=false";
            return JsonSerializer.Deserialize<MoonApiResponse>(
                await httpClient.GetStringAsync(href)
            )!;
        }
    }
}
