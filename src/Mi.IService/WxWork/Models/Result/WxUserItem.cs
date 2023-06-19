using Mi.Core.Models.WxWork;

namespace Mi.IService.WxWork.Models.Result
{
    public class WxUserItem: WxDeptUser
    {
        /// <summary>
        /// 关系用户
        /// </summary>
        public long relation_user_id { get; set; }

        /// <summary>
        /// 关系用户名
        /// </summary>
        public long relation_user_name { get; set; }
    }
}
