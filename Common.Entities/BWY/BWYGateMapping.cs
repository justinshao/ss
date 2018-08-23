using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.BWY
{
    public class BWYGateMapping
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParkingName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GateName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime { get; set; } 
        private DataStatus _datastatus;
        private DateTime _lastupdatetime;
        private int _haveupdate;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
        {
            set { _haveupdate = value; }
            get { return _haveupdate; }
        }
        /// <summary>
        /// 车场编号
        /// </summary>
        public string ParkNo { get; set; }
        /// <summary>
        /// 岗亭编号
        /// </summary>
        public string ParkBoxID { get; set; }
        /// <summary>
        /// 岗亭名称
        /// </summary>
        public string ParkBoxName { get; set; }
        /// <summary>
        /// 数据来源 0-泊物云 1-赛菲姆
        /// </summary>
        public int DataSource { get; set; }
    }
}
