using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Model;

namespace TourPlanner
{
    internal interface IDialogService
    {
        (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback);
    }
}
