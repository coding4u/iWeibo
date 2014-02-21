using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeiboSdk;
using WeiboSdk.Models;
using iWeibo.WP7.Services;
using WeiboSdk.Services;
using System.Threading;

namespace iWeibo.WP7.Views.SinaViews
{
    public partial class SinaLogin : PhoneApplicationPage
    {

        //public static OAuth2LoginBack OAuth2VerifyCompleted { get; set; }
        //public static EventHandler OBrowserCancelled { get; set; }
        //public static EventHandler OBrowserNavigated { get; set; }
        //public static EventHandler OBrowserNavigating { get; set; }

        private string accessToken;

        public SinaLogin()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SinaOAuthControl.OBrowserNavigating = new EventHandler(OAuthBrowserNavigating);
            this.SinaOAuthControl.OBrowserNavigated = new EventHandler(OAuthBrowserNavigated);
            this.SinaOAuthControl.OBrowserCancelled = new EventHandler(CancleEvent);
            this.SinaOAuthControl.OAuth2VerifyCompleted += new OAuth2LoginBack(OAuth2CallBack);
        }

        private void OAuth2CallBack(bool isSucess, SdkAuthError err, SdkAuth2Res response)
        {
            VerifyBack(isSucess, err, response);
        }

        private void VerifyBack(bool isSuccess, SdkAuthError err, SdkAuth2Res response)
        {
            if (err.errCode == SdkErrCode.SUCCESS)
            {
                if (null != response)
                {
                    SinaAccessToken token = new SinaAccessToken
                    {
                        Token = response.accesssToken,
                        TokenSecret = response.refleshToken
                    };

                    accessToken = response.accesssToken;

                    //保存Token
                    TokenIsoStorage.SinaTokenStorage.SaveData(token);

                    GetMyInfoAndNavigate();
                }

            }
            else if(err.errCode==SdkErrCode.NET_UNUSUAL)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("网络异常，请检查网络后重试...");
                    });
            }
            else if (err.errCode == SdkErrCode.SERVER_ERR)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("服务器返回错误，错误代码：" + err.specificCode);
                    });
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("未知错误，请稍后重试...");
                    });
            }
        }

        private void CancleEvent(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (NavigationService.CanGoBack)
                        this.NavigationService.GoBack();
                });
        }

        private void OAuthBrowserNavigated(object sender, EventArgs e)
        {
            Indicator.IsVisible = false;
            //if (null != OBrowserNavigated)
            //    OBrowserNavigated.Invoke(sender, e);
        }

        private void OAuthBrowserNavigating(object sender, EventArgs e)
        {
            Indicator.IsVisible = true;

            //if (null != OBrowserNavigating )
            //{
            //    OBrowserNavigating.Invoke(sender, e);
            //}
        }

        private void GetMyInfoAndNavigate()
        {
            new Thread(() =>
                {
                    WUserService userService = new WUserService(accessToken);
                    userService.GetMyUserInfo(callback =>
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (callback.Succeed)
                                    {
                                        new IsoStorage(Constants.SinaOAuthedUser).SaveData(callback.Data);
                                    }
                                    else
                                    {
                                        MessageBox.Show(callback.ErrorMsg);
                                    }

                                    this.NavigationService.Navigate(new Uri(Constants.SinaTimelineView, UriKind.Relative));

                                });
                        });

                }).Start();
        }

    }
}