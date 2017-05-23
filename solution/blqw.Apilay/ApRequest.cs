using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using blqw.Apilay.Attributes;

namespace blqw.Apilay
{
    /// <summary>
    /// Http请求的抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApRequest<T> : IApRequest<T>
    {
        /// <summary>
        /// 请求方法, 默认: GET
        /// </summary>
        public virtual string Method { get; } = "GET";

        /// <summary>
        /// 请求路径
        /// </summary>
        public abstract string Path { get; }

        /// <summary>
        /// 请求类型, 默认: null
        /// </summary>
        public virtual string ContentType
            => EnumerableBodyProperties().FirstOrDefault().Value?.ContentType;

        /// <summary>
        /// 请求的Url参数, 默认获取被标记为 <seealso cref="QueryValueAttribute"/> 的属性值
        /// </summary>
        public virtual IEnumerable<KeyValuePair<string, string>> Query
            => from x in GetType().GetRuntimeProperties()
               let a = x.GetCustomAttribute<QueryValueAttribute>()
                let value = x.GetValue(this)?.ToString()
                where value != null
                select new KeyValuePair<string, string>(a.Name ?? x.Name, value);

        /// <summary>
        /// 请求头参数, 默认获取被标记为 <seealso cref="HeaderValueAttribute"/> 的属性值
        /// </summary>
        public virtual IEnumerable<KeyValuePair<string, string>> Headers
            => from x in GetType().GetRuntimeProperties()
                let a = x.GetCustomAttribute<HeaderValueAttribute>()
                where a != null
                let value = x.GetValue(this)?.ToString()
                where value != null
                select new KeyValuePair<string, string>(a.Name ?? x.Name, value);

        /// <summary>
        /// 枚举被标记为 <seealso cref="BodyValueAttribute"/> 的属性
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<PropertyInfo, BodyValueAttribute>> EnumerableBodyProperties()
            => from property in GetType().GetRuntimeProperties()
               let body = property.GetCustomAttribute<BodyValueAttribute>()
               where body != null
               select new KeyValuePair<PropertyInfo, BodyValueAttribute>(property, body);

        /// <summary>
        /// 请求正文, 根据实际情况计算Body的值
        /// </summary>
        public virtual byte[] Body
        {
            get
            {
                if (ContentType == null)
                {
                    return null;
                }
                if (ContentType.Contains("x-www-form-urlencoded"))
                {
                    var nv = from x in GetType().GetRuntimeProperties()
                             let a = x.GetCustomAttribute<BodyValueAttribute>()
                             where a != null
                             let value = x.GetValue(this)?.ToString()
                             where value != null
                             select new KeyValuePair<string, string>(a.Name ?? x.Name, value);
                    return new FormUrlEncodedContent(nv).ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 从响应中获取数据实体
        /// </summary>
        /// <param name="statusCode">响应码</param>
        /// <param name="content">响应正文</param>
        /// <param name="getHeader">用于获取请求头的委托</param>
        /// <returns></returns>
        public abstract T GetData(int statusCode, byte[] content, Func<string, string> getHeader);
    }
}
