using System;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
namespace SuiteValue.UI.WP8
{
    public class NavigationPage : PhoneApplicationPage
    {
        /// <summary>
        /// Gets or sets the view-model attached to this view.
        /// </summary>
        public object ViewModel
        {
            get { return (INavigationViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ViewModel. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
        "ViewModel",
        typeof(object),
        typeof(NavigationPage),
        new PropertyMetadata(
        null,
        (d, e) => ((NavigationPage)d).ViewModelChanged((INavigationViewModel)e.OldValue, (INavigationViewModel)e.NewValue)));
        private void ViewModelChanged(INavigationViewModel oldViewModel, INavigationViewModel newViewModel)
        {
            if (oldViewModel != null)
            {
                oldViewModel.RequestNavigateTo -= ViewModel_RequestNavigateTo;
                oldViewModel.RequestNavigateBack -= ViewModel_RequestNavigateBack;
            }
            if (newViewModel != null && !newViewModel.RegisteredForNavigation)
            {
                newViewModel.RequestNavigateTo += ViewModel_RequestNavigateTo;
                newViewModel.RequestNavigateBack += ViewModel_RequestNavigateBack;
                newViewModel.RegisteredForNavigation = true;
            }
            DataContext = newViewModel;
        }
        void ViewModel_RequestNavigateBack(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
        void ViewModel_RequestNavigateTo(object sender, NavigationEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(e.ViewUri);
            if (e.Parameters != null)
            {
                builder.Append("?");
                foreach (var pair in e.Parameters)
                {
                    builder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value));
                }

                // the remove the trailing "&"
                builder.Remove(builder.Length - 1, 1);
            }
            NavigationService.Navigate(new Uri(builder.ToString(), UriKind.Relative));
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString != null)
            {
                if (NavigationContext.QueryString.ContainsKey("ViewModelId"))
                {
                    var id = NavigationContext.QueryString["ViewModelId"];
                    if (PhoneApplicationService.Current.State.ContainsKey(id))
                    {

                        ViewModel = PhoneApplicationService.Current.State[id] as NavigationViewModelBase;
                        PhoneApplicationService.Current.State.Remove(id);
                    }
                }
            }
            if (ViewModel != null)
            {
                (ViewModel as INavigationViewModel).OnNavigatedTo(e.NavigationMode, NavigationContext.QueryString);
            }
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel != null)
            {
                e.Cancel = (ViewModel as INavigationViewModel).OnNavigatingFrom(e.NavigationMode);
            }
            base.OnNavigatingFrom(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (ViewModel != null)
            {
                (ViewModel as INavigationViewModel).OnNavigatedFrom(e.NavigationMode);
            }
            base.OnNavigatedFrom(e);
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (ViewModel != null)
            {
                e.Cancel = (ViewModel as INavigationViewModel).OnBackKeyPress();
            }
            base.OnBackKeyPress(e);
        }
    }
}
