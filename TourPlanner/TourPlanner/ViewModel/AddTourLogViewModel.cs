using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Commands.AddTourLog;
using TourPlanner.Model;

namespace TourPlanner.ViewModel
{
    internal class AddTourLogViewModel : INotifyPropertyChanged
    {

        public AddTourLogViewModel()
        {
            add = new AddTourLogCommand(this);
            Cancel = new AddTourLogCancelCommand(this);
        }
        #region button

        public ICommand add { get; set; }
        public bool CanDo { get; set; } = true;

        public async Task CanAddAsync()
        {
            database connection = new database();
            if (!string.IsNullOrEmpty(name))
            {
                int id  = connection.Get_ID_From_Tour(name);
                if(id == 0)
                {
                    error = "Tour doesnt exist with this name";
                    Log.LogError("Tour doesnt exist with this name");
                    return;
                }

                if(string.IsNullOrEmpty(diff) ||  string.IsNullOrEmpty(comment) || string.IsNullOrEmpty(total_time) || string.IsNullOrEmpty(rating))
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
                }catch(Exception e)
                {
                    Log.LogError(e.Message);
                    error = e.Message;
                    return;
                }

                connection.Create_Tour_Log(new TourLog(id, selecteddate, comment, Diff, Rating, Time));
                error = "Create Tour Log For " + name;
                Log.LogInfo(error);

            }
            else
            {
                error = "Please enter a name";
                Log.LogError("Please enter a name");
            }


        }

        #endregion
        #region Cancel
        public ICommand Cancel { get; set; }

        public async Task CancelAsync()
        {
            foreach (Window item in Application.Current.Windows)
            {
                if(item.DataContext == this)
                {
                    item.Close();
                }
            }
        }


        #endregion  

        #region strings
        public string name { get => _name; set => SetField(ref _name, value); }
        private string _name = "";

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
