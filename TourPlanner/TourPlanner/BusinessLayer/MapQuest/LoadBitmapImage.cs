using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TourPlanner.BusinessLayer.MapQuest
{
    internal class LoadBitmapImage
    {
        public static BitmapImage LoadImage(string id)
        {
            BitmapImage image = new BitmapImage();
            if (File.Exists("..\\..\\..\\PresentationLayer\\tour_images\\" + id + ".png"))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri("..\\..\\..\\PresentationLayer\\tour_images\\" + id + ".png", UriKind.Relative);
                image.DecodePixelWidth = 200;
                image.EndInit();
            }
            return image;
        }
    }
}