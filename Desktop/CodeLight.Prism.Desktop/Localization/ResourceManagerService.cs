using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using Microsoft.Practices.Prism.Events;

namespace CodeValue.CodeLight.Prism.Localization
{
    public class ResourceManagerService : IResourceManagerService
    {
        private IEventAggregator _eventAggregator;
#if SILVERLIGHT
        private readonly Dictionary<string, Dictionary<string, string>> _managers;

        public Dictionary<string, Dictionary<string, string>> Managers
        {
            get { return _managers; }
        }
#else
        private Dictionary<string, WeakReference> _managers;
#endif


        /// <summary>
        /// Current application locale
        /// </summary>
        public Locale CurrentLocale { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ResourceManager class.
        /// </summary>
        public ResourceManagerService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            

            // Set to default culture
#if SILVERLIGHT
            _managers = new Dictionary<string, Dictionary<string, string>>();
            ChangeLocale(CultureInfo.CurrentCulture.Name);
#else
            _managers = new Dictionary<string, WeakReference>();
            ChangeLocale(CultureInfo.CurrentCulture.IetfLanguageTag);
#endif
        }

        /// <summary>
        /// Retreives a string resource with the given key from the given
        /// resource manager.
        /// 
        /// Will load the string relevant to the current culture.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the manager does not exist or has been disposed</exception>
        /// <param name="managerName">Name of the ResourceManager</param>
        /// <param name="resourceKey">Resource to lookup</param>
        /// <returns></returns>
        public string GetResourceString(string managerName, string resourceKey)
        {
#if SILVERLIGHT
            Dictionary<string, string> reference = null;
            ResourceManager manager = null;
            string resource = String.Empty;

            if (!_managers.TryGetValue(managerName, out reference))
                throw new ArgumentException("managerName must be a valid manager");


            if (!reference.ContainsKey(resourceKey))
                throw new ArgumentException("resourceKey must be a valid key");

            resource = reference[resourceKey];
#else
            WeakReference reference = null;
            ResourceManager manager = null;
            string resource = String.Empty;

            if (!_managers.TryGetValue(managerName, out reference))
                throw new ArgumentException("managerName must be a valid manager");

            manager = reference.Target as ResourceManager;
            if (manager == null)
            {
                UnregisterManager(managerName);
                throw new ArgumentException("managerName must be a valid manager");
            }

            resource = manager.GetString(resourceKey);

#endif

            return resource;
        }

        /// <summary>
        /// Changes the current locale
        /// </summary>
        /// <param name="newLocaleName">IETF locale name (e.g. en-US, en-GB)</param>
        public void ChangeLocale(string newLocaleName)
        {
            CultureInfo newCultureInfo = new CultureInfo(newLocaleName);
            Thread.CurrentThread.CurrentCulture = newCultureInfo;
            Thread.CurrentThread.CurrentUICulture = newCultureInfo;
#if SILVERLIGHT
            Locale newLocale = new Locale() { Name = newLocaleName, RTL = IsLanguageRTL(Thread.CurrentThread.CurrentCulture) };
#else
            Locale newLocale = new Locale() { Name = newLocaleName, RTL = newCultureInfo.TextInfo.IsRightToLeft };
#endif

            CurrentLocale = newLocale;

            _eventAggregator.GetEvent<LocaleChangedEvent>().Publish(newLocale);
        }

        /// <summary>
        /// Fires the LocaleChange event to reload bindings
        /// </summary>
        public void Refresh()
        {
#if SILVERLIGHT
            ChangeLocale(CultureInfo.CurrentCulture.Name);

#else
            ChangeLocale(CultureInfo.CurrentCulture.IetfLanguageTag);
#endif

        }

        /// <summary>
        /// Register a ResourceManager, does not fire a refresh
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        public void RegisterManager(string managerName, ResourceManager manager)
        {
            RegisterManager(managerName, manager, false);
        }

        /// <summary>
        /// Register a ResourceManager
        /// </summary>
        /// <param name="managerName">Name to store the manager under, used with GetResourceString/UnregisterManager</param>
        /// <param name="manager">ResourceManager to store</param>
        /// <param name="refresh">Whether to fire the LocaleChanged event to refresh bindings</param>
        
#if SILVERLIGHT
        public void RegisterManager(string managerName, object manager, bool refresh)
        {

            if (!_managers.ContainsKey(managerName))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                Type type = manager.GetType();
                var props = from prop in type.GetProperties()
                            where prop.PropertyType == typeof(string)
                            select prop;
                foreach (var prop in props)
                {
                    values[prop.Name] = (string)prop.GetValue(manager, null);
                }
                _managers[managerName] = values;
            }
#else
        public void RegisterManager(string managerName, ResourceManager manager, bool refresh)
        {
            WeakReference _reference = null;

            _managers.TryGetValue(managerName, out _reference);

            if (_reference == null)
                _managers.Add(managerName, new WeakReference(manager));
            else if (_reference.Target == null)
                _managers[managerName] = new WeakReference(manager);


#endif
            if (refresh)
                Refresh();

        }


        /// <summary>
        /// Remove a ResourceManager
        /// </summary>
        /// <param name="name">Name of the manager to remove</param>
        public void UnregisterManager(string name)
        {
#if SILVERLIGHT
            if (_managers.ContainsKey(name))
            {
                _managers.Remove(name);
            }
#else
                        WeakReference _reference = null;

            _managers.TryGetValue(name, out _reference);

            if (_reference != null)
                _managers.Remove(name);
#endif

        }

#if SILVERLIGHT
#else
#endif


#if SILVERLIGHT
        private bool IsLanguageRTL(CultureInfo cultureInfo)
        {
            // ISO 691.1 http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
            // Hebrew: he
            // Arabic: ar
            // Divehi: dv
            // Hausa : ha
            // Persian: fa
            // Pashto: ps
            // Urdu: ur
            // Yiddish: yi
            var rightToLeftLangCodes = new[] { "he", "ar", "dv", "ha", "fa", "ps", "ur", "yi" };
            return rightToLeftLangCodes.Contains(cultureInfo.TwoLetterISOLanguageName);
        }
#endif


    }
}