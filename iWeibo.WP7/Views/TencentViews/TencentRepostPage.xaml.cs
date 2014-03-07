using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iWeibo.WP7.ViewModels.TencentViewModels;

namespace iWeibo.WP7.Views.TencentViews
{
    public partial class TencentRepostPage : PhoneApplicationPage
    {
        public TencentRepostPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as TencentRepostPageViewModel;

            string statusId = "";
            this.NavigationContext.QueryString.TryGetValue("id", out statusId);
            viewModel.StatusId = statusId;

            string type = "";
            this.NavigationContext.QueryString.TryGetValue("type", out type);
            viewModel.IsRepost = type == "repost" ? true : false;

        }
    }
}