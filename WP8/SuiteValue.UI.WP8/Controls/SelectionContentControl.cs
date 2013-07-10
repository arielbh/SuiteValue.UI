using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using SuiteValue.UI.WP8.Behaviors;

namespace SuiteValue.UI.WP8.Controls
{
    public class SelectionContentControl :TemplateContentControl
    {
        public SelectionContentControl()
        {
            ContentTemplateProperty.RegisterForNotification("ContentTemplate", this, (o, e) => Register(o, e));
            Tap += SelectionContentControl_Tap;
            SizeChanged += SelectionContentControl_SizeChanged;
        }

        

        void SelectionContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            child = VisualTreeHelper.GetChild(this, 0);
        }



        void SelectionContentControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = DataContext;

            if (TappedItem == item)
            {
                BindingExpression expression = GetBindingExpression(TappedItemProperty);
                if (expression != null)
                {
                    expression.UpdateSource();
                }
            }
            _SelfUpdate = true;
            try
            {
                TappedItem = item;
            }
            finally
            {
                _SelfUpdate = false;
            }
            Selected(!IsSelected);
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

            if (SelectedTemplate == null)
            {
                var key = GetKey(newContent);
                SelectedTemplate = (DataTemplate) Application.Current.Resources[key];
                if (SelectedTemplate == null) // No SelectedTemplate to be find
                {
                    SelectedTemplate = ContentTemplate;
                }
            }
            if (newContent == TappedItem)
            {
                Selected(true);
            }

        }

        private string GetKey(object newContent)
        {
            return newContent.GetType().Name + "Selected";
        }

        private void Register(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (EnableVisibilityChangeInLongListSelector) ToggleVisibility();
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
            if (_SelfUpdate) return;
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

        private void ToggleVisibility()
        {
            SizeChanged += g_SizeChanged;
            var uiElement = child as UIElement;
            if (uiElement != null)
                uiElement.Visibility = uiElement.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        void g_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeChanged -= g_SizeChanged;
            SizeChanged += g_SizeChanged2;
            var uiElement = child as UIElement;
            if (uiElement != null)
                uiElement.Visibility = uiElement.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        void g_SizeChanged2(object sender, SizeChangedEventArgs e)
        {
            SizeChanged -= g_SizeChanged2;
            SizeChanged += g_SizeChanged3;

            var uiElement = child as UIElement;
            if (uiElement != null)
                uiElement.Visibility = uiElement.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        void g_SizeChanged3(object sender, SizeChangedEventArgs e)
        {
            SizeChanged -= g_SizeChanged3;
            var uiElement = child as UIElement;
            if (uiElement != null)
                uiElement.Visibility = uiElement.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
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



        public object TappedItem
        {
            get { return (object)GetValue(TappedItemProperty); }
            set { SetValue(TappedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TappedTappedItemObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TappedItemProperty =
            DependencyProperty.Register("TappedItem", typeof(object), typeof(SelectionContentControl), new PropertyMetadata(null, TappedItemChanged));

        private static void TappedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue != null)
            {
                if ((d as SelectionContentControl).Content == e.NewValue)
                {
                    (d as SelectionContentControl).Selected(true);

                }
            }
        }

        private DependencyObject child;



        public bool EnableVisibilityChangeInLongListSelector
        {
            get { return (bool)GetValue(EnableVisibilityChangeInLongListSelectorProperty); }
            set { SetValue(EnableVisibilityChangeInLongListSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableVisibilityChangeInLongListSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableVisibilityChangeInLongListSelectorProperty =
            DependencyProperty.Register("EnableVisibilityChangeInLongListSelector", typeof(bool), typeof(SelectionContentControl), new PropertyMetadata(false));

        private bool _SelfUpdate;
    }
}