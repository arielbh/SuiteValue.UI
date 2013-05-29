﻿using System;
using System.Windows;
using SuiteValue.UI.WP8.Behaviors;

namespace SuiteValue.UI.WP8.Controls
{
    public class SelectionContentControl :TemplateContentControl
    {
        public SelectionContentControl()
        {
            ContentTemplateProperty.RegisterForNotification("ContentTemplate", this, (o, e) => Register(o, e));
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (newContent is UIElement) return;
            if (newContent == null)
            {
                ContentTemplate = null;
                return;
            }
            if (oldContent == null && SelectedTemplate != null) return;
            var key = GetKey(newContent);
            SelectedTemplate = (DataTemplate)Application.Current.Resources[key];
        }

        private string GetKey(object newContent)
        {
            return newContent.GetType().Name + "Selected";
        }

        private void Register(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == SelectedTemplate) return;
            _UnSelectedTemplate = e.NewValue as DataTemplate;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _UnSelectedTemplate = ContentTemplate;
        }


        public DataTemplate SelectedTemplate
        {
            get { return (DataTemplate)GetValue(SelectedTemplateProperty); }
            set { SetValue(SelectedTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTemplateProperty =
            DependencyProperty.Register("SelectedTemplate", typeof(DataTemplate), typeof(SelectionContentControl), new PropertyMetadata(null));


        public void Selected(bool newValue)
        {
            if (newValue)
            {
                ContentTemplate = SelectedTemplate;
            }
            else
            {
                ContentTemplate = _UnSelectedTemplate;
            }
            IsSelected = newValue;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SelectionContentControl), new PropertyMetadata(false, PropertyChangedCallback));

        private DataTemplate _UnSelectedTemplate;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SelectionContentControl).Selected((bool)e.NewValue);
            
        }
    }
}