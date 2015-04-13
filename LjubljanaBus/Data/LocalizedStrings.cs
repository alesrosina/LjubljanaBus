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

namespace LjubljanaBus.Data
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
            //LjubljanaBus.Data.AppResource locsl = new AppResource();
            //locsl.
            //global::System.Globalization.CultureInfo i = new System.Globalization.CultureInfo()
        }

        
       

        private static LjubljanaBus.Data.AppResource localizedresources = new LjubljanaBus.Data.AppResource();

        public LjubljanaBus.Data.AppResource Localizedresources 
        { 
            get
            {
                return localizedresources; } }

    }
}
