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
    internal class ModifyWindowModel : INotifyPropertyChanged
    {
        public ModifyWindowModel()
        {
            search = new ModifySearchCommand(this);
            Modifynow = new ModifyTourCommand(this);
        }

        private Tour ModifingTour;

        #region strings
        public string name { get => _name; set => SetField(ref _name, value); }
        private string _name = "";

        public string find { get => _find; set => SetField(ref _find, value); }
        private string _find = "";
        public string start_add { get => _start_add; set => SetField(ref _start_add, value); }
        private string _start_add = "";


        public string start_code { get => _start_code; set => SetField(ref _start_code, value); }
        private string _start_code = "";

        public string start_city { get => _start_city; set => SetField(ref _start_city, value); }
        private string _start_city = "";

        public string start_country { get => _start_country; set => SetField(ref _start_country, value); }
        private string _start_country = "";

        public string dest_add { get => _dest_add; set => SetField(ref _dest_add, value); }
        private string _dest_add = "";


        public string dest_code { get => _dest_code; set => SetField(ref _dest_code, value); }
        private string _dest_code = "";

        public string dest_city { get => _dest_city; set => SetField(ref _dest_city, value); }
        private string _dest_city = "";

        public string dest_country { get => _dest_country; set => SetField(ref _dest_country, value); }
        private string _dest_country = "";

        #endregion


        #region modifybutton
        public ICommand Modifynow { get; set; }

        public bool Canmodify { get; set; } = false;

        public async Task ModifynowAsync()
        {
            if(ModifingTour == null)
            {
                Log.LogError("Not possible to Modify a tour that doen't exist");
                find = "Didn't select Tour";
                Canmodify = false;
            }
            else
            {



            }

        }

        #endregion
        #region searchbutton
        public ICommand search { get; set; }
        public bool CanSearch { get; internal set; } = true;

        public async Task SearchTour()
        {

            Log.LogInfo("Searching after Tour: " + name);
            database connection = new database();
            try
            {
                int id = connection.Get_ID_From_Tour(name);
                if (id == 0) Log.LogError($"Tour with name: {name} doesn't exist");
                else
                {
                    ModifingTour = await connection.GetTourAsync(id);
                    if(ModifingTour != null)
                    {
                        find = $"Tour with name: {name} exists. You can Modify it now";
                        Canmodify = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
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
