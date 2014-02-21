using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iWeibo.WP7.Services;
using System.IO.IsolatedStorage;
using System.IO;
using Newtonsoft.Json;
using WeiboSdk.Models;

namespace iWeibo.WP7.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void AppBarMenuSettings_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(Constants.SettingsPage, UriKind.Relative));
        }


    }
}