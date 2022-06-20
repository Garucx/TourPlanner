using NUnit.Framework;
using System.IO;
using System.Windows.Media.Imaging;
using TourPlanner;
using TourPlanner.DataLayer;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.MapQuest;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.BusinessLayer.JSON;
using System.Drawing;
using TourPlanner.DataLayer.Model;

namespace TourPlannerTest
{
    public class Tests
    {

        static database db = new database("Host=localhost;Username=postgres;Password=postgres;Database=tour");
        static List<Tour> allTours = new List<Tour>();
        [Test]
        public void CheckDBConnection()
        {
            if (db.connection != null && db.connection.State == ConnectionState.Open)
                Assert.Pass();
        }

        [Test]
        public void CheckDBClosedConnection()
        {

            db.CloseConnection();
            if (db.connection.State == ConnectionState.Closed)
                Assert.Pass();
        }
        [Test]
        public void CheckDBGetAll()
        {
            db = new database("Host=localhost;Username=postgres;Password=postgres;Database=tour");
            if (db.connection != null && db.connection.State == ConnectionState.Open)
            {
                allTours = db.GetAll();
                Assert.That(allTours.Count, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void CheckDBInsert()
        {
            db.Create_new_Tour(new Tour("unit", "unit", "unit", "unit", "unit", 0.5f, 0, null, "unit"));
            var newList = db.GetAll();
            var el = newList.Where(x => x.Name == "unit").FirstOrDefault();
            Assert.That(el, Is.Not.Null);
            Assert.That(el.Name == "unit");
        }
        [Test]
        public void CheckDBUpdate()
        {
            allTours = db.GetAll();
            db.Modify_Tour(allTours.Where(x => x.Name == "unit").FirstOrDefault().ID, new Tour("unitupdate", "unitupdate", "unitupdate", "unitupdate", "unitupdate", 0.5f, 0, null, "unitupdate"));
            allTours = db.GetAll();
            var item = allTours.Where(x => x.Name == "unitupdate").FirstOrDefault();
            Assert.That(item.Name == "unitupdate");
        }

        [Test]
        public void CheckDBDelete()
        {
            allTours = db.GetAll();
            db.Delete_Tour(allTours.Where(x => x.Name == "unitupdate").FirstOrDefault().ID);
            allTours = db.GetAll();
            var item = allTours.Where(x => x.Name == "unitupdate").FirstOrDefault();
            Assert.IsNull(item);
        }

        [Test]
        public void CheckSaveImage()
        {
            BitmapImage image = new BitmapImage(new System.Uri(@"C:\Users\Nemanja\Pictures\Flamaramara.png"));
            SaveBitmapImage.SaveImage(image, "test", @"C:\Users\Nemanja\Pictures\");
            if (File.Exists(@"C:\Users\Nemanja\Pictures\test.png"))
                Assert.Pass();
        }

        [Test]
        public void CheckLoadImage()
        {
            BitmapImage image = new BitmapImage(new System.Uri(@"C:\Users\Nemanja\Pictures\Flamaramara.png"));
            BitmapImage methodimage = LoadBitmapImage.LoadImage("Flamaramara", @"C:\Users\Nemanja\Pictures\");
            Assert.That(methodimage.UriSource, Is.EqualTo(image.UriSource));
        }
        [Test]
        public void CheckDeleteImage()
        {
            if (!File.Exists(@"C:\Users\Nemanja\Pictures\test.png"))
            {
                BitmapImage image = LoadBitmapImage.LoadImage("Flamaramara", @"C:\Users\Nemanja\Pictures\");
                SaveBitmapImage.SaveImage(image, "test", @"C:\Users\Nemanja\Pictures\");
            }
            SaveBitmapImage.DeleteImage("test", @"C:\Users\Nemanja\Pictures\");
            if (!File.Exists(@"C:\Users\Nemanja\Pictures\test.png"))
                Assert.Pass();
        }
        [Test]
        public void CheckDeleteImage2()
        {
            // File existiert nicht
            SaveBitmapImage.DeleteImage("nofile", @"C:\Users\Nemanja\Pictures\");
            // Program stürzt nicht ab => pass
            Assert.Pass();
        }

        [Test]
        public void CheckMapQuestHandler1()
        {

            string url1 = @"https://www.mapquestapi.com/directions/v2/route?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&from=1150 Jurekgasse,Vienna,AT&to=1150 Arnsteingasse, Vienna,AT";

            string url2 = @"https://open.mapquestapi.com/staticmap/v5/map?key=mpUdVgf8ptUM5Bum4hKF2yKU30TlOLw4&start=1150 Jurekgasse,Vienna,AT&end=1150 Arnsteingasse, Vienna, AT";
            var start = new MapQuestRequestData("1150", "Jurekgasse", "Vienna", "AT");
            var dest = new MapQuestRequestData("1150", "Arnsteingasse", "Vienna", "AT");
            var res = MapQuestRequestHandler.GetRouteAsync(start, dest).Result;
            var res2 = MapQuestRequestHandler.GetRouteAsync(start, dest, url1).Result;
            Assert.IsNotNull(res);
            Assert.IsNotNull(res2);
            Assert.That(res.URL.Replace(" ", ""), Is.EqualTo(url2.Replace(" ", "")));
            Assert.That(res2.route.route.distance, Is.EqualTo(res.route.route.distance));
        }

        [Test]
        public void CheckJsonExport()
        {
            var jsontour = new Tour("jsonexport", "jsonexport", "jsonexport", "jsonexport", "jsonexport", 0f, 0, null, "jsonexport");
            Jsonall.Save(jsontour, @"C:\testfolder\");
            if (File.Exists(@"C:\testfolder\jsonexport.json"))
                Assert.Pass();
        }
        [Test]
        public void CheckJsonImport()
        {
            var jsonimport = Jsonall.Open(@"C:\testfolder\jsonexport.json").Result;
            Assert.That(jsonimport.Name == "jsonexport");
        }

        [Test]
        public void CheckBitmapToBitmapImageConverter()
        {
            Bitmap bitmap = new Bitmap(@"C:\testfolder\Flamaramara.png");
            BitmapImage bitmapImage = new BitmapImage(new Uri(@"C:\testfolder\Flamaramara.png"));

            var converted = MapQuestRequestHandler.ToBitmapImage(bitmap);
            Assert.That(converted.BaseUri == bitmapImage.BaseUri);
            Assert.That(converted.GetType() == typeof(BitmapImage));

        }
    }
}