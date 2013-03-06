using SuiteValue.UI.MVVM;

namespace WpfTester
{
    public class MainViewModel : ViewModelBase<string>
    {
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                Name = "Design";
            }
            else
            {
                Name = "Runtime";
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }
        
    }
}