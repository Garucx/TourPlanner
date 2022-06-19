using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer.Logging;
using TourPlanner.BusinessLayer.MapQuest;
using TourPlanner.Commands;
using TourPlanner.DataLayer;
using TourPlanner.Model;
using TourPlanner.PresentationLayer.Commands;
using TourPlanner.PresentationLayer.DialogServices;
using TourPlanner.View;

namespace TourPlanner.ViewModel
{
    internal class TourViewModel
    {

        public TourViewModel()
        {
            // TODO: Get Saved Tours if they exist and initialize _Tour
            database connection = new database();
            _Tour = new ObservableCollection<Tour>(connection.GetAll());
            Log.LogInfo("Refreshing the GUI");
            // Test data
            AddNewTour = new AddNewTourCommand(this);
            Exit = new ExitApplicationCommand(this);
            ModifyWindow = new ModifyWindowCommand(this);
            RefreshWindow = new RefreshCommand(this);
            addtourlog = new AddTourLogWindowCommand(this);
            DeleteWindow = new DeleteTourCommand(this);
            AllTours = new ObservableCollection<Tour>();
            TextSearch = new FullTextSearchCommand(this);

            foreach (var item in Tour.ToList())
            {
                item.TourLogs = connection.GetTourLogsAsync(item.ID).Result;
                AllTours.Add(item);
            }
            connection.CloseConnection();

        }

        public ICommand TextSearch { get; private set; }
        internal async Task FullTextSearch()
        {
            await Task.Delay(1);
            if (string.IsNullOrWhiteSpace(SearchText))
                _Tour = AllTours;
            else
            {
                _Tour.Clear();
                foreach (var item in AllTours)
                {
                    if (item.Name.Contains(SearchText))
                        _Tour.Add(item);
                }
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
            tour.Image_link = "asdas00"; // Muss geändert werden
            tour.Name = $"{res.start.Address}TO{res.dest.Address}";
            Log.LogInfo("Neue Tour erstellt Name: " + tour.Name);
            database connection = new database();
            connection.Create_new_Tour(tour);
            int id = connection.Get_ID_From_Tour(tour.Name);
            tour.ID = id;
            Tour.Add(tour);
            _Tour.Add(tour);
            AllTours.Add(tour);
            route.image.Save("..\\..\\..\\PresentationLayer\\tour_images\\" + tour.ID.ToString() + ".png", ImageFormat.Png);
            connection.CloseConnection();
        }

        private ObservableCollection<Tour> _Tour = new ObservableCollection<Tour>();
        public ObservableCollection<Tour> Tour
        {
            get
            {
                return _Tour;
            }
        }
        public ObservableCollection<Tour> AllTours { get; private set; }
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
            await Task.Delay(1);
            Log.LogInfo("Shutdown");
            CanExit = false;
            Application.Current.Shutdown();
        }
        #endregion
        #region Modify
        public ICommand ModifyWindow { get; set; }
        public bool CanOpenModifyWindow { get; set; } = true;

        internal async Task OpenModifyWindow()
        {
            await Task.Delay(1);
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
            await Task.Delay(1);
            Log.LogInfo("Deleting Log with ID " + SelectedTour.ID);
            database connection = new database();
            connection.Delete_Tour(SelectedTour.ID);
            _Tour.Remove(SelectedTour);
        }


        #endregion
        #region Refresh
        public ICommand RefreshWindow { get; set; }
        public bool CanRefresh { get; set; } = true;

        public async Task Refresh()
        {
            await Task.Delay(1);
            database connection = new database();
            _Tour = new ObservableCollection<Tour>(connection.GetAll());
            connection.CloseConnection();
        }


        #endregion
        #region Add Tour Log
        public ICommand addtourlog { get; set; }
        public bool CanCreateTourLog { get; set; } = true;

        public async Task CreateTourLog()
        {
            await Task.Delay(1);
            Log.LogInfo("Opening Window for Add Tour Logs");
            AddTourLogWindow window = new AddTourLogWindow();
            window.ShowDialog();

        }
        #endregion


        #region IDK
        IDialogService _dialogService = new DialogService();




        public Tour SelectedTour
        {
            get;
            set;
        }

        public string SearchText { get; set; }


        /*
       public IEnumerable<Tour> Tour
       {
           get { return _Tour; }
       }
           */
        #endregion
    }
}
