namespace Mi.Core.Toolkit.Helper
{
    public class IdHelper
    {
        private static readonly Snowflake snowflake = new Snowflake(3, 4);

        /// <summary>
        /// 雪花ID
        /// </summary>
        /// <returns></returns>
        public static long SnowflakeId() => snowflake.NextId();

        public static string UUID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}