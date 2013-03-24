using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Net.NetworkInformation;
using SuiteValue.UI.MVVM;
using Windows.Networking.Connectivity;

namespace SuiteValue.UI.WP8
{
    public class AsyncViewModelBase : AwareViewModelBase, IAsyncViewModel
    {
        private static bool DoWeHaveInternetConnection()
        {
            return DeviceNetworkInformation.IsNetworkAvailable;
        }

        protected void VerifyConnection(Action success, Action fail = null)
        {
            if (DoWeHaveInternetConnection())
            {
                success();
                return;
            }
            if (fail != null) fail();
        }

        protected T VerifyConnection<T>(Func<T> success, Func<T> fail = null)
        {
            if (DoWeHaveInternetConnection())
            {
                return success();
            }
            if (fail != null) return fail();
            return default(T);
        }

        protected async Task<T> VerifyConnection<T>(Func<Task<T>> success, Func<Task<T>> fail = null)
        {
            if (DoWeHaveInternetConnection())
            {
                return await success();
            }
            if (fail != null) return await fail();
            return default(T);
        }


        private Visibility _showMainContent = Visibility.Visible;

        public Visibility ShowMainContent
        {
            get { return _showMainContent; }
            set
            {
                if (value != _showMainContent)
                {
                    _showMainContent = value;
                    OnPropertyChanged(() => ShowMainContent);
                }
            }
        }

        private Visibility _showProgressBar = Visibility.Collapsed;

        public Visibility ShowProgressBar
        {
            get { return _showProgressBar; }
            set
            {
                if (value != _showProgressBar)
                {
                    _showProgressBar = value;
                    OnPropertyChanged(() => ShowProgressBar);
                }
            }
        }

        private bool _isInAsync;

        public bool IsInAsync
        {
            get { return _isInAsync; }
            set
            {
                if (value != _isInAsync)
                {
                    _isInAsync = value;
                    ShowProgressBar = IsInAsync ? Visibility.Visible : Visibility.Collapsed;
                    ShowMainContent = IsInAsync ? Visibility.Collapsed : Visibility.Visible;
                    OnPropertyChanged(() => IsInAsync);
                }
            }
        }

        private string _asyncMessage;

        public string AsyncMessage
        {
            get { return _asyncMessage; }
            set
            {
                if (value != _asyncMessage)
                {
                    _asyncMessage = value;
                    OnPropertyChanged(() => AsyncMessage);
                }
            }
        }

        private bool _hasError;

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                if (value != _hasError)
                {
                    _hasError = value;
                    OnPropertyChanged(() => HasError);
                }
            }
        }
    }

    public interface IAsyncViewModel
    {
        bool IsInAsync { get; set; }
        string AsyncMessage { get; set; }
    }
}