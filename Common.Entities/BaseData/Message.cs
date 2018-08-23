using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities 
{
    /// <summary>
    /// 类Message 客户端通知。
    /// </summary>
    [Serializable]
    public partial class Message
    {
        public Message()
        { }

        private int _id;
        private string _recordid;
        private string _messagetitle;
        private string _messagetxt; 
        private DateTime _lastupdatetime;
        private int _messagestates;
        private int _poststates;
        private DataStatus _datastatus;
        private string _userid;
        private string _username;


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
        {
            set { _recordid = value; }
            get { return _recordid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MessageTitle
        {
            set { _messagetitle = value; }
            get { return _messagetitle; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MessageTxt
        {
            set { _messagetxt = value; }
            get { return _messagetxt; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
        {
            set { _datastatus = value; }
            get { return _datastatus; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        public string LastUpdateTimeToString
        {
            get
            {
                return LastUpdateTime.ToyyyyMMddHHmmss();
            }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MessageStates
        {
            set { _messagestates = value; }
            get { return _messagestates; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PostStates
        {
            set { _poststates = value; }
            get { return _poststates; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
    }
}
