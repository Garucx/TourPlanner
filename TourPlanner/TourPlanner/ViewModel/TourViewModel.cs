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
        
        public TourViewModel()
        {
            // TODO: Get Saved Tours if they exist and initialize _Tour


            // Test data
            _Tour.Add(new Tour("Test1", "Some Desc 1", "Test", "Test", "Test", 10f, 10, new System.Windows.Media.Imaging.BitmapImage(), "https:test"));
            _Tour.Add(new Tour("Test2", "Some Desc 2", "Test", "Test", "Test", 10f, 10, new System.Windows.Media.Imaging.BitmapImage(), "https:test"));
            _Tour.Add(new Tour("Test3", "Some Desc 3", "Test", "Test", "Test", 10f, 10, new System.Windows.Media.Imaging.BitmapImage(), "https:test"));
            AddNewTour = new AddNewTourCommand(this);

        }

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

        IDialogService _dialogService = new DialogService();

        public ICommand AddNewTour
        {
            get;
            private set;
        }

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

        public bool CanAdd { get; internal set; } = true;

        public void SaveChanges()
        {
            // TODO: Save Tour information / changes
        }
    }
}
