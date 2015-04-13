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
using System.Device.Location;
using LjubljanaBus.Helpers;
using LjubljanaBus.Data;
using Microsoft.Phone.Net.NetworkInformation;

namespace LjubljanaBus
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            Visibility v = (Visibility)Resources["PhoneLightThemeVisibility"];
            Uri u;
            ImageBrush ib = new ImageBrush();
            if (v == System.Windows.Visibility.Visible)
                u = new Uri("Images/lpp_white.png", UriKind.Relative);
            else
                u = new Uri("Images/lpp_black.png", UriKind.Relative);
            ib.ImageSource = new System.Windows.Media.Imaging.BitmapImage(u);
            panMain.Background = ib;

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (Settings.IsSettingsEmpty)
            {
                NavigationService.Navigate(new Uri("/AgreeLocationUsePage.xaml", UriKind.Relative));
                return;
                //if (MessageBox.Show(AppResource.msgAgreeToUseLoc, "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    Settings.LocationServices = true;
                //else
                //    Settings.LocationServices = false;
            }
            else
            {
                var entry = NavigationService.BackStack.FirstOrDefault();
                if (entry != null && entry.Source.OriginalString.Contains("AgreeLocationUsePage"))
                {
                    NavigationService.RemoveBackEntry();
                    NavigationService.RemoveBackEntry();
                }
            }

            if (!Settings.LocationServices)
            {
                panMain.Items.Remove(panNearMe);
                //pivotNearMe.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            listBoxFavorites.ItemsSource = App.Favorites.Stations;

            // FlurryWP7SDK.Api.LogPageView();
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
                        string uri = String.Format("/StationPage.xaml?id={0}&lat={1}&lang={2}&name={3}&urbana={4}", s.ID, s.Latitude, s.Langitude, s.Name, (s.HasUrbanomat == true) ? "true" : "false");

                        NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }
                    e.AddedItems.Clear();
                    ListBox box = sender as ListBox;
                    if (box != null)
                    {
                        box.SelectedIndex = -1;
                    }
                }
                
            }
        }

        private void btnScheme_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SchemePage.xaml", UriKind.Relative));
        }

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void btnCallMoneta_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask p = new Microsoft.Phone.Tasks.PhoneCallTask();
            p.DisplayName = "Moneta";
            p.PhoneNumber = "1899";
            p.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private GeoCoordinateWatcher loc = null;

        public void LoadStationsNearMe()
        {

            loc = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            loc.StatusChanged += loc_StatusChanged;
            loc.Start();

        }

        void loc_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

            if (e.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate currLoc = loc.Position.Location;

                if (App.ViewModel.StationsNearMe != null)
                    App.ViewModel.StationsNearMe.Clear();

                App.ViewModel.LoadStationsNearLocation(currLoc);
                listBoxNearMe.ItemsSource = App.ViewModel.StationsNearMe;
                App.ViewModel.IsDataLoading = false;
                loc.Stop();
                //loc.StatusChanged -= loc_StatusChanged;
            }
            else if (loc.Status == GeoPositionStatus.Disabled || !NetworkInterface.GetIsNetworkAvailable())
            {
                
                
                App.ViewModel.IsDataLoading = false;
                //MessageBox.Show(AppResource.msgLocServMustBeEnabled);
                if (App.ViewModel.StationsNearMe == null)
                    App.ViewModel.StationsNearMe = new System.Collections.ObjectModel.ObservableCollection<Station>();
                else
                    App.ViewModel.StationsNearMe.Clear();
                App.ViewModel.StationsNearMe.Add(new Station() { Name = AppResource.listNoData, ID = "0000" });
                listBoxNearMe.ItemsSource = App.ViewModel.StationsNearMe;
                //MessageBox.Show(AppResource.msgLocServMustBeEnabled);

                loc.Stop();
                //loc.StatusChanged -= loc_StatusChanged;
            }
        }

        private void Panorama1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanoramaItem p = e.AddedItems[0] as PanoramaItem;
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
        }

        private void favoritesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            ListBoxItem selectedListBoxItem = listBoxFavorites.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;

            if (selectedListBoxItem != null)
            {
                Station st = selectedListBoxItem.DataContext as Station;
                if (st != null)
                    App.Favorites.RemoveFirst(st.ID);
                //selectedListBoxItem.
            }
           //if(menuItem.Header.ToString() == "") 
        }
    }
}