using Mi.Core.Enum;
using Mi.Core.Models;

namespace Mi.Core.Extension
{
    public static class MessageExtension
    {
        public static MessageModel Success(this MessageModel model)
        {
            return new MessageModel(EnumResponseCode.Success, "操作成功");
        }

        public static MessageModel Success(this MessageModel model, string msg)
        {
            return new MessageModel(EnumResponseCode.Success, msg);
        }

        public static MessageModel Fail(this MessageModel model)
        {
            return new MessageModel(EnumResponseCode.Fail, "操作失败");
        }

        public static MessageModel Fail(this MessageModel model, string msg)
        {
            return new MessageModel(EnumResponseCode.Fail, msg);
        }

        public static MessageModel SuccessOrFail(this MessageModel model, bool successed)
        {
            return successed ? model.Success() : model.Fail();
        }

        public static MessageModel ParameterError(this MessageModel model, string msg)
        {
            return new MessageModel(EnumResponseCode.ParameterError, msg);
        }

        public static MessageModel<T> As<T>(this MessageModel model,T? result)
        {
            return new MessageModel<T>(model.Code, model.Message ?? "", result);
        }

        public static MessageModel<T> As<T>(this MessageModel model)
        {
            var data = Activator.CreateInstance<T>();
            return new MessageModel<T>(model.Code, model.Message ?? "",data);
        }

        public static bool EnsureSuccess<T>(this MessageModel<T> model)
        {
            return model.Code == EnumResponseCode.Success && model.Result != null;
        }

        public static bool EnsureSuccess(this MessageModel model)
        {
            return model.Code == EnumResponseCode.Success;
        }
    }
}