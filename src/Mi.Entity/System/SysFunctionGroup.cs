namespace Mi.Entity.System
{
    /// <summary>
    /// 功能分组，便于权限分配
    /// </summary>
    [Table("SysFunctionGroup")]
    public class SysFunctionGroup : EntityBase, IRemark
    {
        /// <summary>
        /// 组名，同名为一组
        /// </summary>
        [NotNull]
        public string? GroupName { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public long FunctionId { get; set; }

        public string? Remark { get; set; }
    }
}