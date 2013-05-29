using System.Collections.Generic;

namespace SuiteValue.UI.WP8
{
    public interface INavigator
    {
        void Navigate(string viewUri, IDictionary<string, string> parameters = null);
        void Navigate<T>(T viewModel, IDictionary<string, string> parameters = null) where T : NavigationViewModelBase;
        void NavigateBackTo<T>(T viewModel, IDictionary<string, string> parameters = null) where T : NavigationViewModelBase;
        void NavigateBack(IDictionary<string, string> parameters = null);
    }
}