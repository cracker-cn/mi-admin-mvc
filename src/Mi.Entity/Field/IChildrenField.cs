namespace Mi.Entity.Field
{
    public interface IChildrenField<T>
    {
        /// <summary>
        /// 子集
        /// </summary>
        T? Children { get; set; }
    }
}