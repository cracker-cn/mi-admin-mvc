namespace Mi.Entity.System
{
    [Table("SysDict")]
    public class SysDict : EntityBase, IParentId<long>, ISort, IRemark
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        [NotNull]
        public string? DictName { get; set; }

        /// <summary>
        /// 字典Key，唯一
        /// </summary>
        [NotNull]
        public string? DictKey { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string? DictValue { get; set; }

        public int Sort { get; set; }
        public long ParentId { get; set; }
        public string? Remark { get; set; }
    }
}