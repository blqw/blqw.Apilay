using System;
using System.Collections.Generic;

namespace blqw.Apilay
{
    /// <summary>
    /// 表示一个Http请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApRequest<out T>
    {
        /// <summary>
        /// 请求方法
        /// </summary>
        string Method { get; }
        /// <summary>
        /// 请求类型
        /// </summary>
        string ContentType { get; }
        /// <summary>
        /// 请求路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 请求的Url参数
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Query { get; }
        /// <summary>
        /// 请求头参数
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Headers { get; }
        /// <summary>
        /// 请求正文
        /// </summary>
        byte[] Body { get; }
        /// <summary>
        /// 从响应中获取数据实体
        /// </summary>
        /// <param name="statusCode">响应码</param>
        /// <param name="content">响应正文</param>
        /// <param name="getHeader">用于获取请求头的委托</param>
        /// <returns></returns>
        T GetData(int statusCode, byte[] content, Func<string, string> getHeader);
    }

    /// <summary>
    /// 表示一个非泛型的Http请求
    /// </summary>
    public interface IApRequest : IApRequest<object>
    {

    }

}
