using AutoMapper;

using Mi.Core.Factory;
using Mi.Entity.System.Enum;

namespace Mi.Service.System
{
    public class FunctionService : IFunctionService, IScoped
    {
        private readonly IMapper _mapper;
        private readonly MessageModel _message;
        private readonly IMiUser _miUser;
        private readonly IFunctionRepository _functionRepository;
        private readonly CreatorFactory _creatorFactory;
        private readonly IList<SysFunction> _allFunctions;

        public FunctionService(IMapper mapper, MessageModel message, IMiUser miUser
            , IFunctionRepository functionRepository
            , CreatorFactory creatorFactory)
        {
            _mapper = mapper;
            _message = message;
            _miUser = miUser;
            _functionRepository = functionRepository;
            _creatorFactory = creatorFactory;
            _allFunctions = functionRepository.GetAll();
        }

        public async Task<MessageModel> AddOrUpdateFunctionAsync(FunctionOperation operation)
        {
            if (operation.Id <= 0)
            {
                var func = _mapper.Map<SysFunction>(operation);
                func.CreatedBy = _miUser.UserId;
                func.CreatedOn = TimeHelper.LocalTime();
                func.Id = IdHelper.SnowflakeId();
                func.Node = CheckFunctionNode(func);
                if (func.ParentId > 0)
                {
                    var parent = await _functionRepository.GetAsync(operation.ParentId);
                    if (parent.Id > 0)
                    {
                        parent.Children += $",{func.Id}";
                        parent.Node = CheckFunctionNode(parent);
                    }
                }
                await _functionRepository.AddAsync(func);
            }
            else
            {
                var func = _functionRepository.Get(operation.Id);
                operation.CopyTo(func, "Id");
                func.ModifiedBy = _miUser.UserId;
                func.ModifiedOn = TimeHelper.LocalTime();
                await _functionRepository.UpdateAsync(func);
            }

            return _message.Success();
        }

        public EnumTreeNode CheckFunctionNode(SysFunction node)
        {
            var hasChildren = !string.IsNullOrEmpty(node.Children) && node.Children.Split(",").Length > 0;
            var hasParent = node.ParentId > 0;

            if (hasParent && hasChildren)
                return EnumTreeNode.ChildNode;
            else if (hasParent && !hasChildren)
                return EnumTreeNode.LeafNode;

            return EnumTreeNode.RootNode;
        }

        public Task<SysFunction> GetAsync(long id)
        {
            return _functionRepository.GetAsync(id);
        }

        public async Task<IList<SysFunction>> GetFunctionListAsync(FunctionSearch search)
        {
            var exp = ExpressionCreator.New<SysFunction>()
                .AndIf(!string.IsNullOrEmpty(search.FunctionName), x => x.FunctionName.Contains(search.FunctionName!))
                .AndIf(!string.IsNullOrEmpty(search.Url), x => x.Url != null && x.Url.Contains(search.Url!));

            var list = await _functionRepository.GetAllAsync(exp);

            return list;
        }

        public IList<TreeOption> GetFunctionTree()
        {
            var topLevels = _allFunctions.Where(x => x.Node == EnumTreeNode.RootNode);
            return topLevels.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }

        private IList<TreeOption> GetFunctionChildNode(long id)
        {
            var children = _allFunctions.Where(x => x.Node != EnumTreeNode.RootNode && x.ParentId == id);
            return children.Select(x => new TreeOption
            {
                Name = x.FunctionName,
                Value = x.Id.ToString(),
                Children = GetFunctionChildNode(x.Id)
            }).ToList();
        }
    }
}