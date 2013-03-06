using System.Collections.Generic;
using System.ComponentModel;

namespace SuiteValue.UI.WP8
{
    public class NavigationEventArgs : CancelEventArgs
    {
        public string ViewUri{ get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }

        public NavigationEventArgs(string viewUri, IDictionary<string, string> parameters)
        {
            
            this.ViewUri = viewUri;
            this.Parameters = parameters;
        }
    }
}