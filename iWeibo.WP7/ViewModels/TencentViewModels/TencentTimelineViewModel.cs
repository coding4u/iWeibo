using iWeibo.WP7.Adapters;
using iWeibo.WP7.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;

namespace iWeibo.WP7.ViewModels.TencentViewModels
{
    public class TencentTimelineViewModel:ViewModel
    {
        #region Properties

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
                    this.HomeTimelineCommand.RaiseCanExecuteChanged();
                    this.MentionsTimelineCommand.RaiseCanExecuteChanged();
                    this.FavoritesTimelineCommand.RaiseCanExecuteChanged();
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
                selectedPivotIndex = value;
                HandlePivotSelectedIndexChange();
            }
        }

        private Status selectedStatus;

        public Status SelectedStatus
        {
            get
            {
                return selectedStatus;
            }
            set
            {
                selectedStatus = value;
                RaisePropertyChanged(() => this.SelectedStatus);
                HandleSelectedStatusChange();
            }
        }
        
        public ObservableCollection<Status> HomeTimeline { get; set; }
        public ObservableCollection<Status> MentionsTimeline { get; set; }
        public ObservableCollection<Status> FavoritesTimeline { get; set; }

        #endregion

        #region Fields

        private IsoStorage htStorage = new IsoStorage(Constants.TencentHomeTimeline);
        private IsoStorage mtStorage = new IsoStorage(Constants.TencentMentionsTimeline);
        private IsoStorage ftStorage = new IsoStorage(Constants.TencentFavoritesTimeline);


        private long ht_lastTimeStamp = 0;
        private long mt_lastTimeStamp = 0;
        private long ft_lastTimeStamp = 0;

        private int requestNumber=20;

        private StatusesService statusesService = new StatusesService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());

        #endregion

        #region DelegateCommands

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand<string> FavoritesTimelineCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand<object> PictureViewCommand { get; set; }
        

        //public DelegateCommand SelectionChangedCommand { get; set; }
        
        #endregion

        #region Constructor

        public TencentTimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.TencentTimelineView,UriKind.Relative))
        {
            this.HomeTimeline = new ObservableCollection<Status>();
            this.MentionsTimeline = new ObservableCollection<Status>();
            this.FavoritesTimeline = new ObservableCollection<Status>();


            this.PageLoadedCommand = new DelegateCommand(LoadDataFromCache);

            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);

            this.HomeTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                        GetHomeTimeline(1, ht_lastTimeStamp);
                    else
                        GetHomeTimeline();
                }, p => (!this.IsSyncing));

            this.MentionsTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                        GetMentionsTimeline(1, mt_lastTimeStamp);
                    else
                        GetMentionsTimeline();
                }, p => (!this.IsSyncing));

            this.FavoritesTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                        GetFavoritesTimeline(1, ft_lastTimeStamp);
                    else
                        GetFavoritesTimeline();
                }, p => (!this.IsSyncing));

            this.BackKeyPressCommand = new DelegateCommand(OnBackKeyPress,()=>true);

            //this.PictureViewCommand = new DelegateCommand<object>((o) =>
            //    {
            //        Debug.WriteLine(o.GetType().ToString());
            //    });

            //this.SelectionChangedCommand = new DelegateCommand(()=>
            //    {
            //        if (this.SelectedStatus != null)
            //        {
            //            var id = this.SelectedStatus.Id;
            //            new IsoStorage(Constants.TencentSelectedStatus).SaveData(this.SelectedStatus);
            //            this.NavigationService.Navigate(new Uri(Constants.TencentStatusDetail + "?id=" + id, UriKind.Relative));
            //        }
            //    }, () => true);
                
        }

        #endregion

        #region Methods

        private void LoadDataFromCache()
        {
            StatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                if (htStorage.TryLoadData<StatusCollection>(out collection))
                {
                    collection.ForEach(a => HomeTimeline.Add(a));
                    ht_lastTimeStamp = collection.LastTimeStamp;
                }
                else
                    GetHomeTimeline();
            }
            if (MentionsTimeline.Count <= 0)
            {
                if (mtStorage.TryLoadData<StatusCollection>(out collection))
                {
                    collection.ForEach(a => MentionsTimeline.Add(a));
                    mt_lastTimeStamp = collection.LastTimeStamp;
                }
            }
            if (FavoritesTimeline.Count <= 0)
            {
                if (ftStorage.TryLoadData<StatusCollection>(out collection))
                {
                    collection.ForEach(a => FavoritesTimeline.Add(a));
                    ft_lastTimeStamp = collection.LastTimeStamp;
                }
            }
        }

        private void Refresh()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    GetHomeTimeline();
                    break;
                case 1:
                    GetMentionsTimeline();
                    break;
                case 2:
                    GetFavoritesTimeline();
                    break;
            }
        }

        private void GetHomeTimeline(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    statusesService.HomeTimeline(
                        new ServiceArgument() { Reqnum = requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                        (callback) =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        if (pageFlag == 0)
                                        {
                                            HomeTimeline.Clear();
                                            //缓存
                                            htStorage.SaveData(callback.Data);
                                        }
                                        ht_lastTimeStamp = callback.Data.LastTimeStamp;
                                        callback.Data.ForEach(a => HomeTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ExceptionMsg);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        private void GetMentionsTimeline(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    statusesService.MentionsTimeline(
                        new ServiceArgument() { PageFlag = pageFlag, PageTime = pageTime, Reqnum = requestNumber },
                        (callback) =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        if (pageFlag == 0)
                                        {
                                            MentionsTimeline.Clear();
                                            mtStorage.SaveData(callback.Data);
                                        }
                                        mt_lastTimeStamp = callback.Data.LastTimeStamp;
                                        callback.Data.ForEach(a => MentionsTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ExceptionMsg);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        private void GetFavoritesTimeline(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;
            new Thread(() =>
                {
                    statusesService.FavoritesTimeline(
                        new ServiceArgument() { PageFlag = pageFlag, PageTime = pageTime, Reqnum = requestNumber },
                        (callback) =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        if (pageTime == 0)
                                        {
                                            FavoritesTimeline.Clear();
                                            ftStorage.SaveData(callback.Data);
                                        }
                                        ft_lastTimeStamp = callback.Data.LastTimeStamp;
                                        callback.Data.ForEach(a => FavoritesTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ExceptionMsg);
                                    }
                                    this.IsSyncing = false;
                                });
                        });
                }).Start();
        }

        private void HandlePivotSelectedIndexChange()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    if (HomeTimeline.Count <= 0)
                        GetHomeTimeline();
                    break;
                case 1:
                    if (MentionsTimeline.Count <= 0)
                        GetMentionsTimeline();
                    break;
                case 2:
                    if (FavoritesTimeline.Count <= 0)
                        GetFavoritesTimeline();
                    break;
            }
        }

        private void HandleSelectedStatusChange()
        {
            if (this.SelectedStatus != null)
            {
                var id = this.SelectedStatus.Id;
                new IsoStorage(Constants.TencentSelectedStatus).SaveData(this.SelectedStatus);
                this.NavigationService.Navigate(new Uri(Constants.TencentStatusDetailView + "?id=" + id, UriKind.Relative));
                
                this.SelectedStatus = null;
            }
        }

        private void OnBackKeyPress()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
