using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类BaseEmployee。
	/// </summary>
	[Serializable]
	public partial class BaseEmployee
	{
		public BaseEmployee()
		{}
		#region Model
		private int _id;
		private string _employeeid;
		private string _employeename;
		private string _sex;
        private CertifType _certiftype;
		private string _certifno;
		private string _mobilephone;
		private string _homephone;
		private string _email;
		private string _familyaddr;
		private DateTime _regtime;
        private EmployeeType _employeetype;
		private string _remark;
		private DateTime _lastupdatetime;
        private int _haveupdate;
        private DataStatus _datastatus;
		private string _areaname;
		private string _deptid;
		private string _vid;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 人员ID
        /// </summary>
        public string EmployeeID
		{
			set{ _employeeid=value;}
			get{return _employeeid;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 人员名称
        /// </summary>
        public string EmployeeName
		{
			set{ _employeename=value;}
			get{return _employeename;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
		{
			set{ _sex=value;}
			get{return _sex;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 类型(1身份证,2驾驶证,3居住证..)
        /// </summary>
        public CertifType CertifType
		{
			set{ _certiftype=value;}
			get{return _certiftype;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertifNo
		{
			set{ _certifno=value;}
			get{return _certifno;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobilePhone
		{
			set{ _mobilephone=value;}
			get{return _mobilephone;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 家庭电话
        /// </summary>
        public string HomePhone
		{
			set{ _homephone=value;}
			get{return _homephone;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
		{
			set{ _email=value;}
			get{return _email;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string FamilyAddr
		{
			set{ _familyaddr=value;}
			get{return _familyaddr;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime
		{
			set{ _regtime=value;}
			get{return _regtime;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 人员类型(1业主,2职员,3业主和职员)
        /// </summary>
        public EmployeeType EmployeeType
		{
			set{ _employeetype=value;}
			get{return _employeetype;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int HaveUpdate
		{
			set{ _haveupdate=value;}
			get{return _haveupdate;}
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public DataStatus DataStatus
		{
			set{ _datastatus=value;}
			get{return _datastatus;}
		}
		/// <summary>
		/// 
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string AreaName
		{
			set{ _areaname=value;}
			get{return _areaname;}
		}
		/// <summary>
		/// 
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string DeptID
		{
			set{ _deptid=value;}
			get{return _deptid;}
		}
		/// <summary>
		/// 小区编号
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string VID
		{
			set{ _vid=value;}
			get{return _vid;}
		}
		#endregion Model
	}
}

