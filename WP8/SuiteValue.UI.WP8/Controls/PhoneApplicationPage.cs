using System.Windows;

namespace SuiteValue.UI.WP8.Controls
{
    public static class PhoneApplicationPage
    {
        public static ApplicationBar GetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj)
        {
            return (ApplicationBar)obj.GetValue(ApplicationBarProperty);
        }

        public static void SetApplicationBar(Microsoft.Phone.Controls.PhoneApplicationPage obj, ApplicationBar value)
        {
            obj.SetValue(ApplicationBarProperty, value);
        }

        // Using a DependencyProperty as the backing store for ApplicationBar.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty ApplicationBarProperty =
            DependencyProperty.RegisterAttached(
                "ApplicationBar",
                typeof(ApplicationBar),
                typeof(PhoneApplicationPage),
                new PropertyMetadata(null, ApplicationBarPropertyChanged));

        private static void ApplicationBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Microsoft.Phone.Controls.PhoneApplicationPage;
            if (e.NewValue != null)
            {                
                var appBar = e.NewValue as ApplicationBar;
                page.ApplicationBar = appBar.SysAppBar;
            }
            else
            {

            }
        }
    }
}
