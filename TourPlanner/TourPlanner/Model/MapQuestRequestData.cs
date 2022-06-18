using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Model
{
    internal class MapQuestRequestData
    {
        public string AreaCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public MapQuestRequestData(string areaCode, string address, string city, string state)
        {
            AreaCode = areaCode;
            Address = address;
            City = city;
            State = state;
        }

        public string GetString()
        {
            return $"{AreaCode} {Address},{City},{State}";
        }

    }
}
