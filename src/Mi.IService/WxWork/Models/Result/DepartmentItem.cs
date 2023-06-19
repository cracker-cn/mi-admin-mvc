using Mi.Entity.Field;

namespace Mi.IService.WxWork.Models.Result
{
    public class DepartmentItem : IChildren<IList<DepartmentItem>>
    {
        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public string? Leader { get; set; }

        public IList<DepartmentItem>? Children { get; set; }

        public long Id { get; set; }

        public long ParentId { get; set; }
    }
}
