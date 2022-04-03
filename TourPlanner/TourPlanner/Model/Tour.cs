﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TourPlanner.Model
{
    internal class Tour : INotifyPropertyChanged
    {
        
        public Tour()
        {

        }


        public Tour(string name, string tour_desc, string from, string to, string transport_type, float distance, int time, BitmapImage route_information)
        {
            Name = name;
            Tour_desc = tour_desc;
            To = to;
            From = from;
            Transport_type = transport_type;
            Distance = distance;
            Time = time;
            Route_information = route_information;
        }
        public string Name { get => _Name; set => SetField(ref _Name, value); }
        private string _Name;
        string Tour_desc { get => _Tour_desc; set => SetField(ref _Tour_desc, value); }
        private string _Tour_desc;
        string From { get => _From; set => SetField(ref _From, value); }
        private string _From;
        string To { get => _To; set => SetField(ref _To, value); }
        private string _To;
        string Transport_type { get => _Transport_type; set => SetField(ref _Transport_type, value); }
        private string _Transport_type;
        float Distance { get => _Distance; set => SetField(ref _Distance, value); }
        private float _Distance;
        int Time { get => _Time; set => SetField(ref _Time, value); }
        private int _Time;
        BitmapImage Route_information { get => _Route_information; set => SetField(ref _Route_information, value); }
        private BitmapImage _Route_information;



        #region Property Changed
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

    }
}
