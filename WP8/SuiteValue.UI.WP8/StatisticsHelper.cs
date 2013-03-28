using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace SuiteValue.UI.WP8
{
    public static class StatisticsHelper
    {
        private static readonly DispatcherTimer Timer = new DispatcherTimer();

        static StatisticsHelper()
        {
            NavigationEvents = new ObservableCollection<string>();
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(.5)
            };
            Timer.Tick += (s, e) => PrintMemory();
        }

        private static void PrintMemory()
        {
           Memory = (DeviceStatus.ApplicationCurrentMemoryUsage >> 20);
           MemoryUsageLimit = (DeviceStatus.ApplicationMemoryUsageLimit >> 20);
           MemoryPeak = (DeviceStatus.ApplicationPeakMemoryUsage >> 20);
           Debug.WriteLine("Memory: " + Memory.ToString() + " MB");
            OnMemoryUpdated(null, null);
        }


        public static void EnableMemoryOutput()
        {
            Timer.Start();
        }

        public static void DisableMemoryOutput()
        {
            Timer.Stop();
        }

        public static long Memory { get; set; }
        public static long MemoryUsageLimit { get; set; }
        public static long MemoryPeak { get; set; }
        public static event EventHandler OnMemoryUpdated = delegate  {};

        public static void EnableNavigationLogging(this PhoneApplicationPage page)
        {
            page.NavigationService.FragmentNavigation += NavigationService_FragmentNavigation;
            page.NavigationService.JournalEntryRemoved += NavigationService_JournalEntryRemoved;
            page.NavigationService.Navigated += NavigationService_Navigated;
            page.NavigationService.Navigating += NavigationService_Navigating;
            page.NavigationService.NavigationFailed += NavigationService_NavigationFailed;
            page.NavigationService.NavigationStopped += NavigationService_NavigationStopped;

        }

        static void NavigationService_NavigationStopped(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            AddToLog(string.Format("Navigation Stopped: Uri={0} IsNavigationInitiator={1} NavigationMode={2}", e.Uri, e.IsNavigationInitiator, e.NavigationMode));

        }

        static void NavigationService_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            AddToLog(string.Format("Navigate Failed: Uri={0} Exception={1} ", e.Uri, e.Exception));

        }

        static void NavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            AddToLog(string.Format("Navigating: Uri={0} IsNavigationInitiator={1} NavigationMode={2} IsCancelable={3}  ", e.Uri, e.IsNavigationInitiator, e.NavigationMode, e.IsCancelable));

        }

        static void NavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            AddToLog(string.Format("Navigated: Uri={0} IsNavigationInitiator={1} NavigationMode={2}  ", e.Uri, e.IsNavigationInitiator, e.NavigationMode));
            
        }

        static void NavigationService_JournalEntryRemoved(object sender, System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            AddToLog("Journal Entry Removed:" + e.Entry.Source);

        }

        static void NavigationService_FragmentNavigation(object sender, System.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            AddToLog("Fragment Navigation:" + e.Fragment);
        }

        private static void AddToLog(string message)
        {
            if (NavigationEvents.Count == 8)
            {
                NavigationEvents.Clear();
            }

            NavigationEvents.Add(message);
        }

        public static void DisableNavigationLogging(this PhoneApplicationPage page)
        {
            page.NavigationService.FragmentNavigation -= NavigationService_FragmentNavigation;
            page.NavigationService.JournalEntryRemoved -= NavigationService_JournalEntryRemoved;
            page.NavigationService.Navigated -= NavigationService_Navigated;
            page.NavigationService.Navigating -= NavigationService_Navigating;
            page.NavigationService.NavigationFailed -= NavigationService_NavigationFailed;
            page.NavigationService.NavigationStopped -= NavigationService_NavigationStopped;
            NavigationEvents.Clear();
        }

        public static ObservableCollection<string> NavigationEvents { get; set; }
    }
}
