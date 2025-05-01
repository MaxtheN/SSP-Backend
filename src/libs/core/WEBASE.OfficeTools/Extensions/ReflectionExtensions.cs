using System.Reflection;
using WEBASE.OfficeTools.Attributes;

namespace WEBASE.OfficeTools.Extensions;

public static class ReflectionExtensions
{
    public static PropertyInfo[] GetValidProperties(this Type type)
    {
        return type.GetProperties().Where(p => !(p.GetCustomAttributes(typeof(IgnoreWordProperty), true).Any())).ToArray();
    }

    /// <summary>
    /// Gets only required type properties
    /// </summary>
    /// <param name="type"></param>
    /// <param name="requiredType"></param>
    /// <returns></returns>
    public static PropertyInfo[] GetValidProperties(this Type type, Type requiredType)
    {
        return type.GetProperties().Where(p => (!p.GetCustomAttributes(typeof(IgnoreWordProperty), true).Any()) && p.GetType() == requiredType).ToArray();
    }
}
