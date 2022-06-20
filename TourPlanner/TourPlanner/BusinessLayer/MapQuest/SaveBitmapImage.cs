using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TourPlanner.BusinessLayer.Logging;

namespace TourPlanner.BusinessLayer.MapQuest
{
    public class SaveBitmapImage
    {
        public static void SaveImage(BitmapImage bitmap, string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static void SaveImage(BitmapImage bitmap, string name, string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fileStream = new System.IO.FileStream(path + name, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        //public static void DeleteImage(string id)
        //{
        //    if (File.Exists())
        //}
        public static void DeleteImage(string id, string path)
        {
            try
            {
                if (File.Exists(path + id + ".png"))
                    File.Delete(path + id + ".png");

            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }

    }
}