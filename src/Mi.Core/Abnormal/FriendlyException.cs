using Mi.Core.Enum;

namespace Mi.Core.Abnormal
{
    /// <summary>
    /// 友好异常，异常级别<see cref="EnumResponseCode.Fail"/>
    /// </summary>
    public class FriendlyException : Exception
    {
        public FriendlyException(string message) : base(message)
        {
            Message = message;
        }

        public new int HResult => (int)EnumResponseCode.Fail;

        public new string Message { get; }

        public new Exception? InnerException => null;
    }
}