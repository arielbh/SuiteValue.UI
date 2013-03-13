using System.Collections.Generic;
using System.Windows.Navigation;
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
                                                         Test2ViewModel vm = new Test2ViewModel();
                                                         vm.State = "YoYo";
                                                         Navigate<Test2ViewModel>(vm);
                                                     }));
            }
        }
            
       

         
    }
}