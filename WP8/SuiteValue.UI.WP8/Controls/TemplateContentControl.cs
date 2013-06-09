using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using SuiteValue.UI.WP8.Extensions;
using SuiteValue.UI.WP8.Helpers;

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
            if (ContentTemplate != null && !ForceTemplateChange) return;

            if (ContentTemplateSelector != null)
            {
                ContentTemplate = ContentTemplateSelector.SelectTemplate(newContent, this);
                return;
            }

            var key = GetKey(newContent);
            ContentTemplate = (DataTemplate)Application.Current.Resources[key];
        }

        private string GetKey(object newContent)
        {
            string key = newContent.GetType().Name;
            if (!string.IsNullOrEmpty(Suffix))
            {
                key = key + Suffix;
            }
            return key;
        }

        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Suffix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(TemplateContentControl), new PropertyMetadata(string.Empty, SuffixPropertyChanged));

        private static void SuffixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TemplateContentControl).UpdateWithSuffix();
        }

        private void UpdateWithSuffix()
        {
            if (Content != null)
            {
                var key = GetKey(Content);
                ContentTemplate = (DataTemplate)Application.Current.Resources[key]; 
            }
        }



        public bool ForceTemplateChange
        {
            get { return (bool)GetValue(ForceTemplateChangeProperty); }
            set { SetValue(ForceTemplateChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForceTemplateChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForceTemplateChangeProperty =
            DependencyProperty.Register("ForceTemplateChange", typeof(bool), typeof(TemplateContentControl), new PropertyMetadata(false));




        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTemplateSelectorProperty =
            DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(TemplateContentControl), new PropertyMetadata(null));




        public bool EnableOrientationSupport
        {
            get { return (bool)GetValue(EnableOrientationSupportProperty); }
            set { SetValue(EnableOrientationSupportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableOrientationSupport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableOrientationSupportProperty =
            DependencyProperty.Register("EnableOrientationSupport", typeof(bool), typeof(TemplateContentControl), new PropertyMetadata(false, EnableOrientationSupportChanged));

        private static void EnableOrientationSupportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TemplateContentControl).UpdateEnableOrientationSupport((bool)e.NewValue);
        }

        private void UpdateEnableOrientationSupport(bool isEnabledOrientationSupport)
        {
            OrientationHelper.SetIsActive(this, isEnabledOrientationSupport);
            OrientationHelper.SetPageOrientationCallback(this, HandleOrientationChange);
        }

        private void HandleOrientationChange(PageOrientation pageOrientation)
        {
            if (Content == null) return;
            if (Content is UIElement) return;
            var key = GetKey(Content);
            if (pageOrientation == PageOrientation.Landscape)
            {
                key += "_landscape";
            }
            ContentTemplate = (DataTemplate)Application.Current.Resources[key];
        }

    }
}
