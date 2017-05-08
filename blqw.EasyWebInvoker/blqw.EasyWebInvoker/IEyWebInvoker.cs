using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.EasyWebInvoker
{
    /// <summary>
    /// 用于执行 <seealso cref="IEyRequest{T}"/> 的执行器
    /// </summary>
    public interface IEyWebInvoker
    {
        /// <summary>
        /// 使用异步方式发送请求并解析返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="baseUrl">基础路径</param>
        /// <param name="request">请求对象</param>
        /// <param name="cancellationToken">取消操作的取消标记</param>
        /// <returns></returns>
        Task<T> SendAsync<T>(Uri baseUrl, IEyRequest<T> request, CancellationToken cancellationToken);

        /// <summary>
        /// 获取或设置请求超时前等待的毫秒数。
        /// </summary>
        TimeSpan Timeout { get; set; }
    }
}
