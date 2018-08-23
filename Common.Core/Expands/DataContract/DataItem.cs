namespace Common.Core.Expands.DataContract
{
    /// <summary>
    /// 数据项
    /// </summary>
    [System.Runtime.Serialization.DataContract(IsReference=true)]    
    public class DataItem
    {
        /// <summary>
        /// 数据项名称
        /// </summary>
        [System.Runtime.Serialization.DataMember()]        
        public string Name { get; set; }
        /// <summary>
        /// 数据项值
        /// </summary>
        [System.Runtime.Serialization.DataMember()]
        public string Value { get; set; }

        /// <summary>
        /// 数据项值
        /// </summary>
        [System.Runtime.Serialization.DataMember()]
        public string ValueType { get; set; }
    }
}
