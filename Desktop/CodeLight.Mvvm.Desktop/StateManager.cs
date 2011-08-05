using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CodeValue.CodeLight.Mvvm
{
    public class StateManager : DependencyObject
    {
        public static string GetVisualStateProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(VisualStatePropertyProperty);
        }

        public static void SetVisualStateProperty(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStatePropertyProperty, value);
        }

        public static readonly DependencyProperty VisualStatePropertyProperty =
            DependencyProperty.RegisterAttached(
            "VisualStateProperty",
            typeof(string),
            typeof(StateManager),
            new PropertyMetadata((s, e) =>
            {
#if SILVERLIGHT
                var propertyName = (string)e.NewValue;
                var ctrl = s as Control;
                if (ctrl == null)
                    throw new InvalidOperationException("This attached property only supports types derived from Control.");
                if (e.NewValue == null) return;
                VisualStateManager.GoToState(ctrl, (string)e.NewValue, true);
#else
                var propertyName = (string)e.NewValue;
                var ctrl = s as FrameworkElement;
                if (ctrl == null)
                    throw new InvalidOperationException("This attached property only supports types derived from Control.");
                if (e.NewValue == null) return;
                var result = VisualStateManager.GoToState(ctrl, (string)e.NewValue, true);
                if (!result)
                {
                    var result2 = VisualStateManager.GoToElementState(ctrl, (string)e.NewValue, true);
                    Debug.WriteLine(result2);
                }

#endif

            }));
    }
}
