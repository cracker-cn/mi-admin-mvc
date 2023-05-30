namespace Mi.IService.System
{
    public interface IFunctionGroupService
    {
        Task<MessageModel> AddOrUpdateGroupAsync(FuncGroupOperation operation);
    }
}