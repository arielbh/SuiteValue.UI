using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace SuiteValue.UI.WP8.Behaviors
{
    public class PivotCleanupBehavior : Behavior<Pivot>
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PivotCleanupBehavior), new PropertyMetadata(OnChanging));

        private static void OnChanging(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PivotCleanupBehavior).UpdateItemsSource((IEnumerable) e.NewValue);
        }

        private void UpdateItemsSource(IEnumerable newItems)
        {
            foreach (var item in AssociatedObject.Items)
            {
                var x = AssociatedObject.ItemContainerGenerator.ContainerFromItem(item) as PivotItem;
                x.DataContext = null;
                x.Header = null;
                x.Content = null;
                x.ContentTemplate = null;
            }
            AssociatedObject.Items.Clear();
            AssociatedObject.ItemsSource = newItems;
        }
    }
}
