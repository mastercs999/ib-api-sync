using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    /// <summary>
    /// Various extension used here. What did you expect?
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts order action into reversed order action - buy to sell and sell to buy.
        /// </summary>
        /// <param name="orderAction">Order action to be reversed.</param>
        /// <returns>Reversed order action.</returns>
        public static OrderAction Reverse(this OrderAction orderAction)
        {
            return orderAction == OrderAction.Buy ? OrderAction.Sell : OrderAction.Buy;
        }

        /// <summary>
        /// Check whether given status of an order is last - order can't change its status afterwards. For example, cancelled is last status.
        /// </summary>
        /// <param name="orderStatus">Order status to be checked.</param>
        /// <returns>True if the order status is last.</returns>
        public static bool IsLastStatus(this OrderStatus orderStatus)
        {
            return orderStatus == OrderStatus.Filled || orderStatus == OrderStatus.Cancelled || orderStatus == OrderStatus.Inactive;
        }

        /// <summary>
        /// Convers given enum into string. It uses Description attribute if available otherwise it calls regular ToString method.
        /// </summary>
        /// <typeparam name="T">Type of given object. It must be enum.</typeparam>
        /// <param name="source">Value of the enum</param>
        /// <returns>Enum expressed as string.</returns>
        public static string Text<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute desciption = fi.GetCustomAttribute<DescriptionAttribute>();

            return desciption != null ? desciption.Description : source.ToString();
        }

        /// <summary>
        /// Converts given string into enum of given type. It enumarates the enum specified by type argument T and for each
        /// enum value it checks its string value got from Description attribute and then ToString method.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="text">String which should represent one value of given enum.</param>
        /// <returns>Enum value which corresponds with given text.</returns>
        /// <exception cref="InvalidOperationException">Thown when none enum value corresponds with given text.</exception>
        public static T ToEnum<T>(this string text)
        {
            foreach (T value in Utils.GetEnumArray<T>())
                if (value.ToString() == text || value.Text() == text)
                    return value;

            throw new InvalidOperationException($"Couldn't parse '{text}' into enum of type '{typeof(T).Name}'");
        }

        /// <summary>
        /// This method gets Name of given type. It servers for nicer name especially in case of generic types which
        /// append ugly suffix `.
        /// </summary>
        /// <param name="type">Type whose name we want.</param>
        /// <returns>Beautiful name of the type.</returns>
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
