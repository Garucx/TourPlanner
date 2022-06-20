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
using System.Windows;
using System.Windows.Media.Imaging;
using TourPlanner.BusinessLayer.Logging;
using TourPlanner.View;

namespace TourPlanner.BusinessLayer.MapQuest
{
    public class MapQuestRequestHandler
    {

        /*
         * http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from=1150 Jurekgasse,Vienna,AT&to=1150 Arnsteingasse, Vienna, AT
         * https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start=1150 Jurekgasse,Vienna,AT&end=1150 Arnsteingasse, Vienna, AT
        */
        public static async Task<(Rootobject? route, BitmapImage? image, string URL)> GetRouteAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            try
            {

                using var client = new HttpClient();
                var res = await client.GetAsync($"http://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from=sd{start.GetString()}&to={dest.GetString()}");

                var json = await res.Content.ReadAsStringAsync();
                var imgres = GetRouteImageAsync(start, dest).Result;
                return (JsonConvert.DeserializeObject<Rootobject>(json), imgres.img, imgres.URL);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show("Could not create a new tour. Please check your input and your internet connection");
                return (null, null, "");
            }

        }

        public static async Task<(BitmapImage? img, string URL)> GetRouteImageAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            try
            {
                WebClient client = new WebClient();
                string URL = $"https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start={start.GetString()}&end={dest.GetString()}";
                Stream stream = client.OpenRead(URL);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                stream.Flush();
                stream.Close();
                client.Dispose();

                return (ToBitmapImage(bitmap), URL);

            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
                return (null, "");
            }
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}