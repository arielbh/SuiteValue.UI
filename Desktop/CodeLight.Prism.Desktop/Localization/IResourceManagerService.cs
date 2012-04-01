using System.Collections.Generic;
using System.Resources;

namespace CodeValue.CodeLight.Prism.Localization
{
    /// <summary>
    /// Backend service for managing ResourceManagers and their resources
    /// </summary>
    public interface IResourceManagerService
    {
        /// <summary>
        /// Current application locale
        /// </summary>
        Locale CurrentLocale { get; }

        /// <summary>
        /// Retreives a string resource with the given key from the given
        /// resource manager.
        /// 
        /// Will load the string relevant to the current culture.
        /// </summary>
        /// <param name="managerName">Name of the ResourceManager</param>
        /// <param name="resourceKey">Resource to lookup</param>
        /// <returns></returns>
        string GetResourceString(string managerKey, string resourceKey);

        /// <summary>
        /// Changes the current locale
        /// </summary>
        /// <param name="newLocaleName">IETF locale name (e.g. en-US, en-GB)</param>
        void ChangeLocale(string newLocaleName);

        /// <summary>
        /// Register a ResourceManager, does not fire a refresh
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        void RegisterManager(string managerName, ResourceManager manager);

        /// <summary>
        /// Register a ResourceManager
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        /// <param name="refresh">Whether to fire the LocaleChanged event to refresh bindings</param>
        #if SILVERLIGHT
        void RegisterManager(string managerName, object manager, bool refresh);
        #else
        void RegisterManager(string managerName, ResourceManager manager, bool refresh);
        #endif
        /// <summary>
        /// Remove a ResourceManager
        /// </summary>
        /// <param name="name">Name of the manager to remove</param>
        void UnregisterManager(string name);

#if SILVERLIGHT
        Dictionary<string, Dictionary<string, string>> Managers { get;}
#endif
    }
}