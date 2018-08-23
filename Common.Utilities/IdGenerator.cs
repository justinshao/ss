namespace Common.Utilities
{
    using System;
    using System.Threading;
    using System.Configuration;
    /// <summary>
    /// 编号生成器
    /// </summary>
    public class IdGenerator
    {
        private static IdGenerator m_instance = null;
        private static object m_assistantObj = new object();
        public static IdGenerator Instance {
            get {
                if (m_instance == null) {
                    lock (m_assistantObj) {
                        if (m_instance == null) {
                            m_instance = new IdGenerator();
                        }
                    }
                }
                return m_instance;
            }
        }

        private byte m_identity;
        private MyIdGenerator m_idGenerator = null;
        private IdGenerator()
        {
            m_idGenerator = new MyIdGenerator(6);
            string identityStr = ConfigurationManager.AppSettings["IdSeed"] ?? "1";
            m_identity = byte.Parse(identityStr);
        }

        public decimal GetId() {
            return m_idGenerator.GetId(m_identity);
        }


        /// <summary>
        /// 生成无车牌的车牌
        /// </summary>
        /// <returns></returns>
        public string BulidNoPlatenumber(DateTime datetime)
        {
            string platenumber = "无车牌_" + datetime.ToString("yyyyMMddHHmmssffff");
            Random random = new Random();
            int randomindex = random.Next(1, 9999);
            platenumber = platenumber + "_" + randomindex;
            return platenumber;
        }
        /// <summary>
        /// Id 生成器.
        /// </summary>
        /// <remarks>
        /// 只能确保在同一个应用程序中，一分钟内生成 10 的 IdGenerator.SeedWidth 次方 -1 个不重复的数字编号。
        /// 在设置 SeedWidth 属性后，不确保与设置之前产生的编号不重复。
        /// SeedWidth 取值范围在 1-15 之间，包括1和15.
        /// 生成数字编号的字符串长度为 10+IdGenerator.SeedWidth + identity的字符串长度，最大长度为28。
        ///</remarks>
        class MyIdGenerator
        {
            public MyIdGenerator(int width)
            {
                if (width <= 0 || width > 15) {
                    throw new ArgumentOutOfRangeException("SeedWidth", "种子宽度超出允许的范围。(0-15)");
                }
                m_width = width;
                m_max = (long)Math.Pow(10, width) - 1;
                if (m_seed > m_max) { m_seed = 0; }
            }
            private long m_seed = 0;
            private long m_max = 0;
            private int m_width = 0;
            private object locker = new object();
            public decimal GetId(byte identity) {
                decimal prefix = (decimal)(identity * Math.Pow(10, m_width + 10));
                decimal stamp = decimal.Parse(DateTime.Now.ToString("yyMMddHHmm")) * (decimal)Math.Pow(10, m_width);
                lock (locker) {
                    Interlocked.Increment(ref m_seed);
                    decimal id = prefix + stamp + m_seed;
                    if (m_seed > m_max) { m_seed = 0; }
                    return id;
                }
            }
        }
    }
}
