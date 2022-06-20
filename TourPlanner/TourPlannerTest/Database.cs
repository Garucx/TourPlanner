using Npgsql;
using NUnit.Framework;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TourPlanner.DataLayer;
using TourPlanner.DataLayer.Model;
using System.Linq;
using System;

namespace TourPlannerTest
{
    public class Tests
    {
        static string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=tour";
        database connection = new database(connectionString);
        static Tour tour = new Tour("test", "TesttoTest", "alser straﬂe", "Alser Straﬂe", "fastest", 100, 20, new BitmapImage(), "yes");
        static int id;
        static TourLog tourLog;

        [Test]
        public void Reihenfolge()
        {
            TestDatabaseConnection();
            TestCreateTour();
            ModifyTour();
            CreateTourLog();
            ModifyTourLog();
            DeleteTour();
            CloseDatabase();
        }
        public void TestDatabaseConnection()
        {
            Assert.AreEqual(ConnectionState.Open, connection.GetStatus());
        }
        public async Task TestCreateTour()
        {
            connection.Create_new_Tour(tour);
            id = connection.Get_ID_From_Tour(tour.Name);
            Tour a = await connection.GetTourAsync(id);
            Assert.AreEqual(tour.Name, a.Name);
        }

        public async Task ModifyTour()
        {
            tour.ID = id;
            tour.From = "Jorgerstraﬂe";
            connection.Modify_Tour(tour.ID, tour);
            Tour a = await connection.GetTourAsync(tour.ID);
            Assert.AreEqual(tour.From, a.From);
        }

        public async Task CreateTourLog()
        {
            tourLog = new TourLog(tour.ID, System.DateTime.Now, "Nice", 4, 4, 4);
            connection.Create_Tour_Log(tourLog);
            var a = await connection.GetTourLogsAsync(tour.ID);
            Assert.AreEqual(tourLog.Comment,a.FirstOrDefault().Comment);
        }

        public async Task ModifyTourLog()
        {
            tourLog.rating = 5;
            connection.Modify_Tour_Log(tourLog);
            var a = await connection.GetTourLogsAsync(tour.ID);
            Assert.AreEqual(tourLog.rating,a.FirstOrDefault().rating);
        }

        public async Task DeleteTour()
        {
            try
            {
                connection.Delete_Tour(tour.ID);
                Tour a = await connection.GetTourAsync(tour.ID);
            }catch(Exception e)
            {
                Assert.AreEqual("Der gesuchte Tour exestiert nicht", e.Message);
            }

        }

        public void CloseDatabase()
        {
            connection.CloseConnection();
            Assert.AreEqual(ConnectionState.Closed, connection.GetStatus());
        }

    }
}