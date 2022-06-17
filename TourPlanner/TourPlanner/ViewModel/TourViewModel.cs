using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Model;

namespace TourPlanner.ViewModel
{
    internal class TourViewModel
    {
        
        public  TourViewModel()
        {
            // TODO: Get Saved Tours if they exist and initialize _Tour

            database connection = new database();
            _Tour = connection.GetAll();

            // Test data
            AddNewTour = new AddNewTourCommand(this);
            Exit = new ExitApplicationCommand(this);
        }

        #region Create new tour
        internal async Task CreateNewTour()
        {
            var res = _dialogService.ShowDialog(result =>
            {
                var test = result;
            });

            var route = await MapQuestRequestHandler.GetRouteAsync(res.start, res.dest);
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


       
    }
}
