using Coding4Fun.Toolkit.Controls;
using iWeibo.WP7.Adapters;
using iWeibo.WP7.Services;
using iWeibo.WP7.Util;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;

namespace iWeibo.WP7.ViewModels.TencentViewModels
{
    public class TencentRepostPageViewModel:ViewModel,ITextBoxController
    {

        public event FocusEventHandler Focus;
        public event SelectEventHandler Select;

        private IMessageBox messageBox;
        private TService tService = new TService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());

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
                    this.SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool isRepost;

        public bool IsRepost
        {
            get
            {
                return isRepost;
            }
            set
            {
                if (value != isRepost)
                {
                    isRepost = value;
                    RaisePropertyChanged(() => this.IsRepost);
                }
            }
        }

        private string repostText;

        public string RepostText
        {
            get
            {
                return repostText;
            }
            set
            {
                if (value != repostText)
                {
                    repostText = value;
                    RaisePropertyChanged(() => this.RepostText);
                    HandleTextChange();
                }
            }
        }

        private int wordsCounter=140;

        public int WordsCounter
        {
            get
            {
                return wordsCounter;
            }
            set
            {
                if (value != wordsCounter)
                {
                    wordsCounter = value;
                    RaisePropertyChanged(() => this.WordsCounter);
                }
            }
        }

        private string wordsCounterColor;

        public string WordsCounterColor
        {
            get
            {
                return wordsCounterColor;
            }
            set
            {
                if (value != wordsCounterColor)
                {
                    wordsCounterColor = value;
                    RaisePropertyChanged(() => this.WordsCounterColor);
                }
            }
        }

        private bool hasText;

        public bool HasText
        {
            get
            {
                return hasText;
            }
            set
            {
                if (value != hasText)
                {
                    hasText = value;
                    RaisePropertyChanged(() => this.HasText);
                }
            }
        }

        public string StatusId { get; set; }
        
        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand ClearTextCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand AddTopicCommand { get; set; }

                
        public TencentRepostPageViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox message)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.TencentRepostPageView, UriKind.Relative))
        {
            this.PageLoadedCommand = new DelegateCommand(() =>
                {
                    if (Focus != null)
                    {
                        Focus(this);
                    }
                });

            this.ClearTextCommand = new DelegateCommand(() => { this.RepostText = ""; });

            this.SendCommand = new DelegateCommand(() =>
                {
                    if (this.IsRepost)
                        Repost();
                    else
                        Comment();
                }, () => !this.IsSending && !string.IsNullOrEmpty(RepostText) && !(RepostText.Length > 140));

            this.AddTopicCommand = new DelegateCommand(() =>
                {
                    if (this.Select != null)
                    {
                        int start = string.IsNullOrEmpty(RepostText) ? 1 : RepostText.Length + 1;
                        int length = 7;
                        RepostText += "#在此处输入话题#";
                        Select(this, start, length);
                    }
                });
        }

        private void HandleTextChange()
        {
            this.HasText = string.IsNullOrEmpty(this.RepostText) ? false : true;
            this.WordsCounter = 140 - RepostText.Length;
            if (WordsCounter < 0)
            {
                WordsCounterColor = "Red";
            }
            else
            {
                WordsCounterColor = "White";
            }

            this.SendCommand.RaiseCanExecuteChanged();
        }

        private void Repost()
        {
            this.IsSending = true;
            new Thread(() =>
                {
                    tService.Repost(new ServiceArgument() { Reid = this.StatusId, Content = this.RepostText },
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        ToastPrompt toast = new ToastPrompt();
                                        toast.Message = "转发成功...";
                                        toast.Show();
                                        toast.Completed += toast_Completed;
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg, "转发失败", MessageBoxButton.OK);
                                    }

                                    this.IsSending = false;
                                });
                        });
                }).Start();
        }


        private void Comment()
        {
            this.IsSending = true;
            new Thread(() =>
                {
                    tService.Comment(new ServiceArgument() { Reid = this.StatusId, Content = this.RepostText },
                        callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        ToastPrompt toast = new ToastPrompt();
                                        toast.Message = "评论成功...";
                                        toast.Show();
                                        toast.Completed += toast_Completed;
                                    }
                                    else
                                    {
                                        this.messageBox.Show(callback.ExceptionMsg, "评论失败", MessageBoxButton.OK);
                                    }
                                    this.IsSending = false;
                                });
                        });
                }).Start();
        }


        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
