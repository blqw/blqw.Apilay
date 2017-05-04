using System;

namespace blqw.EasyWebInvoker
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class EyExtensions
    {
        /// <summary>
        /// 将异常转为 <seealso cref="EyRequestException"/>
        /// </summary>
        /// <param name="exception">转换前的异常</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static EyRequestException RequestException(this Exception exception, int errorCode)
            => new EyRequestException(errorCode, exception?.Message ?? "未知异常", exception);

        /// <summary>
        /// 将异常转为 <seealso cref="EyRequestException"/>
        /// </summary>
        /// <param name="exception">转换前的异常</param>
        /// <param name="errorCode">错误码</param>
        public static EyRequestException RequestException(this Exception exception, string errorCode)
            => new EyRequestException(errorCode, exception?.Message ?? "未知异常", exception);
    }
}
