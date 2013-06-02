using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using SuiteValue.UI.WP8.Behaviors;

namespace SuiteValue.UI.WP8.Controls
{
    public class ExpanderButton : ToggleButton
    {
        public ExpanderButton()
        {
            Checked += (o,e) => HandleChange();
            Unchecked += (o, e) => HandleChange();
            //ToggleButton.IsCheckedProperty.RegisterForNotification("IsChecked", this, (o, e) => HandleChange());
        }
        

        void HandleChange()
        {

             if (Target != null)
            {
                //AnimateToHight(Target as FrameworkElement, IsChecked.Value);

                if (IsChecked != null)
                {
                    Target.Visibility = IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
                }
                else Target.Visibility = Visibility.Collapsed;
                //if (Target is FrameworkElement && Target.Visibility == Visibility.Collapsed)
                //{
                //    AnimateToHight(Target as FrameworkElement, IsChecked);
                //}
                //if (Target is FrameworkElement && Target.Visibility == Visibility.Visible)
                //{
                //    AnimateToHight(Target as FrameworkElement, IsChecked);
                //}
            }

        }

        //private void AnimateToHight(FrameworkElement target, bool isExpand)
        //{
        //    Storyboard storyboard = new Storyboard();
        //    DoubleAnimation animation = new DoubleAnimation();
        //    if (!isExpand)
        //    {
        //        animation.From = target.ActualHeight;
        //        animation.To = 0;
        //    }
        //    else
        //    {
        //        animation.To = target.ActualHeight;
        //        animation.From = 0;  
        //    }
        //    animation.Duration = new Duration(new TimeSpan(0,0,1));
        //    Storyboard.SetTarget(animation, target);
        //    Storyboard.SetTargetProperty( animation,new PropertyPath("Height"));
        //    storyboard.Children.Add(animation);
        //    storyboard.Begin();
        //}


        public UIElement Target
        {
            get { return (UIElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(UIElement), typeof(ExpanderButton), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExpanderButton).HandleChange();
        }
    }
}