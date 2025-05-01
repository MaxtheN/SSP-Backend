using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer
{
    public enum ToDynamicPropertyMap
    {
        None = 0,
        CamelCase = 1,
    }

    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            return value.ToDynamic(ToDynamicPropertyMap.None);
        }

        public static dynamic ToDynamic(this object value, ToDynamicPropertyMap propertyMap)
        {
            Func<PropertyDescriptor, string> propertyMapper = null;

            switch (propertyMap)
            {
                case ToDynamicPropertyMap.CamelCase:
                    propertyMapper = a => a.Name.FirstCharToLower();
                    break;
                case ToDynamicPropertyMap.None:
                default:
                    propertyMapper = a => a.Name;
                    break;
            }

            return value.ToDynamic(propertyMapper);
        }

        public static dynamic ToDynamic(this object value, Func<PropertyDescriptor, string> propertyMapper)
        {
            if (value == null)
                return null;

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(propertyMapper(property), property.GetValue(value));

            return expando as ExpandoObject;
        }

        //thanks to https://stackoverflow.com/questions/3565015/bestpractice-transform-first-character-of-a-string-into-lower-case
        private static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            return input.First().ToString().ToLower() + input.Substring(1);
        }
    }
}
