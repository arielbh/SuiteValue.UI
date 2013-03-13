using System.Collections.Generic;
using System.Windows.Navigation;
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
    }
}