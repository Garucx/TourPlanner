using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Model;

namespace TourPlanner.BusinessLayer.JSON
{
    internal class Jsonall
    {
        public Jsonall()
        {

        }
        public async static Task Save(Tour tour)
        {
            Task.Run(() =>
            {
                using (StreamWriter file = File.CreateText(@"../../../JSONs/" + tour.Name + ".json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, tour);
                }
            });
        }
        public async static Task<Tour> Open(string Path)
        {
            Tour tour;
            var bytes = File.ReadAllBytes(Path);
            tour = System.Text.Json.JsonSerializer.Deserialize<Tour>(bytes);

            return tour;
        }

    }
}
