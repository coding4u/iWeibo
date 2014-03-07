using Coding4Fun.Toolkit.Controls;
using Meituan.Client.Utilities;
using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows.Navigation;

namespace iWeibo.WP7.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            MemoryDiagnosticsHelper.Start(new TimeSpan(0, 0, 1), true);
        }

        private bool isLeaving = false;

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isLeaving)
            {
                e.Cancel = true;
                isLeaving = true;
                ToastPrompt toast = new ToastPrompt();
                toast.MillisecondsUntilHidden = 3000;//Toast显示3秒
                toast.Message = "再按一次Back键退出...";
                toast.Completed += toast_Completed;
                toast.Show();
            }
            else
            {
                //删除后退堆栈中的所有条目，以退出程序
                int count = NavigationService.BackStack.Count();
                for (int i = 0; i < count; i++)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

        }

        private void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            isLeaving = false;
        }

        private void AppBarMenuSettings_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(Constants.SettingsPageView, UriKind.Relative));
        }


    }
}