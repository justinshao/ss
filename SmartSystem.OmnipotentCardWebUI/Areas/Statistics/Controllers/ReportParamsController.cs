using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common.Entities.Statistics;
using Common.Entities.Enum;
using Common.Entities.Condition;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    public class ReportParamsController : BaseController
    {
        #region 在场车辆
        /// <summary>
        /// 在场车辆报表数据
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="cardtype">卡片类型</param>
        /// <param name="platenumber">车牌编号</param>
        /// <param name="starttime">进场时间</param>
        /// <param name="endtime">进场结束时间</param>
        public void Params_Presence(string parkingid, string cardtype, string platenumber, DateTime starttime, DateTime endtime, string entrancegateid, int pageindex, int pagesize)
        {                      
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                CardType = cardtype,
                PlateNumber = platenumber,
                StartTime = starttime,
                InGateID = entrancegateid,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.Presence
            };
        }
        #endregion

        #region 无牌车辆
        /// <summary>
        /// 无车牌报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="ingateid">进道口编号</param>
        /// <param name="starttime">进场时间</param>
        /// <param name="endtime">进场结束时间</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_NoPlateNumber(string parkingid, string ingateid, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                InGateID = ingateid,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.NoPlateNumber
            };
        }
        #endregion

        #region 进出记录
        /// <summary>
        /// 进出场记录报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="cardtypeid">卡片类型</param>
        /// <param name="cartypeid">车辆类型</param>
        /// <param name="exitgateid">出道口编号</param>
        /// <param name="entrancegateid">进道口编号</param>
        /// <param name="exitoperatorid">出操作人编号</param>
        /// <param name="releasetype">放行类型</param>
        /// <param name="areaid">区域编号</param>
        /// <param name="starttime">进场时间</param>
        /// <param name="endtime">出场时间</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="cardno">卡编号</param>
        /// <param name="owner">车主</param>
        /// <param name="Isexist">是否出场</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="agesize">每页显示数</param>
        public void Params_InOutRecord(string parkingid, string cardtypeid, string cartypeid, string exitgateid, string entrancegateid, string exitoperatorid, int releasetype, string areaid, int isexit, string platenumber, string owner, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                CardType = cardtypeid,
                CarType = cartypeid,
                OutGateID = exitgateid,
                InGateID = entrancegateid,
                OutOperator = exitoperatorid,
                ReleaseType = releasetype,
                AreaID = areaid,
                StartTime = starttime,
                PlateNumber = platenumber,
                IsExit = isexit,
                PageIndex = pageindex,
                PageSize = pagesize,
                Owner=owner,
                ReportType = ReportType.InOut,
                EndTime = endtime
            };
        }
        #endregion

        #region  异常放行
        /// <summary>
        /// 异常放行
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="exitgate">出道口</param>
        /// <param name="exitstarttime">出场开始时间</param>
        /// <param name="exitendtime">出场结束时间</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_ExceptionReleaseInOut(string parkingid, string platenumber, string exitgate, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {                                                                                     
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                PlateNumber = platenumber,
                OutGateID = exitgate,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.ExceptionRelease,
            };
        }
        #endregion

        #region 通道事件
        /// <summary>
        /// 通道进出记录查询
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="eventid">事件名称</param>
        /// <param name="cartype">车类型</param>
        /// <param name="cardtype">卡类型</param>
        /// <param name="inorout">进出方向 1: 进  2: 出</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="gatein">道口编号</param>
        /// <param name="starttime">进开始时间</param>
        /// <param name="endtime">出开始时间</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">页大小</param>
        public void Params_GateInOutRecord(string parkingid, int eventid, string cartype, string cardtype, int inorout, string platenumber, string gatein, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                EventID = eventid,
                CarType = cartype,
                CardType = cardtype,
                InOrOut = inorout,
                PlateNumber = platenumber,
                InGateID = gatein,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.GateEvent
            };
        }
        #endregion

        #region 当班统计
        /// <summary>
        /// 当班统计报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="adminid">操作员编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="boxid">岗亭编号</param>
        /// <param name="PageIndex">页索引</param>
        /// <param name="PageSize">每页显示数</param>
        public void Params_OnDuty(string parkingid, string adminid, DateTime starttime, DateTime endtime, string boxid, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                OutOperator = adminid,
                StartTime = starttime,
                EndTime = endtime,
                BoxID = boxid,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.OnDuty
            };
        }
        #endregion

        #region 订单明细
        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="onlineoffline">线上订单或者线下订单</param>
        /// <param name="ordersource">订单来源</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="isshowdiffamount">是否显示差异订单</param>
        /// <param name="isshowzero">是否显示0元订单</param>
        /// <param name="boxid">岗亭编号</param>
        /// <param name="isnoconfirm">是否显示未确认订单</param>
        /// <param name="adminid">收费员</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_Order(string parkingid, string platenumber, int onlineoffline, int ordersource, DateTime starttime, DateTime endtime, bool isshowdiffamount, bool isshowzero, string boxid, bool isnoconfirm, string adminid, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                PlateNumber = platenumber,
                OnLineOffLine = onlineoffline,
                OrderSource = ordersource,
                StartTime = starttime,
                EndTime = endtime,
                Zero = isshowzero,
                DiffAmount = isshowdiffamount,
                BoxID = boxid,
                IsNoConfirm = isnoconfirm,
                OutOperator = adminid,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.Order
            };
        }
        #endregion

        #region 线上支付
        /// <summary>
        /// 线上支付
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="payway">支付方式</param>
        /// <param name="status">支付状态</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>       
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_OnlinePay(string parkingid, int payway, int status, string platenumber, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                PayWay = payway,
                Status = status,
                PlateNumber = platenumber,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.Order

            };
        }
        #endregion


        #region 临停缴费
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="onlineoffline">线上或线下</param>
        /// <param name="ordersource">订单来源</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="isshowzero">是否显示0元订单</param>
        /// <param name="isdiffamount">是否显示存在差异金额订单</param>
        /// <param name="boxid">岗亭编号</param>
        /// <param name="isnoconfirm">是否显示未确认订单</param>
        /// <param name="adminid">收费员</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_TempPay(string parkingid, int onlineoffline, int ordersource, string platenumber, DateTime starttime, DateTime endtime, bool isshowzero, string boxid, bool isnoconfirm, bool isdiffamount, string adminid, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                OnLineOffLine = onlineoffline,
                OrderSource = ordersource,
                PlateNumber = platenumber,
                StartTime = starttime,
                EndTime = endtime,
                Zero = isshowzero,
                BoxID = boxid,
                IsNoConfirm = isnoconfirm,
                DiffAmount = isdiffamount,
                OutOperator = adminid,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.TempPay
            };
        }
        #endregion

        #region 商家优免
        /// <summary>
        /// 商家优免
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="status">状态</param>
        /// <param name="couponno">优免编号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="sellerid">商家</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_Coupon(string parkingid, int status, string couponno, DateTime starttime, DateTime endtime, string sellerid, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                Status = status,
                CouponNo = couponno,
                StartTime = starttime,
                EndTime = endtime,
                SellerID = sellerid,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.Coupon
            };
        }
        #endregion

        #region 月卡续期
        /// <summary>
        /// 月卡续期报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="onlineoffline">线上或线下</param>
        /// <param name="platenumber">车牌号码</param>
        /// <param name="owner">车主</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_MonthCardExtend(string parkingid, int onlineoffline, string platenumber, string owner, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                PlateNumber = platenumber,
                OnLineOffLine = onlineoffline,
                Owner = owner,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.CardExtension
            };
        }
        #endregion

        #region 月卡信息
        /// <summary>
        /// 月卡信息
        /// </summary>
        /// <param name="parkingid"></param>
        /// <param name="cardtype"></param>
        /// <param name="platenumber"></param>
        /// <param name="addr"></param>
        /// <param name="mobile"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        public void Params_MonthCardInfo(string parkingid, string cardtype, string platenumber, string addr, string mobile, bool due, int pageindex, int pagesize)
        {
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                CardType = cardtype,
                PlateNumber = platenumber,
                Addr = addr,
                Mobile = mobile,
                PageIndex = pageindex,
                PageSize = pagesize,
                Due = due,
                ReportType = ReportType.MonthCardInfo
            };
        }
        #endregion

        #region 访客信息
        /// <summary>
        /// 访客信息
        /// </summary>
        /// <param name="parkingid"></param>
        /// <param name="cardtype"></param>
        /// <param name="platenumber"></param>
        /// <param name="addr"></param>
        /// <param name="mobile"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        public void Params_ParkVisitorInfo()
        {
            int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
            int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
            VisitorReportCondition paras = new VisitorReportCondition();
            if (string.IsNullOrWhiteSpace(Request.Params["ParkingId"]) || Request.Params["ParkingId"].ToString() == "-1")
            {
                List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageIds(base.GetLoginUserVillages.Select(u => u.VID).ToList());
                if (parkings.Count > 0)
                {
                    paras.ParkingIds = parkings.Select(p => p.PKID).ToList();
                }
            }
            else
            {
                paras.ParkingIds = new List<string>() { Request.Params["ParkingId"].ToString() };
            }
            paras.PlateNumber = Request.Params["PlateNumber"].ToString();
            paras.MoblieOrName = Request.Params["MoblieOrName"].ToString();
            paras.BeginTime = DateTime.Parse(Request.Params["BeginTime"].ToString());
            paras.EndTime = DateTime.Parse(Request.Params["EndTime"].ToString());
            if (!string.IsNullOrWhiteSpace(Request.Params["VisitorSource"]) && Request.Params["VisitorSource"].ToString() != "-1")
            {
                paras.VisitorSource = int.Parse(Request.Params["VisitorSource"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(Request.Params["VisitorState"]) && Request.Params["VisitorState"].ToString() != "-1")
            {
                paras.VisitorState = int.Parse(Request.Params["VisitorState"].ToString());
            }
            System.Web.HttpContext.Current.Session["VisitorReportCondition"] = paras;
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                PageIndex = page,
                PageSize = rows,
                ReportType = ReportType.ParkVisitorInfo
            };
        }
        #endregion

        #region 日收费报表
        /// <summary>
        /// 日收费报表
        /// </summary>
        /// <param name="ParkingID">车场编号</param>
        /// <param name="StartTime">开始日期</param>
        /// <param name="EndTime">结束日期</param>
        /// <param name="PageIndex">页索引</param>
        /// <param name="PageSize">每页显示数</param>
        public void Params_FeeDaily(string parkingid, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            endtime = endtime.AddDays(1).AddSeconds(-1);
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.DailyFee
            };
        }
        #endregion

        #region 月收费报表
        /// <summary>
        /// 月收费报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="strstarttime">开始日期</param>
        /// <param name="strendtime">结束日期</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_FeeMonth(string parkingid, string strstarttime, string strendtime, int pageindex, int pagesize)
        {
            DateTime StartTime = DateTime.Parse(strstarttime + "-01 00:00:00");
            DateTime EndTime = DateTime.Parse(strendtime + "-01").AddMonths(1).AddSeconds(-1);
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                StartTime = StartTime,
                EndTime = EndTime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.MonthFee
            };
        }
        #endregion

        #region 日汇总报表
        /// <summary>
        /// 日汇总报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="starttime">开始日期</param>
        /// <param name="endtime">结束日期</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页显示数</param>
        public void Params_GatherDaily(string parkingid, DateTime starttime, DateTime endtime, int pageindex, int pagesize)
        {
            endtime = endtime.AddDays(1).AddSeconds(-1);
            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                StartTime = starttime,
                EndTime = endtime,
                PageIndex = pageindex,
                PageSize = pagesize,
                ReportType = ReportType.DailyGather
            };
        }
        #endregion

        #region 月汇总报表
        /// <summary>
        /// 月汇总报表
        /// </summary>
        /// <param name="parkingid">车场编号</param>
        /// <param name="strstarttime">开始日期</param>
        /// <param name="strendtime">结束日期</param>       
        public void Params_GatherMonth(string parkingid, string strstarttime, string strendtime)
        {
            DateTime StartTime = DateTime.Parse(strstarttime + "-01 00:00:00");
            DateTime EndTime = DateTime.Parse(strendtime + "-01").AddMonths(1).AddSeconds(-1);

            System.Web.HttpContext.Current.Session["QueryParams"] = new InParams
            {
                ParkingID = parkingid,
                StartTime = StartTime,
                EndTime = EndTime,
                ReportType = ReportType.MonthGather
            };
        }
        #endregion

    }
}
