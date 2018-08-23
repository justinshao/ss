using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using SmartSystem.OmnipotentCardWebUI.Areas.Parking.Models;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.Statistics;
using Common.Entities.Enum;
using System.Text;
using Common.Entities.Parking;
using Common.Utilities.Helpers;
using Common.Services.Park;
using Common.Services;
using Common.Entities;
namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK0104,PK0105")]
    public class ParkSettleController : BaseController
    {
        #region 结算单
        /// <summary>
        /// 结算单
        /// </summary>
        /// <returns></returns>
        public ActionResult Settlement()
        {
            return View();
        }
        /// <summary>
        /// 获得帐期
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPriod()
        {
            string pkid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkSettlementServices.GetPriods(pkid);
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 获得最后的结算单
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMaxSettlement()
        {
            string pkid = Request.Params["pkid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkSettlementServices.GetMaxSettlement(pkid);
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 获得最大提现金额
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMaxAmount()
        {
            string pkid = Request.Params["pkid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkingServices.QueryParkingByParkingID(pkid);
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 获得最小提现金额
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMinAmount()
        {
            string pkid = Request.Params["pkid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkingServices.QueryParkingByParkingID(pkid);
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 查询结算单
        /// </summary>
        /// <returns></returns>
        public string Search_Settlements()
        {
            try
            {
                int rows = 0;
                int total = 0;
                string pkid = Request.Params["parkingid"];
                int settlestatus = int.Parse(Request.Params["status"]);
                string priod = Request.Params["priod"];
                if (string.IsNullOrWhiteSpace(pkid))
                    return string.Empty;

                List<ParkSettlementModel> settlements;
                if (pkid != "-1")
                {
                    settlements = ParkSettlementServices.GetSettlements(pkid, settlestatus, priod, base.GetLoginUser.RecordID);
                }
                else
                {
                    settlements = ParkSettlementServices.GetSettlements(this.GetLoginUserVillages.Select(u => u.VID).ToList(), settlestatus, priod, base.GetLoginUser.RecordID);
                }
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(settlements);
                sb.Append("{");
                sb.Append("\"total\":" + total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 审批结算单
        /// </summary>
        /// <returns></returns>
        public bool ApplySettleDocument()
        {
            bool flag = false;
            try
            {
                string recordid = Request.Params["recordid"];
                int settlestatus = int.Parse(Request.Params["settlestatus"]);
                flag = ParkSettlementServices.ApplySettleDocument(recordid, settlestatus);
            }
            catch
            {
               
            }
            return flag;
        }
        /// <summary>
        /// 撤销结算单
        /// </summary>
        /// <returns></returns>
        public bool CancelSettleDocument()
        {
            bool flag = false;
            try
            {
                string recordid = Request.Params["recordid"];
                int settlestatus = int.Parse(Request.Params["settlestatus"]);
                flag = ParkSettlementServices.CancelSettleDocument(recordid, settlestatus);
            }
            catch
            {

            }
            return flag;
        }
        /// <summary>
        /// 生成结算单
        /// </summary>
        /// <returns></returns>
        public JsonResult BuildSettlement()
        {
            JsonResult result = new JsonResult();
            try
            {
                string pkid = Request.Params["pkid"];
                string remark = Request.Params["remark"];
                DateTime StartTime = DateTime.Parse(Request.Params["starttime"]);
                DateTime EndTime = DateTime.Parse(Request.Params["endtime"]);
                result.Data = ParkSettlementServices.BuildSettlement(pkid, StartTime, EndTime, remark, base.GetLoginUser.RecordID);
            }
            catch
            {

            }
            return result;
        }

        public JsonResult IsApprovalSettlement()
        {
            JsonResult result = new JsonResult();
            try
            {
                string pkid = Request.Params["pkid"];
                result.Data = ParkSettlementServices.IsApprovalSettlement(pkid);
            }
            catch
            {

            }
            return result;
        }


       
        /// <summary>
        /// 计算结算金额
        /// </summary>
        /// <returns></returns>
        public JsonResult CalSettleAmount()
        {
            JsonResult result = new JsonResult();
            try
            {
                string pkid = Request.Params["pkid"];
                DateTime StartTime = DateTime.Parse(Request.Params["starttime"]);
                DateTime EndTime = DateTime.Parse(Request.Params["endtime"]);
                result.Data = ParkSettlementServices.CalSettleAmount(pkid, StartTime, EndTime);
            }
            catch
            {

            }
            return result;
        }
        #endregion

        #region 结算核批流程
        public bool SaveFlowOperator()
        {
            bool flag = false;
            try
            {
                string pkid = Request.Params["pkid"];
                string userid = Request.Params["userid"];
                int flowid = int.Parse(Request.Params["flowid"]);
                flag = ParkSettlementApprovalFlowService.SaveFlowOperator(pkid, userid, flowid);
            }
            catch
            {

            }
            return flag;
        }
        /// <summary>
        /// 结算流程
        /// </summary>
        /// <returns></returns>
        public ActionResult SettlementAuditing()
        {
            return View();
        }
        /// <summary>
        /// 获得用户
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUsers()
        {
            JsonResult json = new JsonResult();
            try
            {
                List<string> companys = ParkSettlementApprovalFlowService.GetParentCompany();


                json.Data = Common.Services.SysUserServices.QuerySysUserByCompanys(companys);
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 查询审批流程
        /// </summary>
        /// <returns></returns>
        public string Search_ApprovalFlows()
        {
            try
            {
                int rows = 0;
                int total = 0;
                string pkid = Request.Params["parkingid"];
                if (string.IsNullOrWhiteSpace(pkid))
                    return string.Empty;
                //List<ParkSettlementApprovalFlowModel> flows = ParkSettlementApprovalFlowService.GetSettlementApprovalFlows(pkid);
                List<ParkSettlementApprovalFlowModel> flows;
                if (pkid != "-1")
                {
                    flows = ParkSettlementApprovalFlowService.GetSettlementApprovalFlows(pkid);
                }
                else
                {
                    flows = ParkSettlementApprovalFlowService.GetSettlementApprovalFlows(this.GetLoginUserVillages.Select(u => u.VID).ToList());
                }
                StringBuilder sb = new StringBuilder();
                string str = JsonHelper.GetJsonString(flows);
                sb.Append("{");
                sb.Append("\"total\":" + total + ",");
                sb.Append("\"rows\":" + str + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获得车场所有用户
        /// </summary>
        /// <returns></returns>
        public string GetUserByParkingID(string pkid)
        {
            try
            {
                return JsonHelper.GetJsonString(SysUserServices.QuerySysUserByParkingId(pkid));
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
