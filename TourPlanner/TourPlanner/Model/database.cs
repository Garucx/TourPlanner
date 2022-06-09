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
            connection = new NpgsqlConnection(myConnectionString);
            connection.Open();
            command = new NpgsqlCommand();
            command.Connection = connection;    
        }

        public void Create_new_Tour(Tour newtour)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Task.Run(() =>
                {
                    lock (protection)
                    {
                        command.CommandText = "insert into tours(name,description,tour_from,transport_type,distance,time,route_information) values ((@name),(@description),(@tour_from),(@transport_type),(@distance),(@time),(@route_information))";
                        command.Parameters.AddWithValue("name", newtour.Name);
                        command.Parameters.AddWithValue("description", newtour.Tour_desc);
                        command.Parameters.AddWithValue("tour_from", newtour.From);
                        command.Parameters.AddWithValue("transport_type", newtour.Transport_type);
                        command.Parameters.AddWithValue("time", newtour.Time);
                        command.Parameters.AddWithValue("route_information", newtour.Image_link);
                        command.Parameters.AddWithValue("distance", newtour.Distance);
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

        public void error()
        {
            throw new Exception("Connection mit der Datenbank ist geschlossen");
        }
        public void Create_Tour_Log(TourLog tourLog)
        {
            if(connection.State == System.Data.ConnectionState.Open)
            {
                Task.Run(() => {
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

        public void CloseConnection()
        {
            connection.Close();
        }

    }
}
