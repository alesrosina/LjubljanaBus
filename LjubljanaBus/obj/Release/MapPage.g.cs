﻿#pragma checksum "C:\Users\dev\Documents\Visual Studio 2010\Projects\_my staff\LjubljanaBus\LjubljanaBus\MapPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0432ED16B7ACB4D67457F8E3BFF1EA88"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace LjubljanaBus {
    
    
    public partial class MapPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Maps.Map map1;
        
        internal Microsoft.Phone.Controls.Maps.MapItemsControl mapControl;
        
        internal Microsoft.Phone.Controls.PerformanceProgressBar progressBar1;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton barZoomOut;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton barZoomIn;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton barMyLocation;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem barViewSwitch;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/LjubljanaBus;component/MapPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.map1 = ((Microsoft.Phone.Controls.Maps.Map)(this.FindName("map1")));
            this.mapControl = ((Microsoft.Phone.Controls.Maps.MapItemsControl)(this.FindName("mapControl")));
            this.progressBar1 = ((Microsoft.Phone.Controls.PerformanceProgressBar)(this.FindName("progressBar1")));
            this.barZoomOut = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("barZoomOut")));
            this.barZoomIn = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("barZoomIn")));
            this.barMyLocation = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("barMyLocation")));
            this.barViewSwitch = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("barViewSwitch")));
        }
    }
}
