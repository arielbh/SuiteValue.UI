using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
namespace SuiteValue.UI.WP8
{
    public class NavigationPage : PhoneApplicationPage
    {
        private bool _isLoaded;
        private ProgressIndicator _progressIndicator;

        public NavigationPage()
        {
            Loaded += NavigationPage_Loaded;
        }

        void NavigationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= NavigationPage_Loaded;
            _isLoaded = true;
            _progressIndicator = SystemTray.ProgressIndicator ?? new ProgressIndicator { IsIndeterminate = true };

            if (ViewModel is IAsyncViewModel)
            {
                RegisterProgressBar(ViewModel as IAsyncViewModel);
            }


        }



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
            UnregisterViewModel(oldViewModel);
            RegisterViewModel(newViewModel);
            DataContext = newViewModel;
        }

        private void RegisterViewModel(INavigationViewModel newViewModel)
        {
            if (newViewModel != null && !newViewModel.RegisteredForNavigation)
            {
                newViewModel.RequestNavigateTo += ViewModel_RequestNavigateTo;
                newViewModel.RequestNavigateBack += ViewModel_RequestNavigateBack;
                newViewModel.RequestNavigateBackTo += newViewModel_RequestNavigateBackTo;
                newViewModel.RequestUnregister += ViewModel_RequestUnregister;
                if (newViewModel.SupportOrientation)
                {
                    OrientationChanged += NavigationPage_OrientationChanged;
                }
                newViewModel.RegisteredForNavigation = true;
                
                if (newViewModel is IAsyncViewModel)
                {
                    RegisterProgressBar(newViewModel as IAsyncViewModel);
                }
            }
        }

        void NavigationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            (ViewModel as INavigationViewModel).OrientationChanged(e.Orientation);
        }

        void ViewModel_RequestUnregister(object sender, EventArgs e)
        {
            UnregisterViewModel(ViewModel as INavigationViewModel);
        }

        private void UnregisterViewModel(INavigationViewModel oldViewModel)
        {
            if (oldViewModel != null && !oldViewModel.KeepRegistrationsAlive)
            {
                oldViewModel.RequestNavigateTo -= ViewModel_RequestNavigateTo;
                oldViewModel.RequestNavigateBack -= ViewModel_RequestNavigateBack;
                oldViewModel.RequestNavigateBackTo -= newViewModel_RequestNavigateBackTo;
                oldViewModel.RequestUnregister -= ViewModel_RequestUnregister;
                OrientationChanged -= NavigationPage_OrientationChanged;
                oldViewModel.RegisteredForNavigation = false;

                if (oldViewModel is IAsyncViewModel)
                {
                    UnregisterProgressBar(oldViewModel as IAsyncViewModel);
                }
            }
        }

        private void newViewModel_RequestNavigateBackTo(object sender, NavigationBackEventArgs e)
        {
            if (e.Parameters != null)
            {
                NavigationViewModelBase.NavigationState["back_params"] = e.Parameters;
            }
            JournalEntry target = null;
            var backStackList = NavigationService.BackStack.ToList();
            foreach (var backStack in backStackList)
            {
                var uri = backStack.Source.ToString();
                if (uri.Contains(e.ViewModel.ViewHint))
                {
                    target = backStack;
                    break;
                }
            }
            if (target == null)
            {
                // We can't find anything in the backlog
                e.ViewModel.RollbackFromNavigateBackTo(e.Parameters);
                return;
            }
            var index = backStackList.IndexOf(target);

            for (int i = 0; i < index; i++)
            {
                NavigationService.RemoveBackEntry();
            }
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void UnregisterProgressBar(IAsyncViewModel viewModel)
        {
            if (!_isLoaded) return;
            _progressIndicator.ClearValue(ProgressIndicator.IsVisibleProperty);
            _progressIndicator.ClearValue(ProgressIndicator.TextProperty);
        }

        private void RegisterProgressBar(IAsyncViewModel viewModel)
        {
            if (!_isLoaded) return;
            SystemTray.SetProgressIndicator(this, _progressIndicator);

            Binding binding = new Binding("IsInAsync") { Source = viewModel };
            BindingOperations.SetBinding(
                _progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("AsyncMessage") { Source = viewModel };
            BindingOperations.SetBinding(
                _progressIndicator, ProgressIndicator.TextProperty, binding);
        }

        void ViewModel_RequestNavigateBack(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
        void ViewModel_RequestNavigateTo(object sender, NavigationEventArgs e)
        {
            if (!AlwaysNavigateWhenUsingTheSameAddress)
            {
                string currentViewUri = NavigationService.CurrentSource.OriginalString;
                var queryIndex = currentViewUri.IndexOf('?');
                if (queryIndex > 0)
                {
                    currentViewUri = currentViewUri.Substring(0, queryIndex);
                }
                if (currentViewUri == e.ViewUri)
                {
                    return;
                }
            }

            var builder = new StringBuilder();
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
                    if (NavigationViewModelBase.NavigationState.ContainsKey(id))
                    {

                        ViewModel = NavigationViewModelBase.NavigationState[id] as NavigationViewModelBase;
                        NavigationViewModelBase.NavigationState.Remove(id);
                    }
                }
            }
            if (ViewModel != null)
            {
                RegisterViewModel(ViewModel as INavigationViewModel);
                IDictionary<string, string> parameters;
                if (NavigationViewModelBase.NavigationState.ContainsKey("back_params"))
                {
                    parameters = NavigationViewModelBase.NavigationState["back_params"] as IDictionary<string, string>;
                    NavigationViewModelBase.NavigationState.Remove("back_params");
                }
                else parameters = NavigationContext.QueryString;
                (ViewModel as INavigationViewModel).OnNavigatedTo(e.NavigationMode, parameters, e.IsNavigationInitiator);
            }
            base.OnNavigatedTo(e);
            if (OnFinishedNavigation != null)
            {
                OnFinishedNavigation(this, (INavigationViewModel) ViewModel);
            }
        }

        public Action<NavigationPage, INavigationViewModel>  OnFinishedNavigation { get; set; }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (ViewModel != null)
            {
                e.Cancel = (ViewModel as INavigationViewModel).OnNavigatingFrom(e.NavigationMode);
            }
            UnregisterViewModel(ViewModel as INavigationViewModel);
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
            if (!e.Cancel && ViewModel != null)
            {
                e.Cancel = (ViewModel as INavigationViewModel).OnBackKeyPress();
            }
            base.OnBackKeyPress(e);
        }

        protected bool AlwaysNavigateWhenUsingTheSameAddress { get; set; }
    }
}
