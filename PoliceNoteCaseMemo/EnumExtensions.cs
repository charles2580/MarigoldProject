using System;
using System.ComponentModel;
using System.Reflection;

namespace EPlaceName.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? attrs[0].Description : value.ToString();
        }
    }
}