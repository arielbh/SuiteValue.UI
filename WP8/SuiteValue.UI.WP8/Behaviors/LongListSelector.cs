using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SuiteValue.UI.WP8.Controls;

namespace SuiteValue.UI.WP8.Behaviors
{
    public class LongListSelector : Microsoft.Phone.Controls.LongListSelector
    {
        public LongListSelector()
        {
            SelectionChanged += LongListSelector_SelectionChanged;
            
        }

        //void LongListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    var presenter = GetFirstItemRecursive<ContentPresenter>((DependencyObject) e.OriginalSource);
        //    if (presenter == null)
        //    {
        //        presenter = FindParent<ContentPresenter>((FrameworkElement) e.OriginalSource);
        //    }
        //    if (presenter != null)
        //    {
        //        TappedItem = presenter.DataContext;

        //        BindingExpression expression = GetBindingExpression(TappedItemProperty);
        //        if (expression != null)
        //        {
        //            expression.UpdateSource();
        //        }
        //    }
        //}

        void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

        public object TappedItem
        {
            get { return (object)GetValue(TappedItemProperty); }
            set { SetValue(TappedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TappedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TappedItemProperty =
            DependencyProperty.Register("TappedItem", typeof(object), typeof(LongListSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(LongListSelector),
                new PropertyMetadata(null, OnSelectedItemChanged)
            );

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SelectedItem = e.NewValue;
            GetItemsRecursive(selector, e.NewValue);
 
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        public static void GetItemsRecursive(DependencyObject lb, object item)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(lb);
            var c = lb as SelectionContentControl;
            if (c != null)
            {
                if (c.Content != item)
                {
                    c.Selected(false);
                }
            }
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(lb, i);
                Debug.WriteLine(child.ToString());
                GetItemsRecursive(child, item);
            }
        }

        public static T FindParent<T>(FrameworkElement lb) where T : class
        {
            if (lb is T) return lb as T;
            DependencyObject parent = lb.Parent;
            while (parent != null)
            {
                if (parent is T) return parent as T;
                if (!(parent is FrameworkElement))
                {
                    return null;
                }
                parent = (parent as FrameworkElement).Parent;
            }
            return null;
        }

        public static T GetFirstItemRecursive<T>(DependencyObject lb) where T : class 
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(lb);
         
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(lb, i);
                if (child is T)
                {
                    return child as T;
                }
                Debug.WriteLine(child.ToString());
                var res = GetFirstItemRecursive<T>(child);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
    }


}