namespace Mi.Entity.Field
{
    public interface IParentIdField<T>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        T ParentId { get; set; }
    }
}