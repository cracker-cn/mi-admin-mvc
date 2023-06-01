namespace Mi.Core.Toolkit.Helper
{
    public class IdHelper
    {
        public static readonly Snowflake snowflake = new Snowflake(3, 4);

        /// <summary>
        /// 雪花ID
        /// </summary>
        /// <returns></returns>
        public static long SnowflakeId() => snowflake.NextId();
    }
}