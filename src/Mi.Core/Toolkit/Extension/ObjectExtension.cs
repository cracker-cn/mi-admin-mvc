using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mi.Core.Toolkit.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        ///     获取实体的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="propName">属性名称，忽略大小写</param>
        /// <returns></returns>
        public static object FetchValue<T>(this T model, string propName) where T : class, new()
        {
            var flag = BindingFlags.IgnoreCase;
            return typeof(T).GetProperty(propName, flag)!.GetValue(model)!;
        }

        /// <summary>
        ///     对象间拷贝
        /// </summary>
        /// <typeparam name="TSource">复制实体</typeparam>
        /// <typeparam name="TDest">目标实体</typeparam>
        /// <param name="sourceInstance">复制源数据</param>
        /// <param name="targetInstance">目标源数据</param>
        /// <param name="ignoreProperty">忽略字段</param>
        public static void CopyTo<TSource, TDest>(this TSource sourceInstance, TDest targetInstance,
            params object[] ignoreProperty)
            where TSource : class
            where TDest : class
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TDest);
            var targetProps = targetType.GetProperties().ToList();
            foreach (var item in targetProps)
                if (item.CanWrite)
                {
                    var sourceProp = sourceType.GetProperty(item.Name);
                    if (null != sourceProp && sourceProp.CanRead && !ignoreProperty.Contains(item.Name))
                        targetType.SetPropertyValue(targetInstance, item.Name, sourceProp.GetValue(sourceInstance) ?? new object());
                }
        }

        /// <summary>
        ///     设置对象的属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetPropertyValue(this Type target, object instance, string name, object value)
        {
            var p = target.GetProperty(name)!;
            var v = p.PropertyType.GetFieldValue(value);
            if (p != null && p.CanWrite)
            {
                p.SetValue(instance, v);
                return true;
            }

            return false;
        }

        public static object? GetFieldValue(this Type propType, object fieldValue)
        {
            if (Convert.IsDBNull(fieldValue) || fieldValue == null) return null;

            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (fieldValue != null)
                {
                    var nullableConverter = new NullableConverter(propType);
                    propType = nullableConverter.UnderlyingType;
                }
                else
                {
                    return propType.TypeInitializer;
                }
            }

            return Convert.ChangeType(fieldValue, propType);
        }
    }
}
