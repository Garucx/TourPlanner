using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataLayer.Model
{
    public class TourLog
    {
        public int idfromtour { get; set; }
        public DateTime date_time { get; set; }
        public string Comment { get; set; }
        public int difficulty { get; set; }

        public int rating { get; set; }
        public int total_time { get; set; }
        public int tourLogId { get; set; }

        public TourLog(int idfromtour, DateTime date_time, string Comment, int difficulty, int rating, int total_time)
        {
            this.idfromtour = idfromtour;
            this.date_time = date_time;
            this.Comment = Comment;
            this.difficulty = difficulty;
            this.rating = rating;
            this.total_time = total_time;
        }

        public TourLog(int idfromtour, DateTime date_time, string Comment, int difficulty, int rating, int total_time, int tourLogId)
        {
            this.idfromtour = idfromtour;
            this.date_time = date_time;
            this.Comment = Comment;
            this.difficulty = difficulty;
            this.rating = rating;
            this.total_time = total_time;
            this.tourLogId = tourLogId;
        }

    }
}
