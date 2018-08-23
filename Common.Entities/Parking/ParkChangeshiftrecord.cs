using Common.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Common.Entities.Parking
{
    //ParkChangeshiftrecord
    public class ParkChangeshiftrecord : INotifyPropertyChanged
    {
        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
   		#endregion   
      	/// <summary>
		/// auto_increment
        /// </summary>		
		private int _id;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
        {
            get{ return _id; }
            set{  
            	
                if (_id != value)
                {
                    _id  = value;
                    Notify("ID");
                }
            }
        }

        private string _recordID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string RecordID
        {
            get { return _recordID; }
            set
            {

                if (_recordID != value)
                {
                    _recordID = value;
                    Notify("RecordID");
                }
            }
        }
		/// <summary>
		/// 当班人ID
        /// </summary>		
        private string _userid;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserID
        {
            get{ return _userid; }
            set{  
            	
                if (_userid != value)
                {
                    _userid  = value;
                    Notify("UserID");
                }
            }
        }            
		/// <summary>
		/// StartWorkTime
        /// </summary>		
		private DateTime _startworktime;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime StartWorkTime
        {
            get{ return _startworktime; }
            set{  
            	
                if (_startworktime != value)
                {
                    _startworktime  = value;
                    Notify("StartWorkTime");
                }
            }
        }        
		/// <summary>
		/// EndWorkTime
        /// </summary>		
		private DateTime _endworktime;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime EndWorkTime
        {
            get{ return _endworktime; }
            set{  
            	
                if (_endworktime != value)
                {
                    _endworktime  = value;
                    Notify("EndWorkTime");
                }
            }
        }          
		/// <summary>
        /// BoxID
        /// </summary>		
		private string _boxID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 岗亭ID
        /// </summary>
        public string BoxID
        {
            get { return _boxID; }
            set{

                if (_boxID != value)
                {
                    _boxID = value;
                    Notify("BoxID");
                }
            }
        }

        /// <summary>
        /// 车场Id
        /// </summary>		
        private string _pkID;
        /// <summary>
        /// 车场Id
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID
        {
            get { return _pkID; }
            set
            {

                if (_pkID != value)
                {
                    _pkID = value;
                    Notify("PKID");
                }
            }
        }
             
		/// <summary>
		/// Update_Time
        /// </summary>		
		private DateTime _update_time;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
        {
            get{ return _update_time; }
            set{  
            	
                if (_update_time != value)
                {
                    _update_time  = value;
                    Notify("Update_Time");
                }
            }
        }        
		/// <summary>
		/// IsUpdate
        /// </summary>		
		private int _isupdate;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
        {
            get{ return _isupdate; }
            set{  
            	
                if (_isupdate != value)
                {
                    _isupdate  = value;
                    Notify("IsUpdate");
                }
            }
        }        
		


        private DataStatus _datastatus ;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 状态0默认、1修改、2删除
        /// </summary>
        public DataStatus DataStatus
        {
            get { return _datastatus; }
            set { _datastatus = value; }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BoxName { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { set; get; }
	}
}

