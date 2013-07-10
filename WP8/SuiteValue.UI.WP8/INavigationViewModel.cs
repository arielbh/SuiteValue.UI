using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace SuiteValue.UI.WP8
{
    public interface INavigationViewModel
    {
        event EventHandler<NavigationEventArgs> RequestNavigateTo;
        event EventHandler<EventArgs> RequestNavigateBack;
        event EventHandler<NavigationBackEventArgs> RequestNavigateBackTo;
        event EventHandler<EventArgs> RequestUnregister;
        
        bool RegisteredForNavigation { get; set; }
        bool KeepRegistrationsAlive { get; set; }
        bool SupportOrientation { get; }

        void OnNavigatedTo(NavigationMode mode, IDictionary<string, string> parameter, bool isNavigationInitiator);
        bool OnNavigatingFrom(NavigationMode mode);
        void OnNavigatedFrom(NavigationMode mode);
        bool OnBackKeyPress();
        void UnregisterFromPage();
        void OrientationChanged(PageOrientation orientation);
    }
}