using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace SuiteValue.UI.WPF.Behaviors
{
    public class CommandVisibilityBehavior : Behavior<ButtonBase>
    {
        
  
        protected override void OnAttached()
        {
            base.OnAttached();
#if SILVERLIGHT
            Register(null, null);
            ButtonBase.CommandProperty.RegisterForNotification("Command", AssociatedObject, (o,e) => Register(o, null));
#else

            DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(ButtonBase.CommandProperty, typeof(ButtonBase));
            prop.AddValueChanged(AssociatedObject, Register );
#endif


        }

        private void Register(object sender, EventArgs eventArgs)
        {
            if (AssociatedObject.Command != null)
            {
                UpdateVisibility(AssociatedObject.Command.CanExecute(AssociatedObject.CommandParameter));
                AssociatedObject.Command.CanExecuteChanged += new EventHandler(Command_CanExecuteChanged);
                
            }
        }

        void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility(AssociatedObject.Command.CanExecute(AssociatedObject.CommandParameter));
        }

        public void UpdateVisibility(bool canExecute)
        {
            AssociatedObject.Visibility = canExecute
                          ? Visibility.Visible
                          : Visibility.Collapsed;
        }


    }

    public static class DependencyPropertyExtensions
    {
        /// Listen for change of the dependency property  
        public static void RegisterForNotification(this DependencyProperty dependencyProperty, string propertyName, FrameworkElement element, PropertyChangedCallback callback)
        {
            //Bind to a depedency property  
            Binding b = new Binding(propertyName) { Source = element };
            var prop = System.Windows.DependencyProperty.RegisterAttached(
                "ListenAttached" + propertyName,
                typeof(object),
                typeof(UserControl),
                new System.Windows.PropertyMetadata(callback));

            element.SetBinding(prop, b);
        }

    }
 
}