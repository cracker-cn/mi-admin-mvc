namespace Mi.Entity.Field
{
    public interface ISortField
    {
        /// <summary>
        /// 排序值，越小越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}