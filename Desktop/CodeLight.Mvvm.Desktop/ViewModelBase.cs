using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using CodeValue.CodeLight.Mvvm.Validation;

namespace CodeValue.CodeLight.Mvvm
{
    public abstract class ViewModelBase : ValidatingObject
    {
        private object _model;
        private static bool? _isInDesignMode;

        public virtual object Model
        {
            get { return _model; }
            set
            {
                _model = value;
                ModelChanged();
            }
        }

        protected virtual void ModelChanged()
        {
            OnPropertyChanged("Model");
        }
        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running under Blend or Visual Studio).
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
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
                    if (!_isInDesignMode.Value
                        && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        _isInDesignMode = true;
                    }
#endif
                }

                return _isInDesignMode.Value;
            }
        }

    }

    public class ViewModelBase<T> : ViewModelBase
    {
        public new virtual T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }
}
