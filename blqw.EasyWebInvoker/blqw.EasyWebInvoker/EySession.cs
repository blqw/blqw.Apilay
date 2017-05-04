using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using blqw.EasyWebInvoker.Attributes;

namespace blqw.EasyWebInvoker
{
    /// <summary>
    /// 表示一个会话
    /// </summary>
    public class EySession
    {
        /// <summary>
        /// 请求执行器
        /// </summary>
        public IEyWebInvoker Invoker { get; }

        /// <summary>
        /// 表示一个会话, 默认使用 <seealso cref="EyWebInvoker"/> 执行器
        /// </summary>
        public EySession() => Invoker = new EyWebInvoker();

        /// <summary>
        /// 表示一个会话, 并指定一个执行器
        /// </summary>
        /// <param name="invoker"></param>
        public EySession(IEyWebInvoker invoker) => Invoker = invoker ?? new EyWebInvoker();

        /// <summary>
        /// 导入配置
        /// </summary>
        /// <param name="getConfig">用于获取配置值的委托</param>
        public void ImportConfig(Func<string, string> getConfig)
        {
            var props = from property in GetType().GetRuntimeProperties()
                        where property.CanWrite && !property.SetMethod.IsStatic
                           && property.PropertyType == typeof(string)
                        let config = property.GetCustomAttribute<ImportConfigAttribute>()
                        where config != null
                        select new { property, config };
            foreach (var p in props)
            {
                var value = getConfig(p.config.Name ?? p.property.Name);
                if (value != null)
                {
                    p.property.SetValue(this, value);
                }
            }
        }

        protected Task<T> Invoke<T>(string baseUrl, IEyRequest<T> request, CancellationToken cancellationToken)
        {
            try
            {
                return Invoker.SendAsync(new Uri(baseUrl), request, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e.RequestException(1);
            }
        }

    }
}
