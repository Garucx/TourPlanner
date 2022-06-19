using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Model;
using TourPlanner.View;

namespace TourPlanner.ViewModel
{
    internal class TourViewModel
    {

        public TourViewModel()
        {
            // TODO: Get Saved Tours if they exist and initialize _Tour
            database connection = new database();
            _Tour = connection.GetAll();
            Log.LogInfo("Refreshing the GUI");
            connection.CloseConnection();
            // Test data
            AddNewTour = new AddNewTourCommand(this);
            Exit = new ExitApplicationCommand(this);
            ModifyWindow = new ModifyWindowCommand(this);
            RefreshWindow = new RefreshCommand(this);
            addtourlog = new AddTourLogWindowCommand(this);
            DeleteWindow= new DeleteTourCommand(this);
            Tour = new ObservableCollection<Tour>();
            foreach (var item in _Tour)
            {
                Tour.Add(item);
            }
        }

        #region Create new tour
        internal async Task CreateNewTour()
        {
            var res = _dialogService.ShowDialog(result =>
            {
                var test = result;
            });

            var route = await MapQuestRequestHandler.GetRouteAsync(res.start, res.dest);
            Tour tour = new Tour();
            tour.Distance = route.route.route.distance;
            tour.Time = route.route.route.time;
            tour.Tour_desc = $"From {res.start.Address} {res.start.AreaCode} {res.start.City} to {res.dest.Address} {res.dest.AreaCode} {res.dest.City}";
            tour.Transport_type = route.route.route.options.routeType;
            tour.From = res.start.Address;
            tour.To = res.dest.Address;
            tour.Image_link = "asdas00";
            tour.Name = $"{res.start.Address}TO{res.dest.Address}";
            Log.LogInfo("Neue Tour erstellt Name: " + tour.Name);
            database connection = new database();
            connection.Create_new_Tour(tour);
            int id = connection.Get_ID_From_Tour(tour.Name);
            tour.ID = id;
            Tour.Add(tour);
            _Tour.Add(tour);
            connection.CloseConnection();
        }
        public bool CanAdd { get; internal set; } = true;

        public ICommand AddNewTour
        {
            get;
            private set;
        }
        #endregion
        #region Exit
        public ICommand Exit { get; private set; }
        public bool CanExit { get; internal set; } = true;

        internal async Task ExitApplication()
        {
            Log.LogInfo("Shutsown");
            CanExit = false;
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
        #region Modify
        public ICommand ModifyWindow { get; set; }
        public bool CanOpenModifyWindow { get; set; } = true;

        internal async Task OpenModifyWindow()
        {
            Log.LogInfo("Opening Modify Window");
            ModifyWPF viewModel = new ModifyWPF();
            viewModel.ShowDialog();
        }

        #endregion
        #region Delete
        public ICommand DeleteWindow { get; set; }

        public bool CanOpenDeleteWindow { get; set; } = true;

        internal async Task OpenDeleteWindow()
        {
            Log.LogInfo("Deleting Log with ID " + SelectedTour.ID);
            database connection = new database();
            connection.Delete_Tour(SelectedTour.ID);
            Tour.Remove(SelectedTour);
           
        }

    
    #endregion
    #region Refresh
    public ICommand RefreshWindow { get; set; }
    public bool CanRefresh { get; set; } = true;

    public async Task Refresh()
    {
        database connection = new database();
        _Tour = connection.GetAll();
        Tour.Clear();
        foreach (var item in _Tour)
        {
            Tour.Add(item);
        }
        connection.CloseConnection();
    }


    #endregion
    #region Add Tour Log
    public ICommand addtourlog { get; set; }
    public bool CanCreateTourLog { get; set; } = true;

    public async Task CreateTourLog()
    {
        Log.LogInfo("Opening Window for Add Tour Logs");
        AddTourLogWindow window = new AddTourLogWindow();
        window.ShowDialog();

    }
    #endregion


    #region IDK
    IDialogService _dialogService = new DialogService();

    private List<Tour> _Tour = new List<Tour>();



    public Tour SelectedTour
    {
        get;
        set;
    }

    public ObservableCollection<Tour> Tour { get; private set; }
    /*
   public IEnumerable<Tour> Tour
   {
       get { return _Tour; }
   }
       */
    #endregion
}
    }
