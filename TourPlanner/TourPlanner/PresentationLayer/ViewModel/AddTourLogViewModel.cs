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
using TourPlanner.PresentationLayer.Commands.AddTourLog;

namespace TourPlanner.PresentationLayer.ViewModel
{
    internal class AddTourLogViewModel : INotifyPropertyChanged
    {
        public Tour SelectedTour;
        public AddTourLogViewModel(Tour item)
        {
            add = new AddTourLogCommand(this);
            Cancel = new AddTourLogCancelCommand(this);
            SelectedTour = item;
        }
        #region button

        public ICommand add { get; set; }
        public bool CanDo { get; set; } = true;

        public async Task CanAddAsync()
        {
            database connection = new database();

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
            TourLog temp = new TourLog(SelectedTour.ID, selecteddate, comment, Diff, Rating, Time);
            connection.Create_Tour_Log(temp);
            var List = await connection.GetTourLogsAsync(SelectedTour.ID);
            temp.tourLogId = List.Last().tourLogId;
            if(SelectedTour.TourLogs == null)
            {
                SelectedTour.TourLogs = new List<TourLog>();
            }
            SelectedTour.TourLogs.Add(temp);
            error = "Create Tour Log For " + SelectedTour.Name;
            Log.LogInfo(error);
            connection.CloseConnection();

        }

        #endregion
        #region Cancel
        public ICommand Cancel { get; set; }

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
