using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace SuiteValue.UI.WP8.Behaviors
{
    public class WebBrowserSourceBehavior : Behavior<WebBrowser>
    {

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(WebBrowserSourceBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WebBrowserSourceBehavior).AssociatedObject.Navigate((Uri)e.NewValue);
        }
    }
}