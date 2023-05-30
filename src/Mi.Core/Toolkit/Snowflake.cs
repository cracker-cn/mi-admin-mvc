namespace Mi.Core.Toolkit
{
    /// <summary>
    /// 雪花算法
    /// </summary>
    /// <remarks>
    ///     最高位是符号位，始终为0，不可用。
    ///     41位的时间序列，精确到毫秒级，41位的长度可以使用69年。时间位还有一个很重要的作用是可以根据时间进行排序。
    ///     10位的机器标识，10位的长度最多支持部署1024个节点。
    ///     12位的计数序列号，序列号即一系列的自增id，可以支持同一节点同一毫秒生成多个ID序号，12位的计数序列号支持每个节点每毫秒产生4096个ID序号。
    /// </remarks>
    public class Snowflake
    {
        private static Lazy<Snowflake> lazyInstance => new Lazy<Snowflake>(() => new Snowflake(2, 3));
        public static Snowflake Instance => lazyInstance.Value;

        /// <summary>
        ///     雪花ID
        /// </summary>
        /// <param name="workerId">工作机器ID</param>
        /// <param name="datacenterId">数据中心ID</param>
        public Snowflake(long workerId, long datacenterId)
        {
            if (workerId > maxWorkerId || workerId < 0)
                throw new ArgumentOutOfRangeException(nameof(workerId), $"不能大于 {maxWorkerId} 或小于 0");
            if (datacenterId > maxDatacenterId || datacenterId < 0)
                throw new ArgumentOutOfRangeException(nameof(datacenterId), $"不能大于 {maxDatacenterId} 或小于 0");

            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = 0L;
            LastTimestamp = -1L;
        }

        #region 属性

        // 机器id所占的位数
        private const int workerIdBits = 5;

        // 数据标识id所占的位数
        private const int datacenterIdBits = 5;

        // 支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数)
        private const long maxWorkerId = -1L ^ -1L << workerIdBits;

        // 支持的最大数据标识id，结果是31
        private const long maxDatacenterId = -1L ^ -1L << datacenterIdBits;

        // 序列在id中占的位数
        private const int sequenceBits = 12;

        // 数据标识id向左移17位(12+5)
        private const int datacenterIdShift = sequenceBits + workerIdBits;

        // 机器ID向左移12位
        private const int workerIdShift = sequenceBits;

        // 时间截向左移22位(5+5+12)
        private const int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

        // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095)
        private const long sequenceMask = -1L ^ -1L << sequenceBits;

        /// <summary>
        ///     数据中心ID(0~31)
        /// </summary>
        public long DatacenterId { get; }

        /// <summary>
        ///     工作机器ID(0~31)
        /// </summary>
        public long WorkerId { get; }

        /// <summary>
        ///     毫秒内序列(0~4095)
        /// </summary>
        public long Sequence { get; private set; }

        /// <summary>
        ///     上次生成ID的时间截
        /// </summary>
        public long LastTimestamp { get; private set; }

        /// <summary>
        ///     开始时间戳。首次使用前设置，否则无效，默认2010-1-1
        /// </summary>
        public DateTime StartTimestamp { get; set; } = new(2010, 1, 1);

        private static readonly object syncRoot = new();

        #endregion 属性

        #region 核心方法

        /// <summary>
        ///     获得下一个ID，线程安全，19位有序数字
        /// </summary>
        /// <returns></returns>
        public long NextId()
        {
            lock (syncRoot)
            {
                var timestamp = GetCurrentTimestamp();
                if (timestamp > LastTimestamp) // 时间戳改变，毫秒内序列重置
                {
                    Sequence = 0L;
                }
                else if (timestamp == LastTimestamp) // 如果是同一时间生成的，则进行毫秒内序列
                {
                    Sequence = Sequence + 1 & sequenceMask;
                    if (Sequence == 0) // 毫秒内序列溢出
                        timestamp = GetNextTimestamp(LastTimestamp); // 阻塞到下一个毫秒,获得新的时间戳
                }
                else // 当前时间小于上一次ID生成的时间戳，证明系统时钟被回拨，此时需要做回拨处理
                {
                    Sequence = Sequence + 1 & sequenceMask;
                    if (Sequence > 0)
                        timestamp = LastTimestamp; // 停留在最后一次时间戳上，等待系统时间追上后即完全度过了时钟回拨问题。
                    else // 毫秒内序列溢出
                        timestamp = LastTimestamp + 1; // 直接进位到下一个毫秒
                }

                LastTimestamp = timestamp; // 上次生成ID的时间截

                // 移位并通过或运算拼到一起组成64位的ID
                var id = timestamp << timestampLeftShift
                         | DatacenterId << datacenterIdShift
                         | WorkerId << workerIdShift
                         | Sequence;
                return id;
            }
        }

        /// <summary>尝试分析</summary>
        /// <param name="id"></param>
        /// <param name="time">时间</param>
        /// <param name="workerId">节点</param>
        /// <param name="datacenterId">数据中心</param>
        /// <param name="sequence">序列号</param>
        /// <returns></returns>
        public bool TryParse(long id, out DateTime time, out int workerId, out int datacenterId, out int sequence)
        {
            var timestamp = id >> timestampLeftShift;
            time = StartTimestamp.AddMilliseconds(timestamp);

            var dataId = (id ^ timestamp << timestampLeftShift) >> datacenterIdShift;
            var workId = (id ^ (timestamp << timestampLeftShift | dataId << datacenterIdShift)) >> workerIdShift;
            var seq = id & sequenceMask;

            datacenterId = (int)dataId;
            workerId = (int)workId;
            sequence = (int)seq;

            return true;
        }

        /// <summary>
        ///     阻塞到下一个毫秒，直到获得新的时间戳
        /// </summary>
        /// <param name="lastTimestamp">上次生成ID的时间截</param>
        /// <returns>当前时间戳</returns>
        private long GetNextTimestamp(long lastTimestamp)
        {
            var timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp) timestamp = GetCurrentTimestamp();
            return timestamp;
        }

        /// <summary>
        ///     获取当前时间戳
        /// </summary>
        /// <returns></returns>
        private long GetCurrentTimestamp()
        {
            return (long)Math.Round((DateTime.Now - StartTimestamp).TotalMicroseconds);
        }

        #endregion 核心方法
    }
}