using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SuiteValue.UI.WPF.Behaviors
{
    public class TextBoxCarretPositionBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
#if SILVERLIGHT
            AssociatedObject.KeyDown += (o, e) =>
                                            {

                                            };
#else
            AssociatedObject.PreviewMouseLeftButtonDown += (o, e) => CaretPosition = AssociatedObject.CaretIndex;
            AssociatedObject.PreviewKeyDown += (o, e) =>
            {
                if (((int)e.Key >= 34 && (int)e.Key <= 69) || (e.Key == Key.Space))
                {
                    CaretPosition = AssociatedObject.CaretIndex + 1;
                    return;
                }
                if (e.Key == Key.Back)
                {
                    CaretPosition = Math.Max(0, AssociatedObject.CaretIndex - 1);
                    return;
                }
                CaretPosition = AssociatedObject.CaretIndex;
            };
            AssociatedObject.PreviewTouchDown += (o, e) => CaretPosition = AssociatedObject.CaretIndex;

#endif
        }



        public int CaretPosition
        {
            get { return (int)GetValue(CaretPositionProperty); }
            set { SetValue(CaretPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaretPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaretPositionProperty =
            DependencyProperty.Register("CaretPosition", typeof(int), typeof(TextBoxCarretPositionBehavior), new PropertyMetadata(0));

    }
}
