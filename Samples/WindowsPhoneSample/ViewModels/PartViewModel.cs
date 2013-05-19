using SuiteValue.UI.MVVM;

namespace WindowsPhoneSample.ViewModels
{
    public class PartViewModel : ViewModelBase
    {
        private string _status = "I'm OK";

        public string Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(() => Status);
                }
            }
        }
         
    }
}