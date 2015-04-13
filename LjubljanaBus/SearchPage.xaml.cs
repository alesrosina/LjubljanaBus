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
using LjubljanaBus.Helpers;

namespace LjubljanaBus
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //var coll = 
                listResults.Items.Clear();
                foreach (var item in App.ViewModel.Stations)
                {
                    if (RemoveSumniki(item.NameWithID.ToLower()).Contains(txtSearch.Text.ToLower()))
                    {
                        listResults.Items.Add(item);
                    }
                }

               //listResults.ItemsSource = App.ViewModel.Stations.Select(a => a.NameWithID.ToLower().Contains(txtSearch.Text.ToLower()));
               listResults.Focus();
               // Settings.FlurryLogEvent("Search", "Search string", txtSearch.Text);
               
            }
        }

        private string RemoveSumniki(string input)
        {
            return input.Replace("š", "s").Replace("č", "c").Replace("ž", "z");//.Replace("Š", "S").Replace("Č", "C").Replace("Ž", "Z");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearch.SelectAll();
            txtSearch.Focus();

            // FlurryWP7SDK.Api.LogPageView();
        }

        private void listResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                }
                //listBoxStations.SelectedItem = null;
            }
        }
    }
}