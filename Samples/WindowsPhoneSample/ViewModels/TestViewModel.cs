using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using SuiteValue.UI.MVVM;
using SuiteValue.UI.WP8;

namespace WindowsPhoneSample.ViewModels
{
    public class TestViewModel : NavigationViewModelBase
    {

        protected override void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
            base.OnNavigatedTo(mode, parameter, isNavigationInitiator);
            State = "Test1: Navigated";
            Callback = HandleOrientation;
            PartViewModel = new PartViewModel();
        }

        private PartViewModel _partViewModel;

        public PartViewModel PartViewModel
        {
            get { return _partViewModel; }
            set
            {
                if (value != _partViewModel)
                {
                    _partViewModel = value;
                    OnPropertyChanged(() => PartViewModel);
                }
            }
        }

        public void HandleOrientation(PageOrientation pageOrientation)
        {
            
        }

        private Action<PageOrientation> _callback;

        public Action<PageOrientation> Callback
        {
            get { return _callback; }
            set
            {
                if (value != _callback)
                {
                    _callback = value;
                    OnPropertyChanged(() => Callback);
                }
            }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnPropertyChanged(() => State);
                }
            }
        }

        private DelegateCommand _navigateCommand;

        public DelegateCommand NavigateCommand
        {
            get
            {
                return _navigateCommand ?? (_navigateCommand = new DelegateCommand(
                                                     () =>
                                                     {
                                                         //Test2ViewModel vm = new Test2ViewModel();
                                                         //vm.State = "YoYo";
                                                         //vm.Payload = this;
                                                         //Navigate<Test2ViewModel>(vm);
                                                         Navigate(new TestValidationViewModel());
                                                     }));
            }
        }
            
        private DelegateCommand _testAsyncCommand;

        public DelegateCommand TestAsyncCommand
        {
            get
            {
                return _testAsyncCommand ?? (_testAsyncCommand = new DelegateCommand(
                                                     async () =>
                                                     {
                                                         IsInAsync = true;
                                                         AsyncMessage = "Doing important stuff...";
                                                         var result = await Calculate();
                                                         IsInAsync = false;


                                                     }));
            }
        }

        private Task<bool> Calculate()
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();


            Task.Run(() =>
            {
                Thread.Sleep(3000);
                source.SetResult(true);

            });
            return source.Task;
        }




            
       
        private bool _cameraVisibility;
        public bool CameraVisibility
        {
            get { return _cameraVisibility; }
            set
            {
                _cameraVisibility = value;
                OnPropertyChanged(() => CameraVisibility);
            }
        }

        private bool _emailVisibility;
        public bool EmailVisibility
        {
            get { return _emailVisibility; }
            set
            {
                _emailVisibility = value;
                OnPropertyChanged(() => EmailVisibility);
            }
        }
         
        private bool _phoneVisibility;
        public bool PhoneVisibility
        {
            get { return _phoneVisibility; }
            set
            {
                _phoneVisibility = value;
                OnPropertyChanged(() => PhoneVisibility);
            }
        }

        private bool _searchVisibility;
        public bool SearchVisibility
        {
            get { return _searchVisibility; }
            set
            {
                _searchVisibility = value;
                OnPropertyChanged(() => SearchVisibility);
            }
        }
    }
}