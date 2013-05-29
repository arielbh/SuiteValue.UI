using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

        void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

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
                if (c.Content == item)
                {
                    c.Selected(true);
                }
                else
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
    }


}