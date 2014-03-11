using iWeibo.WP7.Adapters;
using iWeibo.WP7.Models.SinaModels;
using iWeibo.WP7.Models.TencentModels;
using iWeibo.WP7.Services;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.Prism.Commands;
using System;
using TencentWeiboSDK.Model;
using WeiboSdk.Models;

namespace iWeibo.WP7.ViewModels
{
    public class SettingsViewModel:ViewModel
    {
        private IMessageBox messageBox;

        private IsoStorage tencentUserStorage = new IsoStorage(Constants.TencentOAuthedUser);

        private IsoStorage sinaUserStorage = new IsoStorage(Constants.SinaOAuthedUser);

        private string sinaUserName="新浪微博";

        public string SinaUserName
        {
            get
            {
                return sinaUserName;
            }
            set
            {
                if (value != sinaUserName)
                {
                    sinaUserName = value;
                    RaisePropertyChanged(() => this.SinaUserName);
                }
            }
        }

        private string tencentUserName="腾讯微博";

        public string TencentUserName
        {
            get
            {
                return tencentUserName;
            }
            set
            {
                if (value != tencentUserName)
                {
                    tencentUserName = value;
                    RaisePropertyChanged(() => this.TencentUserName);
                }
            }
        }

        private string sinaUserPicUrl;

        public string SinaUserPicUrl
        {
            get
            {
                return sinaUserPicUrl;
            }
            set
            {
                if (value != sinaUserPicUrl)
                {
                    sinaUserPicUrl = value;
                    RaisePropertyChanged(() => this.SinaUserPicUrl);
                }
            }
        }

        private string tencentUserPicUrl;

        public string TencentUserPicUrl
        {
            get
            {
                return tencentUserPicUrl;
            }
            set
            {
                if (value != tencentUserPicUrl)
                {
                    tencentUserPicUrl = value;
                    RaisePropertyChanged(() => this.TencentUserPicUrl);
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

        public DelegateCommand PageLoadedCommand { get; set; }
        
        public DelegateCommand DeleteSinaUserCommand { get; set; }

        public DelegateCommand DeleteTencentUserCommand { get; set; }

        public DelegateCommand EmailCommand { get; set; }

        public DelegateCommand RatingCommand { get; set; }

        public DelegateCommand MarketDetailCommand { get; set; }
        

        public SettingsViewModel(
            INavigationService navicationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navicationService,phoneApplicationServiceFacade,new Uri(Constants.SettingsPageView,UriKind.Relative))
        {
            this.messageBox = messageBox;

            this.PageLoadedCommand = new DelegateCommand(PageLoaded);
            this.DeleteSinaUserCommand = new DelegateCommand(DeleteSinaUser, () => SinaConfig.Validate());
            this.DeleteTencentUserCommand = new DelegateCommand(DeleteTencentUser, () => TencentConfig.Validate());
            this.EmailCommand = new DelegateCommand(OpenEmail);
            this.RatingCommand = new DelegateCommand(Rating);
            this.MarketDetailCommand = new DelegateCommand(OpenMarket);

        }

        private void PageLoaded()
        {
            if (TencentConfig.Validate())
            {
                var tencentUser = tencentUserStorage.LoadData<User>();
                this.TencentUserName = tencentUser.Nick;
                this.TencentUserPicUrl = tencentUser.Head + @"/100";
            }

            if (SinaConfig.Validate())
            {
                var sinaUser = sinaUserStorage.LoadData<WUser>();
                this.SinaUserName = sinaUser.ScreenName;
                this.SinaUserPicUrl = sinaUser.AvatarLarge;
            }

        }

        private void  DeleteSinaUser()
        {
            sinaUserStorage.Clear();
            TokenIsoStorage.SinaTokenStorage.Clear();
            new IsoStorage(Constants.SinaHomeTime).Clear();
            new IsoStorage(Constants.SinaMentionsTimeline).Clear();
            new IsoStorage(Constants.SinaFavoritesTimeline).Clear();
            new IsoStorage(Constants.SinaSelectedStatus).Clear();

            NavigateToMainPage();
        }

        private void DeleteTencentUser()
        {
            tencentUserStorage.Clear();
            TokenIsoStorage.TencentTokenStorage.Clear();
            new IsoStorage(Constants.TencentHomeTimeline).Clear();
            new IsoStorage(Constants.TencentMentionsTimeline).Clear();
            new IsoStorage(Constants.TencentFavoritesTimeline).Clear();
            new IsoStorage(Constants.TencentSelectedStatus).Clear();

            NavigateToMainPage();
        }

        private void NavigateToMainPage()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        private void OpenEmail()
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.Subject = "爱微博反馈";
            ect.To = "coding4u@outlook.com";
            ect.Show();
        }

        private void Rating()
        {
            MarketplaceReviewTask mdr = new MarketplaceReviewTask();
            mdr.Show();
        }

        private void OpenMarket()
        {
            MarketplaceDetailTask mdt = new MarketplaceDetailTask();
            mdt.ContentType = MarketplaceContentType.Applications;
            mdt.ContentIdentifier = "88f91696-9f3d-41b7-862d-0c268bb211d2";
            mdt.Show();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
