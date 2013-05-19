using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
#if WINDOWS_PHONE

#else
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Orientation = Windows.UI.ViewManagement.ApplicationViewState; 
#endif

namespace SuiteValue.UI.WP8.Helpers
{
    public static class OrientationHelper
    {
#if WINDOWS_PHONE
        private const String VisualStateGroupName = "PageOrientationStates";
#else
        private const String VisualStateGroupName = "ApplicationViewStates";
#endif


        #region IsActive DP

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(OrientationHelper),
                                                new PropertyMetadata(default(bool), OnIsActivePropertyChanged));

        public static void SetIsActive(UIElement element, bool value)
        {
            element.SetValue(IsActiveProperty, value);
        }

        public static bool GetIsActive(UIElement element)
        {
            return (bool)element.GetValue(IsActiveProperty);
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Control;
            if (control == null)
                return;

            SetupOrientationAwareControl(control, (bool)e.NewValue);

#if WINDOWS_PHONE
            //The control itself can be a page.In every case we need to retry until the containing page is loaded.
            control.Loaded += OnControlLoaded;
#endif

        }

        #endregion

#if WINDOWS_PHONE
        //Note: On the phone side Orientation is Page-specific so we keep the orientation aware controls
        //on the page itself using a Dependency Property
        #region OrientationAwareControls Private DP

        private static readonly DependencyProperty OrientationAwareControlsProperty =
            DependencyProperty.RegisterAttached("OrientationAwareControls", typeof(IList<Control>),
                                                typeof(OrientationHelper), null);

        private static void SetOrientationAwareControls(DependencyObject element, IList<Control> value)
        {
            element.SetValue(OrientationAwareControlsProperty, value);
        }

        private static IList<Control> GetOrientationAwareControls(DependencyObject element)
        {
            return (IList<Control>)element.GetValue(OrientationAwareControlsProperty);
        }

        #endregion
#else
        //Note: On the Windows 8 side Orientation is global
        private static IList<WeakReference> orientationAwareControls; 
#endif


        private static void SetupOrientationAwareControl(Control control, bool isActive)
        {
#if WINDOWS_PHONE
            var page = FindParentPage(control);
            if (page == null)
                return;
            if (isActive)
            {
                //Add the orientation aware control to the list stored on the Page
                if (GetOrientationAwareControls(page) == null)
                {
                    SetOrientationAwareControls(page, new List<Control>());
                    //Start listening for changes
                    page.OrientationChanged += PageOnOrientationChanged;
                }
                if (!GetOrientationAwareControls(page).Contains(control))
                    GetOrientationAwareControls(page).Add(control);
                UpdateOrientationAwareControls(new[] { control }, page.Orientation); //Update this control only

            }
            else
            {
                var orientationAwareControls = GetOrientationAwareControls(page);
                if (orientationAwareControls != null)
                {
                    if (orientationAwareControls.Contains(control))
                    {
                        orientationAwareControls.Remove(control);
                        control.Loaded -= OnControlLoaded;
                    }
                    //Do cleanup for this page if there are no more controls
                    if (!orientationAwareControls.Any())
                    {
                        SetOrientationAwareControls(page, null);
                        page.OrientationChanged -= PageOnOrientationChanged;
                    }
                }
            }
#else
            if (isActive)
            {
                if (orientationAwareControls == null)
                    orientationAwareControls = new List<WeakReference>();

                if (!GetOrientationAwareControls().Contains(control))
                {
                    orientationAwareControls.Add(new WeakReference(control));
                    // Note: On the Windows 8 side, probably due to aggressive optimizations,
                    // if the control is not named then when changing orientations/view states any
                    // WeakReference to that control will show it as GCed!
                    // Disclaimer : I ONLY TESTED ON THE SIMULATOR!!
                    if (string.IsNullOrEmpty(control.Name))
                    {
                        control.Name = Guid.NewGuid().ToString();
                    }
                    control.Loaded += OnControlLoaded;

                    if (control.Parent != null)
                    {
                        UpdateOrientationAwareControls(new[] { control }, ApplicationView.Value);
                    }
                }
                if (GetOrientationAwareControls().Any())
                {
                    //TODO: Handle Window changes (Closed)
                    Window.Current.SizeChanged += OnWindowSizeChanged;
                }


            }
            else
            {
                if (GetOrientationAwareControls().Contains(control))
                {
                    var controlWeakRef = orientationAwareControls.Single(weak => control.Equals(weak.Target));
                    orientationAwareControls.Remove(controlWeakRef);
                    control.Loaded -= OnControlLoaded;
                }
                if (!GetOrientationAwareControls().Any())
                {
                    orientationAwareControls = null;
                    Window.Current.SizeChanged -= OnWindowSizeChanged;
                }
            }
#endif
        }


        private static void OnControlLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var control = sender as Control;
#if WINDOWS_PHONE
            SetupOrientationAwareControl(control, GetIsActive(control));
#else
            UpdateOrientationAwareControls(new[] { control }, ApplicationView.Value);
#endif
        }

#if WINDOWS_PHONE

        private static PhoneApplicationPage FindParentPage(FrameworkElement el)
        {
            FrameworkElement parent = el;
            while (parent != null)
            {
                if (parent is PhoneApplicationPage)
                    return parent as PhoneApplicationPage;
                parent = parent.Parent as FrameworkElement;
            }
            return null;
        }

        private static void PageOnOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            var page = sender as PhoneApplicationPage;
            UpdateOrientationAwareControls(GetOrientationAwareControls(page), page.Orientation); //Update all controls
        }
#else
        private static void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            UpdateOrientationAwareControls(GetOrientationAwareControls(), ApplicationView.Value);
        }
        private static IEnumerable<Control> GetOrientationAwareControls()
        {
            if(orientationAwareControls == null || !orientationAwareControls.Any())
                return Enumerable.Empty<Control>();

            IList<Control> aliveControls = new List<Control>();
            foreach (var controlWeakRef in orientationAwareControls.ToList())
            {
                if (controlWeakRef.IsAlive)
                    aliveControls.Add(controlWeakRef.Target as Control);
                else
                    orientationAwareControls.Remove(controlWeakRef);
            }
            return aliveControls;
        }
#endif


        private static void UpdateOrientationAwareControls(IEnumerable<Control> controls, PageOrientation orientation)
        {
            
            if (controls == null || !controls.Any())
                return;

            foreach (var control in controls)
            {
                var reduced = ReduceToLandscapePortrait(orientation);
                VisualStateManager.GoToState(control, GetVisualStateName(reduced), true);
                var callback = control.GetValue(PageOrientationCallbackProperty) as Action<PageOrientation>;
                if (callback != null)
                {
                    callback(reduced);
                }
            }
        }

        private static String GetVisualStateName(PageOrientation orientation)
        {
            return orientation.ToString();
        }

        private static PageOrientation ReduceToLandscapePortrait(PageOrientation orientation)
        {
            switch (orientation)
            {
               case PageOrientation.LandscapeLeft: return PageOrientation.Landscape;
               case PageOrientation.LandscapeRight: return PageOrientation.Landscape;
               case PageOrientation.PortraitDown: return PageOrientation.Portrait;
               case PageOrientation.PortraitUp: return PageOrientation.Portrait;
                default:
                    return orientation;
            }
        }

        public static Action<PageOrientation> GetPageOrientationCallback(DependencyObject obj)
        {
            return (Action<PageOrientation>)obj.GetValue(PageOrientationCallbackProperty);
        }

        public static void SetPageOrientationCallback(DependencyObject obj, Action<PageOrientation> value)
        {
            obj.SetValue(PageOrientationCallbackProperty, value);
        }

        // Using a DependencyProperty as the backing store for PageOrientationCallback.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageOrientationCallbackProperty =
            DependencyProperty.RegisterAttached("PageOrientationCallback", typeof(Action<PageOrientation>), typeof(OrientationHelper), new PropertyMetadata(null));






    }
}
