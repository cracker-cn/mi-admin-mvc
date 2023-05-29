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

        public static MessageModel SuccessOrFail(this MessageModel model,bool successed)
        {
            return successed ? model.Success() : model.Fail();
        }

        public static MessageModel ParameterError(this MessageModel model, string msg)
		{
			return new MessageModel(EnumResponseCode.ParameterError, msg);
		}

		public static bool EnsureSuccess<T>(this MessageModel<T> model)
		{
			return model.Code == EnumResponseCode.Success && model.Result != null;
		}
	}
}