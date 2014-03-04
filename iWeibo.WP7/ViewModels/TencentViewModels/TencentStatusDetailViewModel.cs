using Coding4Fun.Toolkit.Controls;
using iWeibo.WP7.Adapters;
using iWeibo.WP7.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;

namespace iWeibo.WP7.ViewModels.TencentViewModels
{
    public class TencentStatusDetailViewModel:ViewModel
    {
        private IMessageBox messageBox;
        private IsoStorage storage = new IsoStorage(Constants.TencentSelectedStatus);

        private bool isSyncing;

        public bool IsSyncing
        {
            get
            {
                return isSyncing;
            }
            set
            {
                if (value != isSyncing)
                {
                    isSyncing = value;
                    RaisePropertyChanged(() => this.IsSyncing);
                }
            }
        }

        private int selectedPivotIndex;

        public int SelectedPivotIndex
        {
            get
            {
                return selectedPivotIndex;
            }
            set
            {
                if (value != selectedPivotIndex)
                {
                    selectedPivotIndex = value;
                    RaisePropertyChanged(() => this.SelectedPivotIndex);
                    HandleSelectedPivotIndexChange();
                }
            }
        }       

        private Status status;

        public Status Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    status = value;
                    RaisePropertyChanged(() => this.Status);
                }
            }
        }

        private string favoriteText="添加";

        public string FavoriteText
        {
            get
            {
                return favoriteText;
            }
            set
            {
                if (value != favoriteText)
                {
                    favoriteText = value;
                    RaisePropertyChanged(() => this.FavoriteText);
                }
            }
        }

        private string favoriteIconUri="favor";

        public string FavoriteIconUri
        {
            get
            {
                return favoriteIconUri;
            }
            set
            {
                if (value != favoriteIconUri)
                {
                    favoriteIconUri = value;
                    RaisePropertyChanged(() => this.FavoriteIconUri);
                }
            }
        }

        private bool canDelete;

        public bool CanDelete
        {
            get
            {
                return canDelete;
            }
            set
            {
                if (value != canDelete)
                {
                    canDelete = value;
                    RaisePropertyChanged(() => this.CanDelete);
                }
            }
        }

        public ObservableCollection<Status> CommentsTimeline { get; set; }

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<string> CommentsTimelineCommand { get; set; }
        public DelegateCommand CommentCommand { get; set; }
        public DelegateCommand RepostCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand CopyCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand ViewPictureCommand { get; set; }


        public string StatusId { get; set; }
        

        public TencentStatusDetailViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.TencentStatusDetail,UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.CommentsTimeline = new ObservableCollection<Status>();
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);
            this.PageLoadedCommand = new DelegateCommand(() =>
                {
                    Status s;
                    if (storage.TryLoadData<Status>(out s))
                    {
                        this.Status = s;
                        this.CanDelete = s.IsSelf;
                    }
                    else
                        GetStatus();
                });

            this.CommentsTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                        GetCommentsTimeline(1, lastTimeStamp);
                    else
                        GetCommentsTimeline();
                }, p => !this.IsSyncing);
            this.CommentCommand = new DelegateCommand(() => { });
            this.RepostCommand = new DelegateCommand(() => { });
            this.CopyCommand = new DelegateCommand(CopyStatus);
            this.DeleteCommand = new DelegateCommand(DeleteStatus, () => this.CanDelete && !this.IsSyncing);
            this.FavoriteCommand = new DelegateCommand(() =>
                {
                    if (this.FavoriteText.Contains("取消"))
                        RemoveFavorite();
                    else
                        AddFavorite();
                }, () => !this.IsSyncing);

            this.ViewPictureCommand = new DelegateCommand(() =>
                {
                    if (this.Status.HasPic)
                        this.NavigationService.Navigate(new Uri(Constants.PictureView + "?PicUrl=" + this.Status.ImageUrl, UriKind.Relative));
                    else
                        this.NavigationService.Navigate(new Uri(Constants.PictureView + "?PicUrl=" + this.Status.Source.ImageUrl, UriKind.Relative));
                });
        }

        private void Refresh()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    GetStatus();
                    break;
                case 1:
                    GetCommentsTimeline();
                    break;
            }
        }

        private void HandleSelectedPivotIndexChange()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    if (this.Status == null)
                        GetStatus();
                    break;
                case 1:
                    if (this.CommentsTimeline.Count <= 0)
                        GetCommentsTimeline();
                    break;
            }
        }

        private TService tService = new TService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());

        private void GetStatus()
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    tService.Show(new ServiceArgument() { Id = this.StatusId }, callback =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                if (callback.Succeed)
                                {
                                    this.Status = callback.Data;
                                    this.CanDelete = callback.Data.IsSelf;
                                }
                                else
                                {
                                    this.messageBox.Show(callback.ExceptionMsg);
                                }
                                this.IsSyncing = false;
                            });
                    });
                }).Start();
        }

        private long lastTimeStamp = 0;
        private int requestNumber = 20;
        

        private void GetCommentsTimeline(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;
            var statusService = new StatusesService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
            new Thread(() =>
                {
                    statusService.SingleCommentsTimeline(
                        new ServiceArgument() { Rootid = this.StatusId, Flag = 2, PageFlag = pageFlag, PageTime = pageTime, Reqnum = requestNumber },
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        if (pageTime == 0)
                                            CommentsTimeline.Clear();
                                        if (callback.Data.Count > 0)
                                            lastTimeStamp = callback.Data.LastTimeStamp;
                                        callback.Data.ForEach(a => CommentsTimeline.Add(a));
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }


        private void AddFavorite()
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    tService.AddFavorite(
                        new ServiceArgument() { Id = StatusId },
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        this.FavoriteText = "取消";
                                        this.FavoriteIconUri = "unfavor";
                                        var toast = new ToastPrompt();
                                        toast.Message = "已添加到收藏...";
                                        toast.Show();
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg, "未能添加收藏", MessageBoxButton.OK);
                                    }
                                    
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        private void RemoveFavorite()
        {
            this.IsSyncing = true;
            new Thread(() =>
            {
                tService.DelFavorite(
                    new ServiceArgument() { Id = StatusId },
                    callback =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            if (callback.Succeed)
                            {
                                this.FavoriteText = "添加";
                                this.FavoriteIconUri = "favor";
                                var toast = new ToastPrompt();
                                toast.Message = "已取消收藏...";
                                toast.Show();
                            }
                            else
                            {
                                this.messageBox.Show(callback.ExceptionMsg, "未能取消收藏", MessageBoxButton.OK);
                            }
                            this.IsSyncing = false;
                        });
                    });
            }).Start();

        }

        private void CopyStatus()
        {
            Clipboard.SetText(this.Status.OrigText);
            var toast = new ToastPrompt();
            toast.Message = "已将文本复制到剪贴板";
            toast.Show();
        }

        private void DeleteStatus()
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    tService.Delete(
                        new ServiceArgument() { Id = StatusId },
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        var toast = new ToastPrompt();
                                        toast.Message = "删除成功...";                                        
                                        toast.Show();
                                        toast.Completed += toast_Completed;
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg, "删除失败", MessageBoxButton.OK);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (this.Status == null)
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
        }



        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
