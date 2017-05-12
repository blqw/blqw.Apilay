using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Apilay
{
    /// <summary>
    /// 使用 <seealso cref="HttpClient"/> 执行 <seealso cref="IApRequest{T}"/> 的执行器
    /// </summary>
    public class ApWebInvoker : IApWebInvoker
    {
        /// <summary>
        /// 用于执行请求的 <seealso cref="HttpClient"/>
        /// </summary>
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// 将字符串转为 <seealso cref="HttpMethod"/>
        /// </summary>
        /// <param name="method">待转换的字符串</param>
        /// <returns></returns>
        private static HttpMethod ToHttpMethod(string method)
        {
            switch (method?.ToUpperInvariant())
            {
                case "GET":
                case null:
                    return HttpMethod.Get;
                case "DELETE":
                    return HttpMethod.Delete;
                case "HEAD":
                    return HttpMethod.Head;
                case "OPTIONS":
                    return HttpMethod.Options;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "TRACE":
                    return HttpMethod.Trace;
                default:
                    return new HttpMethod(method);
            }
        }

        /// <summary>
        /// 获取或设置请求超时前等待的毫秒数。
        /// </summary>
        public TimeSpan Timeout
        {
            get => _client.Timeout;
            set => _client.Timeout = value;
        }

        /// <summary>
        /// 使用异步方式发送请求并解析返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="baseUrl">基础路径</param>
        /// <param name="request">请求对象</param>
        public Task<T> SendAsync<T>(string baseUrl, IApRequest<T> request)
            => SendAsync(new Uri(baseUrl), request, CancellationToken.None);

        /// <summary>
        /// 使用异步方式发送请求并解析返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="baseUrl">基础路径</param>
        /// <param name="request">请求对象</param>
        /// <param name="cancellationToken">取消操作的取消标记</param>
        public Task<T> SendAsync<T>(string baseUrl, IApRequest<T> request, CancellationToken cancellationToken)
            => SendAsync(new Uri(baseUrl), request, cancellationToken);

        /// <summary>
        /// 使用异步方式发送请求并解析返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="baseUrl">基础路径</param>
        /// <param name="request">请求对象</param>
        public Task<T> SendAsync<T>(Uri baseUrl, IApRequest<T> request)
            => SendAsync(baseUrl, request, CancellationToken.None);

        /// <summary>
        /// 使用异步方式发送请求并解析返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="baseUrl">基础路径</param>
        /// <param name="request">请求对象</param>
        /// <param name="cancellationToken">取消操作的取消标记</param>
        /// <returns></returns>
        public async Task<T> SendAsync<T>(Uri baseUrl, IApRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var url = new UriBuilder(new Uri(baseUrl, request.Path));
            var encode = new FormUrlEncodedContent(request.Query);
            var query = await encode.ReadAsStringAsync();
            if (url.Query.Length > 1)
            {
                url.Query += "&" + query;
            }
            else
            {
                url.Query = query;
            }

            var method = ToHttpMethod(request.Method);
            var message = new HttpRequestMessage(method, url.Uri);
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    message.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            var body = request.Body;
            if (body != null)
            {
                var contentType = request.ContentType;
                message.Content = new ByteArrayContent(request.Body);
                message.Content.Headers.ContentType = contentType == null ? null : MediaTypeHeaderValue.Parse(contentType);
            }

            var response = await _client.SendAsync(message, cancellationToken);
            var statusCode = (int)response.StatusCode;
            var content = await response.Content.ReadAsByteArrayAsync();

            return request.GetData(statusCode, content, name => response.Headers.TryGetValues(name, out var values) ? string.Join(", ", values) : null);
        }

    }
}
