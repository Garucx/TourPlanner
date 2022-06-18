using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModel;

namespace TourPlanner.Commands
{
    internal class ModifySearchCommand : ICommand
    {
        private ModifyWindowModel _viewModel;
        public ModifySearchCommand(ModifyWindowModel viewModel)
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
            return _viewModel.CanSearch;
        }

        public void Execute(object? parameter)
        {
            _viewModel.SearchTour();
        }

    }
}
