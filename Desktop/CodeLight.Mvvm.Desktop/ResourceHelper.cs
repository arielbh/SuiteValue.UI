using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace CodeValue.CodeLight.Mvvm
{
    public class ResourceHelper
    {
        public static string ExecutingAssemblyName
        {
            get { return GetAssemblyName(Assembly.GetExecutingAssembly()); }
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            string name = assembly.FullName;
            return name.Substring(0, name.IndexOf(','));
        }

        public static Stream GetStream(string relativeUri, string assemblyName)
        {
            StreamResourceInfo res =
                Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative)) ??
                Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));
            return res != null ? res.Stream : null;
        }

        public static Stream GetStream(string relativeUri, Type typeInAssembly)
        {
            return GetStream(relativeUri, GetAssemblyName(typeInAssembly.Assembly));
        }

        public static Stream GetStream(string relativeUri)
        {
            return GetStream(relativeUri, ExecutingAssemblyName);
        }

        public static BitmapImage GetBitmap(string relativeUri, string assemblyName = null)
        {

#if SILVERLIGHT

                Stream s = GetStream(relativeUri, assemblyName);
                if (s == null) return null;
                using (s)
                {
                    var bmp = new BitmapImage();
                    bmp.SetSource(s);
                    return bmp;
                }
#else
            if (assemblyName == null) assemblyName = GetAssemblyName(Assembly.GetCallingAssembly());
            BitmapImage source = new BitmapImage();
            source.BeginInit();
            source.UriSource =
                new Uri(string.Format("pack://application:,,,/{0};component/{1}", assemblyName, relativeUri));
            source.EndInit();
            return source;
#endif
        }

        //public static BitmapImage GetBitmap(string relativeUri, Type typeInAssembly)
        //{
        //    return GetBitmap(relativeUri, GetAssemblyName(typeInAssembly.Assembly));
        //}

        //public static BitmapImage GetBitmap(string relativeUri)
        //{
        //    return GetBitmap(relativeUri, ExecutingAssemblyName);
        //}

        public static string GetString(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;
            using (var reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetString(string relativeUri)
        {
            return GetString(relativeUri, ExecutingAssemblyName);
        }

    }
}