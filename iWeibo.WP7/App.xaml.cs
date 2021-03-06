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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TencentWeiboSDK;
using WeiboSdk;
using iWeibo.WP7.Models.TencentModels;
using iWeibo.WP7.Models.SinaModels;
using System.IO.IsolatedStorage;
using Meituan.Client.Utilities;
using Microsoft.Phone.Tasks;

namespace iWeibo.WP7
{
    public partial class App : Application
    {
        /// <summary>
        ///提供对电话应用程序的根框架的轻松访问。
        /// </summary>
        /// <returns>电话应用程序的根框架。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        private ViewModels.ViewModelLocator ViewModelLocator
        {
            get { return (ViewModels.ViewModelLocator)this.Resources["ViewModelLocator"]; }
        }

        /// <summary>
        /// Application 对象的构造函数。
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // 特定于电话的初始化
            InitializePhoneApplication();


            // 调试时显示图形分析信息。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //显示内存使用计数器
                //MemoryDiagnosticsHelper.Start(new TimeSpan(0, 0, 1), true);

                // 显示当前帧速率计数器。
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 显示在每个帧中重绘的应用程序区域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // 该模式显示递交给 GPU 的包含彩色重叠区的页面区域。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                //  注意: 仅在调试模式下使用此设置。禁用用户空闲检测的应用程序在用户不使用电话时将继续运行
                // 并且消耗电池电量。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
            
            // 配置 AppKey 和 AppSecret
            //Tencent
            OAuthConfigruation.APP_KEY = TencentConfig.AppKey;
            OAuthConfigruation.APP_SECRET = TencentConfig.AppSecret;
            OAuthConfigruation.IfSaveAccessToken = false;// 若你希望自己管理 AccessToken 则设置此参数.
            //Sina
            SdkData.AppKey = SinaConfig.AppKey;
            SdkData.AppSecret = SinaConfig.AppSecret;
            SdkData.RedirectUri = SinaConfig.ReDirectUri;

            CreateDirectories();
        }

        //创建目录
        private void CreateDirectories()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists("Tencent"))
                {
                    isf.CreateDirectory("Tencent");
                }
                if (!isf.DirectoryExists("Sina"))
                {
                    isf.CreateDirectory("Sina");
                }
            }
        }

        // 应用程序启动(例如，从“开始”菜单启动)时执行的代码
        // 此代码在重新激活应用程序时不执行
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // 激活应用程序(置于前台)时执行的代码
        // 此代码在首次启动应用程序时不执行
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // 停用应用程序(发送到后台)时执行的代码
        // 此代码在应用程序关闭时不执行
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 应用程序关闭(例如，用户点击“后退”)时执行的代码
        // 此代码在停用应用程序时不执行
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            this.ViewModelLocator.Dispose();
        }

        // 导航失败时执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 导航已失败；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        // 出现未处理的异常时执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            var exMsg =e.ExceptionObject.Message + Environment.NewLine + e.ExceptionObject.StackTrace;

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Content = new TextBlock()
            {
                Text = exMsg+Environment.NewLine,
                TextWrapping = TextWrapping.Wrap
            };

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "糟糕，程序似乎崩溃了...",
                Message = "是否将此信息发送给开发者，以帮助其改进此应用？",
                Content=scrollViewer,
                LeftButtonContent = "发送",
                RightButtonContent = "取消",
                IsFullScreen = true
            };

            messageBox.Dismissed += (s1, e1) =>
                {

                    if (e1.Result == CustomMessageBoxResult.LeftButton)
                    {
                        // Send Email
                        EmailComposeTask ect = new EmailComposeTask();
                        ect.Subject = "[爱微博反馈]";
                        ect.Body = exMsg + Environment.NewLine;
                        ect.To = "coding4u@outlook.com";
                        ect.Show();
                    }
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        // 出现未处理的异常；强行进入调试器
                        System.Diagnostics.Debugger.Break();
                    }
                    else
                    {
                        //删除后退堆栈中的所有条目，以退出程序
                        //while (RootFrame.BackStack.Any())
                        //{
                        //    RootFrame.RemoveBackEntry();
                        //}

                        for (int i = 0; i < RootFrame.BackStack.Count()-1; i++)
                        {
                            RootFrame.RemoveBackEntry();
                        }
                        if (RootFrame.CanGoBack)
                            RootFrame.GoBack();

                        //var currentPage=((PhoneApplicationPage)RootFrame.Content);
                        //for (int i = 0; i < currentPage.NavigationService.BackStack.Count()-1; i++)
                        //{
                        //    currentPage.NavigationService.RemoveBackEntry();
                        //}
                        //if (currentPage.NavigationService.CanGoBack)
                        //    currentPage.NavigationService.GoBack();

                    }

                };

            messageBox.Show();

        }

        #region 电话应用程序初始化

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 请勿向此方法中添加任何其他代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建框架但先不将它设置为 RootVisual；这允许初始
            // 屏幕保持活动状态，直到准备呈现应用程序时。
            //RootFrame = new PhoneApplicationFrame();
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理导航故障
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 确保我们未再次初始化
            phoneApplicationInitialized = true;
        }

        // 请勿向此方法中添加任何其他代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 设置根视觉效果以允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要它
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}