using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuiteValue.UI.WP8;
using WindowsPhoneSample.ViewModels;

namespace WindowsPhoneSample.Views
{
    public partial class TestView : NavigationPage
    {
        public TestView()
        {
            InitializeComponent();
            ViewModel = new TestViewModel();
        }

    }
}