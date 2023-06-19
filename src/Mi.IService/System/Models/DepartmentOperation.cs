namespace Mi.IService.System.Models
{
    public class DepartmentOperation
    {
        public string? name { get; set; }

        public string? name_en { get; set; }

        public int parentid { get; set; }

        public int order { get; set; }

        public int dept_id { get; set; }

        public int? id => dept_id > 0 ? dept_id : null;
    }
}
