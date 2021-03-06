using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.PresentationLayer.ViewModel;

namespace TourPlanner.PresentationLayer.Commands.CreateTour
{
    internal class AddNewTourCommand : ICommand
    {
        private TourViewModel _viewModel;
        public AddNewTourCommand(TourViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _viewModel.CanAdd;
        }

        public void Execute(object? parameter)
        {
            _viewModel.CreateNewTour();
        }
    }
}
