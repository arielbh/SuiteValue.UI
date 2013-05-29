using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            //var container = GetItemsRecursive(selector, e.NewValue);
            //if (container is ContentPresenter)
            //{
            //    VisualStateManager.GoToState((container as ContentPresenter).Content as Control, "Selected", true);
                
            //}
            //else
            //{
            //    VisualStateManager.GoToState((Control)container, "Selected", true);
                
            //}
            //List<CustomUserControl> userControlList = new List<CustomUserControl>();
            //GetItemsRecursive<CustomUserControl>(MyLongListSelector, ref userControlList);

            //if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            //{
            //    foreach (CustomUserControl userControl in userControlList)
            //    {
            //        if (e.AddedItems[0].Equals(userControl.DataContext))
            //        {
            //            VisualStateManager.GoToState(userControl, "Selected", true);
            //        }
            //    }
            //}

            //if (e.RemovedItems.Count > 0 && e.RemovedItems[0] != null)
            //{
            //    foreach (CustomUserControl userControl in userControlList)
            //    {
            //        if (e.RemovedItems[0].Equals(userControl.DataContext))
            //        {
            //            VisualStateManager.GoToState(userControl, "Normal", true);
            //        }
            //    }

            //}
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        public static DependencyObject GetItemsRecursive(DependencyObject lb, object item)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(lb);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(lb, i);
                var frameworkElement = child as FrameworkElement;
                if (frameworkElement != null && frameworkElement.DataContext == item)
                {
                    return frameworkElement as DependencyObject;
                }


                return GetItemsRecursive(child, item);
            }

            return null;
        }
    }


}