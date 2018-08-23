using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Common.Entities;
using Common.Entities.Parking;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Models
{
    public class ParkFeeRuleView
    {
        /// <summary>
        /// 停车场区域编号
        /// </summary>
        public string AreaID { get; set; }
        /// <summary>
        /// 停车场区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 卡类编号
        /// </summary>
        public string CarTypeID { get; set; }
        /// <summary>
        /// 卡类名称
        /// </summary>
        public string CarTypeName { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarModelID { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarModelName { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 规则编号
        /// </summary>
        public string FeeRuleID { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public FeeType FeeType { get; set; }
        /// <summary>
        /// 规则类型名称
        /// </summary>
        public string FeeTypeName { get; set; }
        /// <summary>
        /// 停车场编号
        /// </summary>
        public string ParkingID { get; set; }
        /// <summary>
        /// 最后修改世界
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 收费规则文件内容
        /// </summary>
        public string RuleText { get; set; }
        /// <summary>
        /// 是否脱机
        /// </summary>
        public bool IsOffline { get; set; }
       
        #region ////首段收费规则明细
        /// <summary>
        /// 默认规则明细编号
        /// </summary>
        public string FirstRuleDetailID { get; set; }
        /// <summary>
        /// 默认白天开始时间
        /// </summary>
        public string FirstStartTime { get; set; }
        /// <summary>
        /// 默认白天结束时间
        /// </summary>
        public string FirstEndTime { get; set; }
        /// <summary>
        /// 首段收费
        /// </summary>
        public decimal FirstFirstFee { get; set; }
        /// <summary>
        /// 首段时长
        /// </summary>
        public int FirstFirstTime { get; set; }
        /// <summary>
        /// 免费时长
        /// </summary>
        public int FirstFreeTime { get; set; }
        /// <summary>
        /// 收费限额
        /// </summary>
        public decimal FirstLimit { get; set; }
        /// <summary>
        /// 首段后每段收费1
        /// </summary>
        public decimal FirstLoop1PerFee { get; set; }
        /// <summary>
        /// 首段后每段时长1
        /// </summary>
        public int FirstLoop1PerTime { get; set; }
        /// <summary>
        /// 第二段每段收费
        /// </summary>
        public decimal FirstLoop2PerFee { get; set; }
        /// <summary>
        /// 第二段每段时长
        /// </summary>
        public int FirstLoop2PerTime { get; set; }
        /// <summary>
        /// 第二段开始时间
        /// </summary>
        public int FirstLoop2Start { get; set; }
        /// <summary>
        /// 循环包含时段(1都不含,2只含首时段,3都含)
        /// </summary>
        public LoopType FirstLoopType { get; set; }
        /// <summary>
        /// 夸段补时
        /// </summary>
        public int FirstSupplement { get; set; }
        /// <summary>
        /// 计费设置
        /// </summary>
        public int FirstFeeRuleTypes { get; set; }
        #endregion

        #region ////最后段收费规则明细
        /// <summary>
        /// 默认规则明细编号
        /// </summary>
        public string LastRuleDetailID { get; set; }
        /// <summary>
        /// 夜晚开始时间
        /// </summary>
        public string LastStartTime { get; set; }
        /// <summary>
        /// 夜晚结束时间
        /// </summary>
        public string LastEndTime { get; set; }
        /// <summary>
        /// 首段收费
        /// </summary>
        public decimal LastFirstFee { get; set; }
        /// <summary>
        /// 首段时长
        /// </summary>
        public int LastFirstTime { get; set; }
        /// <summary>
        /// 免费时长
        /// </summary>
        public int LastFreeTime { get; set; }
        /// <summary>
        /// 收费限额
        /// </summary>
        public decimal LastLimit { get; set; }
        /// <summary>
        /// 首段后每段收费1
        /// </summary>
        public decimal LastLoop1PerFee { get; set; }
        /// <summary>
        /// 首段后每段时长1
        /// </summary>
        public int LastLoop1PerTime { get; set; }
        /// <summary>
        /// 第二段每段收费
        /// </summary>
        public decimal LastLoop2PerFee { get; set; }
        /// <summary>
        /// 第二段每段时长
        /// </summary>
        public int LastLoop2PerTime { get; set; }
        /// <summary>
        /// 第二段开始时间
        /// </summary>
        public int LastLoop2Start { get; set; }
        /// <summary>
        /// 循环包含时段(1都不含,2只含首时段,3都含)
        /// </summary>
        public LoopType LastLoopType { get; set; }
        /// <summary>
        /// 夸段补时
        /// </summary>
        public int LastSupplement { get; set; }
        /// <summary>
        /// 计费设置
        /// </summary>
        public int LastFeeRuleTypes { get; set; }
        #endregion
        public string FirstRuleDetailDes
        {
            get
            {
                if (this.FeeType == Common.Entities.FeeType.Custom) {
                    return this.RuleText;
                }
                StringBuilder strSb = new StringBuilder();
                if (this.FeeType == FeeType.DayAndNight)
                {
                    string strSupplement = this.FirstSupplement==1 ? "补时" : "不补时";
                    strSb.AppendFormat("白天段:从{0}到{1}【跨段{2}】<br/>", this.FirstStartTime, this.FirstEndTime,strSupplement);
                }
                strSb.AppendFormat("循环计费包含时段：{0}", ((LoopType)this.FirstLoopType).GetDescription());
                strSb.AppendFormat("<br/>收费限额：{0}元", this.FirstLimit);
                strSb.AppendFormat("<br/>计费设置：{0}", this.FirstLoop1PerTime == 0 ? FeeRuleType.NumberOfTimes.GetDescription() : FeeRuleType.TimeFee.GetDescription());
                strSb.AppendFormat("<br/>免费时间：{0}分钟", this.FirstFreeTime);

                strSb.AppendFormat("<br/>停车时间：{0}分钟内,收费{1}元", this.FirstFirstTime, this.FirstFirstFee);

                if (this.FirstLoop1PerTime != 0)
                {
                    strSb.AppendFormat(",之后每{0}分钟收费{1}元", this.FirstLoop1PerTime, this.FirstLoop1PerFee);
                }
                if (this.FirstLoop2Start != 0)
                {
                    strSb.AppendFormat("<br/>入场{0}分钟后，每{1}分钟收费{2}元", this.FirstLoop2Start, this.FirstLoop2PerTime, this.FirstLoop2PerFee);
                }
                return strSb.ToString();
            }
        }
        public string LastRuleDetailDes
        {
            get
            {
                if (this.FeeType != FeeType.DayAndNight)
                {
                    return string.Empty;
                }
                StringBuilder strSb = new StringBuilder();
                string strSupplement = this.LastSupplement==1 ? "补时" : "不补时";
                strSb.AppendFormat("夜间段:从{0}到{1}【跨段{2}】<br/>", this.LastStartTime, this.LastEndTime,strSupplement);
                strSb.AppendFormat("循环计费包含时段：{0}", ((LoopType)this.LastLoopType).GetDescription());
                strSb.AppendFormat("<br/>收费限额：{0}元", this.LastLimit);
                strSb.AppendFormat("<br/>计费设置：{0}", this.LastLoop1PerTime == 0 ? FeeRuleType.NumberOfTimes.GetDescription() : FeeRuleType.TimeFee.GetDescription());
                strSb.AppendFormat("<br/>免费时间：{0}分钟", this.LastFreeTime);

                strSb.AppendFormat("<br/>停车时间：{0}分钟内,收费{1}元", this.LastFirstTime, this.LastFirstFee);

                if (this.LastLoop1PerTime != 0)
                {
                    strSb.AppendFormat(",之后每{0}分钟收费{1}元", this.LastLoop1PerTime, this.LastLoop1PerFee);
                }
                if (this.LastLoop2Start != 0)
                {
                    strSb.AppendFormat("<br/>入场{0}分钟后，每{1}分钟收费{2}元", this.LastLoop2Start, this.LastLoop2PerTime, this.LastLoop2PerFee);
                }
                return strSb.ToString();
            }
        }
        public ParkFeeRule ToParkFeeRule() {
            ParkFeeRule rule = new ParkFeeRule();

            rule.FeeRuleID = this.FeeRuleID;
            rule.AreaID = this.AreaID;
            rule.RuleName = this.RuleName;
            rule.FeeType = this.FeeType;
            rule.CarTypeID = this.CarTypeID;
            rule.CarModelID = this.CarModelID;
            rule.RuleText = this.RuleText;
            rule.IsOffline = this.IsOffline;
            List<ParkFeeRuleDetail> details = new List<ParkFeeRuleDetail>();
            details.Add(GetFirstFeeRuleDetail());
            if (this.FeeType == FeeType.DayAndNight)
            {
                details.Add(GetLastFeeRuleDetail());
            }
            rule.ParkFeeRuleDetails = details;
            return rule;
        }
        private ParkFeeRuleDetail GetFirstFeeRuleDetail()
        {
            ParkFeeRuleDetail detail = new ParkFeeRuleDetail();
            detail.RuleDetailID = this.FirstRuleDetailID;
            detail.RuleID = this.FeeRuleID;
            detail.StartTime = string.IsNullOrWhiteSpace(this.FirstStartTime) ? "07:00" : this.FirstStartTime;
            detail.EndTime = string.IsNullOrWhiteSpace(this.FirstEndTime) ? "17:00" : this.FirstEndTime;
            detail.Supplement = this.FirstSupplement==1;
            detail.LoopType = this.FirstLoopType;
            detail.Limit = this.FirstLimit;
            detail.FreeTime = this.FirstFreeTime;
            detail.FirstTime = this.FirstFirstTime;
            detail.FirstFee = this.FirstFirstFee;

            if (this.FirstFeeRuleTypes == (int)FeeRuleType.TimeFee)
            {
                detail.Loop1PerTime = this.FirstLoop1PerTime;
                detail.Loop1PerFee = this.FirstLoop1PerFee;
                detail.Loop2Start = this.FirstLoop2Start;
                detail.Loop2PerTime = this.FirstLoop2PerTime;
                detail.Loop2PerFee = this.FirstLoop2PerFee;
            }
            else
            {
                detail.Loop1PerTime = 0;
                detail.Loop1PerFee = 0;
                detail.Loop2Start = 0;
                detail.Loop2PerTime = 0;
                detail.Loop2PerFee = 0;
            }

            return detail;
        }
        private ParkFeeRuleDetail GetLastFeeRuleDetail()
        {
            ParkFeeRuleDetail detail = new ParkFeeRuleDetail();
            detail.RuleDetailID = this.LastRuleDetailID;
            detail.RuleID = this.FeeRuleID;
            detail.StartTime = string.IsNullOrWhiteSpace(this.LastStartTime) ? "17:00" : this.LastStartTime;
            detail.EndTime = string.IsNullOrWhiteSpace(this.LastEndTime) ? "07:00" : this.LastEndTime;
            detail.Supplement = this.FirstSupplement==1;
            detail.LoopType = this.LastLoopType;
            detail.Limit = this.LastLimit;
            detail.FreeTime = this.LastFreeTime;
            detail.FirstTime = this.LastFirstTime;
            detail.FirstFee = this.LastFirstFee;

            if (this.LastFeeRuleTypes == (int)FeeRuleType.TimeFee)
            {
                detail.Loop1PerTime = this.LastLoop1PerTime;
                detail.Loop1PerFee = this.LastLoop1PerFee;
                detail.Loop2Start = this.LastLoop2Start;
                detail.Loop2PerTime = this.LastLoop2PerTime;
                detail.Loop2PerFee = this.LastLoop2PerFee;
            }
            else
            {
                detail.Loop1PerTime = 0;
                detail.Loop1PerFee = 0;
                detail.Loop2Start = 0;
                detail.Loop2PerTime = 0;
                detail.Loop2PerFee = 0;
            }
            return detail;
        }

        public ParkFeeRuleView ToParkFeeRuleView(ParkFeeRule rule, List<ParkArea> areas, List<ParkCarType> carTyeps, List<ParkCarModel> carModels)
        {
            this.AreaID = rule.AreaID;
            ParkArea area = areas.FirstOrDefault(p => p.AreaID == rule.AreaID);
            if (area != null)
            {
                this.AreaName = area.AreaName;
            }
            this.CarTypeID = rule.CarTypeID;
            ParkCarType card = carTyeps.FirstOrDefault(p => p.CarTypeID == rule.CarTypeID);
            if (card != null)
            {
                this.CarTypeName = card.CarTypeName;
            }
            this.CarModelID = rule.CarModelID;
            ParkCarModel carModel = carModels.FirstOrDefault(p => p.CarModelID == rule.CarModelID);
            if (carModel != null)
            {
                this.CarModelName = carModel.CarModelName;
            }
            this.RuleName = rule.RuleName;
            this.FeeRuleID = rule.FeeRuleID;
            this.FeeType = rule.FeeType;
            this.RuleText = rule.RuleText;
            this.FeeTypeName = ((FeeType)rule.FeeType).GetDescription();
            this.LastUpdateTime = rule.LastUpdateTime;
            this.IsOffline = rule.IsOffline;
            FillParkFeeRuleDetail(rule.ParkFeeRuleDetails);
            return this;
        }
        private void FillParkFeeRuleDetail(List<ParkFeeRuleDetail> details)
        {
            if (details == null || details.Count == 0) return;

            ParkFeeRuleDetail first = details.OrderBy(p => p.ID).FirstOrDefault();
            this.FirstRuleDetailID = first.RuleDetailID;
            this.FirstStartTime = first.StartTime;
            this.FirstEndTime = first.EndTime;
            this.FirstFirstFee = (decimal)first.FirstFee;
            this.FirstFirstTime = first.FirstTime;
            this.FirstFreeTime = first.FreeTime;
            this.FirstLimit = (decimal)first.Limit;
            this.FirstLoop1PerFee = (decimal)first.Loop1PerFee;
            this.FirstLoop1PerTime = first.Loop1PerTime;
            this.FirstLoop2PerFee = (decimal)first.Loop2PerFee;
            this.FirstLoop2PerTime = first.Loop2PerTime;
            this.FirstLoop2Start = first.Loop2Start;
            this.FirstLoopType = first.LoopType;
            this.FirstSupplement = first.Supplement?1:0;
            this.FirstFeeRuleTypes = first.Loop1PerTime > 0 ? (int)FeeRuleType.TimeFee : (int)FeeRuleType.NumberOfTimes;
            if (details.Count > 1)
            {
                ParkFeeRuleDetail last = details.OrderBy(p => p.ID).LastOrDefault();

                this.LastRuleDetailID = last.RuleDetailID;
                this.LastStartTime =  last.StartTime;
                this.LastEndTime =  last.EndTime;
                this.LastFirstFee = (decimal)last.FirstFee;
                this.LastFirstTime = last.FirstTime;
                this.LastFreeTime = last.FreeTime;
                this.LastLimit = (decimal)last.Limit;
                this.LastLoop1PerFee = (decimal)last.Loop1PerFee;
                this.LastLoop1PerTime = last.Loop1PerTime;
                this.LastLoop2PerFee = (decimal)last.Loop2PerFee;
                this.LastLoop2PerTime = last.Loop2PerTime;
                this.LastLoop2Start = last.Loop2Start;
                this.LastLoopType = last.LoopType;
                this.LastSupplement = last.Supplement?1:0;
                this.LastFeeRuleTypes = last.Loop1PerTime > 0 ? (int)FeeRuleType.TimeFee : (int)FeeRuleType.NumberOfTimes;
            }
        }
    }
}