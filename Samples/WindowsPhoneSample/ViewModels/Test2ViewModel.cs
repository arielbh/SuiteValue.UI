using System.Collections.Generic;
using System.Windows.Navigation;
using SuiteValue.UI.MVVM;
using SuiteValue.UI.WP8;

namespace WindowsPhoneSample.ViewModels
{
    public class Test2ViewModel : NavigationViewModelBase
    {
        protected override void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
            base.OnNavigatedTo(mode, parameter, isNavigationInitiator);
            State = "Test2: Navigated " + State;
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
                                                         State = "Really";
//                                                         NavigateBack();
                                                         NavigateBackTo(Payload, new Dictionary<string, string>() {{"Test", "Test"}});

                                                         //var vm = new Test3ViewModel();
                                                         //vm.State = "YoYo";
                                                         //vm.Payload = Payload;
                                                         //Navigate(vm);
                                                     }));
            }
        }

        public TestViewModel Payload { get; set; }
    }

    public class Test3ViewModel : NavigationViewModelBase
    {
        protected override void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator)
        {
            base.OnNavigatedTo(mode, parameter, isNavigationInitiator);
            State = "Test2: Navigated " + State;
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
                                                         NavigateBackTo(Payload);
                                                     }));
            }
        }

        public TestViewModel Payload { get; set; }
    }

}