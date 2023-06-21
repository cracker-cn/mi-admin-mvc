using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Models.WxWork
{
    public class WxConfig
    {
        [NotNull]
        public string? corpid { get; set; }

        [NotNull]
        public string? wx_work_contact_list_secret_sync { get; set; }

        [NotNull]
        public string? wx_work_contact_list_secret { get; set; }
    }
}
