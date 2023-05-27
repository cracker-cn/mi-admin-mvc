namespace Mi.Entity.System
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("SysRole")]
    public class SysRole : EntityBase, IRemarkField
    {
        public string? RoleName { get; set; }

        public string? Remark { get; set; }
    }
}