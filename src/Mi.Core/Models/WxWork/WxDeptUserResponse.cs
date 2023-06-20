namespace Mi.Core.Models.WxWork
{
    public class WxDeptUserResponse : WxApiResponseBase
    {
        public IList<WxDeptUser>? userlist { get; set; }
    }

    public class WxDeptUser
    {
        public string? userid { get; set; }

        public string? name { get; set; }

        public IList<int>? department { get; set; }

        public string? open_userid { get; set; }
    }
}
