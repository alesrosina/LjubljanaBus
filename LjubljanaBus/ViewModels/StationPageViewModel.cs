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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LjubljanaBus
{
    public class StationPageViewModel : INotifyPropertyChanged
    {

        public StationPageViewModel()
        {
            //this.IsDataLoaded = false;
            this.Lines = new ObservableCollection<LinesViewModel>();
        }

        public ObservableCollection<LinesViewModel> Lines { get; private set; }

        private bool _isdataloading = false;

        public bool IsDataLoading
        {
            get { return _isdataloading; }
            private set
            {
                if (value != _isdataloading)
                {
                    _isdataloading = value;
                    NotifyPropertyChanged("IsDataLoading");
                }
            }
        }


        #region downloading stuff

        public void StartDownloading(string id)
        {
            this.IsDataLoading = true;
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
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            
            ParseHtml(streamRead.ReadToEnd());

            //Console.WriteLine(responseString);
            // Close the stream object
            streamResponse.Close();
            streamRead.Close();

            // Release the HttpWebResponse
            response.Close();
            //allDone.Set();

            this.IsDataLoading = false;
            //Deployment.Current.Dispatcher.BeginInvoke(() => { UpdateUI(responseString); });

        }

        public void ParseHtml(string html)
        {

            string retValue = "";
            string newHtml = Regex.Replace(html, @"<[^>]*>", "");

            string[] lines = newHtml.Replace("\r", "").Replace("\n\n", "").Split('\n');

            int tmp = 0;

            foreach (string item in lines)
            {
                string busid = "", ure = "", busime = "";
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

                        ure = item.Replace(tmp1[0] + " : ", "");

                        ///////////
                        //tukej setej stuff v objektke pa te fore
                        ///////////
                        //Console.WriteLine("{0} # {1} # {2}", busid, busime, ure);
                        this.Lines.Add(new LinesViewModel() { ID = busid, Name = busime, Times = ure });
                        //retValue += string.Format("{0} # {1} # {2}\n", busid, busime, ure);
                    }


                }



                busid = ""; ure = ""; busime = ""; tmp = 0;
            }
            //return retValue;
        }


        #endregion


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
