using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;

namespace SuiteValue.UI.WPF.Prism
{
    public abstract class ModuleBase : IModule
    {
        private readonly string _moduleName;

        protected ModuleBase(string moduleName = null)
        {
            _moduleName = moduleName;
            RegisterResources(_moduleName);
        }

        public abstract void Initialize();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterResources(string moduleName = null)
        {
            var dictionary = new ResourceDictionary();
            
            

            if (string.IsNullOrEmpty(moduleName))
            {
                StackTrace stackTrace = new StackTrace();
                Assembly assembly = stackTrace.GetFrame(2).GetMethod().Module.Assembly;
                string assemblyName = assembly.FullName;
                string[] nameParts = assemblyName.Split(',');
                moduleName = nameParts[0];
            }
#if SILVERLIGHT

            StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri(moduleName + ";component/Resources/ModuleResources.xaml", UriKind.Relative));
            if (resourceInfo == null) return;
            var resourceReader = new StreamReader(resourceInfo.Stream);
            string xaml = resourceReader.ReadToEnd();
            var resourceTheme = XamlReader.Load(xaml) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceTheme);
#else
        dictionary.Source = new Uri(
            "pack://application:,,,/" + moduleName + ";component/Resources/ModuleResources.xaml");
        Application.Current.Resources.MergedDictionaries.Add(dictionary);
#endif
        }
    }
}