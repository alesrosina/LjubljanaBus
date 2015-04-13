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
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using System.Device.Location;
using LjubljanaBus.Helpers;
using LjubljanaBus.Data;

namespace LjubljanaBus
{
    public partial class MapPage : PhoneApplicationPage
    {

        private GeoCoordinateWatcher loc = null;

        public MapPage()
        {
            InitializeComponent();

            //if (!Settings.IsSettingsEmpty)
            //{
                if (!Settings.LocationServices)
                {
                    this.ApplicationBar.Buttons.RemoveAt(2);
                    //barMyLocation.IsEnabled = false;
                }
                else
                    ((ApplicationBarIconButton)this.ApplicationBar.Buttons[2]).Text = AppResource.mapPageMyLoc;
            //}
            ((ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0]).Text = AppResource.mapPageAerialView;

            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).Text = AppResource.mapPageZoomOut;
            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = AppResource.mapPageZoomIn;
            
           
        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {

            
            progressBar1.IsIndeterminate = true;
            map1.ViewChangeStart += map1_ViewChangeStart;
            map1.ViewChangeEnd += map1_ViewChangeEnd;
            

            if (map1.ZoomLevel < 10.0)
                map1.SetView(App.ViewModel.Stations.First(a => a.Name.ToLower() == "konzorcij").Location, 16.0);

            

            App.ViewModel.LoadStationsNearLocation(map1.TargetCenter, 500);
            //mapControl.ItemsSource = App.ViewModel.StationsNearMe;
            FillPushpins();
        }

        void map1_ViewChangeStart(object sender, MapEventArgs e)
        {
            //barMyLocation.IsEnabled = false;
            progressBar1.IsIndeterminate = true;
        }

        void map1_ViewChangeEnd(object sender, MapEventArgs e)
        {
            App.ViewModel.LoadStationsNearLocation(map1.TargetCenter, 500);
            //mapControl.ItemsSource = App.ViewModel.StationsNearMe;
            FillPushpins();
            
            //mapControl.UpdateLayout();
            //barMyLocation.IsEnabled = true;
            progressBar1.IsIndeterminate = false;
        }

        private void barViewSwitch_Click(object sender, EventArgs e)
        {

            if (map1.Mode is AerialMode)
            {
                map1.Mode = new RoadMode();
                ((ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0]).Text = AppResource.mapPageAerialView;
            }
            else
            {
                ((ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0]).Text = AppResource.mapPageStreetView;
                map1.Mode = new AerialMode(true);
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Station s = btn.Content as Station;
                if (s != null)
                {
                    if (s.ID != "0")
                    {
                        string uri = String.Format("/StationPage.xaml?id={0}&lat={1}&lang={2}&name={3}&urbana={4}", s.ID, s.Latitude, s.Langitude, s.Name, s.HasUrbanomat);

                        NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                    }

                }
            }
           // MessageBox.Show( btn.Content.ToString());
            //mapControl.Items
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar1.IsIndeterminate = true;
            FlurryWP7SDK.Api.LogPageView();
        }

        private void barZoomIn_Click(object sender, EventArgs e)
        {
            map1.ZoomLevel += 0.4;
        }

        private void barZoomOut_Click(object sender, EventArgs e)
        {
            map1.ZoomLevel -= 0.4;
            
        }

        private void barMyLocation_Click(object sender, EventArgs e)
        {
            if (map1.IsIdle)
            {
                if (loc == null)
                {
                    loc = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);

                    loc.StatusChanged += loc_StatusChanged;
                }

                //if (loc.Status == GeoPositionStatus.Disabled)
                //{
                //    loc.StatusChanged -= loc_StatusChanged;
                //    MessageBox.Show(AppResource.msgLocServMustBeEnabled);
                //    //App.ViewModel.StationsNearMe.Add(new Station() { Name = "No data available.", ID = "0" });
                //    return;
                //}
                progressBar1.IsIndeterminate = true;
                loc.Start();
            }
        }

        void loc_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

            if (e.Status == GeoPositionStatus.Ready)
            {
                progressBar1.IsIndeterminate = false;
                Pushpin p = drawPushPin(loc.Position.Location);
                //loc.
                mapControl.Items.Add(p);
                //pinMyLocation.Visibility = System.Windows.Visibility.Visible;
                map1.SetView(loc.Position.Location, 17.0);
                
                loc.Stop();
            }
            else if (loc.Status == GeoPositionStatus.Disabled)
            {
                //loc.StatusChanged -= loc_StatusChanged;

                if (progressBar1.IsIndeterminate)
                    MessageBox.Show(AppResource.msgLocServMustBeEnabled);
                progressBar1.IsIndeterminate = false;
                ApplicationBarIconButton b = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
                if (b != null)
                {
                    b.IsEnabled = false;
                }
                //App.ViewModel.StationsNearMe.Add(new Station() { Name = "No data available.", ID = "0" });
                //return;
                loc.Stop();
                
            }
        }

        private Pushpin drawPushPin(GeoCoordinate location)
        {
            Pushpin p = new Pushpin();
            p.Template = this.Resources["pinMyLoc"] as ControlTemplate;
            p.Location = location;
            return p;
        }



        private void FillPushpins()
        {
            mapControl.Items.Clear();

            foreach (var item in App.ViewModel.StationsNearMe)
            {
                mapControl.Items.Add(item);
            }
            if (loc != null)
            {
                if (loc.Position != null)
                {
                    if (loc.Status == GeoPositionStatus.Ready)
                        mapControl.Items.Add(drawPushPin(loc.Position.Location));
                }
            }
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.StationsNearMe != null)
                App.ViewModel.StationsNearMe.Clear();
        }
    }
}