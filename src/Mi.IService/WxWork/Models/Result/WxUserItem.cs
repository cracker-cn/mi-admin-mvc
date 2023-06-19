namespace Mi.IService.WxWork.Models.Result
{
    public class WxUserItem
    {
        public string? userid { get; set; }

        public string? name { get; set; }

        public IList<int>? department { get; set; }

        public string? open_userid { get; set; }

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
