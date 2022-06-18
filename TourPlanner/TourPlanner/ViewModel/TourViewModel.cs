using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        
        public  TourViewModel()
        {
            // TODO: Get Saved Tours if they exist and initialize _Tour
            database connection = new database();
            _Tour = connection.GetAll();
            Log.LogInfo("Refreshing the GUI");
            connection.CloseConnection();
            // Test data
            AddNewTour = new AddNewTourCommand(this);
            Exit = new ExitApplicationCommand(this);
            DeleteWindow = new DeleteWindowCommand(this);
            ModifyWindow = new ModifyWindowCommand(this);
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
            _Tour.Add(tour);
            connection.CloseConnection();
            // Neue Route inserten 
            // Log erstellen?
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
            DeleteWPF viewModel = new DeleteWPF();
           viewModel.ShowDialog();
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

        public IEnumerable<Tour> Tour
        {
            get { return _Tour; }
        }
        #endregion
    }
}
