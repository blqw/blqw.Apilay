using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using blqw.Apilay.Attributes;

namespace blqw.Apilay
{
    /// <summary>
    /// 表示一个会话
    /// </summary>
    public class ApSession
    {
        /// <summary>
        /// 请求执行器
        /// </summary>
        public IApWebInvoker Invoker { get; }

        /// <summary>
        /// 表示一个会话, 默认使用 <seealso cref="ApWebInvoker"/> 执行器
        /// </summary>
        public ApSession() => Invoker = new ApWebInvoker();

        /// <summary>
        /// 表示一个会话, 并指定一个执行器
        /// </summary>
        /// <param name="invoker"></param>
        public ApSession(IApWebInvoker invoker) => Invoker = invoker ?? new ApWebInvoker();

        /// <summary>
        /// 导入配置
        /// </summary>
        /// <param name="getConfig">用于获取配置值的委托</param>
        public void ImportConfig(Func<string, string> getConfig)
        {
            var props = from p in GetType().GetRuntimeProperties()
                        where p.CanWrite && !p.SetMethod.IsStatic
                        let a = p.GetCustomAttribute<ImportConfigAttribute>()
                        where a != null
                        select new KeyValuePair<string, PropertyInfo>(a.Name ?? p.Name, p);
            foreach (var p in props)
            {
                var value = (object)getConfig(p.Key);
                if (value != null)
                {
                    if (p.Value.PropertyType == typeof(Uri))
                    {
                        value = new Uri((string)value);
                    }
                    else if (p.Value.PropertyType != typeof(string))
                    {
                        value = Convert.ChangeType(value, p.Value.PropertyType);
                    }
                    p.Value.SetValue(this, value);
                }
            }
        }

        protected Task<T> Invoke<T>(string baseUrl, IApRequest<T> request, CancellationToken cancellationToken)
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
