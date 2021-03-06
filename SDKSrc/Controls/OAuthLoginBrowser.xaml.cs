﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services.Util;
using System.IO.IsolatedStorage;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Util;
using System.Threading;

namespace TencentWeiboSDK.Controls
{
    /// <summary>
    /// OAuth 登录控件，用于 OAuth 授权，并返回 AccessToken 对象.
    /// </summary>
    public partial class OAuthLoginBrowser : UserControl
    {
        private OAuthService service = new OAuthService();
        private const string callbackUrl = "http://t.qq.com";
        private RequestToken requestToken = null;
        private Action<Callback<TencentAccessToken>> actionTokenCallback = null;
        UserService userService = new UserService();

        public EventHandler OBrowserCancelled { get; set; }
        public EventHandler OBrowserNavigated { get; set; }
        public EventHandler OBrowserNavigating { get; set; }

        /// <summary>
        /// 构造函数.
        /// </summary>
        public OAuthLoginBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 开始进行 OAuth 授权，若用户授权成功，则返回 AccessToken.
        /// </summary>
        /// <param name="actionTokenCallback">回调返回 AccessToken.</param>
        public void OAuthLogin(Action<Callback<TencentAccessToken>> actionTokenCallback)
        {
            this.actionTokenCallback = actionTokenCallback;

            if (string.IsNullOrEmpty(OAuthConfigruation.APP_KEY))
            {
                MessageBox.Show("请输入ConsumerKey!");

                OnCallbackAccessToken(null);
                return;
            }

            if (string.IsNullOrEmpty(OAuthConfigruation.APP_SECRET))
            {
                MessageBox.Show("请输入ConsumerSecret!");

                OnCallbackAccessToken(null);
                return;
            }

            service.GetRequestTokenRequest(callbackUrl, (action) =>
            {
                this.requestToken = action.Data;

                this.Dispatcher.BeginInvoke(() =>
                {
                    webBrowser1.Navigate(new Uri(requestToken.AccessUrl));
                });
            });
        }

        private void webBrowser1_Navigating(object sender, Microsoft.Phone.Controls.NavigatingEventArgs e)
        {
            if (null != OBrowserNavigating)
            {
                OBrowserNavigating.Invoke(sender, e);
            }
            if (e.Uri.ToString().StartsWith(callbackUrl))
            {
                if (null != OBrowserNavigated)
                {
                    OBrowserNavigated.Invoke(sender, e);
                }
                e.Cancel = true;
                var result = OAuthHelper.GetQueryParameters(e.Uri.ToString());
                requestToken.Verifier = result["oauth_verifier"];
                service.GetAccessTokenRequest(requestToken, MergeAccessToken);
            }
        }
        /// <summary>
        /// 合并AccessToken并清空缓存
        /// </summary>
        /// <param name="callback"></param>
        private void MergeAccessToken(Callback<TencentAccessToken> callback)
        {
            if (OAuthConfigruation.IfSaveAccessToken)
            {
                AccessTokens oldAccessTokens = TokenIso.Current.LoadData();
                if (oldAccessTokens.Count == 0)
                {
                    OAuthConfigruation.AccessToken = callback.Data;
                    using (AutoResetEvent are = new AutoResetEvent(false))
                    {
                        userService.UserInfo((userInfocallback) =>
                        {
                            callback.Data.Head = userInfocallback.Data.Head;
                            oldAccessTokens.Add(callback.Data);
                            TokenIso.Current.SaveData(oldAccessTokens);
                            are.Set();
                        });
                        are.WaitOne();
                    }
                    
                }
                else
                {
                    oldAccessTokens.ForEach((item) => { item.Open = false; });
                    OAuthConfigruation.AccessToken = callback.Data;
                    using (AutoResetEvent are = new AutoResetEvent(false))
                    {
                        userService.UserInfo((userInfocallback) =>
                        {
                            callback.Data.Head = userInfocallback.Data.Head;
                            oldAccessTokens.Add(callback.Data);
                            TokenIso.Current.SaveData(oldAccessTokens);
                            are.Set();
                        });
                        are.WaitOne();
                    }
                }
               
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                if (actionTokenCallback != null)
                {
                    actionTokenCallback(callback);
                }
            });
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            webBrowser1.Width = this.Width;
            webBrowser1.Height = this.Height;
        }

        private void OnCallbackAccessToken(TencentAccessToken accessToken)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    if (null != this.actionTokenCallback)
                    {
                        actionTokenCallback(new Callback<TencentAccessToken>( accessToken));
                    }
                });
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //
        }

        private void webBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (null != OBrowserNavigated)
            {
                OBrowserNavigated.Invoke(sender, e);
            }
        }

    }
}
