using System;

namespace blqw.Apilay.Attributes
{
    /// <summary>
    /// 特性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class AttributeBase : Attribute
    {
        /// <summary>
        /// 参数或配置名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 表示请求Url参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryValueAttribute : AttributeBase { }
    /// <summary>
    /// 表示请求头参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HeaderValueAttribute : AttributeBase { }
    /// <summary>
    /// 表示属性关联指定配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImportConfigAttribute : AttributeBase { }
    /// <summary>
    /// 表示请求正文参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BodyValueAttribute : AttributeBase
    {
        /// <summary>
        /// 请求正文类型
        /// </summary>
        public string ContentType { get; }
        /// <summary>
        /// 表示请求正文参数
        /// </summary>
        public BodyValueAttribute() { }
        /// <summary>
        /// 表示请求正文参数
        /// </summary>
        /// <param name="type">简化请求类型</param>
        public BodyValueAttribute(string type = "form")
            => ContentType = ToContentType(type ?? throw new ArgumentNullException(nameof(type)));
        /// <summary>
        /// 将简化的字符串转为标准 ContentType
        /// </summary>
        /// <param name="type">简化请求类型</param>
        /// <returns></returns>
        public static string ToContentType(string type)
        {
            switch (type?.ToLowerInvariant())
            {
                case "form":
                case "urlencode":
                    return "application/x-www-form-urlencoded";
                case "xml":
                    return "text/xml;charset=utf-8";
                case "json":
                    return "application/json;charset=utf-8";
                case "string":
                case "text":
                    return "text/plain;charset=utf-8";
                case "protobuf":
                    return "application/x-protobuf;charset=utf-8";
                default:
                    return type ?? "";
            }
        }
    }

}
