using System;
using System.Windows.Input;

namespace SuiteValue.UI.MVVM
{
    public class DelegateCommand<T> : ICommand
    {
        private static bool CanExecute(T parameter) { return true; }

        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute ?? CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Refresh()
        {
            var pc = CanExecuteChanged;
            if (pc != null)
                pc(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
        //add {
        //    if (_canExecute != null)
        //        CommandManager.RequerySuggested += value;
        //}
        //remove {
        //    if (_canExecute != null)
        //        CommandManager.RequerySuggested -= value;
        //}

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

    }


}