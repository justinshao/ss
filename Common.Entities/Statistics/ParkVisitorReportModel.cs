using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
    public class ParkVisitorReportModel
    {
        public string VName { get; set; }
        public string PKName { get; set; }
        public string PlateNumber { get; set; }
        public int AlreadyVisitorCount { get; set; }
        public DateTime BeginTime { set; get; }
        public string BeginTimeDes {
            get {
                return BeginTime.ToyyyyMMddHHmmss();
            }
        }
        public string ShortBeginTimeDes
        {
            get
            {
                return BeginTime.ToString("MM-dd HH:mm:ss");
            }
        }
        public DateTime EndTime { set; get; }
        public string EndTimeDes
        {
            get
            {
                return BeginTime.ToyyyyMMddHHmmss();
            }
        }
        public string ShortEndTimeDes
        {
            get
            {
                return EndTime.ToString("MM-dd HH:mm:ss");
            }
        }
                /// <summary>
        /// 访客来源 0-监控端 1-微信
        /// </summary>
        public int VisitorSource { get; set; }
        public string VisitorSourceDes
        {
            get
            {
                if (VisitorSource == 0) {
                    return "监控端";
                }
                if (VisitorSource == 1)
                {
                    return "微信";
                }
                return string.Empty;
            }
        }
        public string OperatorID { get; set; }
        public string VisitorName { get; set; }
           /// <summary>
        /// 审核状态0未审核，1已审核，2拒绝
        /// </summary>
        public int IsExamine { get; set; }
          /// <summary>
        /// 访客状态 0-正常 2-取消
        /// </summary>
        public int VisitorState { get; set; }
        public string VisitorStateDes
        {
            get
            {
                if (VisitorState == 0)
                {
                    return "正常";
                }
                if (VisitorState == 1)
                {
                    return "取消";
                }
                return string.Empty;
            }
        }
        public DateTime CreateTime { get; set; }
        public string CreateTimeDes
        {
            get
            {
                return CreateTime.ToyyyyMMddHHmmss();
            }
        }
        public string ShortCreateTimeDes
        {
            get
            {
                return CreateTime.ToString("MM-dd HH:mm:ss");
            }
        }
        public int VisitorCount { get; set; }
        public string VisitorCountDes
        {
            get
            {
                return string.Format("{0}/{1}", AlreadyVisitorCount,VisitorCount==-1?"不限":VisitorCount.ToString());
            }
        }
        public string VisitorMobilePhone { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string OperatorName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NickName))
                    return NickName;
                return UserName;
            }
        }

    }
}
