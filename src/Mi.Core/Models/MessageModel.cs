using Mi.Core.Enum;

namespace Mi.Core.Models
{
	public class MessageModel
	{
		public EnumResponseCode Code { get; set; }
		public string? Message { get; set; }

		public MessageModel()
		{ }

		public MessageModel(EnumResponseCode code, string? message)
		{
			Code = code;
			Message = message;
		}
	}

	public class MessageModel<T> : MessageModel
	{
		public T? Result { get; set; }

		public MessageModel()
		{ }

		public MessageModel(EnumResponseCode code, string msg, T? result)
		{
			Code = code;
			Message = msg;
			Result = result;
		}

		public MessageModel(bool successed, string msg, T? result)
		{
			Code = successed ? EnumResponseCode.Success : EnumResponseCode.Fail;
			Message = msg;
			Result = result;
		}
	}
}