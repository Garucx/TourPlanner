using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModel;

namespace TourPlanner.Commands
{
    internal class ModifyTourCommand : ICommand
    {
        private ModifyWindowModel _viewModel;
        public ModifyTourCommand(ModifyWindowModel viewModel)
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
            return _viewModel.Canmodify;
        }

        public void Execute(object? parameter)
        {
            _viewModel.ModifynowAsync();
        }


    }
}
