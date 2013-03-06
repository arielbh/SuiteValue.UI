using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Windows.Markup;
using System.Windows.Data;

namespace SuiteValue.UI.WP8.Controls
{
    [ContentProperty("Buttons")]
    public class ApplicationBar : DependencyObject, Microsoft.Phone.Shell.IApplicationBar
    {
        #region Fields

        private readonly ApplicationBarIconButtonCollection _buttons;
        private readonly ApplicationBarMenuItemCollection _menuItems;
        private readonly Microsoft.Phone.Shell.IApplicationBar _sysAppBar;

        #endregion

        #region Ctor

        public ApplicationBar()
        {
            _buttons = new ApplicationBarIconButtonCollection();
            _menuItems = new ApplicationBarMenuItemCollection();
            _sysAppBar = new Microsoft.Phone.Shell.ApplicationBar();

            // Bind artificial DataContext property with DataContext from visual tree so non-visual children will have the same DataContext.
            // This is to compansate the lack of inheritance-context in Silverlight.
            BindingOperations.SetBinding(this, ApplicationBar.DataContextProperty, new Binding());                        
        }

        #endregion

        #region Dependency Properties



        public FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty); }
            set { SetValue(FlowDirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FlowDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlowDirectionProperty =
            DependencyProperty.Register("FlowDirection", typeof(FlowDirection), typeof(ApplicationBar), new PropertyMetadata(FlowDirection.LeftToRight, FlowDirectionChanged));

        private static void FlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ApplicationBar bar = d as ApplicationBar;
            var newButtons = (e.NewValue as IEnumerable);
            if (newButtons != null)
            {
                newButtons = newButtons.Cast<ApplicationBarIconButton>().ToList();
            }
            bar.UpdateItemsSource(newButtons as List<ApplicationBarIconButton>, (FlowDirection)e.NewValue);
        }

        private void UpdateItemsSource(List<ApplicationBarIconButton> buttons, FlowDirection flowDirection)
        {
            if (buttons != null)
            {
                if (buttons.Count > 0)
                {
                    if (flowDirection == FlowDirection.RightToLeft)
                        buttons.Reverse();
                    Buttons.Dettach(SysAppBar);
                    Buttons.Clear();
                    foreach (ApplicationBarIconButton button in buttons)
                    {
                        Buttons.Add(button);
                    }
                    Buttons.Attach(DataContext, SysAppBar);
                }
            }
            else
            {
                Buttons.Dettach(SysAppBar);
                Buttons.Clear();
                Buttons.Attach(DataContext, SysAppBar);
            }
        }


        public object MenuSource
        {
            get { return (object)GetValue(MenuSourceProperty); }
            set { SetValue(MenuSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuSourceProperty =
            DependencyProperty.Register("MenuSource", typeof(object), typeof(ApplicationBar), new PropertyMetadata(null, MenuSourceChanged));

        private static void MenuSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ApplicationBar bar = d as ApplicationBar;
            if (e.NewValue != null && e.NewValue is IEnumerable)
            {
                var newMenuItems = (e.NewValue as IEnumerable).Cast<ApplicationBarMenuItem>().ToList();
                if (newMenuItems.Count > 0)
                {
                    bar.MenuItems.Dettach(bar.SysAppBar);
                    bar.MenuItems.Clear();
                    foreach (ApplicationBarMenuItem item in newMenuItems)
                    {
                        bar.MenuItems.Add(item);
                    }
                    bar.MenuItems.Attach(bar.DataContext, bar.SysAppBar);
                }
            }
            else
            {
                bar.MenuItems.Dettach(bar.SysAppBar);
                bar.MenuItems.Clear();
                bar.MenuItems.Attach(bar.DataContext, bar.SysAppBar);
            }
        }


        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ApplicationBar), new PropertyMetadata(null, ItemsSourceChangedCallback));

        private static void ItemsSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ApplicationBar;
            var newButtons = (e.NewValue as IEnumerable);
            if (newButtons != null)
            {
                newButtons = newButtons.Cast<ApplicationBarIconButton>().ToList();
            }
            bar.UpdateItemsSource(newButtons as List<ApplicationBarIconButton>, bar.FlowDirection);
        }

        #region DataContext Property
        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        /// <value>Identifies the DataContext dependency property</value>
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(
            "DataContext",
            typeof(object),
            typeof(ApplicationBar),
              new PropertyMetadata(default(object), (d, e) => ((ApplicationBar)d).DataContextChanged(e.OldValue, e.NewValue)));

        /// <summary>
        /// Invoked on DataContext change.
        /// </summary>
        /// <param name="d">The object that was changed</param>
        /// <param name="e">Dependency property changed event arguments</param>
        private void DataContextChanged(object oldDataContext, object newDataContext)
        {
            // Attach items to app-bar and update DataContext.
            _buttons.Attach(newDataContext, SysAppBar);
            _menuItems.Attach(newDataContext, SysAppBar);
            SysAppBar.IsVisible = IsVisible;
        }
        #endregion

        #region BackgroundColor
        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(
                "BackgroundColor",
                typeof(Color),
                typeof(ApplicationBar),
                new PropertyMetadata(default(Color), (d, e) => ((ApplicationBar)d).BackgroundColorChanged((Color)e.NewValue)));

        private void BackgroundColorChanged(Color newColor)
        {
            _sysAppBar.BackgroundColor = newColor;
        } 
        #endregion

        #region ForegroundColor
        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(
                "ForegroundColor",
                typeof(Color),
                typeof(ApplicationBar),
                new PropertyMetadata(default(Color), (d, e) => ((ApplicationBar)d).ForegroundColorChanged((Color)e.NewValue)));

        private void ForegroundColorChanged(Color newColor)
        {
            _sysAppBar.ForegroundColor = newColor;
        } 
        #endregion

        #region IsMenuEnabled
        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(IsMenuEnabledProperty); }
            set { SetValue(IsMenuEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMenuEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMenuEnabledProperty =
            DependencyProperty.Register(
                "IsMenuEnabled",
                typeof(bool),
                typeof(ApplicationBar),
                new PropertyMetadata(default(bool), (d, e) => ((ApplicationBar)d).IsMenuEnabledChanged((bool)e.NewValue)));

        private void IsMenuEnabledChanged(bool isMenuEnabled)
        {
            _sysAppBar.IsMenuEnabled = isMenuEnabled;
        }
        #endregion

        #region IsVisible
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(
                "IsVisible",
                typeof(bool),
                typeof(ApplicationBar),
                new PropertyMetadata(true, (d, e) => ((ApplicationBar)d).IsVisibleChanged((bool)e.NewValue)));

        private void IsVisibleChanged(bool isVisible)
        {
            _sysAppBar.IsVisible = isVisible;
        }
        #endregion

        #region Mode
        public Microsoft.Phone.Shell.ApplicationBarMode Mode
        {
            get { return (Microsoft.Phone.Shell.ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                "Mode",
                typeof(Microsoft.Phone.Shell.ApplicationBarMode),
                typeof(ApplicationBar),
                new PropertyMetadata(default(Microsoft.Phone.Shell.ApplicationBarMode), (d, e) => ((ApplicationBar)d).ModeChanged((Microsoft.Phone.Shell.ApplicationBarMode)e.NewValue)));

        private void ModeChanged(Microsoft.Phone.Shell.ApplicationBarMode mode)
        {
            _sysAppBar.Mode = mode;
        }
        #endregion

        #region Opacity
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Opacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(
                "Opacity",
                typeof(double),
                typeof(ApplicationBar),
                new PropertyMetadata(default(double), (d, e) => ((ApplicationBar)d).OpacityChanged((double)e.NewValue)));

        private void OpacityChanged(double opacity)
        {
            _sysAppBar.Opacity = opacity;
        }
        #endregion        

        #endregion

        #region Properties

        public double DefaultSize
        {
            get { return _sysAppBar.DefaultSize; }
        }

        public double MiniSize
        {
            get { return _sysAppBar.MiniSize; }
        }

        IList Microsoft.Phone.Shell.IApplicationBar.Buttons
        {
            get { return _buttons; }
        }

        IList Microsoft.Phone.Shell.IApplicationBar.MenuItems
        {
            get { return _menuItems; }
        }

        public ApplicationBarIconButtonCollection Buttons
        {
            get { return _buttons; }
        }

        public ApplicationBarMenuItemCollection MenuItems
        {
            get { return _menuItems; }
        }

        internal Microsoft.Phone.Shell.IApplicationBar SysAppBar
        {
            get { return _sysAppBar; }
        }        

        #endregion

        #region Events

        public event EventHandler<Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs> StateChanged
        {
            add { _sysAppBar.StateChanged += value; }
            remove { _sysAppBar.StateChanged -= value; }
        }

        #endregion        
    }
}
