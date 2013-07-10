using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SuiteValue.UI.WP8
{
    public class NavigationViewModelBase : CommandableViewModelBase, INavigationViewModel, INavigator
    {
        internal static Dictionary<string, object> NavigationState = new Dictionary<string, object>();

        private string _viewHint;
        private bool _isInWaiting;
        private TaskCompletionSource<IDictionary<string, string>> _taskCompletionSource;
        
        public event EventHandler<NavigationEventArgs> RequestNavigateTo;
        public event EventHandler<EventArgs> RequestNavigateBack;
        public event EventHandler<NavigationBackEventArgs> RequestNavigateBackTo;
        public event EventHandler<EventArgs> RequestUnregister;

        public bool RegisteredForNavigation { get; set; }
        public bool SupportOrientation { get; protected set; }

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

        public void UnregisterFromPage()
        {
            KeepRegistrationsAlive = false;
            if (RequestUnregister != null)
            {
                RequestUnregister(this, new EventArgs());
            }
        }

        protected virtual void OrientationChanged(PageOrientation orientation)
        {
            
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
            NavigationState[uniqueKey] = viewModel;

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


        protected virtual void NavigateBack(IDictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                NavigationState.Add("back_params", parameters);
            }
            if (RequestNavigateBack != null)
            {
                RequestNavigateBack(this, EventArgs.Empty);
            }
        }


        protected virtual Task<IDictionary<string, string>> NavigateAndWait<T>(T viewModel, IDictionary<string, string> parameters = null) 
            where T : NavigationViewModelBase
        {
            _isInWaiting = true;
            _taskCompletionSource = new TaskCompletionSource<IDictionary<string, string>>();
            Navigate(viewModel, parameters);
            return _taskCompletionSource.Task;
        }


        void INavigationViewModel.OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
            if (mode == NavigationMode.Back)
            {
                if (_isInWaiting)
                {

                    _taskCompletionSource.SetResult(parameter);
                    _isInWaiting = false;
                }
            }
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

        void INavigator.NavigateBack(IDictionary<string, string> parameters)
        {
            NavigateBack(parameters);
        }

        Task<IDictionary<string, string>> INavigator.NavigateAndWait<T>(T viewModel, IDictionary<string, string> parameters) 
        {
            return NavigateAndWait(viewModel, parameters);
        }

        void INavigationViewModel.OrientationChanged(PageOrientation orientation)
        {
            OrientationChanged(orientation);
        }

        public bool KeepRegistrationsAlive { get; set; }

    }           


}