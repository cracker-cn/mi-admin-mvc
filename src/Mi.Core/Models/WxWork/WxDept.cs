using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Models.WxWork
{
    public class WxDept : WxApiResponseBase
    {
        public IList<Department>? department { get; set; }
    }

    public class Department
    {
        public int id { get; set; }

        [NotNull]
        public string? name { get; set; }

        public int parentid { get; set; }

        public int order { get; set; }

        [NotNull]
        public IList<string>? department_leader { get; set; }

        public string? name_en { get; set; }
    }
}
