using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace CodeValue.CodeLight.Prism.Localization
{
    /// <summary>
    /// Helper class for binding to resource strings
    /// </summary>
    public class LocalizationHelper : INotifyPropertyChanged
    {
        private IResourceManagerService _resourceManager;
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the LocalisationHelper class.
        /// </summary>
        public LocalizationHelper()
        {
#if SILVERLIGHT
             _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
            var prop = DesignerProperties.IsInDesignModeProperty;
            _isInDesignMode
                = (bool)DependencyPropertyDescriptor
                             .FromProperty(prop, typeof(FrameworkElement))
                             .Metadata.DefaultValue;

            // Just to be sure
            if (!_isInDesignMode
                && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
            {
                _isInDesignMode = true;
            }
#endif

            if (!_isInDesignMode)
            {
                // Get the services we need from the service locator
                // We can't use ctor injection as we are XAML created
                _resourceManager = ServiceLocator.Current.GetInstance<IResourceManagerService>();
                _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

                // Refresh all the bindings when the locale changes
                _eventAggregator.GetEvent<LocaleChangedEvent>().Subscribe(OnLocaleChange);
            }
        }

        public void OnLocaleChange(Locale e)
        {
#if SILVERLIGHT
            RaisePropertyChanged("Values");
#else
            RaisePropertyChanged(string.Empty);
#endif

        }
#if SILVERLIGHT


        /// <summary>
        /// Gets a resource string from the ResourceManager
        /// 
        /// You can bind to this property using the Values[KEY] syntax e.g.:
        /// 
        /// {Binding Source={StaticResource local}, Path=Values[KEY]
        /// </summary>
        public Dictionary<string, string> Values { get { return _resourceManager.Managers[_managerName]; } }
#else
        /// <summary>
        /// Gets a resource string from the ResourceManager
        /// 
        /// You can bind to this property using the .[KEY] syntax e.g.:
        /// 
        /// {Binding Source={StaticResource localisation}, Path=.[MainScreenResources.IntroTextLine1]}
        /// </summary>
        /// <param name="Key">Key to retrieve in the format [ManagerName].[ResourceKey]</param>
        /// <returns></returns>
        public string this[string Key]
        {
            get
            {
                if (!ValidKey(Key))
                    throw new ArgumentException(@"Key is not in the valid [ManagerName].[ResourceKey] format");

                if (_isInDesignMode)
                    throw new Exception("Design mode is not supported");

                return _resourceManager.GetResourceString(GetManagerKey(Key), GetResourceKey(Key));
            }
        }
#endif

#if SILVERLIGHT
            public string ManagerName
        {
            get { return _managerName; }
            set
            {
                _managerName = value;
                if (!_isInDesignMode)
                {
                    //Values = _resourceManager.Managers[_managerName];
                }
            }
        }
#else
        #region Private Key Methods
        private bool ValidKey(string input)
        {
            return input.Contains(".");
        }

        private string GetManagerKey(string input)
        {
            return input.Split('.')[0];
        }

        private string GetResourceKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }
        #endregion
#endif


        private string _managerName;
        private bool _isInDesignMode;

    
        protected void RaisePropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;

            if (evt != null)
                evt.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
