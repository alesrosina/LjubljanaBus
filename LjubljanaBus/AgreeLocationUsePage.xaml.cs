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
    public partial class AgreeLocationUsePage : PhoneApplicationPage
    {
        public AgreeLocationUsePage()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            base.OnNavigatedTo(e);

            if (!Settings.IsSettingsEmpty)
                NavigationService.GoBack();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // FlurryWP7SDK.Api.LogPageView();
        }

        private void btnPrivacy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Settings.LocationServices = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));


            // Settings.FlurryLogEvent("FirstRun", "AgreeToLocServices", "true");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.LocationServices = false;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            // Settings.FlurryLogEvent("FirstRun", "AgreeToLocServices", "false");
        }
    }
}