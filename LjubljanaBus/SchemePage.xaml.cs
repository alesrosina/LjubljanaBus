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
using Microsoft.Phone.Shell;
using LjubljanaBus.Data;

namespace LjubljanaBus
{
    public partial class SchemePage : PhoneApplicationPage
    {
        private PageOrientation prevOrient;

        public SchemePage()
        {
            InitializeComponent();

            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).Text = AppResource.mapPageZoomOut;
            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = AppResource.mapPageZoomIn;

        }

        private double initialScale;
        private double initialRatioX, initialRatioY;
        //private double 

        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            //initialAngle = transform.Rotation;
            initialScale = transform.ScaleX;
            initialRatioX = transform.TranslateX;
            initialRatioY = transform.TranslateY;
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            //transform.Rotation = initialAngle + e.TotalAngleDelta;
            //transform.CenterX = imgScheme.ActualWidth / 2;
            //transform.CenterY = imgScheme.ActualWidth / 2;
            transform.ScaleX = initialScale * e.DistanceRatio;
            transform.ScaleY = initialScale * e.DistanceRatio;

            transform.TranslateX = initialRatioX * e.DistanceRatio;
            transform.TranslateY = initialRatioY * e.DistanceRatio;
            //transform.TranslateY = imgScheme.ActualHeight * transform.ScaleY;

        }


        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
           
            transform.TranslateX += e.HorizontalChange;
            transform.TranslateY += e.VerticalChange;
            //imgScheme.UpdateLayout();
        }

        private void OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            
            if (transform.TranslateX > 0)
                transform.TranslateX = 0;

            if (transform.TranslateY > 0)
                transform.TranslateY = 0;

            //tole se mal nezanesljiv, ko je v landscape
            if (imgScheme.ActualWidth * transform.ScaleX > canvasImage.ActualWidth)
            {
                if (transform.TranslateX < (1 - imgScheme.ActualWidth) * transform.ScaleX + canvasImage.ActualWidth)
                    transform.TranslateX = (1 - imgScheme.ActualWidth) * transform.ScaleX + canvasImage.ActualWidth;
            }
            if (imgScheme.ActualHeight * transform.ScaleY > canvasImage.ActualHeight)
            {
                if (transform.TranslateY < (1 - imgScheme.ActualHeight) * transform.ScaleY + canvasImage.ActualHeight)
                    transform.TranslateY = (1 - imgScheme.ActualHeight) * transform.ScaleY + canvasImage.ActualHeight;
            }
        }

        private void OnDoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            transform.ScaleX = transform.ScaleX * 1.2;
            transform.ScaleY = transform.ScaleY * 1.2;

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            transform.ScaleX = transform.ScaleX * 0.7;
            transform.ScaleY = transform.ScaleY * 0.7;
            transform.TranslateX = -200;
            transform.TranslateY = -100;

            prevOrient = this.Orientation;
            // FlurryWP7SDK.Api.LogPageView();
        }

        private void OnPinchCompleted(object sender, PinchGestureEventArgs e)
        {

            if (transform.ScaleX * (imgScheme.ActualWidth * 2) < canvasImage.ActualWidth)
            {
                transform.ScaleX = canvasImage.ActualWidth / (imgScheme.ActualWidth * 2);
                transform.ScaleY = transform.ScaleX;
            }

            if (transform.ScaleY * (imgScheme.ActualHeight * 2) < canvasImage.ActualHeight)
            {
                transform.ScaleY = canvasImage.ActualHeight / (imgScheme.ActualHeight * 2);
                transform.ScaleX = transform.ScaleY;
            }
        }

        private void barZoomOut_Click(object sender, EventArgs e)
        {
            transform.ScaleX = transform.ScaleX * 0.8;
            transform.ScaleY = transform.ScaleY * 0.8;
        }

        private void barZoomIn_Click(object sender, EventArgs e)
        {
            transform.ScaleX = transform.ScaleX * 1.2;
            transform.ScaleY = transform.ScaleY * 1.2;
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            //todo rotation transition ne dela kul :)
            RotateTransitionMode mode;
            if (e.Orientation == PageOrientation.LandscapeLeft)
                mode = RotateTransitionMode.Out90Clockwise;
            else if (e.Orientation == PageOrientation.LandscapeRight)
                mode = RotateTransitionMode.In90Clockwise;
            else
                mode = RotateTransitionMode.In90Clockwise;

            TransitionElement transitionElement = new RotateTransition { Mode = mode };

            PhoneApplicationPage phoneApplicationPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;
            ITransition transition = transitionElement.GetTransition(phoneApplicationPage);
            transition.Completed += delegate
            {
                transition.Stop();
            };
            transition.Begin();
        }


    }
}