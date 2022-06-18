using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Model;

namespace TourPlanner.ViewModel
{
    internal class DeleteWindowModel : INotifyPropertyChanged
    { 
        public DeleteWindowModel()
        {
            delete = new DeleteTourCommand(this);
        }
        public string name { get => _name; set => SetField(ref _name, value); }
        private string _name ="";
        public string error { get => _error; set => SetField(ref _error, value); }
        private string _error = "";

        #region Button
        public ICommand delete { get; set; }

        public bool löschen { get; internal set; } = true;

        public void DelteTour()
        {
            database connection = new database();
            try
            {
                int id = connection.Get_ID_From_Tour(name);
                if (id == 0) error = "Tour does not exist";
                else
                {
                    connection.Delete_Tour(id);
                    error = "Tour got Deleted";
                }
            }catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                connection.CloseConnection();
            }

        }
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
