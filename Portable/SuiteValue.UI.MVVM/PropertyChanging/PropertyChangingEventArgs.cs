using System;
using System.Diagnostics;

namespace System.ComponentModel
{
    /// <devdoc>
    /// <para>Provides data for the <see langword='PropertyChanging'/> 
    /// event.</para>
    /// </devdoc> 
    public class PropertyChangingEventArgs : EventArgs
    {
        private readonly string propertyName;

        /// <devdoc>
        /// <para>Initializes a new instance of the <see cref='System.ComponentModel.PropertyChangingEventArgs'/>
        /// class.</para> 
        /// </devdoc>
        public PropertyChangingEventArgs(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <devdoc>
        ///    <para>Indicates the name of the property that is changing.</para>
        /// </devdoc>
        public virtual string PropertyName
        {
            get { return propertyName; }
        }
    }
}
