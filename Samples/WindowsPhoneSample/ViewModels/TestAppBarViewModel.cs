using System;
using SuiteValue.UI.MVVM;
using SuiteValue.UI.WP8;
using SuiteValue.UI.WP8.Controls;

namespace WindowsPhoneSample.ViewModels
{
    public class TestAppBarViewModel 
        : NavigationViewModelBase
    {
        public TestAppBarViewModel()
        {
            AppbarCommands = new[]
            {
                new AppBarData {IconUri = new Uri("/Assets/feature.search.png", UriKind.Relative), IsVisible = true, Text = "Margol"},
                new AppBarData {IconUri = new Uri("/Assets/feature.phone.png", UriKind.Relative), IsVisible = true, Text = "Grrr"},
                new AppBarData {IconUri = new Uri("/Assets/feature.email.png", UriKind.Relative), IsVisible = true, Text = "Bah"},

            };
        }
         
    }
}