using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace LjubljanaBus
{
    class WebPostRequest
    {
        HttpWebRequest theRequest;
        HttpWebResponse theResponse;
        string _data;

        public WebPostRequest(string url, string data)
        {
            theRequest = (HttpWebRequest)WebRequest.Create(url);
            theRequest.Method = "POST";
            _data = data;
        }

        public string GetResponse()
        {
            // Set the encoding type

            //theRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10");
            
            theRequest.ContentType = "application/x-www-form-urlencoded";
            theRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10";
            // Build a string containing all the parameters
            //string Parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));

            // We write the parameters into the request
            theRequest.BeginGetRequestStream(new AsyncCallback(bla), theRequest);
            StreamWriter sw = new StreamWriter(theRequest.BeginGetRequestStream());
            sw.Write(_data);
            sw.Close();

            // Execute the query
            theResponse = (HttpWebResponse)theRequest.GetResponse();
            StreamReader sr = new StreamReader(theResponse.GetResponseStream());
            return sr.ReadToEnd();
        }

    }
}
