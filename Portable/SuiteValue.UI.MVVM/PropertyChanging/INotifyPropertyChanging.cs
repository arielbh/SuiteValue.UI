namespace System.ComponentModel
{
    /// <summary>
    /// Notifies clients that a property value is changing.
    /// 
    /// </summary>
    public interface INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs when a property value is changing.
        /// 
        /// </summary>
        event PropertyChangingEventHandler PropertyChanging;
    }
}