namespace Mi.Core.GlobalVar
{
    public static class CacheConst
    {
        public const string DICT = "all_dict";

        public const string FUNCTION = "all_function";

        public readonly static TimeSpan Week = TimeSpan.FromDays(7);

        public readonly static TimeSpan Year = TimeSpan.FromDays(366);

        public readonly static string CONTROLLER_TYPES = nameof(CONTROLLER_TYPES).ToLower();
    }
}