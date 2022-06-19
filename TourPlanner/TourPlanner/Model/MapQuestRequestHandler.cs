using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TourPlanner.View;

namespace TourPlanner.Model
{
    internal class MapQuestRequestHandler
    {

        /*
         * http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from=1150 Jurekgasse,Vienna,AT&to=1150 Arnsteingasse, Vienna, AT
         * https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start=1150 Jurekgasse,Vienna,AT&end=1150 Arnsteingasse, Vienna, AT
        */
        public static async Task<(Rootobject route ,Bitmap image)> GetRouteAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            using var client = new HttpClient();
            var res = await client.GetAsync($"http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from={start.GetString()}&to={dest.GetString()}");

            var json = await res.Content.ReadAsStringAsync();
            return (JsonConvert.DeserializeObject<Rootobject>(json),GetRouteImageAsync(start,dest).Result);
            
        }

        public static async Task<Bitmap> GetRouteImageAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            // Dieser Scheiß stürzt das Program ab. Ka wie ich das fixen soll
            WebClient client = new WebClient();
            Stream stream = client.OpenRead($"https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start={start.GetString()}&end={dest.GetString()}");
            Bitmap bitmap; bitmap = new Bitmap(stream);

            //using (MemoryStream memory = new MemoryStream())
            //{
            //    bitmap.Save(memory, ImageFormat.Png);
            //    memory.Position = 0;
            //    BitmapImage bitmapImage = new BitmapImage();
            //    bitmapImage.BeginInit();
            //    bitmapImage.StreamSource = memory;
            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapImage.EndInit();
            //    var t = new TestShowImage();
            //    t.image.Source = bitmapImage;
            //    t.Show();
            //}
            
            stream.Flush();
            stream.Close();
            client.Dispose();

            return bitmap;
        }
    }
}
