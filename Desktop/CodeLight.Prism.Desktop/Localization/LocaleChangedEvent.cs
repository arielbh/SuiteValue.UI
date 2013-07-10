using Microsoft.Practices.Prism.Events;

namespace SuiteValue.UI.WPF.Prism.Localization
{
    /// <summary>
    /// Aggregate event fired when the locale changes.
    /// 
    /// Usually used to update the Shell FlowDirection
    /// and also to fire NotifyPropertyChanged from the helper
    /// </summary>
    public class LocaleChangedEvent : CompositePresentationEvent<Locale>
    {
    }
}