using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace CodeValue.CodeLight.Mvvm.Behaviors
{
    public class NumbersOnlyBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += (o, e) =>
            {
                e.Handled = (e.Key < Key.D0 || e.Key > Key.D9);
                if (e.Handled)
                {
                    if ((e.Key >= Key.NumPad0) && (e.Key <= Key.NumPad9))
                    {
                        e.Handled = false;

                    }
                }
                if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                {
                    e.Handled = false;
                }
            };
            AssociatedObject.TextChanged += (o, e) =>
            {
                if (string.IsNullOrEmpty(AssociatedObject.Text) || AssociatedObject.Text == "-")
                {
                    Number = null;
                }

                else Number = int.Parse(AssociatedObject.Text);
            };
        }

        public int? Number
        {
            get { return (int?)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int?), typeof(NumbersOnlyBehavior), new PropertyMetadata(null));

        



        
    }
}
