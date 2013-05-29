using System;
using Microsoft.Phone.Controls;
using SuiteValue.UI.MVVM;

namespace WindowsPhoneSample.ViewModels
{
    public class PartViewModel : ViewModelBase
    {
        public PartViewModel()
        {
            Callback = HandleOrientation;
        }
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
         
    }
}