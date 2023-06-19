using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Models.WxWork
{
    public class WxWorkConfig
    {
        [NotNull]
        public string? corpid { get; set; }

        [NotNull]
        public string? wx_work_member_secret { get; set; }

        [NotNull]
        public string? wx_work_contact_list_secret { get; set; }
    }
}
