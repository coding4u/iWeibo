using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace iWeibo.WP7.Views
{
    public partial class PostNew : PhoneApplicationPage
    {
        public PostNew()
        {
            InitializeComponent();
        }

        //private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    statusTextBox.Focus();
        //}

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            int num = statusTextBox.Text.Length + 1;
            int num2 = 7;
            statusTextBox.Text += "#在此处输入话题#";
            statusTextBox.Select(num, num2);
            statusTextBox.Focus();
        }


    }
}