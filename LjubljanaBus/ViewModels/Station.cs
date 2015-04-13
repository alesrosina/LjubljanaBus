using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;

namespace LjubljanaBus
{
    public class Station
    {
        private string _latitude;

        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (value != _latitude)
                {
                    _latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        private string _langitude;

        public string Langitude
        {
            get
            {
                return _langitude;
            }
            set
            {
                if (value != _langitude)
                {
                    _langitude = value;
                    NotifyPropertyChanged("Langitude");
                }
            }
        }

        private string _lines;

        public string Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                if (value != _lines)
                {
                    _lines = value;
                    NotifyPropertyChanged("Lines");
                }
            }
        }



        private string _id;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _distance;

        public string Distance
        {
            get { return _distance; }
            set
            {
                if (value != _distance)
                {
                    _distance = value;
                    NotifyPropertyChanged("Distance");
                }
            }
        }

        private bool _hasUrbana;

        public bool HasUrbanomat
        {
            get { return _hasUrbana; }
            set
            {
                if (value != _hasUrbana)
                {
                    _hasUrbana = value;
                    NotifyPropertyChanged("HasUrbanomat");
                }
            }
        }

        public string Category
        {
            get {
                string cat = this.Name.Substring(0, 1);
                int tmp;
                if (int.TryParse(cat, out tmp))
                {
                    return "#";
                }
                else
                    return cat.ToLower();
                
                 }

        }

        public string FriendlyID
        {
            get { return "Številka postajališča: " + this.ID; }
        }

        public GeoCoordinate Location
        {
            get
            {
                double latd, langd;
                Double.TryParse(this.Latitude, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture. NumberFormat, out latd);
                Double.TryParse(this.Langitude, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture. NumberFormat, out langd);
                return new GeoCoordinate(latd, langd);
            }
        }

        public string NameWithID
        {
            get { return this.Name + " (" + this.ID + ")"; }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            string tmp;
            if (this.HasUrbanomat)
                tmp = this.Name + " (u)";
            else
                tmp = this.Name;

            return tmp;
        }
    }
}