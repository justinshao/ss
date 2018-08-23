 
    /// <summary>
    /// 字节数组扩展类
    /// </summary>
    public static class BytesExtension
    {
        /// <summary>
        /// 将二进制数据转化为十六进制字符串
        /// </summary>
        /// <param name="bytes">二进制数据</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHexString(this byte[] bytes)
        {
            string s = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符,2表示2位
                s = s + bytes[i].ToString("X2");
            }
            return s;
        }
    } 