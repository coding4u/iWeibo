using Coding4Fun.Toolkit.Controls;
using iWeibo.WP7.Adapters;
using Microsoft.Phone;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace iWeibo.WP7.ViewModels
{
    public class PictureViewViewModel:ViewModel
    {

        private bool isOpening;

        public bool IsOpening
        {
            get
            {
                return isOpening;
            }
            set
            {
                if (value != isOpening)
                {
                    isOpening = value;
                    RaisePropertyChanged(() => this.IsOpening);
                }
            }
        }

        private bool isDownloading;

        public bool IsDownloading
        {
            get
            {
                return isDownloading;
            }
            set
            {
                if (value != isDownloading)
                {
                    isDownloading = value;
                    RaisePropertyChanged(() => this.IsDownloading);
                }
            }
        }

        private int downloadProgress;

        public int DownloadProgress
        {
            get
            {
                return downloadProgress;
            }
            set
            {
                if (value != downloadProgress)
                {
                    downloadProgress = value;
                    RaisePropertyChanged(() => this.DownloadProgress);
                }
            }
        }

        private string downloadPercentage;

        public string DownloadPercentage
        {
            get
            {
                return downloadPercentage;
            }
            set
            {
                if (value != downloadPercentage)
                {
                    downloadPercentage = value;
                    RaisePropertyChanged(() => this.DownloadPercentage);
                }
            }
        }

        private WriteableBitmap picture;

        public WriteableBitmap Picture
        {
            get
            {
                return picture;
            }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged(() => this.Picture);
                }
            }
        }

        public string PictureUrl { get; set; }

        public bool IsSaved { get; set; }

        
        public DelegateCommand PageLoadedCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        private WebClient webClient;

        public PictureViewViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.PictureView, UriKind.Relative))
        {
            this.PageLoadedCommand=new DelegateCommand(DownloadPicture);
            this.SaveCommand = new DelegateCommand(SavePicture, () => !this.IsSaved);
        }

        private void DownloadPicture()
        {
            webClient = new WebClient();
            webClient.OpenReadCompleted += webClient_OpenReadCompleted;

            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            //webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            if (!string.IsNullOrEmpty(this.PictureUrl))
            {
                webClient.OpenReadAsync(new Uri(this.PictureUrl));
                //webClient.DownloadStringAsync(new Uri(this.PictureUrl));
                this.IsDownloading = true;
            }
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            this.IsDownloading = false;
            if (e.Cancelled || e.Error != null)
            {
                return;
            }
            byte[] imageBytes = new byte[e.Result.Length];
            e.Result.Read(imageBytes, 0, imageBytes.Length);
            e.Result.Seek(0, SeekOrigin.Begin);
            var imageSource = PictureDecoder.DecodeJpeg(e.Result);
            this.Picture = imageSource;
        }

        //private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    this.IsDownloading = false;

        //    if(e.Cancelled==true)
        //    {
        //        return ;
        //    }
        //    if (e.Error != null)
        //    {
        //        return;
        //    }

        //    Deployment.Current.Dispatcher.BeginInvoke(() =>
        //    {
        //        var str = e.Result;
        //        BitmapImage bmp = new BitmapImage();

        //        bmp.CreateOptions = BitmapCreateOptions.None;

        //        bmp.ImageOpened += bmp_ImageOpened;
        //        bmp.UriSource = new Uri(this.PictureUrl);

        //    });
        //}

        //private void bmp_ImageOpened(object sender, RoutedEventArgs e)
        //{
        //    this.Picture = new WriteableBitmap(sender as BitmapImage);
        //}

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.DownloadProgress = e.ProgressPercentage;
            this.DownloadPercentage = string.Format("{0}%   {1}/{2}KB", e.ProgressPercentage, e.BytesReceived/1024, e.TotalBytesToReceive/1024); 
        }

        private void SavePicture()
        {
            ToastPrompt toast = new ToastPrompt();
            MemoryStream fileStream = new MemoryStream();
            try
            {
                MediaLibrary library = new MediaLibrary();

                string lName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";

                // 将WriteableBitmap转换为JPEG流编码，并储存到独立存储里.
                Extensions.SaveJpeg(this.Picture, fileStream, this.Picture.PixelWidth, this.Picture.PixelHeight, 0, 100);
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Seek(0, SeekOrigin.Current);

                //把图片加在WP7 手机的媒体库.
                Picture pic = library.SavePicture(lName, fileStream);
                //fileStream.Close();

                this.IsSaved = true;
                this.SaveCommand.RaiseCanExecuteChanged();

                toast.Message = "保存成功...";
                toast.Show();
            }
            catch (Exception ex)
            {
                toast.Title = "保存失败";
                toast.Message = ex.Message;
                toast.Show();
            }
            finally
            {
                fileStream.Close();
            }
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
