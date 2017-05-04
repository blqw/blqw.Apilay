using System;

namespace blqw.EasyWebInvoker
{
    /// <summary>
    /// 请求异常
    /// </summary>
    public class EyRequestException : Exception
    {
        /// <summary>
        /// 异常码
        /// </summary>
        public string ErrCode { get; }

        /// <summary>
        /// 请求异常
        /// </summary>
        /// <param name="errcode">异常码</param>
        /// <param name="message">异常消息</param>
        public EyRequestException(int errcode, string message)
            : base(message)
        {
            HResult = errcode;
            ErrCode = errcode.ToString();
        }

        /// <summary>
        /// 请求异常
        /// </summary>
        /// <param name="errcode">异常码</param>
        /// <param name="message">异常消息</param>
        /// <param name="inner">内部异常</param>
        public EyRequestException(int errcode, string message, Exception inner)
            : base(message, inner)
        {
            HResult = errcode;
            ErrCode = errcode.ToString();
        }

        /// <summary>
        /// 请求异常
        /// </summary>
        /// <param name="errcode">异常码</param>
        /// <param name="message">异常消息</param>
        public EyRequestException(string errcode, string message)
            : base(message)
        {
            ErrCode = errcode;
        }

        /// <summary>
        /// 请求异常
        /// </summary>
        /// <param name="errcode">异常码</param>
        /// <param name="message">异常消息</param>
        /// <param name="inner">内部异常</param>
        public EyRequestException(string errcode, string message, Exception inner)
            : base(message, inner)
        {
            ErrCode = errcode;
        }
    }
}
