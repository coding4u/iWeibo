using Coding4Fun.Toolkit.Controls;
using iWeibo.WP7.Adapters;
using iWeibo.WP7.Models.SinaModels;
using iWeibo.WP7.Models.TencentModels;
using iWeibo.WP7.Services;
using iWeibo.WP7.Util;
using Microsoft.Phone;
using Microsoft.Phone.Reactive;
using Microsoft.Practices.Prism.Commands;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;
using WeiboSdk;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP7.ViewModels
{
    public class PostNewViewModel:ViewModel,ITextBoxController
    {
        #region Fields

        private IPhotoChooserTask photoChoosertask;
        private IMessageBox messageBox;

        public event FocusEventHandler Focus;
        public event SelectEventHandler Select;

        #endregion

        #region Notification Properties

        private bool isSendTencent;

        public bool IsSendTencent
        {
            get
            {
                return isSendTencent;
            }
            set
            {
                if (value != isSendTencent)
                {
                    isSendTencent = value;
                    RaisePropertyChanged(() => this.IsSendTencent);
                }
            }
        }

        private bool isSendSina;

        public bool IsSendSina
        {
            get
            {
                return isSendSina;
            }
            set
            {
                if (value != isSendSina)
                {
                    isSendSina = value;
                    RaisePropertyChanged(() => this.IsSendSina);
                }
            }
        }

        private bool isSending;

        public bool IsSending
        {
            get
            {
                return isSending;
            }
            set
            {
                if (value != isSending)
                {
                    isSending = value;
                    RaisePropertyChanged(() => this.IsSending);
                }
            }
        }

        private string statusText;

        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                if (value != statusText)
                {
                    statusText = value;
                    RaisePropertyChanged(() => this.StatusText);
                    HandelTextChange();
                }
            }
        }

        private int wordsCount=140;

        public int WordsCount
        {
            get
            {
                return wordsCount;
            }
            set
            {
                if (value != wordsCount)
                {
                    wordsCount = value;
                    RaisePropertyChanged(() => this.WordsCount);
                }
            }
        }

        private string wordsCountColor;

        public string WordsCountColor
        {
            get
            {
                return wordsCountColor;
            }
            set
            {
                if (value != wordsCountColor)
                {
                    wordsCountColor = value;
                    RaisePropertyChanged(() => this.WordsCountColor);
                }
            }
        }

        private bool isTencentAuthorized;

        public bool IsTencentAuthorized
        {
            get { return isTencentAuthorized; }
            set
            {
                if (value != isTencentAuthorized)
                {
                    isTencentAuthorized = value;
                    RaisePropertyChanged(() => this.IsTencentAuthorized);
                }
            }
        }

        private bool isSinaAuthorized;

        public bool IsSinaAuthorized
        {
            get { return isSinaAuthorized; }
            set
            {
                if (value != isSinaAuthorized)
                {
                    isSinaAuthorized = value;
                    RaisePropertyChanged(() => this.IsSinaAuthorized);
                }
            }
        }

        private bool hasPic=false;

        public bool HasPic
        {
            get
            {
                return hasPic;
            }
            set
            {
                if (value != hasPic)
                {
                    hasPic = value;
                    RaisePropertyChanged(() => this.HasPic);
                }
            }
        }

        private bool choosingPhoto;

        public bool ChoosingPhoto
        {
            get
            {
                return choosingPhoto;
            }
            set
            {
                if (value != choosingPhoto)
                {
                    choosingPhoto = value;
                    RaisePropertyChanged(() => this.ChoosingPhoto);
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

        private BitmapImage bmp;

        public BitmapImage BMP
        {
            get
            {
                return bmp;
            }
            set
            {
                if (value != bmp)
                {
                    bmp = value;
                    RaisePropertyChanged(() => this.BMP);
                }
            }
        }

        #endregion

        #region DelegateCommands
        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand ClearTextCommand { get; set; }
        public DelegateCommand ClearPicCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand ChoosePhotoCommand { get; set; }
        public DelegateCommand AddTopicCommand { get; set; }
        #endregion

        #region Constructor

        public PostNewViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IPhotoChooserTask photoChooserTask,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.PostNewView,UriKind.Relative))
        {
            this.photoChoosertask = photoChooserTask;
            this.messageBox = messageBox;

            // Subscribe to handle new photo stream
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto != null)
                .Subscribe(result =>
                {
                    this.ChoosingPhoto = false;
                    SetImageSource(result.EventArgs.ChosenPhoto);
                    HasPic = true;
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            // Subscribe to user cancelling photo capture to re-enable Capture command
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto == null && e.EventArgs.Error == null)
                .Subscribe(p =>
                {
                    this.ChoosingPhoto = false;
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            // Subscribe to Error condition
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.Error != null && !string.IsNullOrEmpty(e.EventArgs.Error.Message))
                .Subscribe(p =>
                {
                    this.ChoosingPhoto = false;
                    MessageBox.Show(p.EventArgs.Error.Message);
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            this.PageLoadedCommand = new DelegateCommand(() =>
                {
                    if (Focus != null)
                    {
                        Focus(this);
                    }
                    this.IsTencentAuthorized = TencentConfig.Validate();
                    this.IsSinaAuthorized = SinaConfig.Validate();
                    this.IsSendSina = this.IsSinaAuthorized;
                    this.IsSendTencent = this.IsTencentAuthorized;

                });
            this.ClearTextCommand = new DelegateCommand(() => StatusText = "");
            this.ClearPicCommand = new DelegateCommand(() =>
                {
                    HasPic = false;
                    Picture = null;
                    BMP = null;
                });
            this.SendCommand = new DelegateCommand(() =>
                {
                    if (IsSendSina)
                        SendSina();
                    if (IsSendTencent)
                        SendTencent();
                }, () => !this.IsSending && !string.IsNullOrEmpty(StatusText) && !(StatusText.Length > 140));

            this.ChoosePhotoCommand = new DelegateCommand(this.ChoosePhoto, () => !this.ChoosingPhoto);
            this.AddTopicCommand = new DelegateCommand(() =>
                {
                    if (this.Select != null)
                    {
                        int start = string.IsNullOrEmpty(StatusText)?1:StatusText.Length + 1;
                        int length = 7;
                        StatusText += "#在此处输入话题#";
                        Select(this, start, length);
                    }
                });
        }

        #endregion

        #region Methods
        private void SendTencent()
        {
            IsSending = true;
            UploadPic pic = (null != BMP) ? new UploadPic(BMP) : null;
            ToastPrompt toast = new ToastPrompt();

            new Thread(() =>
                {
                    TService tService=new TService();

                    if (null != pic)
                    {
                        tService.Add(
                            new ServiceArgument() { Content = StatusText, Pic = pic }, (callback) =>
                                {
                                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                                            {
                                                if (callback.Succeed)
                                                {
                                                    toast.Title = "发送成功...";
                                                }
                                                else
                                                {
                                                    toast.Title = "发送失败...";
                                                    toast.Message = callback.ExceptionMsg;
                                                }
                                                toast.Show();
                                            });
                                });
                    }
                    else
                    {
                        tService.AddPic(new ServiceArgument() { Content = StatusText }, (callback) =>
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        if (callback.Succeed)
                                        {
                                            toast.Title = "发送成功...";
                                        }
                                        else
                                        {
                                            toast.Title = "发送失败...";
                                            toast.Message = callback.ExceptionMsg;
                                        }
                                        toast.Show();

                                    });
                            });
                    }
                }).Start();
        }

        private void SendSina()
        {
            IsSending = true;
            UploadPicture pic = (null != BMP) ? new WeiboSdk.UploadPicture(BMP) : null;
            ToastPrompt toast = new ToastPrompt(); 

            new Thread(() =>
                {
                    string token = TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>().Token;
                    WStatusService wService = new WStatusService(token);
                    if (pic != null)
                    {
                        wService.AddPic(StatusText, pic, callback =>
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        toast.Title = "发送成功...";
                                    }
                                    else
                                    {
                                        toast.Title = "发送失败...";
                                        toast.Message = callback.ErrorMsg;
                                    }
                                    toast.Show();
                                    this.IsSending = false;
                                });
                            });
                    }
                    else
                    {
                        wService.Add(StatusText, callback =>
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        toast.Title = "发送成功...";
                                    }
                                    else
                                    {
                                        toast.Title = "发送失败...";
                                        toast.Message = callback.ErrorMsg;
                                    }
                                    toast.Show();
                                    this.IsSending = false;
                                });
                            });
                    }

                }).Start();
        }

        private void HandelTextChange()
        {           
            WordsCount = 140 - StatusText.Length;
            if (WordsCount < 0)
            {
                WordsCountColor = "Red";
            }
            else
            {
                WordsCountColor = "White";
            }

            SendCommand.RaiseCanExecuteChanged();
        }

        private void ChoosePhoto()
        {
            if (!this.ChoosingPhoto)
            {
                this.photoChoosertask.Show();
                this.ChoosingPhoto = true;
                this.ChoosePhotoCommand.RaiseCanExecuteChanged();
            }
        }

        private void SetImageSource(Stream chosenPhoto)
        {
            //SavingPicFile = true;
            BitmapImage tmpImg = new BitmapImage();
            tmpImg.SetSource(chosenPhoto);
            BMP = tmpImg;

            // Store the image bytes.
            byte[] imageBytes = new byte[chosenPhoto.Length];
            chosenPhoto.Read(imageBytes, 0, imageBytes.Length);

            // Seek back so we can create an image.
            chosenPhoto.Seek(0, SeekOrigin.Begin);

            // Create an image from the stream.
            var imageSource = PictureDecoder.DecodeJpeg(chosenPhoto);
            this.Picture = imageSource;

            // //Save the stream

            //var isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            //using (var stream = isoFile.CreateFile(fileName))
            //{
            //    stream.Write(imageBytes, 0, imageBytes.Length);
            //}

            //SavingPicFile = false;
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        #endregion

    }
}
