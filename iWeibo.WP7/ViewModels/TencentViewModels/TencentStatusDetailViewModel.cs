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
        private TencentAccessToken accessToken = TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>();

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

        public ObservableCollection<Status> CommentsTimeline { get; set; }

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand CommentsTimelineCommand { get; set; }

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

        private void GetStatus()
        {
            var tService = new TService(accessToken);
            new Thread(() =>
                {
                    tService.Show(new ServiceArgument() { Id = this.StatusId }, callback =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                if (callback.Succeed)
                                {
                                    this.Status = callback.Data;
                                }
                                else
                                {
                                    this.messageBox.Show(callback.ExceptionMsg);
                                }
                            });
                    });
                }).Start();
        }

        private long lastTimeStamp = 0;
        private int requestNumber = 20;

        private void GetCommentsTimeline(int pageFlag=0,long pageTime=0)
        {
            var statusService = new StatusesService(accessToken);
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
                                        lastTimeStamp = callback.Data.LastTimeStamp;
                                        callback.Data.ForEach(a => CommentsTimeline.Add(a));
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg);
                                    }
                                });
                        });
                }).Start();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
