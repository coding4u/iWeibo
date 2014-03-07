using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using iWeibo.WP7.ViewModels;

namespace iWeibo.WP7.Views
{
    public partial class PictureView : PhoneApplicationPage
    {

        private const double doubleTapScale = 2;
        private bool isDouble = false;
        private Point point;
        private double initialScale = 1f;

        public PictureView()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as PictureViewViewModel;
            viewModel.PictureUrl = this.NavigationContext.QueryString.ContainsKey("PicUrl") ? this.NavigationContext.QueryString["PicUrl"]: "";
        }



        private void GestureListener_DoubleTap(object sender, GestureEventArgs e)
        {
            if (transform.ScaleX < doubleTapScale)
            {

                double s = doubleTapScale;
                rest_s_x.To = s;
                rest_s_y.To = s;
                rest_s.Begin();
                transform.ScaleY = s;
                transform.ScaleX = s;
                isDouble = true;
            }
            else
            {
                double s = 1f;
                rest_s_x.To = s;
                rest_s_y.To = s;
                rest_s.Begin();
                transform.ScaleY = s;
                transform.ScaleX = s;
                isDouble = false;
                reset();
            }

        }

        private void reset()
        {
            if (transform.ScaleX < 1)
            {
                rest_s_x.To = 1;
                rest_s_y.To = 1;
                rest_s.Begin();
                transform.ScaleY = transform.ScaleX = 1f;
            }
            else if (transform.ScaleX > 3)
            {
                rest_s_x.To = 3;
                rest_s_y.To = 3;
                rest_s.Begin();
                transform.ScaleY = transform.ScaleX = 3f;
            }
            double tsx = (transform.ScaleX - 1) * image.ActualWidth / 2;
            double tsy = (transform.ScaleY - 1) * image.ActualHeight / 2;
            rest_x_t.To = transform.TranslateX;
            double x = transform.TranslateX - tsx;
            if (x > 0)
            {
                rest_x_t.To = tsx;
            }
            double x2 = transform.TranslateX + tsx;
            if (x2 < 0)
            {
                rest_x_t.To = -tsx;
            }
            rest_y_t.To = transform.TranslateY;
            double y = transform.TranslateY - tsy;
            if (y > 0)
            {
                rest_y_t.To = tsy;
            }
            double y2 = transform.TranslateY + tsy;
            if (y2 < 0)
            {
                rest_y_t.To = -tsy;
            }
            if (x > 0 || x2 < 0 || y > 0 || y2 < 0)
            {
                rest_t.Begin();
                transform.TranslateX = tsx;
                transform.TranslateY = tsy;
            }
        }

        private void GestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
        {
            point = new Point(transform.TranslateX, transform.TranslateY);
        }

        private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            var p = e.GetPosition(sender as UIElement);
            point.X += e.HorizontalChange;
            point.Y += e.VerticalChange;
            animx.To = point.X;
            animy.To = point.Y;
            story.Begin();
        }

        private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            reset();
        }

        private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            rest_s.Stop();
            initialScale = transform.ScaleX;
        }

        private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
        {
            var s = initialScale * e.DistanceRatio;
            if (s > 4f)
            {
                s = 4f;
            }
            animx_s.To = s;
            animy_s.To = s;
            story_s.Begin();
        }

        private void GestureListener_PinchCompleted(object sender, PinchGestureEventArgs e)
        {
            if (transform.ScaleX <= 1)
            {
                isDouble = false;
            }
            else
            {
                isDouble = true;
            }
            reset();
        }

    }
}