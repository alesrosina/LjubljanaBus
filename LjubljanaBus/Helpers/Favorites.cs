using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;

namespace LjubljanaBus.Helpers
{
    public class Favorites
    {
        private ObservableCollection<Station> _stations;
        //private ObservableCollection<StationViewModel> _tmpStations;

        public ObservableCollection<Station> Stations
        {
            get { return _stations; }
            private set { _stations = value; }
        }
        private string _filename = "favs";

        public Favorites()
        {
            _stations = new ObservableCollection<Station>();
            Fill();
        }
        public Favorites(bool fillCollection)
        {
            if (fillCollection)
            {
                _stations = new ObservableCollection<Station>();
                Fill();
            }
        }

        public void Add(Station station)
        {
            Add(station, true);
        }

        private void Add(Station station, bool addToCollection)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                FileMode fm;
                if (!file.FileExists(_filename))
                {
                    //file.CreateFile(_filename);
                    fm = FileMode.CreateNew;
                }
                else
                    fm = FileMode.Append;
                try
                {
                    using (StreamWriter sw = new StreamWriter(file.OpenFile(_filename, fm, FileAccess.Write)))
                    {
                        sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\n", station.ID, station.Name, station.Latitude, station.Langitude, (station.HasUrbanomat == true) ? "true" : "false");
                        //sw.WriteLine("1. Buy supplies.");
                        sw.Close();
                        sw.Dispose();
                    }
                    if (addToCollection)
                        _stations.Add(station);
                }
                catch (IsolatedStorageException ex) { }

            }
        }

        public Station GetFirst(string stationId)
        {
            try
            {
                return _stations.First(a => a.ID == stationId);
            }
            catch
            {
                return null;
            }
        }

        public ObservableCollection<Station> Fill()
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(_filename))
                    return null;

                try
                {
                    using (StreamReader reader = new StreamReader(file.OpenFile(_filename, FileMode.Open, FileAccess.Read)))
                    {
                        string line;// = reader.ReadToEnd();
                        
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] tmp = line.Split(new char[] { '\t' });
                            if (tmp.Length == 4)
                                _stations.Add(new Station() { ID = tmp[0], Name = tmp[1], Latitude = tmp[2], Langitude = tmp[3]});
                            else if(tmp.Length==5)
                                _stations.Add(new Station() { ID = tmp[0], Name = tmp[1], Latitude = tmp[2], Langitude = tmp[3], HasUrbanomat = (tmp[4] == "true") ? true : false });
                        }

                        reader.Close();
                        reader.Dispose();
                    }
                }
                catch (IsolatedStorageException ex)
                {
                }

            }

            return _stations;
        }

        public void RemoveFirst(string stationId)
        {
            _stations.Remove(_stations.First(a => a.ID == stationId));
            WriteCollectionToFile();
        }

        private void WriteCollectionToFile()
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream fs = file.OpenFile(_filename, FileMode.Truncate);
                fs.Close();
                fs.Dispose();
                
            }

            foreach (var item in this.Stations)
            {
                this.Add(item, false);
            }
        }
    }
}
