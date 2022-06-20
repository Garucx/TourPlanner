using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataLayer.Model;

namespace TourPlanner.BusinessLayer.JSON
{
    public class Jsonall
    {
        public Jsonall()
        {

        }
        public async static Task Save(Tour tour)
        {
            await Task.Run(() =>
            {
                using (StreamWriter file = File.CreateText(@"../../../JSONs/" + tour.Name + ".json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, tour);
                }
            });
        }
        public async static Task Save(Tour tour, string path)
        {
            await Task.Run(() =>
            {
                using (StreamWriter file = File.CreateText(path + tour.Name + ".json"))
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
