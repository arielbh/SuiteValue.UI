using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CodeValue.CodeLight.Prism.Extensions;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;

namespace CodeValue.CodeLight.Prism.Navigation
{
    public static class NavigationExtensions
    {
        public static void RequestNavigateWithParameters(this IRegionManager regionManager, string regionName,
                                                         string source, params object[] parameters)
        {
            var query = new UriQuery();
            var i = 0;
            foreach (var parameter in parameters)
            {
                query.Add(i.ToString(), Serialize(parameter).CompressString());
                i++;
            }


            regionManager.RequestNavigate(regionName, new Uri(source + query, UriKind.Relative), result =>
            {

            });
        }

        public static object[] Decipher(this IRegionManager regionManager, UriQuery parameters, params Type[] types)
        {
            var list = new List<object>();
            for (int i = 0; i < parameters.Count(); i++)
            {
                list.Add(Deserialize(parameters[i.ToString()].DecompressString(), types[i]));
            }
            return list.ToArray();
        }

        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static object Deserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }
    }
}
