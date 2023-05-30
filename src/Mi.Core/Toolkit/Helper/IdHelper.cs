namespace Mi.Core.Toolkit.Helper
{
    public class IdHelper
    {
        /// <summary>
        /// 雪花ID
        /// </summary>
        /// <returns></returns>
        public static long SnowflakeId() => Snowflake.Instance.NextId();
    }
}