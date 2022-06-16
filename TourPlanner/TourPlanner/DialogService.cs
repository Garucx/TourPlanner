using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Model;
using TourPlanner.View;
using TourPlanner.ViewModel;

namespace TourPlanner
{

    internal class DialogService : IDialogService
    {
        public (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback)
        {
            var dialog = new AddTourDialog();
            dialog.Owner = Application.Current.MainWindow;
            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;
            dialog.ShowDialog();
            MapQuestRequestData start = new MapQuestRequestData(dialog.Start_AreaCode,dialog.Start_Address,dialog.Start_City, dialog.Start_Country);
            MapQuestRequestData dest = new MapQuestRequestData(dialog.Dest_AreaCode, dialog.Dest_Address, dialog.Dest_City, dialog.Dest_Country); ;

            return (start,dest);
        }
    }
}
