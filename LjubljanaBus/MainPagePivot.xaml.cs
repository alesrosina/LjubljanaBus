using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using LjubljanaBus.Helpers;

namespace LjubljanaBus
{
    public partial class MainPagePivot : PhoneApplicationPage
    {
        public MainPagePivot()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPagePivot_Loaded);
        }

        void MainPagePivot_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.IsSettingsEmpty)
            {
                if (MessageBox.Show("This application uses your location. By clicking OK, you will allow it to access location services. You can change this setting later in settings page.", "Location services", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    Settings.LocationServices = true;
                else
                    Settings.LocationServices = false;
            }

            if (!Settings.LocationServices)
            {
                pivotMain.Items.Remove(pivotNearMe);
                //pivotNearMe.Visibility = System.Windows.Visibility.Collapsed;
            }

            Visibility v = (Visibility)Resources["PhoneLightThemeVisibility"];
            Uri u;
            //ImageBrush ib = new ImageBrush();
            if (v == System.Windows.Visibility.Visible)
                u = new Uri("Images/lpp_white.png", UriKind.Relative);
            else
                u = new Uri("Images/lpp_black.png", UriKind.Relative);
            backgroundImage.ImageSource = new System.Windows.Media.Imaging.BitmapImage(u);

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            listBoxFavorites.ItemsSource = App.Favorites.Stations;

        }

        private void listBoxStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                Station s = e.AddedItems[0] as Station;
                if (s != null)
                {
                    if (s.ID != "0")
                    {
                        string uri = String.Format("/StationPage.xaml?id={0}&lat={1}&lang={2}&name={3}", s.ID, s.Latitude, s.Langitude, s.Name);

                        NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }
                }
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem p = e.AddedItems[0] as PivotItem;
            if (p != null)
            {
                if (p.Name == "panNearMe" && Settings.LocationServices)
                {
                    LoadStationsNearMe();
                    App.ViewModel.IsDataLoading = true;

                }
                else
                {
                    if (loc != null)
                        loc.Stop();
                    App.ViewModel.IsDataLoading = false;
                }
            }
            //MessageBox.Show("krneki;");
        }


        private GeoCoordinateWatcher loc = null;

        public void LoadStationsNearMe()
        {
            //if (App.ViewModel.StationsNearMe == null)
            //    App.ViewModel.StationsNearMe = new List<Station>();
            //App.ViewModel.StationsNearMe.Clear();

            if (loc == null)
            {
                loc = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);

                loc.StatusChanged += loc_StatusChanged;
            }

            if (loc.Status == GeoPositionStatus.Disabled)
            {
                loc.StatusChanged -= loc_StatusChanged;
                App.ViewModel.StationsNearMe.Add(new Station() { Name = "No data available.", ID = "0" });
                return;
            }

            loc.Start();

        }

        void loc_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

            if (e.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate currLoc = loc.Position.Location;

                App.ViewModel.LoadStationsNearLocation(currLoc);
                listBoxNearMe.ItemsSource = App.ViewModel.StationsNearMe;
                App.ViewModel.IsDataLoading = false;
                loc.Stop();
                loc.StatusChanged -= loc_StatusChanged;
            }
        }

        private void barCallUrbana_Click(object sender, EventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask p = new Microsoft.Phone.Tasks.PhoneCallTask();
            p.DisplayName = "Moneta";
            p.PhoneNumber = "1899";
            p.Show();

        }

        private void barMap_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));

        }

        private void barScheme_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SchemePage.xaml", UriKind.Relative));
        }

        private void barSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void barAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void barSearch_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }
    }
}