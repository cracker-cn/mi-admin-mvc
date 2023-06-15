using Mi.Entity.Field;

namespace Mi.IService.System.Models.Result
{
    public class UserRoleOption : SelectionOption, IRemark
    {
        public string? Remark { get; set; }
    }
}