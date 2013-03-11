using System.Windows;
using SuiteValue.UI.MVVM;

namespace SuiteValue.UI.WP8
{
    public class AwareViewModelBase : ViewModelBase
    {
        private FlowDirection _flowDirection;

        public FlowDirection FlowDirection
        {
            get { return _flowDirection; }
            set
            {
                if (value != _flowDirection)
                {
                    _flowDirection = value;
                    OnPropertyChanged(() => FlowDirection);
                }
            }
        }
    }
}