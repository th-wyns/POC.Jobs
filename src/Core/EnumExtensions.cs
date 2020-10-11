using System;
using System.ComponentModel;
using System.Reflection;

namespace POC
{
    /// <summary>
    /// EnumExtensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enumerationValue">The enumeration value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">enumerationValue</exception>
        public static string GetDescription<TEnum>(this TEnum enumerationValue) where TEnum : struct
        {
            var typeInfo = typeof(TEnum);
            var memberName = Enum.GetName(typeInfo, enumerationValue);
            var memberInfo = typeInfo.GetField(memberName);
            return memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? memberName;
        }
    }
}
