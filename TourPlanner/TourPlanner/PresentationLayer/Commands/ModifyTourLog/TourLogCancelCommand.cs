using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.PresentationLayer.ViewModel;

namespace TourPlanner.PresentationLayer.Commands.ModifyTourLog
{
    internal class TourLogCancelCommand : ICommand
    {
        private ModifyTourLogViewModel _viewModel;
        public TourLogCancelCommand(ModifyTourLogViewModel viewModel)
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
            return _viewModel.Candoboth;
        }

        public void Execute(object? parameter)
        {
            _viewModel.CancelAsync();
        }
    }
}
