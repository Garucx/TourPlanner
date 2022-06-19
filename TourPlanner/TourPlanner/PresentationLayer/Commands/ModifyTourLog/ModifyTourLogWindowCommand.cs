﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModel;

namespace TourPlanner.PresentationLayer.Commands.ModifyTourLog
{
    internal class ModifyTourLogWindowCommand : ICommand
    {
        private TourViewModel _viewModel;
        public ModifyTourLogWindowCommand(TourViewModel viewModel)
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

        public void Execute(object? parameter)
        {
            _viewModel.ModifyLog();
        }
    }
}
