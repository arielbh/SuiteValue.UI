using System;
using System.Windows;
using System.Windows.Input;

namespace SuiteValue.UI.WP8.Controls
{
    public class ApplicationBarMenuItem : FrameworkElement, Microsoft.Phone.Shell.IApplicationBarMenuItem
    {
        #region Fields

        private readonly Microsoft.Phone.Shell.IApplicationBarMenuItem _sysAppBarMenuItem;
        private bool _isAttached;        

        #endregion

        #region Ctor

        public ApplicationBarMenuItem()
        {
            _sysAppBarMenuItem = CreateApplicationBarMenuItem();
            _sysAppBarMenuItem.Click += sysAppBarMenuItem_Click;
        }

        #endregion

        #region Properties

        internal bool IsAttached
        {
            get { return _isAttached; }
        }

        protected Microsoft.Phone.Shell.IApplicationBarMenuItem SysAppBarMenuItem
        {
            get { return _sysAppBarMenuItem; }
        }

        #endregion

        #region Dependency Properties

        #region IsEnabled
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(
                "IsEnabled",
                typeof(bool),
                typeof(ApplicationBarMenuItem),
                new PropertyMetadata(true, (d, e) => ((ApplicationBarMenuItem)d).IsEnabledChanged((bool)e.NewValue)));

        private void IsEnabledChanged(bool isEnabled)
        {
            SysAppBarMenuItem.IsEnabled = isEnabled;
        }
        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ApplicationBarMenuItem),
                new PropertyMetadata(default(string), (d, e) => ((ApplicationBarMenuItem)d).TextChanged((string)e.NewValue)));

        private void TextChanged(string text)
        {
            SysAppBarMenuItem.Text = text;
        }
        #endregion

        #region Command
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ApplicationBarMenuItem),
                new PropertyMetadata(default(ICommand), (d, e) => ((ApplicationBarMenuItem)d).CommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue)));

        private void CommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= command_CanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += command_CanExecuteChanged;                
            }
        }

        private void command_CanExecuteChanged(object sender, EventArgs e)
        {
            var command = sender as ICommand;
            IsEnabled = command.CanExecute(CommandParameter);
        }
        #endregion

        #region CommandParameter
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(ApplicationBarMenuItem),
                new PropertyMetadata(null, (d, e) => ((ApplicationBarMenuItem)d).CommandParameterChanged(e.OldValue, e.NewValue)));

        private void CommandParameterChanged(object oldParameter, object newParameter)
        {
        }
        #endregion

        #endregion

        #region Internals
        internal void Attach(Microsoft.Phone.Shell.IApplicationBar sysAppBar)
        {
            if (!_isAttached)
            {
                OnAttach(sysAppBar);
                _isAttached = true;

                if (Command != null)
                {
                    IsEnabled = Command.CanExecute(CommandParameter);
                }
            }
        }

        internal void Dettach(Microsoft.Phone.Shell.IApplicationBar sysAppBar)
        {
            if (_isAttached)
            {
                OnDettach(sysAppBar);
                _isAttached = false;
            }
        }
        #endregion

        #region Events
        public event EventHandler Click
        {
            add { _sysAppBarMenuItem.Click += value; }
            remove { _sysAppBarMenuItem.Click -= value; }
        }
        #endregion

        #region Event Handlers

        private void sysAppBarMenuItem_Click(object sender, EventArgs e)
        {
            if (Command != null)
            {
                bool canExecute = Command.CanExecute(CommandParameter);
                IsEnabled = canExecute;
                if (canExecute)
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        #endregion

        #region Virtuals

        protected virtual Microsoft.Phone.Shell.IApplicationBarMenuItem CreateApplicationBarMenuItem()
        {
            return new Microsoft.Phone.Shell.ApplicationBarMenuItem();
        }

        protected virtual void OnAttach(Microsoft.Phone.Shell.IApplicationBar sysAppBar)
        {
            sysAppBar.MenuItems.Add(_sysAppBarMenuItem);
        }

        protected virtual void OnDettach(Microsoft.Phone.Shell.IApplicationBar sysAppBar)
        {
            sysAppBar.MenuItems.Remove(_sysAppBarMenuItem);
        }

        #endregion                      
    }
}
