using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TencentWeiboSDK;
using System.Threading;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Model;
using iWeibo.WP7.Services;

namespace iWeibo.WP7.Views.TencentViews
{
    public partial class TencentLogin : PhoneApplicationPage
    {

        public TencentLogin()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.TencentOauthControl.OBrowserNavigating = new EventHandler(OAuthBrowserNavigating);
            this.TencentOauthControl.OBrowserNavigated = new EventHandler(OAuthBrowserNavigated);
            this.TencentOauthControl.OBrowserCancelled = new EventHandler(OAuthBrowserCanceled);

            Login();
        }

        private void OAuthBrowserCanceled(object sender, EventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void OAuthBrowserNavigated(object sender, EventArgs e)
        {
            this.Indicator.IsVisible = false;
        }

        private void OAuthBrowserNavigating(object sender, EventArgs e)
        {
            this.Indicator.IsVisible = true;
        }

        private void Login()
        {
            Indicator.IsVisible = true;
                //开始进行OAuth授权。
                TencentOauthControl.OAuthLogin((callback) =>
                {
                    // 若已获得 AccessToken 则跳转到 TimelineView 页面
                    // 注意： 若 OAuthConfigruation.IfSaveAccessToken 属性为 False，则需要在此处保存用户的 AccessToken(callback.Data) 以便下次使用.
                    if (callback.Succeed)
                    {
                        OAuthConfigruation.AccessToken = callback.Data;
                        TokenIsoStorage.TencentTokenStorage.SaveData(callback.Data);
                        GetMyInfoAndNavigate();
                    }
                });

        }


        private void GetMyInfoAndNavigate()
        {
            this.Indicator.IsVisible = true;
            new Thread(() =>
                {
                    new UserService().UserInfo(callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    this.Indicator.IsVisible = false;
                                    if (callback.Succeed)
                                    {
                                        new IsoStorage(Constants.TencentOAuthedUser).SaveData(callback.Data);
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ExceptionMsg);
                                    }

                                    NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.RelativeOrAbsolute));

                                });

                        });
                }).Start();
        }

    }
}