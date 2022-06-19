using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModel;

namespace TourPlanner.PresentationLayer.Commands
{
    internal class FullTextSearchCommand : ICommand
    {

        private TourViewModel _viewModel;
        public FullTextSearchCommand(TourViewModel viewModel)
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
            return _viewModel.CanRefreshandCreatePDF;
        }

        void ICommand.Execute(object? parameter)
        {
            _viewModel.FullTextSearch();
        }
    }
}
