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
using System.IO.IsolatedStorage;
using System.Linq;
using System.Collections.Generic;


namespace LjubljanaBus.Helpers
{
    public static class Settings
    {
        public static bool LocationServices
        {
            get
            {
                bool loc = false;
                object locSetting = IsolatedStorageSettings.ApplicationSettings.FirstOrDefault(x => x.Key == "location").Value;
                if (locSetting != null)
                    bool.TryParse(IsolatedStorageSettings.ApplicationSettings["location"].ToString(), out loc);
                return loc;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["location"] = value;
            }
        }

        public static bool IsSettingsEmpty
        {
            get
            {
                //string locSetting = "";
                //IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("location", out locSetting);
                //if (!string.IsNullOrWhiteSpace(locSetting))
                if(IsolatedStorageSettings.ApplicationSettings.FirstOrDefault(x=>x.Key=="location").Value != null)
                    return false;
                else
                    return true;
            }
        }


        // public static void FlurryLogEvent(string eventName, string paramName, string paramValue)
        // {
        //     var flurryParams = new List<FlurryWP7SDK.Models.Parameter>();
        //     flurryParams.Add(new FlurryWP7SDK.Models.Parameter(paramName, paramValue));

        //     FlurryWP7SDK.Api.LogEvent(eventName, flurryParams);
        // }

        //public static bool IsPropertyExistent(string propertyName)
        //{
        //    if (IsolatedStorageSettings.ApplicationSettings[propertyName] != null)
        //        return true;
        //    else
        //        return false;

        //}
    }
}
