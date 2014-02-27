using iWeibo.WP7.Adapters;
using iWeibo.WP7.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP7.ViewModels.SinaViewModels
{
    public class SinaTimelineViewModel:ViewModel
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
                    HandlePivotSelectedIndexChange();
                }
            }
        }

        public ObservableCollection<WStatus> HomeTimeline { get; set; }
        public ObservableCollection<WStatus> MentionsTimeline { get; set; }
        public ObservableCollection<WStatus> FavoritesTimeline { get; set; }

        #endregion

        #region DelegateCommands
        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand<string> FavoritesTimleineCommand { get; set; }
        #endregion

        #region Fields

        private long htPreviousCursor=0;
        private long htNextCursor;
        private long mtPreviousCursor=0;
        private long mtNextCursor;
        private long ftPreviousCursor=0;
        private long ftNextCursor;

        private IsoStorage htStorage = new IsoStorage(Constants.SinaHomeTime);
        private IsoStorage mtStorage = new IsoStorage(Constants.SinaMentionsTimeline);
        private IsoStorage ftStorage = new IsoStorage(Constants.SinaFavoritesTimeline);

        private int requestCount=20;

        private TimelineService timelineService = new TimelineService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());

        #endregion

        #region Constructor

        public SinaTimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.SinaTimelineView,UriKind.Relative))
        {
            HomeTimeline = new ObservableCollection<WStatus>();
            MentionsTimeline = new ObservableCollection<WStatus>();
            FavoritesTimeline = new ObservableCollection<WStatus>();

            this.PageLoadedCommand=new DelegateCommand(LoadDataFromCache,()=>!this.IsSyncing);
            this.BackKeyPressCommand=new DelegateCommand(OnBackKeyPress,()=>true);
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);
            this.HomeTimelineCommand=new DelegateCommand<string>(p=>
                {
                    if(p=="Next")
                        GetHomeTimeline(htNextCursor);
                    else
                        GetHomeTimeline(htPreviousCursor);
                },p=>!this.IsSyncing);
            this.MentionsTimelineCommand=new DelegateCommand<string>(p=>
                {
                    if(p=="Next")
                        GetMentionsTimeline(mtNextCursor);
                    else
                        GetMentionsTimeline(mtPreviousCursor);
                },p=>!this.IsSyncing);
            this.FavoritesTimleineCommand=new DelegateCommand<string>(p=>
                {
                    if(p=="Next")
                        GetFavoritesTimeline(ftNextCursor);
                    else
                        GetFavoritesTimeline(ftPreviousCursor);
                },p=>!this.IsSyncing);
        }

        #endregion

        #region Methods

        private void LoadDataFromCache()
        {
            WStatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                if (htStorage.TryLoadData<WStatusCollection>(out collection))
                {
                    collection.Statuses.ForEach(a => HomeTimeline.Add(a));
                }
                else
                {
                    GetHomeTimeline();
                }
            }
            if (MentionsTimeline.Count <= 0)
            {
                if (mtStorage.TryLoadData<WStatusCollection>(out collection))
                    collection.Statuses.ForEach(a => MentionsTimeline.Add(a));

            }
            WFavoriteCollection fCollection;
            if(FavoritesTimeline.Count<=0)
            {
                if (ftStorage.TryLoadData<WFavoriteCollection>(out fCollection))
                    fCollection.Favorites.ForEach(a => FavoritesTimeline.Add(a));
            }
        }

        private void HandlePivotSelectedIndexChange()
        {
            switch(SelectedPivotIndex)
            {
                case 0:
                    if(HomeTimeline.Count<=0)
                        GetHomeTimeline();
                    break;
                case 1:
                    if(MentionsTimeline.Count<=0)
                        GetMentionsTimeline();
                    break;
                case 2:
                    if(FavoritesTimeline.Count<=0)
                        GetFavoritesTimeline();
                    break;
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

        private void OnBackKeyPress()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView,UriKind.Relative));
        }

        private void GetHomeTimeline(long sinceId=0,long maxId=0)
        {
            this.IsSyncing=true;

            new Thread(()=>
                {
                    timelineService.GetFriendsTimeline(requestCount,maxId,sinceId,callback=>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(()=>
                                {
                                    if(callback.Succeed)
                                    {
                                        if(maxId==0&&sinceId==0)
                                        {
                                            HomeTimeline.Clear();
                                            htStorage.SaveData(callback.Data);
                                        }
                                        htPreviousCursor=callback.Data.PreviousCursor;
                                        htNextCursor=callback.Data.NextCursor;
                                        callback.Data.Statuses.ForEach(a=>HomeTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ErrorMsg);
                                    }
                                    this.IsSyncing=false;
                                });
                        });
                }).Start();

        }

        private void GetMentionsTimeline(long sinceId=0,long maxId=0)
        {
            this.IsSyncing=true;
            new Thread(()=>
                {
                    timelineService.GetMentionsTimeline(requestCount,maxId,sinceId,callback=>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(()=>
                                {
                                    if(callback.Succeed)
                                    {
                                        if(maxId==0&&sinceId==0)
                                        {
                                            MentionsTimeline.Clear();
                                            mtStorage.SaveData(callback.Data);
                                        }
                                        mtPreviousCursor=callback.Data.PreviousCursor;
                                        mtNextCursor=callback.Data.NextCursor;
                                        callback.Data.Statuses.ForEach(a=>MentionsTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ErrorMsg);
                                    }
                                    this.IsSyncing=false;
                                });
                        });
                }).Start();

        }

        private void GetFavoritesTimeline(long sinceId=0,long maxId=0)
        {
            this.IsSyncing=true;
            new Thread(()=>
                {
                    timelineService.GetFavoritesTimeline(requestCount,maxId,sinceId,callback=>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(()=>
                                {
                                    if(callback.Succeed)
                                    {
                                        if(maxId==0&&sinceId==0)
                                        {
                                            FavoritesTimeline.Clear();
                                            ftStorage.SaveData(callback.Data);
                                        }
                                        ftPreviousCursor=callback.Data.PreviousCursor;
                                        ftNextCursor=callback.Data.NextCursor;
                                        callback.Data.Favorites.ForEach(a=>FavoritesTimeline.Add(a));
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ErrorMsg);
                                    }
                                    this.IsSyncing=false;
                                });
                        });
                }).Start();

        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        #endregion 
    }
}
