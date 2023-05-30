using Mi.Core.Factory;
using Mi.IRepository.BASE;

namespace Mi.Service.System
{
    public class FunctionGroupService : IFunctionGroupService, IScoped
    {
        private readonly MessageModel _message;
        private readonly IMiUser _miUser;
        private readonly CreatorFactory _creatorFactory;
        private readonly IRepositoryBase<SysFunctionGroup> _groupRepository;

        public FunctionGroupService(MessageModel message, IMiUser miUser, CreatorFactory creatorFactory, IRepositoryBase<SysFunctionGroup> groupRepository)
        {
            _message = message;
            _miUser = miUser;
            _creatorFactory = creatorFactory;
            _groupRepository = groupRepository;
        }

        public async Task<MessageModel> AddOrUpdateGroupAsync(FuncGroupOperation operation)
        {
            if (operation.FunctionIds.Count == 0) return _message.ParameterError("分组下功能是必需的");

            var model = await _groupRepository.GetAsync(operation.GroupId);
            if (model.Id > 0)
            {
                await _groupRepository.ExecuteAsync("delete from SysFunctionGroup where Name=@name", new { name = operation.GroupName });
            }

            var list = new List<SysFunctionGroup>();
            foreach (var id in operation.FunctionIds)
            {
                var item = _creatorFactory.New<SysFunctionGroup>();
                item.GroupName = operation.GroupName;
                item.Remark = operation.Remark;
                item.FunctionId = id;
                list.Add(item);
            }
            await _groupRepository.AddManyAsync(list);

            return _message.Success();
        }
    }
}