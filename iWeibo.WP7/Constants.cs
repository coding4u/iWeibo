using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.WP7
{
    public static class Constants
    {

        #region User Info Storage Path
        public const string TencentOAuthedUser = "/Tencent/OAuthedUserInfo.dat";
        public const string SinaOAuthedUser = "/Sina/OAuthedUserInfo.dat";
        #endregion

        #region Views Path
        public const string MainPageView = "/Views/MainPage.xaml";
        public const string PostNewView = "/Views/PostNew.xaml";
        public const string SettingsPageView = "/Views/SettingsPage.xaml";
        public const string PictureView = "/Views/PictureView.xaml";

        public const string SinaLoginView = "/Views/SinaViews/SinaLogin.xaml";
        public const string SinaTimelineView = "/Views/SinaViews/SinaTimeline.xaml";
        public const string SinaStatusDetailView = "/Views/SinaViews/SinaStatusDetail.xaml";
        public const string SinaRepostPageView = "/Views/SinaViews/SinaRepostPage.xaml";

        public const string TencentLoginView = "/Views/TencentViews/TencentLogin.xaml";
        public const string TencentTimelineView = "/Views/TencentViews/TencentTimeline.xaml";
        public const string TencentStatusDetailView = "/Views/TencentViews/TencentStatusDetail.xaml";
        public const string TencentRepostPageView = "/Views/TencentViews/TencentRepostPage.xaml";

        #endregion

        #region Timeline Storage Path
        public const string SinaHomeTime = "/Sina/HomeTimeline.dat";
        public const string SinaMentionsTimeline = "/Sina/MentionsTimeline.dat";
        public const string SinaFavoritesTimeline = "/Sina/FavoritesTimeline.dat";

        public const string TencentHomeTimeline = "/Tencent/HomeTimeline.dat";
        public const string TencentMentionsTimeline = "/Tencent/MentionsTimeline.dat";
        public const string TencentFavoritesTimeline = "/Tencent/FavoritesTimeline.dat";

        #endregion

        #region Status Storage Path
        public const string TencentSelectedStatus = "/Tencent/SelectedStatus.dat";
        public const string SinaSelectedStatus = "/Sina/SelectedStatus.dat";
        #endregion
    }
}
