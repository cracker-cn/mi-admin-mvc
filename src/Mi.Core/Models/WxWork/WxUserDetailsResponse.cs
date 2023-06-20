namespace Mi.Core.Models.WxWork
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public class WxUserDetailsResponse : WxApiResponseBase
    {
        public IList<WxUserDetailslist>? userlist { get; set; }
    }

    public class WxUserDetailsAttr
    {
        public int type { get; set; }
        public string? name { get; set; }
        public WxUserDetailsText? text { get; set; }
        public WxUserDetailsWeb? web { get; set; }
    }

    public class WxUserDetailsExtattr
    {
        public IList<WxUserDetailsAttr>? attrs { get; set; }
    }

    public class ExternalAttr
    {
        public int type { get; set; }
        public string? name { get; set; }
        public WxUserDetailsText? text { get; set; }
        public WxUserDetailsWeb? web { get; set; }
        public WxUserDetailsMiniprogram? miniprogram { get; set; }
    }

    public class WxUserDetailsExternalProfile
    {
        public string? external_corp_name { get; set; }
        public WechatChannels wechat_channels { get; set; }
        public List<ExternalAttr> external_attr { get; set; }
    }

    public class WxUserDetailsMiniprogram
    {
        public string appid { get; set; }
        public string pagepath { get; set; }
        public string title { get; set; }
    }

    public class WxUserDetailsText
    {
        public string value { get; set; }
    }

    public class WxUserDetailslist
    {
        public string? userid { get; set; }
        public string? name { get; set; }
        public IList<int>? department { get; set; }
        public IList<int>? order { get; set; }
        public string? position { get; set; }
        public string? mobile { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? biz_mail { get; set; }

        public IList<int>? is_leader_in_dept { get; set; }

        public IList<string>? direct_leader { get; set; }

        public string? avatar { get; set; }

        public string thumb_avatar { get; set; }

        public string telephone { get; set; }

        public string alias { get; set; }

        public int status { get; set; }

        public string address { get; set; }

        public string english_name { get; set; }

        public string open_userid { get; set; }

        public int main_department { get; set; }

        public WxUserDetailsExtattr extattr { get; set; }

        public string qr_code { get; set; }

        public string external_position { get; set; }

        public WxUserDetailsExternalProfile external_profile { get; set; }
    }

    public class WxUserDetailsWeb
    {
        public string url { get; set; }

        public string title { get; set; }
    }

    public class WechatChannels
    {
        public string nickname { get; set; }

        public int status { get; set; }
    }
}
