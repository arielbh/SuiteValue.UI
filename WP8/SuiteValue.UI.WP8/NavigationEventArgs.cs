using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SuiteValue.UI.WP8
{
    public class NavigationEventArgs : CancelEventArgs
    {
        public string ViewUri{ get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }

        public NavigationEventArgs(string viewUri, IDictionary<string, string> parameters)
        {
            
            this.ViewUri = viewUri;
            this.Parameters = parameters;
        }
    }

    public class NavigateWithWaitEventArgs : NavigationEventArgs
    {
        public NavigateWithWaitEventArgs(string viewUri, NavigationViewModelBase viewModel, IDictionary<string, string> parameters, TaskCompletionSource<object> waitHandle)
            : base(viewUri, parameters)
        {
            ViewModel = viewModel;
            WaitHandle = waitHandle;
        }

        public NavigationViewModelBase ViewModel { get; set; }
        public TaskCompletionSource<object> WaitHandle { get; private set; }
    }

    public class NavigationBackEventArgs : EventArgs
    {
        public NavigationViewModelBase ViewModel { get; set; }
        public IDictionary<string, string> Parameters { get; private set; }


        public NavigationBackEventArgs(NavigationViewModelBase viewModel, IDictionary<string, string> parameters)
        {
            ViewModel = viewModel;
            this.Parameters = parameters;

        }
    }
}