namespace Mi.Entity.Field
{
    public interface IChildren<T>
    {
        /// <summary>
        /// 子集
        /// </summary>
        T? Children { get; set; }
    }
}