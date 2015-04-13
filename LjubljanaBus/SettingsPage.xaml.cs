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
using System.IO.IsolatedStorage;
using LjubljanaBus.Helpers;
using System.IO;
using Microsoft.Phone.Tasks;
using LjubljanaBus.Data;

namespace LjubljanaBus
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            btnLocation.IsChecked = Settings.LocationServices;
            if (btnLocation.IsChecked.Value)
                btnLocation.Content = AppResource.onLabel;
            else
                btnLocation.Content = AppResource.offLabel;

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (!file.FileExists("error_log"))
                    btnSendError.Visibility = System.Windows.Visibility.Collapsed;
                else
                    btnSendError.Visibility = System.Windows.Visibility.Visible;
            }

            // FlurryWP7SDK.Api.LogPageView();
        }

        private void btnLocation_Checked(object sender, RoutedEventArgs e)
        {
            Settings.LocationServices = btnLocation.IsChecked.Value;
            if (btnLocation.IsChecked.Value)
                btnLocation.Content = AppResource.onLabel;
            else
                btnLocation.Content = AppResource.offLabel;
        }

        private void btnSendError_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (!file.FileExists("error_log"))
                {
                    MessageBox.Show(AppResource.msgNoErrorToSend);
                    return;
                }


                try
                {

                    string errorLog = "";
                    using (StreamReader reader = new StreamReader(file.OpenFile("error_log", FileMode.Open, FileAccess.Read)))
                    {
                        errorLog = reader.ReadToEnd();

                        EmailComposeTask em = new EmailComposeTask();
                        em.To = "ales.rosina@shelastyle.net";
                        em.Subject = "[WP7 Ljubljana bus] error report";
                        em.Body = errorLog;
                        em.Show();


                    }

                    file.DeleteFile("error_log");

                }
                catch (IsolatedStorageException ex) { }
            }
        }
    }
}