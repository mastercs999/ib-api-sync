using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    public static class Extensions
    {
        public static OrderAction Reverse(this OrderAction orderAction)
        {
            return orderAction == OrderAction.Buy ? OrderAction.Sell : OrderAction.Buy;
        }

        public static bool IsLastStatus(this OrderStatus orderStatus)
        {
            return orderStatus == OrderStatus.Filled || orderStatus == OrderStatus.Cancelled || orderStatus == OrderStatus.Inactive;
        }

        public static string Text<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute desciption = fi.GetCustomAttribute<DescriptionAttribute>();

            return desciption != null ? desciption.Description : source.ToString();
        }
        public static T ToEnum<T>(this string text)
        {
            foreach (T value in Utils.GetEnumArray<T>())
                if (value.ToString() == text || value.Text() == text)
                    return value;

            throw new Exception($"Couldn't parse '{text}' into enum of type '{typeof(T).Name}'");
        }

        public static string GetRealTypeName(this Type type)
        {
            // We handle only generic types
            if (!type.IsGenericType)
                return type.Name;

            // First part is sanatized name
            StringBuilder sb = new StringBuilder();
            sb.Append(type.Name.Contains('`') ? type.Name.Substring(0, type.Name.IndexOf('`')) : type.Name);

            // Now append type arguments
            sb.Append('<');
            sb.Append(String.Join(",", type.GetGenericArguments().Select(x => x.GetRealTypeName())));
            sb.Append('>');

            return sb.ToString();
        }
    }
}
