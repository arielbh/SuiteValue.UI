using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace SuiteValue.UI.WP8
{
    public class NavigationViewModelBase : CommandableViewModelBase, INavigationViewModel
    {
        public event EventHandler<NavigationEventArgs> RequestNavigateTo;
        public event EventHandler<EventArgs> RequestNavigateBack;

        public bool RegisteredForNavigation { get; set; }

        protected virtual void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter)
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

        protected virtual void NavigateBack()
        {
            if (RequestNavigateBack != null)
            {
                RequestNavigateBack(this, EventArgs.Empty);
            }
        }


        void INavigationViewModel.OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter)
        {
            OnNavigatedTo(mode, parameter);
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
    }
}