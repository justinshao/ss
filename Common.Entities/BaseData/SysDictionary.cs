using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Common.Entities
{
	/// <summary>
	/// 类SysDictionary。
	/// </summary>
	[Serializable]
	public partial class SysDictionary
	{
		public SysDictionary()
		{}
		#region Model
		private int _id;
		private string _catagrayname;
		private string _dicvalue;
		private int _orderid;
		private string _des;
		private int _dicid;
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
        /// 
        /// </summary>
        public string CatagrayName
		{
			set{ _catagrayname=value;}
			get{return _catagrayname;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string DicValue
		{
			set{ _dicvalue=value;}
			get{return _dicvalue;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public string Des
		{
			set{ _des=value;}
			get{return _des;}
		}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 
        /// </summary>
        public int DicID
		{
			set{ _dicid=value;}
			get{return _dicid;}
		}
		#endregion Model

	}
}

