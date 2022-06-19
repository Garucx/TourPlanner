using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BusinessLayer.MapQuest;

namespace TourPlanner.PresentationLayer.DialogServices
{
    internal interface IDialogService
    {
        (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback);
    }
}
