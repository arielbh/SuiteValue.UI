using System.Windows;
using System.Windows.Controls;
using SuiteValue.UI.WP8.Extensions;

namespace SuiteValue.UI.WP8.Controls
{
    public class TemplateContentControl : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent is UIElement) return;
            if (newContent == null)
            {
                ContentTemplate = null;
                return;
            }
            if (oldContent == null && ContentTemplate != null) return;
            if (ContentTemplateSelector != null)
            {
                ContentTemplate = ContentTemplateSelector.SelectTemplate(newContent, this);
                return;
            }

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



        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTemplateSelectorProperty =
            DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(TemplateContentControl), new PropertyMetadata(null));






    }
}
