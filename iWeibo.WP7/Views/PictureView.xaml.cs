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

        bool isDragging;
        bool isPinching;
        Point ptPinchPositionStart;

        public PictureView()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as PictureViewViewModel;
            viewModel.PictureUrl = this.NavigationContext.QueryString.ContainsKey("PicUrl") ? this.NavigationContext.QueryString["PicUrl"]+@"/2000" : "";
        }


        void OnGestureListenerDragStarted(object sender, DragStartedGestureEventArgs args)
        {
            isDragging = args.OriginalSource == image;
        }

        void OnGestureListenerDragDelta(object sender, DragDeltaGestureEventArgs args)
        {
            if (isDragging)
            {
                translateTransform.X += args.HorizontalChange;
                translateTransform.Y += args.VerticalChange;
            }
        }

        void OnGestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs args)
        {
            if (isDragging)
            {
                TransferTransforms();
                isDragging = false;
            }
        }

        void OnGestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs args)
        {
            isPinching = args.OriginalSource == image;

            if (isPinching)
            {
                // Set transform centers
                Point ptPinchCenter = args.GetPosition(image);
                ptPinchCenter = previousTransform.Transform(ptPinchCenter);

                scaleTransform.CenterX = ptPinchCenter.X;
                scaleTransform.CenterY = ptPinchCenter.Y;

                ptPinchPositionStart = args.GetPosition(this);
            }
        }

        void OnGestureListenerPinchDelta(object sender, PinchGestureEventArgs args)
        {
            if (args.DistanceRatio > 2.0)
            {
                return;
            }
            if (isPinching)
            {
                // Set scaling
                scaleTransform.ScaleX = args.DistanceRatio;
                scaleTransform.ScaleY = args.DistanceRatio;

                // Set translation
                Point ptPinchPosition = args.GetPosition(this);
                translateTransform.X = ptPinchPosition.X - ptPinchPositionStart.X;
                translateTransform.Y = ptPinchPosition.Y - ptPinchPositionStart.Y;
            }
        }

        void OnGestureListenerPinchCompleted(object sender, PinchGestureEventArgs args)
        {
            if (isPinching)
            {
                TransferTransforms();
                isPinching = false;
            }
        }

        void TransferTransforms()
        {
            previousTransform.Matrix = Multiply(previousTransform.Matrix, currentTransform.Value);

            // Set current transforms to default values
            scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            scaleTransform.CenterX = scaleTransform.CenterY = 0;

            translateTransform.X = translateTransform.Y = 0;
        }

        Matrix Multiply(Matrix A, Matrix B)
        {
            return new Matrix(A.M11 * B.M11 + A.M12 * B.M21,
                              A.M11 * B.M12 + A.M12 * B.M22,
                              A.M21 * B.M11 + A.M22 * B.M21,
                              A.M21 * B.M12 + A.M22 * B.M22,
                              A.OffsetX * B.M11 + A.OffsetY * B.M21 + B.OffsetX,
                              A.OffsetX * B.M12 + A.OffsetY * B.M22 + B.OffsetY);
        }


    }
}