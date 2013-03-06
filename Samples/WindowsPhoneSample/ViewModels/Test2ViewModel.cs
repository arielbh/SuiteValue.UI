using SuiteValue.UI.WP8;

namespace WindowsPhoneSample.ViewModels
{
    public class Test2ViewModel : NavigationViewModelBase
    {
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationMode mode, System.Collections.Generic.IDictionary<string, string> parameter)
        {
            base.OnNavigatedTo(mode, parameter);
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