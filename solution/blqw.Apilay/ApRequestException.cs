using System;

namespace blqw.Apilay
{
    /// <summary>
    /// 请求异常
    /// </summary>
    public class ApRequestException : Exception
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
        public ApRequestException(int errcode, string message)
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
        public ApRequestException(int errcode, string message, Exception inner)
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
        public ApRequestException(string errcode, string message)
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
        public ApRequestException(string errcode, string message, Exception inner)
            : base(message, inner)
        {
            ErrCode = errcode;
        }
    }
}
