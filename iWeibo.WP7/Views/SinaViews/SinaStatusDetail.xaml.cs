using System;
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
using Microsoft.Phone.Controls;
using iWeibo.WP7.ViewModels.SinaViewModels;

namespace iWeibo.WP7.Views.SinaViews
{
    public partial class SinaStatusDetail : PhoneApplicationPage
    {
        public SinaStatusDetail()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = this.DataContext as SinaStatusDetailViewModel;
            string id="";
            viewModel.StatusId = this.NavigationContext.QueryString.TryGetValue("id", out id) ? id : "";
        }
    }
}