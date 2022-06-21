using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer.JSON;
using TourPlanner.BusinessLayer.Logging;
using TourPlanner.BusinessLayer.MapQuest;
using TourPlanner.BusinessLayer.PDF;
using TourPlanner.DataLayer;
using TourPlanner.DataLayer.Model;
using TourPlanner.PresentationLayer.Commands;
using TourPlanner.PresentationLayer.Commands.AddTourLog;
using TourPlanner.PresentationLayer.Commands.CreateTour;
using TourPlanner.PresentationLayer.Commands.ModifyTour;
using TourPlanner.PresentationLayer.Commands.ModifyTourLog;
using TourPlanner.PresentationLayer.DialogServices;
using TourPlanner.PresentationLayer.View;
using TourPlanner.View;

namespace TourPlanner.PresentationLayer.ViewModel
{
    internal class TourViewModel
    {

        public TourViewModel()
        {
            try
            {
                database connection = new database();
                _Tour = new ObservableCollection<Tour>(connection.GetAll());
                Log.LogInfo("Refreshing the GUI");
                initCommands();

                foreach (var item in Tour.ToList())
                {
                    item.TourLogs = connection.GetTourLogsAsync(item.ID).Result;
                    AllTours.Add(item);
                }
                connection.CloseConnection();
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }

        private void initCommands()
        {
            AddNewTour = new AddNewTourCommand(this);
            Exit = new ExitApplicationCommand(this);
            ModifyWindow = new ModifyWindowCommand(this);
            RefreshWindow = new RefreshCommand(this);
            addtourlog = new AddTourLogWindowCommand(this);
            DeleteWindow = new DeleteTourCommand(this);
            AllTours = new ObservableCollection<Tour>();
            TextSearch = new FullTextSearchCommand(this);
            CreatePdf = new CreatePDFCommand(this);
            DeleteTourLog = new DeleteTourLogCommand(this);
            ModifyTourLog = new ModifyTourLogWindowCommand(this);
            CreateSumm = new CreateSummPDFCommand(this);
            JSONout = new SaveJsonCommand(this);
            JsonIn = new OpenJsonCommand(this);
        }

        #region textsearch

        public ICommand TextSearch { get; private set; }
        internal async Task FullTextSearch()
        {
            try
            {
                await Task.Delay(1);
                if (string.IsNullOrWhiteSpace(SearchText))

                {
                    _Tour.Clear();
                    foreach (var item in AllTours)
                        _Tour.Add(item);
                }
                else
                {
                    _Tour.Clear();
                    foreach (var item in AllTours)
                    {
                        if (item.Name.Contains(SearchText))
                            _Tour.Add(item);
                    }
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

        }
        #endregion
        #region Create new tour
        internal async Task CreateNewTour()
        {
            try
            {
                var res = _dialogService.ShowDialog(result =>
                {
                    var test = result;
                });
                var route = await MapQuestRequestHandler.GetRouteAsync(res.start, res.dest);
                Tour tour = new Tour();
                tour.Distance = route.route.route.distance;
                tour.Time = route.route.route.time;
                tour.Tour_desc = $"From {res.start.Address} {res.start.AreaCode} {res.start.City} to {res.dest.Address} {res.dest.AreaCode} {res.dest.City}";
                tour.Transport_type = route.route.route.options.routeType;
                tour.From = $"{res.start.Address}";
                tour.To = res.dest.Address;
                tour.Image_link = route.URL;
                tour.Name = $"{res.start.Address}TO{res.dest.Address}";
                tour.Route_information = route.image;
                Log.LogInfo("Neue Tour erstellt Name: " + tour.Name);
                database connection = new database();
                connection.Create_new_Tour(tour);
                int id = connection.Get_ID_From_Tour(tour.Name);
                tour.ID = id;
                _Tour.Add(tour);
                AllTours.Add(tour);
                SaveBitmapImage.SaveImage(tour.Route_information, "..\\..\\..\\PresentationLayer\\tour_images\\" + tour.ID.ToString() + ".png");
                connection.CloseConnection();
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.LogError(ex.Message);
            }

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
            await Task.Delay(1);
            Log.LogInfo("Shutdown");
            CanExit = false;
            Application.Current.Shutdown();
        }
        #endregion
        #region Modify
        public ICommand ModifyWindow { get; set; }
        public bool CanOpenModifyWindow { get; set; } = true;

        internal async Task OpenModifyWindow()
        {
            try
            {
                await Task.Delay(1);
                Log.LogInfo("Opening Modify Window");
                if (SelectedTour == null)
                {
                    Log.LogError("Kein Tour ausgewählt zum Überschreiben");
                    MessageBox.Show("Please select a Tour");
                    return;
                }
                ModifyWPF viewModel = new ModifyWPF();
                viewModel.DataContext = new ModifyWindowModel(SelectedTour);
                viewModel.ShowDialog();
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

        }

        #endregion
        #region Delete
        public ICommand DeleteWindow { get; set; }

        public bool CanOpenDeleteWindow { get; set; } = true;

        internal async Task OpenDeleteWindow()
        {
            try
            {
                if(SelectedTour == null)
                {
                    throw new Exception("Bitte Wählen sie eine Tour aus");
                }
                await Task.Delay(1);
                Log.LogInfo("Deleting Log with ID " + SelectedTour.ID);
                database connection = new database();
                connection.Delete_Tour(SelectedTour.ID);
                SaveBitmapImage.DeleteImage(SelectedTour.ID.ToString(), $"..\\..\\..\\PresentationLayer\\tour_images\\");
                AllTours.Remove(SelectedTour);
                _Tour.Remove(SelectedTour);
            }catch(NullReferenceException n)
            {
                MessageBox.Show(n.Message);
                Log.LogError(n.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }

        }


        #endregion
        #region Refresh
        public ICommand RefreshWindow { get; set; }
        public bool CanRefreshandCreatePDF { get; set; } = true;

        public async Task Refresh()
        {
            try
            {
                await Task.Delay(1);
                database connection = new database();
                _Tour = new ObservableCollection<Tour>(connection.GetAll());
                foreach (var item in _Tour.ToList())
                {
                    item.TourLogs = connection.GetTourLogsAsync(item.ID).Result;
                    AllTours.Add(item);
                }
                connection.CloseConnection();
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

        }


        #endregion
        #region Add Tour Log
        public ICommand addtourlog { get; set; }
        public bool CanCreateTourLog { get; set; } = true;

        public async Task CreateTourLog()
        {
            try
            {
                if (SelectedTour == null)
                {
                    Log.LogError("Kein Tour ausgewählt zum Überschreiben");
                    MessageBox.Show("Please select a Tour");
                    return;
                }
                Log.LogInfo("Opening Window for Add Tour Logs");
                AddTourLogWindow window = new AddTourLogWindow();
                window.DataContext = new AddTourLogViewModel(SelectedTour);
                window.ShowDialog();
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
            await Task.Delay(1);


        }
        #endregion
        #region PDF Tour
        public ICommand CreatePdf { get; set; }

        public async Task CreatePDF()
        {
            try
            {
                if (SelectedTour == null)
                {
                    throw new ArgumentNullException("Please select a Tour to create a PDF");
                }
                CreatePDF pdfcreator = new CreatePDF("../../../PDFs/");
                pdfcreator.CreateTourPDF(SelectedTour.Name, SelectedTour, true);
            }
            catch (ArgumentNullException ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

        }

        #endregion
        #region PDF Summ
        public ICommand CreateSumm { get; set; }

        public async Task CreateSummPDF()
        {
            try
            {
                CreatePDF pdfcreator = new CreatePDF("../../../PDFs/");
                pdfcreator.CreateSummarize_report(AllTours.ToList());
            }
            catch (ArgumentNullException ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

        }


        #endregion
        #region Delte Tour LOg
        public ICommand DeleteTourLog { get; set; }

        public async Task DeleteLog()
        {
            try
            {
                if (SelectedTourLog == null)
                {
                    throw new ArgumentNullException("Please select a Tour log");
                }
                database connection = new database();
                connection.Delte_Tour_Log(SelectedTourLog.tourLogId);
                SelectedTour.TourLogs.Remove(SelectedTourLog);
                connection.CloseConnection();

            }
            catch (ArgumentNullException ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }


        #endregion
        #region Modify Tour Log
        public ICommand ModifyTourLog { get; set; }

        public async void ModifyLog()
        {
            try
            {
                if (SelectedTourLog == null)
                {
                    throw new ArgumentNullException("No Log Selected");
                }
                ModifyTourLogWindow window = new ModifyTourLogWindow();
                window.DataContext = new ModifyTourLogViewModel(SelectedTourLog);
                window.ShowDialog();

            }
            catch (ArgumentNullException ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }
        #endregion
        #region JSON OUT
        public ICommand JSONout { get; set; }

        public async Task SaveTour()
        {
            try
            {
                if (SelectedTour == null)
                {
                    throw new InvalidOperationException("Please Seleced a Tour for Saving");
                }
                Jsonall.Save(SelectedTour);

            }
            catch (ArgumentNullException ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }

        }

        #endregion
        #region JSON In
        public ICommand JsonIn { get; set; }

        public async Task ReadJson()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    if (!openFileDialog.FileName.Contains(".json"))
                    {
                        throw new Exception("Wählen sie eine Json datei aus");
                    }
                    Tour tour = await Jsonall.Open(openFileDialog.FileName);
                    if(AllTours.ToList().Exists(x => x.ID == tour.ID))
                    {
                        throw new Exception("Dieser Tour kann nicht erstellt werden, da er bereits exestiert");
                    }
                    if (tour == null)
                    {
                        throw new ArgumentException("Das ausgewählte Dokument kann nicht Seraliziert werden");
                    }
                    if (tour.Route_information == null)
                    {
                        tour.Route_information = LoadBitmapImage.LoadImage(tour.ID.ToString(), $"../../../PresentationLayer/tour_images/");
                    }
                    AllTours.Add(tour);
                    _Tour.Add(tour);
                    database connection = new database();
                    connection.Create_new_Tour(tour);
                    connection.CloseConnection();
                }
            }
            catch (ArgumentException ar)
            {
                Log.LogError(ar.Message);
                MessageBox.Show(ar.Message);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        #region IDK

        private ObservableCollection<Tour> _Tour = new ObservableCollection<Tour>();
        public ObservableCollection<Tour> Tour
        {
            get
            {
                return _Tour;
            }
        }
        public ObservableCollection<Tour> AllTours { get; private set; }
        IDialogService _dialogService = new DialogService();




        public Tour SelectedTour
        {
            get;
            set;
        }
        public TourLog SelectedTourLog
        {
            get;
            set;
        }
        public string SearchText { get; set; }


        /*
       public IEnumerable<Tour> Tour
       {
           get { return _Tour; }
       }
           */
        #endregion
    }
}
