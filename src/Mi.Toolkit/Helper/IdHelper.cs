using Mi.Toolkit.Core;

namespace Mi.Toolkit.Helper
{
    public class IdHelper
    {
        /// <summary>
        /// 雪花ID
        /// </summary>
        /// <returns></returns>
        public static long SnowflakeId() => new Snowflake(1,2).NextId();
    }
}