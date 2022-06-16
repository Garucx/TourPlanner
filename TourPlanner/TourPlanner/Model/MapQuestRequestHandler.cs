using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Model
{
    internal class MapQuestRequestHandler
    {

        /*
         * http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from=1150 Jurekgasse,Vienna,AT&to=1150 Arnsteingasse, Vienna, AT
         * https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start=1150 Jurekgasse,Vienna,AT&end=1150 Arnsteingasse, Vienna, AT
        */
        public static async Task<Rootobject> GetRouteAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            using var client = new HttpClient();
            var res = await client.GetAsync($"http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from={start.GetString()}&to={dest.GetString()}");

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Rootobject>(json);
            
        }

        public static async Task GetRouteImageAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {

        }
    }
}
