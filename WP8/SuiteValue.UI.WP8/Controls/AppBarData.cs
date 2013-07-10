using System;
using System.Windows.Input;
using SuiteValue.UI.MVVM;

namespace SuiteValue.UI.WP8.Controls
{
    public class AppBarData : NotifyObject
    {
        private ICommand _command;

        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (value != _command)
                {
                    _command = value;
                    OnPropertyChanged(() => Command);
                }
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged(() => Text);
                }
            }
        }

        private Uri _iconUri;

        public Uri IconUri
        {
            get { return _iconUri; }
            set
            {
                if (value != _iconUri)
                {
                    _iconUri = value;
                    OnPropertyChanged(() => IconUri);
                }
            }
        }

        private bool _isVisible = true;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    OnPropertyChanged(() => IsVisible);
                }
            }
        }

        private object _commandParameter;

        public object CommandParameter
        {
            get { return _commandParameter; }
            set
            {
                if (value != _commandParameter)
                {
                    _commandParameter = value;
                    OnPropertyChanged(() => CommandParameter);
                }
            }
        }
    }
}