namespace Mi.Core.Toolkit.Helper
{
    public class TimeHelper
    {
        public static DateTime LocalTime()
        {
            return DateTime.Now.ToLocalTime();
        }

        public static long Timestamp()
        {
            return (long)LocalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}