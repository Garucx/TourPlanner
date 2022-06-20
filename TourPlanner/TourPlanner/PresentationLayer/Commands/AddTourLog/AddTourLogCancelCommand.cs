using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.PresentationLayer.ViewModel;

namespace TourPlanner.PresentationLayer.Commands.AddTourLog
{

    internal class AddTourLogCancelCommand : ICommand
    {

        private AddTourLogViewModel _viewModel;
        public AddTourLogCancelCommand(AddTourLogViewModel viewModel)
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
            return _viewModel.CanDo;
        }

        public void Execute(object? parameter)
        {
            _viewModel.CancelAsync();
        }
    }
}
