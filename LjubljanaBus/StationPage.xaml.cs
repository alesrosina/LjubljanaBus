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
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using LjubljanaBus.Helpers;
using Microsoft.Phone.Shell;
using LjubljanaBus.Data;

namespace LjubljanaBus
{
    public partial class StationPage : PhoneApplicationPage
    {
        public StationPage()
        {
            InitializeComponent();

            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).Text = AppResource.stationPageRefresh;
            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = AppResource.stationPageFav;
        }

        string id, name;
        string lat, lang;

        private Station _station;


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            FlurryWP7SDK.Api.LogPageView();

            id = NavigationContext.QueryString["id"];
            name = NavigationContext.QueryString["name"];
            bool urbana = (NavigationContext.QueryString["urbana"] == "true") ? true : false;
            //double latd, langd;
            //Double.TryParse(NavigationContext.QueryString["lat"], out latd);
            //Double.TryParse(NavigationContext.QueryString["lang"], out langd);

            _station = new Station()
            {
                ID = id,
                Name = name,
                Langitude = NavigationContext.QueryString["lang"],
                Latitude = NavigationContext.QueryString["lat"],
                HasUrbanomat = urbana
            };

            if (urbana)
                imgUrbana.Visibility = Visibility.Visible;
            else
                imgUrbana.Visibility = Visibility.Collapsed;

            PageTitle.Text = name;

            if (App.Favorites.GetFirst(id) != null)
            {
                var gumb = this.ApplicationBar.Buttons[1] as ApplicationBarIconButton;
                gumb.IconUri = new Uri("/icons/appbar-favs.png", UriKind.Relative);
                //gumb.IsEnabled = true;
            }

            if (ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("name=" + name)) != null)
            {
                var gumb = this.ApplicationBar.Buttons[2] as ApplicationBarIconButton;
                gumb.IsEnabled = false;
            }

            //GeoCoordinate loc = new GeoCoordinate(latd, langd);
            //stationPin.a = id;
            stationPinText.Text = _station.NameWithID;
            //mapItems.Items.Add(loc);
            stationPin.Location = _station.Location;
            //stationPin.a

            map1.SetView(_station.Location, 15.0);
            //map1.

            progressBar1.IsIndeterminate = true;
            //progressBar1.IsEnabled = true;

            //App.StationViewModel.StartDownloading(id);
            StartDownloading();


            Settings.FlurryLogEvent("Station info", "Station name", _station.NameWithID);

            
        }
        #region downloading stuff

        private void StartDownloading()
        {
            string postData = "__EVENTTARGET=&__EVENTARGUMENT=&tb_postaja=" + id + "&b_send=Prika%C5%BEi";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://wbus.talktrack.com/wap.aspx");

            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10";

            // Set the Method property to 'POST' to post data to the URI.
            request.Method = "POST";

            // start the asynchronous operation
            request.BeginGetRequestStream((ar) => { GetRequestStreamCallback(ar, postData); }, request);


        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            //Console.WriteLine("Please enter the input data to be posted:");
            //string postData = Console.ReadLine();

            // Convert the string into a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Write to the request stream.
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                // End the operation
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                ObservableCollection<LinesViewModel> list = ParseHtml(streamRead.ReadToEnd());
                //Console.WriteLine(responseString);
                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
                //allDone.Set();
                Deployment.Current.Dispatcher.BeginInvoke(() => { UpdateUI(list); });
            }
            catch (WebException ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { ShowMsgErrorBox(); });
            }

        }

        public void ShowMsgErrorBox()
        {
            MessageBox.Show(AppResource.msgNoInternetConn);
        }

        //public delegate UpdateUI;

        public void UpdateUI(ObservableCollection<LinesViewModel> list)
        {
            //textBlock1.Text = value;
            //progressBar1.IsEnabled = false;
            progressBar1.IsIndeterminate = false;
            listLines.ItemsSource = list;
        }



        public static ObservableCollection<LinesViewModel> ParseHtml(string html)
        {

            ObservableCollection<LinesViewModel> retValue = new ObservableCollection<LinesViewModel>();
            string newHtml = Regex.Replace(html, @"<[^>]*>", "");

            string[] lines = newHtml.Replace("\r", "").Replace("\n\n", "").Split('\n');

            int tmp = 0;
            string busid = "", ure = "", busime = "", busno = "";
            foreach (string item in lines)
            {
                
                string[] tmp1 = item.Split(new char[] { ':' }); //2
                if (tmp1.Length >= 2)
                {
                    string[] tmp2 = tmp1[0].Split(new char[] { ' ' }); //3

                    int.TryParse(tmp2[0], out tmp);

                    if (tmp2.Length >= 3 && tmp != 0)
                    {
                        if (tmp2[1].Length == 1)
                        {
                            busid = String.Format("{0}{1}", tmp2[0], tmp2[1]);
                            busime = tmp1[0].Replace(tmp2[0] + " " + tmp2[1] + " ", "");
                        }
                        else
                        {
                            busid = tmp2[0];
                            busime = tmp1[0].Replace(busid, "");
                        }

                        busno = tmp2[0];
                        ure = item.Replace(tmp1[0] + ": ", "");

                        string[] ureS = ure.Replace("n","").Split(',');
                        string ureSpan = AppResource.strDueIn + " ";
                        for(int i=0;i<ureS.Length ; i++)
                        {
                            string[] tmpHs = ureS[i].Trim().Split(':');
                            int hour, min;
                            int.TryParse(tmpHs[0],out hour);
                            int.TryParse(tmpHs[1], out min);

                            DateTime tmpDt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
                            TimeSpan ts = tmpDt.Subtract(DateTime.Now);
                            //ts.Minutes
                            if (ts.TotalMinutes < 0)
                            {
                                tmpDt = tmpDt.AddDays(1);
                                ts = tmpDt.Subtract(DateTime.Now);
                                ureSpan += AppResource.strTommorowAt + " " + ureS[i].Trim();
                            }
                            else
                            {
                                ureSpan += Math.Round(ts.TotalMinutes, 0).ToString() + " min"; // (" + ureS[i] + ")";
                            }
                            if (i < ureS.Length - 1)
                                ureSpan += ", ";
                        }

                        ///////////
                        //tukej setej stuff v objektke pa te fore
                        ///////////
                        //Console.WriteLine("{0} # {1} # {2}", busid, busime, ure);
                        retValue.Add(new LinesViewModel() { ID = busid.Trim(), Name = busime.Trim(), Times = ureSpan, IDNumberOnly = busno.Trim() });
                        //retValue += string.Format("{0} # {1} # {2}\n", busid, busime, ure);
                    }


                }



                busid = ""; ure = ""; busime = ""; tmp = 0;
            }
            return retValue;
        }

        #endregion



        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //textBlock1.Text = "";
            //App.StationViewModel.Lines.Clear();
            progressBar1.IsIndeterminate = true;
            StartDownloading();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            Uri uri = new Uri("/icons/appbar-favs.png", UriKind.Relative);
            var gumb = this.ApplicationBar.Buttons[1] as ApplicationBarIconButton;

            if (gumb.IconUri == uri)
            {
                App.Favorites.RemoveFirst(_station.ID);
                gumb.IconUri = new Uri("/icons/appbar.favs.addto.rest.png", UriKind.Relative);
            }
            else
            {
                App.Favorites.Add(_station);
                gumb.IconUri = uri;
            }

            
            
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {

            StandardTileData NewTileData = new StandardTileData
            {
                Title = _station.Name,
                BackgroundImage = new Uri("/Images/station.png", UriKind.Relative),
                Count = 0
            };

            string uri = String.Format("/StationPage.xaml?id={0}&lat={1}&lang={2}&name={3}&urbana={4}", _station.ID, _station.Latitude, _station.Langitude, _station.Name, (_station.HasUrbanomat == true) ? "true" : "false");
            ShellTile.Create(new Uri(uri, UriKind.Relative), NewTileData);
        }

    }
}