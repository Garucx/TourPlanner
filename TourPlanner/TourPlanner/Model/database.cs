using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Model
{
    internal class database
    {
        string myConnectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString.ToString();
        private NpgsqlConnection connection;
        private NpgsqlCommand command;
        object protection = new object();
        public database()
        {
            Log.LogInfo("Opening up new Database Connection");
            connection = new NpgsqlConnection(myConnectionString);
            connection.Open();
            command = new NpgsqlCommand();
            command.Connection = connection;
        }

        public void Modify_Tour(int id, Tour tour)
        {
            Log.LogInfo("Modifying Tour " + tour.Name);
            if (connection.State == System.Data.ConnectionState.Open)
            {
                lock (protection)
                {
                    command.CommandText = "update tours set name = (@name), description = (@description),tour_from= (@tour_from),transport_type =(@transport_type),distance =(@distance),time = (@time),route_information = (@route_information),tour_to =(@tour_to) where id = (@id)";
                    command.Parameters.AddWithValue("name", tour.Name);
                    command.Parameters.AddWithValue("description", tour.Tour_desc);
                    command.Parameters.AddWithValue("tour_from", tour.From);
                    command.Parameters.AddWithValue("transport_type", tour.Transport_type);
                    command.Parameters.AddWithValue("time", tour.Time);
                    command.Parameters.AddWithValue("route_information", tour.Image_link);
                    command.Parameters.AddWithValue("distance", tour.Distance);
                    command.Parameters.AddWithValue("tour_to", tour.To);
                    command.Parameters.AddWithValue("id", id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }
            }
            else
                error();

        }

        public void Modify_Tour_Log(int id, TourLog tourLog)
        {
            Log.LogInfo("Modifying Tour Log " + id.ToString());
            if (connection.State == System.Data.ConnectionState.Open)
            {
                lock (protection)
                {
                    command.CommandText = "update tour_logs set tour_id = (@tour_id), date_time = (@date_time),tour_comment = (@tour_comment),difficulty =(@difficulty),total_time = (@total_time),rating =(@rating) where id = (@id)";
                    command.Parameters.AddWithValue("tour_id", tourLog.idfromtour);
                    command.Parameters.AddWithValue("date_time", tourLog.date_time);
                    command.Parameters.AddWithValue("tour_comment", tourLog.Comment);
                    command.Parameters.AddWithValue("difficulty", tourLog.difficulty);
                    command.Parameters.AddWithValue("total_time", tourLog.total_time);
                    command.Parameters.AddWithValue("rating", tourLog.rating);
                    command.Parameters.AddWithValue("id", id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }
            }
            else
                error();

        }

        public int Get_ID_From_log(TourLog tour)
        {
            Log.LogInfo("Getting ID from Tour Log");
            int id = 0;
            lock (protection)
            {
                command.CommandText = "select id from tour_logs where tour_id=(@tour_id) and difficulty=(@dif) and total_time =(@time);";
                command.Parameters.AddWithValue("tour_id", tour.idfromtour);
                command.Parameters.AddWithValue("dif", tour.difficulty);
                command.Parameters.AddWithValue("time", tour.total_time);
                command.Prepare();
                using (NpgsqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (dataReader[0] != null)
                        {
                            id = (int)dataReader[0];
                        }
                        else
                        {
                            throw new Exception("Der angegebene Tour wird in der Datenbank nicht gefunden");
                        }
                    }
                }
                command.Parameters.Clear();
            }
            return id;
        }
        public void Delete_Tour(int id)
        {
            Log.LogInfo("Deleting Tour with Id: " + id.ToString());
            Task.Run(() =>
            {
                lock (protection)
                {
                    command.CommandText = "delete from tour_logs where tour_id= (@id);";
                    command.Parameters.AddWithValue("id", id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    command.CommandText = "delete from tours where id= (@id);";
                    command.Parameters.AddWithValue("id", id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            });
        }

        public int Get_ID_From_Tour(string name,string from,string to)
        {
            Log.LogInfo("Getting ID From Tour");
            int id = 0;
            lock (protection)
            {
                command.CommandText = "select id from tours where name=(@name) and tour_from=(@from) and tour_to =(@to);";
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("from", from);
                command.Parameters.AddWithValue("to", to);
                command.Prepare();
                using (NpgsqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (dataReader[0] != null)
                        {
                            id = (int)dataReader[0];
                        }
                        else
                        {
                            return id;
                        }
                    }
                }
                command.Parameters.Clear();
            }
            return id;
        }

        public void Create_new_Tour(Tour newtour)
        {
            Log.LogInfo("Creating new Tour: " + newtour.Name);
            if (connection.State == System.Data.ConnectionState.Open)
            {
                lock (protection)
                {
                    command.CommandText = "insert into tours(name,description,tour_from,transport_type,distance,time,route_information,tour_to) values ((@name),(@description),(@tour_from),(@transport_type),(@distance),(@time),(@route_information),(@tour_to))";
                    command.Parameters.AddWithValue("name", newtour.Name);
                    command.Parameters.AddWithValue("description", newtour.Tour_desc);
                    command.Parameters.AddWithValue("tour_from", newtour.From);
                    command.Parameters.AddWithValue("transport_type", newtour.Transport_type);
                    command.Parameters.AddWithValue("time", newtour.Time);
                    command.Parameters.AddWithValue("route_information", newtour.Image_link);
                    command.Parameters.AddWithValue("distance", newtour.Distance);
                    command.Parameters.AddWithValue("tour_to", newtour.To);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
            else
            {
                error();
            }

        }

        private void error()
        {
            Log.LogError("Connection mit der Datenbank ist geschlossen");
            throw new Exception("Connection mit der Datenbank ist geschlossen");
        }
        public void Create_Tour_Log(TourLog tourLog)
        {
            Log.LogInfo("Creating new TourLog");
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Task.Run(() =>
                {
                    lock (protection)
                    {
                        NpgsqlParameter date = new NpgsqlParameter("date_time", NpgsqlTypes.NpgsqlDbType.Timestamp);
                        command.CommandText = "insert into tour_logs(tour_id,date_time,tour_comment,difficulty,total_time,rating) values ((@tour_id),(@date_time),(@tour_comment),(@difficulty),(@total_time),(@rating))";
                        command.Parameters.AddWithValue("tour_id", tourLog.idfromtour);
                        command.Parameters.AddWithValue("date_time", tourLog.date_time);
                        command.Parameters.AddWithValue("tour_comment", tourLog.Comment);
                        command.Parameters.AddWithValue("difficulty", tourLog.difficulty);
                        command.Parameters.AddWithValue("total_time", tourLog.total_time);
                        command.Parameters.AddWithValue("rating", tourLog.rating);
                        command.Prepare();
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                });
            }
            else
            {
                error();
            }
        }

        public async Task<Tour> GetTourAsync(int id)
        {
            Log.LogInfo("Getting Tour with ID: " + id.ToString());
            Tour tour = null;
            command.CommandText = "select * from tours where id = (@id);";
            command.Parameters.AddWithValue("id", id);
            command.Prepare();
            using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (await dataReader.ReadAsync())
                {
                    if (dataReader[0] != null)
                    {
                        tour = new Tour();
                        tour.Name = (string)dataReader[1];
                        tour.Tour_desc = (string)dataReader[2];
                        tour.From = (string)dataReader[3];
                        tour.Transport_type = (string)dataReader[4];
                        tour.Distance = Convert.ToSingle((decimal)dataReader[5]);
                        tour.Time = (int)dataReader[6];
                        tour.Image_link = (string)dataReader[7];
                        tour.To = (string)dataReader[8];
                    }
                    command.Parameters.Clear();
                }
            }
            if (tour == null) throw new Exception("Der gesuchte Tour exestiert nicht");
            return tour;
        }

        public List<Tour> GetAll()
        {
            List<Tour> alltours = new List<Tour>();
            Log.LogInfo("Requesting every log in the Database");

            command.CommandText = "select * from tours;";
            command.Prepare();
            using (NpgsqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    if (dataReader[0] != null)
                    {
                        alltours.Add(new Tour((string)dataReader[1], (string)dataReader[2], (string)dataReader[3], (string)dataReader[8], (string)dataReader[4], Convert.ToSingle((decimal)dataReader[5]), (int)dataReader[6], (string)dataReader[7]));
                    }
                }
            }

            return alltours;
        }



        public void CloseConnection()
        {
            Log.LogInfo("Database Connection schließt");
            connection.Close();
        }

    }
}

