using System;

namespace blqw.Apilay
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class ApExtensions
    {
        /// <summary>
        /// 将异常转为 <seealso cref="ApRequestException"/>
        /// </summary>
        /// <param name="exception">转换前的异常</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static ApRequestException RequestException(this Exception exception, int errorCode)
            => new ApRequestException(errorCode, exception?.Message ?? "未知异常", exception);

        /// <summary>
        /// 将异常转为 <seealso cref="ApRequestException"/>
        /// </summary>
        /// <param name="exception">转换前的异常</param>
        /// <param name="errorCode">错误码</param>
        public static ApRequestException RequestException(this Exception exception, string errorCode)
            => new ApRequestException(errorCode, exception?.Message ?? "未知异常", exception);
    }
}
