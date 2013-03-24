using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace SuiteValue.UI.WP8.Behaviors
{
    public class WebBrowserSourceBehavior : Behavior<WebBrowser>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Navigating += AssociatedObjectNavigating;
        }

        private void AssociatedObjectNavigating(object sender, NavigatingEventArgs e)
        {
            if (NavigatingAction != null)
            {
                e.Cancel = NavigatingAction(e.Uri);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Navigating -= AssociatedObjectNavigating;

        }

        public Uri Source
        {
            get { return (Uri) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof (Uri), typeof (WebBrowserSourceBehavior),
                                        new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WebBrowserSourceBehavior).AssociatedObject.Navigate((Uri) e.NewValue);
        }



        public Func<Uri, bool> NavigatingAction
        {
            get { return (Func<Uri, bool>) GetValue(NavigatingActionProperty); }
            set { SetValue(NavigatingActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigatingAction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigatingActionProperty =
            DependencyProperty.Register("NavigatingAction", typeof (Func<Uri, bool>), typeof (WebBrowserSourceBehavior),
                                        new PropertyMetadata(null));


        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof (string), typeof (WebBrowserSourceBehavior), new PropertyMetadata(OnHtmlChanged));

        public static string GetHtml(DependencyObject dependencyObject)
        {
            return (string) dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var html = e.NewValue.ToString();
            (d as WebBrowserSourceBehavior).AssociatedObject.NavigateToString(html);
        }
    }
} 
    
