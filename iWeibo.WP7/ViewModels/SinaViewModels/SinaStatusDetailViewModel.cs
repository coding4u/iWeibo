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
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP7.ViewModels.SinaViewModels
{
    public class SinaStatusDetailViewModel:ViewModel
    {
        private IMessageBox messageBox;
        private IsoStorage storage = new IsoStorage(Constants.SinaSelectedStatus);
        private WStatusService statusService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());

        public string StatusId { get; set; }

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
                    this.RefreshCommand.RaiseCanExecuteChanged();
                    this.CommentCommand.RaiseCanExecuteChanged();
                    this.RepostCommand.RaiseCanExecuteChanged();
                    this.FavoriteCommand.RaiseCanExecuteChanged();
                    this.CommentsTimelineCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private WStatus status;

        public WStatus Status
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

        public bool CanDelete { get; set; }


        public ObservableCollection<WStatus> CommentsTimeline { get; set; }


        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand<string> CommentsTimelineCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand CommentCommand { get; set; }
        public DelegateCommand RepostCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand CopyCommand { get; set; }
        //public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand ViewPictureCommand { get; set; }
                        

        public SinaStatusDetailViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.SinaStatusDetailView,UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.CommentsTimeline = new ObservableCollection<WStatus>();

            this.PageLoadedCommand = new DelegateCommand(() =>
                {
                    WStatus s;
                    if (storage.TryLoadData<WStatus>(out s))
                    {
                        this.Status = s;
                        FavoriteText=s.Favorited?"取消":"添加";
                        FavoriteIconUri=s.Favorited?"unfavor":"favor";
                    }
                    else
                        GetStatus();
                });

            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);

            this.CommentsTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                        GetCommentsTimeliine(this.nextCursor);
                    else
                        GetCommentsTimeliine();
                }, p => !this.IsSyncing);

            this.CommentCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.Navigate(new Uri(Constants.SinaRepostPageView + "?id=" + this.Status.Id + "&type=comment", UriKind.Relative));
                }, () => !this.IsSyncing);

            this.RepostCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.Navigate(new Uri(Constants.SinaRepostPageView + "?id=" + this.Status.Id + "&type=repost", UriKind.Relative));
                }, () => !this.IsSyncing);

            this.FavoriteCommand = new DelegateCommand(() =>
                {
                    if (this.FavoriteText.Contains("取消"))
                    {
                        RemoveFavorite();
                    }
                    else
                    {
                        AddFavorite();
                    }
                }, () => !this.IsSyncing);

            this.CopyCommand = new DelegateCommand(CopyStatus);

            this.ViewPictureCommand = new DelegateCommand(() =>
            {
                if (!this.Status.IsRetweetedStatus)
                    this.NavigationService.Navigate(new Uri(Constants.PictureView + "?PicUrl=" + this.Status.OriginalPic, UriKind.Relative));
                else
                    this.NavigationService.Navigate(new Uri(Constants.PictureView + "?PicUrl=" + this.Status.RetweetedStatus.OriginalPic, UriKind.Relative));
            });
        }


        private void GetStatus()
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    statusService.GetStatus(this.StatusId, callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        this.Status = callback.Data;
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ErrorMsg);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        //private long previousCursor;
        private long nextCursor;
        private int requestCount = 20;

        private void GetCommentsTimeliine(long maxId=0)
        {
            this.IsSyncing = true;
            var timelineService = new TimelineService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());
            new Thread(() =>
                {
                    timelineService.GetCommentsTimleine(this.StatusId, requestCount, maxId, 0,
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        if (maxId == 0)
                                        {
                                            this.CommentsTimeline.Clear();
                                        }
                                        this.nextCursor = callback.Data.NextCursor;
                                        callback.Data.Comments.ForEach(a => this.CommentsTimeline.Add(a));

                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ErrorMsg);
                                    }

                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        private void Refresh()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                        GetStatus();
                    break;
                case 1:
                        GetCommentsTimeliine();
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
                        GetCommentsTimeliine();
                    break;
            }
        }

        private void AddFavorite()
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    statusService.AddFavorite(this.StatusId, callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        this.FavoriteIconUri = "unfavor";
                                        this.FavoriteText = "取消";
                                        var toast = new ToastPrompt();
                                        toast.Message = "已添加至收藏...";
                                        toast.Show();
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ErrorMsg, "添加收藏失败", MessageBoxButton.OK);
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
                statusService.DelFavorite(this.StatusId, callback =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (callback.Succeed)
                        {
                            this.FavoriteIconUri = "favor";
                            this.FavoriteText = "添加";
                            var toast = new ToastPrompt();
                            toast.Message = "已取消收藏...";
                            toast.Show();
                        }
                        else
                        {
                            this.messageBox.Show(callback.ErrorMsg, "取消收藏失败", MessageBoxButton.OK);
                        }

                        this.IsSyncing = false;
                    });
                });
            }).Start();

        }

        private void CopyStatus()
        {
            Clipboard.SetText(this.Status.Text);
            var toast = new ToastPrompt();
            toast.Message = "已将文本复制到剪贴板";
            toast.Show();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
