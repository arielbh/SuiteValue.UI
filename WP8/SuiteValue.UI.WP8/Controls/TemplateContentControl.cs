using System.Windows;
using System.Windows.Controls;

namespace SuiteValue.UI.WP8.Controls
{
    public class TemplateContentControl : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent is UIElement || ContentTemplate != null || newContent == null) return;

            string key = newContent.GetType().Name;
            if (!string.IsNullOrEmpty(Suffix))
            {
                key = key + Suffix;
            }
            ContentTemplate = (DataTemplate)Application.Current.Resources[key];
        }

        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Suffix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(TemplateContentControl), new PropertyMetadata(string.Empty));
    }
}
