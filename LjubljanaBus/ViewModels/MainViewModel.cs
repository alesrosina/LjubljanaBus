using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using System.Device.Location;
using LjubljanaBus.Data;

namespace LjubljanaBus
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            //this.Items = new ObservableCollection<ItemViewModel>();
            this.Stations = new List<Station>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        //public ObservableCollection<ItemViewModel> Items { get; private set; }

        public List<Station> Stations { get; private set; }

        public ObservableCollection<Station> StationsNearMe { get; set; }

        public List<StationsInGroup<Station>> StationsGrouped { get; set; }

        //public ObservableCollection<StationViewModel> Favorites { get; private set; }


        public bool IsDataLoaded
        {
            get;
            private set;
        }

        private bool _isdataloading = false;

        public bool IsDataLoading
        {
            get { return _isdataloading; }
            set
            {
                if (value != _isdataloading)
                {
                    _isdataloading = value;
                    NotifyPropertyChanged("IsDataLoading");
                }
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            LoadStationsFromFile();

            //LoadStationsInGroups();

            this.IsDataLoaded = true;
        }

        public void LoadStationsNearLocation(GeoCoordinate location, int distance = 500)
        {
            if (App.ViewModel.StationsNearMe == null)
                App.ViewModel.StationsNearMe = new ObservableCollection<Station>();
            else
                App.ViewModel.StationsNearMe.Clear();

            foreach (Station item in App.ViewModel.Stations)
            {
                double tmp = item.Location.GetDistanceTo(location);
                if (tmp < distance)
                {
                    App.ViewModel.StationsNearMe.Add(item);
                }
            }

            if (App.ViewModel.StationsNearMe.Count == 0)
            {
                App.ViewModel.StationsNearMe.Add(new Station() { Name = AppResource.listNoData, ID = "0" });
            }
        }


        private void LoadStationsFromFile()
        {
            //FileStream file = ;
            string line;// = reader.ReadToEnd();


            using (StreamReader reader = new StreamReader(Application.GetResourceStream(new Uri(@"/LjubljanaBus;component/Data/stations.csv", UriKind.Relative)).Stream))
            //using (StreamReader reader = new StreamReader(File.OpenRead("Data/stations.csv")))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tmp = line.Split(new char[] { ';' });
                    if (tmp.Length == 5)
                    {
                        string id;
                        if (tmp[2].Length == 1)
                            id = "00" + tmp[2];
                        else if (tmp[2].Length == 2)
                            id = "0" + tmp[2];
                        else
                            id = tmp[2];



                        this.Stations.Add(new Station()
                        {
                            Langitude = tmp[0],
                            Latitude = tmp[1],
                            ID = id,
                            Name = tmp[3],
                            HasUrbanomat = (tmp[4] == "true") ? true : false
                        });


                    }
                }
            }
        }

        private void LoadStationsInGroups()
        {
            //StationsInGroup<Station> stGroup = new StationsInGroup<Station>();
            this.StationsGrouped = new List<StationsInGroup<Station>>();
            string tmpCat = "";
            bool first = true;
            List<Station> tmpStat = new List<Station>();
            foreach (Station item in this.Stations)
            {
                if (first)
                {
                    tmpCat = item.Category;
                    first = false;
                }


                if (tmpCat != item.Category)
                {
                    StationsInGroup<Station> tmpS = new StationsInGroup<Station>(tmpCat);
                    tmpS.AddRange(tmpStat);
                    this.StationsGrouped.Add(tmpS);
                    tmpStat.Clear();
                    //StationsInGroup<Station> stTmp = new StationsInGroup<Station>(item.Category);
                    tmpCat = item.Category;
                }

                tmpStat.Add(item);

            }
            StationsInGroup<Station> tmpS1 = new StationsInGroup<Station>(tmpCat);
            tmpS1.AddRange(tmpStat);
            this.StationsGrouped.Add(tmpS1);
            tmpStat.Clear();

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
    }
}