using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SuiteValue.UI.WPF.Converters
{
    public class AddEmptyItemConverter : IValueConverter
    {
        private object InstanceCreate(Type type, string propertyToNull)
        {
            object result = Activator.CreateInstance(type);
            if (!string.IsNullOrEmpty(propertyToNull))
            {
                type.GetProperty(propertyToNull).SetValue(result, null, null);
            }
            return result;
        }

        public object Convert(dynamic value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {

                Type type = value.GetType();
                if (type.IsGenericType)
                    if (value.Count > 0)
                        value.Insert(0, InstanceCreate(value[0].GetType(), parameter.ToString()));
                if (type.IsArray)
                {
                    if (value.Length > 0)
                    {
                        List<dynamic> l = new List<dynamic>(value);
                        l.Insert(0, InstanceCreate(value[0].GetType(), parameter.ToString()));
                        return l;

                    }
                }
            }
            return value;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
