using AutoMapper;

using Mi.Core.Models.WxWork;
using Mi.Core.Others;
using Mi.IService.WxWork;
using Mi.IService.WxWork.Models.Result;

namespace Mi.Application.WxWork
{
    public class WxUserService : IWxUserService, IScoped
    {
        private readonly WxWorkRequest _request;
        private readonly WxWorkConfig _config;
        private readonly MessageModel _msg;
        private readonly IMapper _mapper;

        public WxUserService(WxWorkRequest request, WxWorkConfig config, MessageModel msg, IMapper mapper)
        {
            _request = request;
            _config = config;
            _msg = msg;
            _mapper = mapper;
        }

        public async Task<MessageModel<IList<WxUserItem>>> GetDeptMemberListAsync(long deptId)
        {
            var url = "user/simplelist";
            var result = await _request.GetAsync<WxWorkDeptUser>(url, _config.wx_work_contact_list_secret, "department_id=" + deptId);
            var list = new List<WxUserItem>();
            if (!result.Succeed() || result.userlist == null) return _msg.Fail().As<IList<WxUserItem>>(list);
            foreach (var item in result.userlist)
            {
                var model = _mapper.Map<WxUserItem>(item);
                list.Add(model);
            }
            return _msg.Success().As<IList<WxUserItem>>(list);
        }
    }
}
