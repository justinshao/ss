using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类SysUserRoles。
	/// </summary>
	[Serializable]
	public partial class SysUserRolesMapping
	{
        public SysUserRolesMapping()
		{}
		#region Model
		private int _id;
		private string _recordid;
		private string _userrecordid;
		private string _roleid;
		private DateTime _lastupdatetime;
        private int _haveupdate;
		private DataStatus _datastatus;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyNo { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate2 { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecordID
		{
			set{ _recordid=value;}
			get{return _recordid;}
		}
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserRecordID
		{
			set{ _userrecordid=value;}
			get{return _userrecordid;}
		}
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
		#endregion Model
	}
}

