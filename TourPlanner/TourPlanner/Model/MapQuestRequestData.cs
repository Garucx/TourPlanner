using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Model
{
    internal class MapQuestRequestData
    {
        string AreaCode { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }

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
