﻿using System.Text;

using AutoMapper;

using Dapper;

using Mi.Core.GlobalVar;
using Mi.Core.Service;
using Mi.Core.Toolkit.API;
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

        public async Task<IList<Option>> GetOptionsAsync(string key)
        {
            return await Task.FromResult(_Options.Where(x => x.Name == key).ToList());
        }

        public async Task<string> GetValueAsync(string key)
        {
            var dict = await _dictRepository.GetAsync(x => x.Key == key);
            return dict.Value ?? "";
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            var dict = await _dictRepository.GetAsync(x => x.Key == key);
            dict.Value = value;
            dict.ModifiedOn = TimeHelper.LocalTime();
            dict.ModifiedBy = _miUser.UserId;

            if (await _dictRepository.UpdateAsync(dict))
            {
                _cache.Remove(CacheKeyConst.DICT);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Dictionary<string, string> keyValues)
        {
            var list = new List<SysDict>();
            var now = TimeHelper.LocalTime();
            foreach (var kvp in keyValues)
            {
                var item = await _dictRepository.GetAsync(x => x.Key == kvp.Key);
                item.Value = kvp.Value;
                item.ModifiedOn = now;
                item.ModifiedBy = _miUser.UserId;
                list.Add(item);
            }

            if (await _dictRepository.UpdateManyAsync(list))
            {
                _cache.Remove(CacheKeyConst.DICT);
                return true;
            }

            return false;
        }

        private List<Option> GetOptions()
        {
            return _dictRepository.GetAll().Select(x => new Option { Name = x.Key, Value = x.Value }).ToList();
        }

        private List<Option> _Options => _cache.GetOrCreate(CacheKeyConst.DICT, opt => GetOptions()) ?? GetOptions();

        #region Admin_UI

        public async Task<MessageModel<PagingModel<SysDictItem>>> GetDictListAsync(DictSearch search)
        {
            var repo = DotNetService.Get<Repository<SysDictItem>>();
            var sql = new StringBuilder(@"select d.*,(select count(*) from SysDict where id = d.ParentId) ChildCount from SysDict d where d.IsDeleted = 0");
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

            return new MessageModel<PagingModel<SysDictItem>>(true, await repo.GetPagingAsync(search, sql.ToString(), parameters));
        }

        public async Task<MessageModel> AddOrUpdateDictAsync(DictOperation operation)
        {
            if (operation.DictId <= 0)
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
                var dict = await _dictRepository.GetAsync(operation.DictId);
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
                _cache.Remove(CacheKeyConst.DICT);
            }
            return _message.SuccessOrFail(flag);
        }

        #endregion Admin_UI
    }
}