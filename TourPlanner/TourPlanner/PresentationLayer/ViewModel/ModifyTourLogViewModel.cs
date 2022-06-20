using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer.Logging;
using TourPlanner.DataLayer;
using TourPlanner.DataLayer.Model;
using TourPlanner.PresentationLayer.Commands.ModifyTourLog;

namespace TourPlanner.PresentationLayer.ViewModel
{
    internal class ModifyTourLogViewModel : INotifyPropertyChanged
    {
        public TourLog SelectedTourLog;
        public ModifyTourLogViewModel(TourLog tour)
        {
            Cancel = new TourLogCancelCommand(this);
            mod = new ModifyTourLogCommand(this);
            SelectedTourLog = tour;
            comment = SelectedTourLog.Comment;
            rating = SelectedTourLog.rating.ToString();
            diff = SelectedTourLog.difficulty.ToString();
            selecteddate = SelectedTourLog.date_time;
            total_time = SelectedTourLog.total_time.ToString();
        }


        #region Modify
        public ICommand mod { get; set; }

        public async Task ModifyTourLog()
        {

            if (string.IsNullOrEmpty(diff) || string.IsNullOrEmpty(comment) || string.IsNullOrEmpty(total_time) || string.IsNullOrEmpty(rating))
            {
                error = "Please fillout every Box";
                Log.LogError(error);
                return;
            }
            int Rating = 0;
            int Diff = 0;
            int Time = 0;

            try
            {
                Rating = int.Parse(rating);
                Diff = int.Parse(diff);
                Time = int.Parse(total_time);
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
                error = e.Message;
                return;
            }
            database connection = new database();

            SelectedTourLog.total_time = Time;
            SelectedTourLog.rating = Rating;
            SelectedTourLog.difficulty = Diff;
            SelectedTourLog.date_time = selecteddate;
            SelectedTourLog.Comment = comment;

            connection.Modify_Tour_Log(SelectedTourLog);
            connection.CloseConnection();
            error = $"Tour with ID {SelectedTourLog.tourLogId} got updated";
        }
        #endregion

        #region Cancel
        public ICommand Cancel { get; set; }
        public bool Candoboth { get; set; } = true;

        public async Task CancelAsync()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this)
                {
                    item.Close();
                }
            }
        }


        #endregion  
        #region strings

        private DateTime _date = DateTime.Now;
        public DateTime selecteddate { get => _date; set => SetField(ref _date, value); }
        public string comment { get => _comment; set => SetField(ref _comment, value); }
        private string _comment = "";
        public string diff { get => _diff; set => SetField(ref _diff, value); }
        private string _diff = "";

        public string total_time { get => _total_time; set => SetField(ref _total_time, value); }
        private string _total_time = "";

        public string rating { get => _rating; set => SetField(ref _rating, value); }
        private string _rating = "";
        public string error { get => _error; set => SetField(ref _error, value); }
        private string _error = "";
        #endregion
        #region Property Changed
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
