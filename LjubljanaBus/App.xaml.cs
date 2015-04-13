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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.IO;

namespace LjubljanaBus
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;
        //private static StationPageViewModel stationViewModel = null;
        private static LjubljanaBus.Helpers.Favorites favorites = null;
        //private static GeoCoordinateWatcher geowatcher = null;
        public const string flurryApiKey = "WT9F2NPCV87VL68SMUXE";

        //public static Geo

        public static LjubljanaBus.Helpers.Favorites Favorites
        {
            get {
                if (favorites == null)
                    favorites = new Helpers.Favorites();
                return App.favorites; }
            //set { App.favorites = value; }
        }

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        //public static StationPageViewModel StationViewModel
        //{
        //    get
        //    {
        //        // Delay creation of the view model until necessary
        //        if (stationViewModel == null)
        //            stationViewModel = new StationPageViewModel();

        //        return stationViewModel;
        //    }
        //}

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
                KBB.Mobile.Controls.MemoryMonitor monitor = new KBB.Mobile.Controls.MemoryMonitor(true, true);
            }

            // Standard Silverlight initialization
            InitializeComponent();



            LjubljanaBus.Data.AppResource.Culture = System.Globalization.CultureInfo.CurrentCulture; //new System.Globalization.CultureInfo("sl-SI");


            // Phone-specific initialization
            InitializePhoneApplication();



        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            FlurryWP7SDK.Api.StartSession(flurryApiKey);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            FlurryWP7SDK.Api.StartSession(flurryApiKey);
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            FlurryWP7SDK.Api.EndSession();
            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            FlurryWP7SDK.Api.EndSession();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            else
                FlurryWP7SDK.Api.LogError("NvigationFailed", e.Exception);
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //if (!file.FileExists(_filename))
                    //    file.CreateFile(_filename);
                    FlurryWP7SDK.Api.LogError("NvigationFailed", e.ExceptionObject);

                    try
                    {
                        var currentPage = ((App)Application.Current).RootFrame.Content as PhoneApplicationPage;
                        
                        using (StreamWriter sw = new StreamWriter(file.OpenFile("error_log", FileMode.Append, FileAccess.Write)))
                        {
                            sw.WriteLine("{0}\n{1}\n{2}\n{3}\n------------------\n", DateTime.Now.ToString(), currentPage.NavigationService.CurrentSource, e.ExceptionObject.Message, e.ExceptionObject.StackTrace);
                        }
                    }
                    catch (IsolatedStorageException ex) { }
                }
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame(); //PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}