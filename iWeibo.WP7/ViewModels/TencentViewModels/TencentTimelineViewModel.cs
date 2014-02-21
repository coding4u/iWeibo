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
    public class TencentTimelineViewModel:ViewModel
    {
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
                selectedPivotIndex = value;
                HandlePivotSelectedIndexChange();
            }
        }

        public ObservableCollection<Status> HomeTimeline { get; set; }
        public ObservableCollection<Status> MentionsTimeline { get; set; }
        public ObservableCollection<Status> FavoritesTimeline { get; set; }

        private IsoStorage htStorage = new IsoStorage(Constants.TencentHomeTimeline);
        private IsoStorage mtStorage = new IsoStorage(Constants.TencentMentionsTimeline);
        private IsoStorage ftStorage = new IsoStorage(Constants.TencentFavoritesTimeline);


        private long ht_lastTimeStamp = 0;
        private long mt_lastTimeStamp = 0;
        private long ft_lastTimeStamp = 0;

        private int requestNumber=20;

        private StatusesService statusesService = new StatusesService();

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand<string> FavoritesTimelineCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }



        public TencentTimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.TencentTimelineView,UriKind.Relative))
        {
            this.PageLoadedCommand = new DelegateCommand(LoadDataFromCache, () => !this.IsSyncing);

            this.HomeTimeline = new ObservableCollection<Status>();
            this.HomeTimelineCommand = new DelegateCommand<string>(parameter =>
                {
                    if (parameter == "Refresh")
                        GetHomeTimeline();
                    else
                        GetHomeTimeline(1, ht_lastTimeStamp);
                }, p => (!this.IsSyncing));

            this.MentionsTimeline = new ObservableCollection<Status>();
            this.MentionsTimelineCommand = new DelegateCommand<string>(parameter =>
                {
                    if (parameter == "Refresh")
                        GetMentionsTimeline();
                    else
                        GetMentionsTimeline(1, mt_lastTimeStamp);
                }, p => (!this.IsSyncing));

            this.FavoritesTimeline = new ObservableCollection<Status>();
            this.FavoritesTimelineCommand = new DelegateCommand<string>(parameter =>
                {
                    if (parameter == "Refresh")
                        GetFavoritesTimeline();
                    else
                        GetFavoritesTimeline(1, ft_lastTimeStamp);
                }, p => (!this.IsSyncing));

            this.BackKeyPressCommand = new DelegateCommand(OnBackKeyPress,()=>true);
                
        }

        private void LoadDataFromCache()
        {
            this.IsSyncing = true;
            StatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                htStorage.TryLoadData<StatusCollection>(out collection);
                if (collection.Count > 0)
                {
                    collection.ForEach(a => HomeTimeline.Add(a));
                }
                else
                {
                    GetHomeTimeline();
                }
            }
            if (MentionsTimeline.Count <= 0)
            {
                mtStorage.TryLoadData<StatusCollection>(out collection);
                collection.ForEach(a => MentionsTimeline.Add(a));
            }
            if (FavoritesTimeline.Count <= 0)
            {
                ftStorage.TryLoadData<StatusCollection>(out collection);
                collection.ForEach(a => FavoritesTimeline.Add(a));
            }
            this.IsSyncing = false;
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

        private void OnBackKeyPress()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
