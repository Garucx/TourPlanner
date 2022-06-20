using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.PresentationLayer.ViewModel;

namespace TourPlanner.PresentationLayer.Commands
{
    internal class CreateSummPDFCommand : ICommand
    {

        private TourViewModel _viewModel;
        public CreateSummPDFCommand(TourViewModel viewModel)
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
            _viewModel.CreateSummPDF();
        }


    }
}
