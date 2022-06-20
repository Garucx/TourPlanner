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
using TourPlanner.BusinessLayer.PDF;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer.Logging;
using log4net;

namespace TourPlannerTest
{
    public class Tests
    {

        static database db = new database("Host=localhost;Username=postgres;Password=postgres;Database=tour");
        static List<Tour> allTours = new List<Tour>();

        static Tests()
        {
            
        }
        [Test]
        public void CheckDBConnection()
        {
            
            if (db.GetStatus() == ConnectionState.Open)
                Assert.Pass();
        }

        [Test]
        public void CheckDBClosedConnection()
        {
            db.CloseConnection();
            if (db.GetStatus() == ConnectionState.Closed)
                Assert.Pass();
        }
        [Test]
        public void CheckDBGetAll()
        {
            if (db.GetStatus() == ConnectionState.Open)
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
            BitmapImage image = new BitmapImage(new System.Uri("..\\..\\..\\Testfolder\\Flamaramara.png", UriKind.Relative));
            SaveBitmapImage.SaveImage(image, "test", "..\\..\\..\\Testfolder\\");
            if (File.Exists("..\\..\\..\\Testfolder\\test.png"))
                Assert.Pass();
        }

        [Test]
        public void CheckLoadImage()
        {
            BitmapImage image = new BitmapImage(new System.Uri("..\\..\\..\\Testfolder\\Flamaramara.png", UriKind.Relative));
            BitmapImage methodimage = LoadBitmapImage.LoadImage("Flamaramara", "..\\..\\..\\Testfolder\\");
            Assert.That(methodimage.UriSource, Is.EqualTo(image.UriSource));
        }
        [Test]
        public void CheckDeleteImage()
        {
            if (!File.Exists("..\\..\\..\\Testfolder\\test.png"))
            {
                BitmapImage image = LoadBitmapImage.LoadImage("Flamaramara", "..\\..\\..\\Testfolder\\");
                SaveBitmapImage.SaveImage(image, "test", "..\\..\\..\\Testfolder\\");
            }
            SaveBitmapImage.DeleteImage("test", "..\\..\\..\\Testfolder\\");
            if (!File.Exists("..\\..\\..\\Testfolder\\test.png"))
                Assert.Pass();
        }
        [Test]
        public void CheckDeleteImage2()
        {
            // File existiert nicht
            SaveBitmapImage.DeleteImage("nofile", @"..\\..\\..\\Testfolder\\");
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
            Jsonall.Save(jsontour, "..\\..\\..\\Testfolder\\");
            if (File.Exists("..\\..\\..\\Testfolder\\jsonexport.json"))
                Assert.Pass();
        }
        [Test]
        public void CheckJsonImport()
        {
            var jsonimport = Jsonall.Open("..\\..\\..\\Testfolder\\jsonexport.json").Result;
            Assert.That(jsonimport.Name == "jsonexport");
        }

        [Test]
        public void CheckBitmapToBitmapImageConverter()
        {
            Bitmap bitmap = new Bitmap("..\\..\\..\\Testfolder\\Flamaramara.png");
            BitmapImage bitmapImage = new BitmapImage(new Uri("..\\..\\..\\Testfolder\\Flamaramara.png",UriKind.Relative));

            var converted = MapQuestRequestHandler.ToBitmapImage(bitmap);
            Assert.That(converted.BaseUri == bitmapImage.BaseUri);
            Assert.That(converted.GetType() == typeof(BitmapImage));

        }

        [Test]
        public void CheckPDFCreation()
        {
            Tour tour = new Tour("PDFTest", "Description", "Antonigasse", "Blumengasse", "Fastes", 400, 300, new BitmapImage(), "asd");
            CreatePDF pdfcreator = new CreatePDF("./");
            pdfcreator.CreateTourPDF(tour.Name, tour, true);
            Task.Delay(200).Wait();
            Assert.IsTrue(File.Exists($"./{tour.Name}.pdf"));
        }
        [Test]
        public void CheckPDFTourCreation()
        {
            Tour tour = new Tour("PDFTest", "Description", "Antonigasse", "Blumengasse", "Fastes", 400, 300, new BitmapImage(), "asd");
            CreatePDF pdfcreator = new CreatePDF("./");
            pdfcreator.CreateTourPDF(tour.Name, tour, true);
            Task.Delay(200).Wait();
            Assert.IsTrue(File.Exists($"./{tour.Name}.pdf"));
        }
        [Test]
        public void CheckSummarizeCreation()
        {
            Tour tour = new Tour("PDFTest", "Description", "Antonigasse", "Blumengasse", "Fastes", 400, 300, new BitmapImage(), "asd");
            tour.TourLogs = new List<TourLog>();
            TourLog log = new TourLog(1, DateTime.Now, "Comment", 4, 4, 4, 5);
            tour.TourLogs.Add(log);
            CreatePDF pdfcreator = new CreatePDF("./");
            List<Tour> tours = new List<Tour>();
            tours.Add(tour);
            pdfcreator.CreateSummarize_report(tours);
            Task.Delay(200).Wait();
            Assert.IsTrue(File.Exists($"./Summarize.pdf"));
        }

        [Test]
        public void TestLogError()
        {
            string error = "This_is_a_error";
            Log.LogError(error);
            var temp = File.ReadAllLines("..\\..\\..\\Logs\\tourplannerlogs.txt");
            string[] lines = temp.LastOrDefault().Split(' ');
            Assert.AreEqual(error,lines.LastOrDefault().Trim());
            Assert.AreEqual("ERROR", lines[3]);

        }
        [Test]
        public void TestLogInfo()
        {
            string error = "This_is_a_Info";
            Log.LogInfo(error);
            var temp = File.ReadAllLines("..\\..\\..\\Logs\\tourplannerlogs.txt");
            string[] lines = temp.LastOrDefault().Split(' ');
            Assert.AreEqual(error, lines.LastOrDefault().Trim());
            Assert.AreEqual("INFO", lines[3]);

        }
        [Test]
        public void TestLogfatal()
        {
            string error = "This_is_a_fatal";
            Log.LogFatal(error);
            var temp = File.ReadAllLines("..\\..\\..\\Logs\\tourplannerlogs.txt");
            string[] lines = temp.LastOrDefault().Split(' ');
            Assert.AreEqual(error, lines.LastOrDefault().Trim());
            Assert.AreEqual("FATAL", lines[3]);

        }

    }

}