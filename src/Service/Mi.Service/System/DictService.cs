using System.Text;

using AutoMapper;

using Dapper;

using Mi.Core.GlobalVar;
using Mi.Core.Service;
using Mi.Core.Toolkit.API;
using Mi.Entity.Field;
using Mi.IService.System.Models.Result;
using Mi.Repository.BASE;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.Service.System
{
    public class DictService : IDictService, IScoped
    {
        private readonly IDictRepository _dictRepository;
        private readonly IMemoryCache _cache;
        private readonly IMiUser _miUser;
        private readonly IMapper _mapper;
        private readonly MessageModel _message;

        public DictService(IDictRepository dictRepository, IMemoryCache cache, IMiUser miUser, IMapper mapper
            , MessageModel message)
        {
            _dictRepository = dictRepository;
            _cache = cache;
            _miUser = miUser;
            _mapper = mapper;
            _message = message;
        }

        #region Admin_UI

        public async Task<MessageModel<PagingModel<DictItem>>> GetDictListAsync(DictSearch search)
        {
            var repo = DotNetService.Get<Repository<DictItem>>();
            var sql = new StringBuilder(@"select d.*,(select count(*) from SysDict where id = d.ParentId) ChildCount,(select name from SysDict where id=d.ParentId) ParentName from SysDict d where d.IsDeleted = 0");
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(search.Vague))
            {
                sql.Append(" and ( d.name like @text or d.key like @text )");
                parameters.Add("text", "%" + search.Vague + "%");
            }
            if (!string.IsNullOrEmpty(search.Remark))
            {
                sql.Append(" and d.remark like @remark ");
                parameters.Add("remark", "%" + search.Remark + "%");
            }
            if(search.ParentId.HasValue && search.ParentId > 0)
            {
                sql.Append(" and d.ParentId = @parentId ");
                parameters.Add("parentId", search.ParentId);
            }

            return new MessageModel<PagingModel<DictItem>>(true, await repo.GetPagingAsync(search, sql.ToString(), parameters));
        }

        public async Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation)
        {
            if (operation.Id <= 0)
            {
                var dict = _mapper.Map<SysDict>(operation);
                dict.Id = IdHelper.SnowflakeId();
                dict.CreatedBy = _miUser.UserId;
                dict.CreatedOn = TimeHelper.LocalTime();
                if (dict.ParentId > 0)
                {
                    dict.ParentKey = _dictRepository.Get(dict.ParentId).Key;
                }
                await _dictRepository.AddAsync(dict);
            }
            else
            {
                var dict = await _dictRepository.GetAsync(operation.Id);
                if (operation.ParentId > 0 && operation.ParentId != dict.ParentId)
                {
                    dict.ParentKey = _dictRepository.Get(dict.ParentId).Key;
                }
                operation.CopyTo(dict, "Id");
                await _dictRepository.UpdateAsync(dict);
            }

            return _message.Success();
        }

        public async Task<MessageModel> RemoveDictAsync(IList<string> ids)
        {
            var list = await _dictRepository.GetAllAsync(x => ids.Contains(x.Id.ToString()));
            var now = TimeHelper.LocalTime();
            foreach (var item in list)
            {
                item.ModifiedBy = _miUser.UserId;
                item.ModifiedOn = now;
                item.IsDeleted = 1;
            }

            var flag = await _dictRepository.UpdateManyAsync(list);

            if (flag)
            {
                _cache.Remove(CacheConst.DICT);
            }
            return _message.SuccessOrFail(flag);
        }

        public async Task<MessageModel<SysDict>> GetAsync(long id)
        {
            var dict = await _dictRepository.GetAsync(id);

            return new MessageModel<SysDict>(dict);
        }

        public List<SysDict> GetAll() => _dictRepository.GetAll().ToList();

        public async Task<List<Option>> GetParentListAsync()
        {
            var sql = "select Name,Id AS Value from SysDict where IsDeleted = 0 and Id in (select ParentId from SysDict where IsDeleted = 0) ";
            var repo = DotNetService.Get<Repository<Option>>();
            
            return await repo.GetListAsync(sql);
        }

        #endregion Admin_UI
    }
}