using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuiteValue.UI.WP8.Extensions
{
    public class DataTemplateSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SuiteValue.UI.WP8.Extensions.DataTemplateSelector"/> class.
        /// </summary>
        public DataTemplateSelector()
        {
        }

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
        /// </summary>
        /// 
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return (DataTemplate)null;
        }
    }
}
