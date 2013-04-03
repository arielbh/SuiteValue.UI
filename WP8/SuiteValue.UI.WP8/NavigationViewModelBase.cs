using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace SuiteValue.UI.WP8
{
    public class NavigationViewModelBase : CommandableViewModelBase, INavigationViewModel, INavigator
    {
        private string _viewHint;
        public event EventHandler<NavigationEventArgs> RequestNavigateTo;
        public event EventHandler<EventArgs> RequestNavigateBack;
        public event EventHandler<NavigationBackEventArgs> RequestNavigateBackTo;


        public bool RegisteredForNavigation { get; set; }

        protected virtual void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
        }

        protected virtual bool OnNavigatingFrom(NavigationMode mode)
        {
            return false;
        }

        protected virtual void OnNavigatedFrom(NavigationMode mode)
        {

        }

        protected virtual bool OnBackKeyPress()
        {
            return false;
        }

        protected virtual void Navigate(string viewUri, IDictionary<string, string> parameters = null)
        {
            if (RequestNavigateTo != null)
            {
                RequestNavigateTo(this, new NavigationEventArgs(viewUri, parameters));
            }
        }

        protected virtual void Navigate<T>(T viewModel, IDictionary<string, string> parameters = null)
            where T : NavigationViewModelBase
        {
            
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            var uniqueKey = Guid.NewGuid().ToString();
            parameters["ViewModelId"] = uniqueKey;
            PhoneApplicationService.Current.State[uniqueKey] = viewModel;

            Navigate(viewModel.ViewHint, parameters);
        }

        protected virtual void NavigateBackTo<T>(T viewModel, IDictionary<string, string> parameters = null) where T : NavigationViewModelBase
        {
            if (RequestNavigateBackTo != null)
            {
                RequestNavigateBackTo(this, new NavigationBackEventArgs(viewModel, parameters));
            }
            
        }


        void INavigator.NavigateBackTo<T>(T viewModel, IDictionary<string, string> parameters)
        {
            NavigateBackTo(viewModel, parameters);
            
        }

        internal void RollbackFromNavigateBackTo(IDictionary<string, string> parameters)
        {
            Navigate(this, parameters);

        }

        protected virtual string DeriveViewNameByConvention()
        {
            return GetType().Name.Replace("Model", "") + ".xaml";
        }

        protected virtual string DecideViewHint()
        {
            return "/Views/" + DeriveViewNameByConvention();
        }

        public string ViewHint
        {
            get { return _viewHint ?? (_viewHint = DecideViewHint()); }
        }


        protected virtual void NavigateBack()
        {
            if (RequestNavigateBack != null)
            {
                RequestNavigateBack(this, EventArgs.Empty);
            }
        }


        void INavigationViewModel.OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
            OnNavigatedTo(mode, parameter, isNavigationInitiator);
        }

        bool INavigationViewModel.OnNavigatingFrom(NavigationMode mode)
        {
            return OnNavigatingFrom(mode);
        }

        void INavigationViewModel.OnNavigatedFrom(NavigationMode mode)
        {
            OnNavigatedFrom(mode);
        }

        bool INavigationViewModel.OnBackKeyPress()
        {
            return OnBackKeyPress();
        }

        void INavigator.Navigate(string viewUri, IDictionary<string, string> parameters)
        {
            Navigate(viewUri, parameters);
        }

        void INavigator.Navigate<T>(T viewModel, IDictionary<string, string> parameters)
        {
            Navigate(viewModel, parameters);
        }
    }


}