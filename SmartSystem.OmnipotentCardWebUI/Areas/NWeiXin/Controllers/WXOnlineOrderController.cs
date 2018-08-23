using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using SmartSystem.WeiXinServices;
using Common.Entities.Order;
using Common.Entities.Enum;
using Common.Utilities.Helpers;
using Common.Services;
using Common.Entities.Condition;
using Common.Utilities;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.BWY;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace SmartSystem.OmnipotentCardWebUI.Areas.NWeiXin.Controllers
{
     [CheckPurview(Roles = "PK010910")]
    public class WXOnlineOrderController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.DefaultStartDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.DefaultEndDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.NeedShowBWYParking = !string.IsNullOrWhiteSpace(SystemDefaultConfig.BWPKID);
            return View();
        }
        public string GetOnlineOrderData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["Query"]))
                {
                    return strData.ToString();
                }
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                List<BWYGateMapping> gates = BWYGateMappingServices.QueryAll();
                int total = 0;
                List<OnlineOrder> result = OnlineOrderServices.QueryPage(GetOnlineOrderCondition(), page, rows, out total);
                var obj = from p in result
                          select new
                          {
                              OrderID = p.OrderID.ToString(),
                              PKName = p.PKName,
                              PlateNo = p.PlateNo,
                              Amount = p.Amount,
                              MonthNum = p.OrderType == OnlineOrderType.MonthCardRecharge ? p.MonthNum.ToString() : string.Empty,
                              PayerNickName = p.PayerNickName,
                              SyncResultTimes = p.SyncResultTimes,
                              LastSyncResultTime = p.LastSyncResultTime != DateTime.MinValue ? p.LastSyncResultTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                              RefundOrderId = p.RefundOrderId,
                              Remark = p.Remark,
                              OrderType = (int)p.OrderType,
                              OrderTypeDes = p.OrderType.GetDescription(),
                              Status = (int)p.Status,
                              StatusDes = p.Status.GetDescription(),
                              OrderTime = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                              RealPayTime = p.RealPayTime!=DateTime.MinValue ? p.RealPayTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                              BWYParkingName = GetBWYParkingName(p.ExternalPKID,gates)
                          };
                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(obj) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询微信订单信息失败");
            }

            return strData.ToString();
        }
        private string GetBWYParkingName(string parkingId, List<BWYGateMapping> gates)
        {
            if (string.IsNullOrWhiteSpace(parkingId)) {
                return string.Empty;
            }
            BWYGateMapping model =gates.FirstOrDefault(p => p.ParkingID == parkingId);
            if (model != null)
            {
                return model.ParkingName;
            }
            return string.Empty;
        }
        private OnlineOrderCondition GetOnlineOrderCondition()
        {
            DateTime start = DateTime.Now.Date;
            DateTime sTime;
            if (!string.IsNullOrWhiteSpace(Request.Params["StartTime"]) && DateTime.TryParse(Request.Params["StartTime"], out sTime))
            {
                start = sTime;
            }
            DateTime end = DateTime.Now;
            DateTime eTime;
            if (!string.IsNullOrWhiteSpace(Request.Params["EndTime"]) && DateTime.TryParse(Request.Params["EndTime"], out eTime))
            {
                end = eTime;
            }
            OnlineOrderCondition condition = new OnlineOrderCondition();
            if (string.IsNullOrWhiteSpace(Request.Params["CompanyId"]))
                throw new MyException("获取单位编号失败");

            condition.CompanyId = Request.Params["CompanyId"].ToString();
            if (!string.IsNullOrWhiteSpace(Request.Params["ParkingId"]) && Request.Params["ParkingId"].ToString() != "-1")
            {
                condition.ParkingId = Request.Params["ParkingId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["ExternalPKID"]) && Request.Params["ExternalPKID"].ToString() != "-1")
            {
                condition.ExternalPKID = Request.Params["ExternalPKID"].ToString();
            }
            condition.PaymentChannel = PaymentChannel.WeiXinPay;
            condition.Start = start;
            condition.End = end;
            string messgage = string.Empty;
            if (!string.IsNullOrWhiteSpace(Request.Params["OrderId"]))
            {
                condition.OrderId = Request.Params["OrderId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["PlateNo"]))
            {
                condition.PlateNo = Request.Params["PlateNo"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["Status"]) && Request.Params["Status"].ToString() != "-1")
            {
                condition.Status = (OnlineOrderStatus)int.Parse(Request.Params["Status"].ToString());
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["OrderType"]) && Request.Params["OrderType"].ToString() != "-1")
            {
                condition.Ordertype = (OnlineOrderType)int.Parse(Request.Params["OrderType"].ToString());
            }
            return condition;
        }
        public string GetOrderStatus()
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            List<EnumContext> enums = EnumHelper.GetEnumContextList(typeof(OnlineOrderStatus));
            foreach (var item in enums)
            {
                strTree.Append(",{\"id\":\"" + item.EnumValue + "\",");
                strTree.Append("\"text\":\"" + item.Description + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetOrderType()
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            List<EnumContext> enums = EnumHelper.GetEnumContextList(typeof(OnlineOrderType));
            foreach (var item in enums)
            {
                strTree.Append(",{\"id\":\"" + item.EnumValue + "\",");
                strTree.Append("\"text\":\"" + item.Description + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetParkingData(string companyId) {
            List<BaseParkinfo> parkings = new List<BaseParkinfo>();
            List<BaseCompany> companys= CompanyServices.GetCurrLoginUserRoleCompany(companyId, GetLoginUser.RecordID);
            if (companys.Count > 0) {
                parkings = ParkingServices.QueryParkingByCompanyIds(companys.Select(p=>p.CPID).ToList());
            }
            
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            foreach (var item in parkings)
            {
                strTree.Append(",{\"id\":\"" + item.PKID + "\",");
                strTree.Append("\"text\":\"" + item.PKName + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetExternalParkingData()
        {
            Dictionary<string, string> parkings = new Dictionary<string, string>();
            List<BWYGateMapping> gates = BWYGateMappingServices.QueryAll();
            foreach (var item in gates) {
                if (!parkings.ContainsKey(item.ParkingID)) {
                    parkings.Add(item.ParkingID, item.ParkingName);
                }
            }
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");
            foreach (var item in parkings)
            {
                strTree.Append(",{\"id\":\"" + item.Key + "\",");
                strTree.Append("\"text\":\"" + item.Value + "\"");
                strTree.Append("}");
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        
        /// <summary>
        /// 同步支付结果
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01091001")]
        public JsonResult SynchWeiXinPaymentResult(decimal orderId)
        {
            try
            {
                bool result = OnlineOrderServices.SyncPaymentResult(orderId);
                if (!result) throw new MyException("同步失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "同步支付结果失败");
                return Json(MyResult.Error("同步支付结果失败"));
            }
        }
        /// <summary>
        /// 手动退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [CheckPurview(Roles = "PK01091002")]
        public JsonResult ManualRefund(decimal orderId)
        {
            try
            {
                bool result = OnlineOrderServices.ManualRefund(orderId, Server.MapPath("~"));
                if (!result) throw new MyException("退款失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "手动退款失败");
                return Json(MyResult.Error("手动退款失败"));
            }
        }

        /// <summary>
        /// 获取线上订单操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnlieOrderOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010201").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020101":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnpayresult";
                            option.iconCls = "icon-edit";
                            option.text = "同步支付结果";
                            option.handler = "SynchPaymentResult";
                            option.sort = 0;
                            options.Add(option);
                            break;
                        }
                    case "PK01020102":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnmanualrefund";
                            option.iconCls = "icon-edit";
                            option.text = "手动退款";
                            option.handler = "ManualRefund";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 3;
            options.Add(roption);

            SystemOperatePurview roption1 = new SystemOperatePurview();
            roption1.text = "导出";
            roption1.handler = "Export";
            roption1.iconCls = "icon-save";
            roption1.sort = 3;
            options.Add(roption1);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
        public ActionResult Export_OnlineOrderInfo()
        {
            try
            {
                // 1.获取数据集合  
                List<OnlineOrder> result = OnlineOrderServices.QueryAll(GetOnlineOrderCondition());
                var obj = (from p in result
                           select new
                           {
                               OrderID = p.OrderID.ToString(),
                               PKName = p.PKName != null ? p.PKName : "",
                               PlateNo = p.PlateNo,
                               Amount = p.Amount,
                               MonthNum = p.OrderType == OnlineOrderType.MonthCardRecharge ? p.MonthNum.ToString() : string.Empty,
                               PayerNickName = p.PayerNickName != null ? p.PayerNickName : "",
                               SyncResultTimes = p.SyncResultTimes,
                               LastSyncResultTime = p.LastSyncResultTime != DateTime.MinValue ? p.LastSyncResultTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                               RefundOrderId = p.RefundOrderId != null ? p.RefundOrderId : "",
                               OrderTypeDes = p.OrderType.GetDescription() != null ? p.OrderType.GetDescription() : "",
                               StatusDes = p.Status.GetDescription(),
                               OrderTime = p.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                               RealPayTime = p.RealPayTime != DateTime.MinValue ? p.RealPayTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                               Remark = p.Remark!=null? p.Remark : ""
                               //OrderType = (int)p.OrderType, 
                               //Status = (int)p.Status, 
                               //BWYParkingName = GetBWYParkingName(p.ExternalPKID, gates)
                           }).ToList();
                string str = JsonHelper.GetJsonString(obj);
                var dt = JsonToDataTable(str.ToString());
                // 2.设置单元格抬头
                // key：实体对象属性名称，可通过反射获取值
                // value：Excel列的名称
                Dictionary<string, string> cellheader = new Dictionary<string, string> {
                    { "OrderID", "订单编号" },
                    { "PKName", "车场名称" },
                    { "PlateNo", "车牌号" },
                    { "Amount", "支付金额" },
                    { "MonthNum", "续期月数" },
                    { "PayerNickName", "支付人" },
                    { "SyncResultTimes", "同步支付次数" },
                    { "LastSyncResultTime", "最后同步时间" },
                    { "RefundOrderId", "退款订单号" },
                    { "OrderTypeDes", "订单类型" },
                    { "StatusDes", "订单状态" },
                    { "OrderTime", "订单时间" },
                    { "RealPayTime", "支付时间" },
                    { "Remark", "备注" },
                // 3.进行Excel转换操作，并返回转换的文件下载链接 
                };
                return EntityListToExcel(cellheader, dt, "微信订单信息");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [ValidateInput(false)]
        /// <summary>
        /// 实体类集合导出到Excle2003
        /// </summary>
        /// <param name="cellHeard">单元头的Key和Value：{ { "UserName", "姓名" }, { "Age", "年龄" } };</param>
        /// <param name="enList">数据源</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>文件的下载地址</returns>
        public ActionResult EntityListToExcel(Dictionary<string, string> cellHeard, DataTable dt, string sheetName)
        {
            try
            {
                // 1.解析单元格头部，设置单元头的中文名称

                //创建Excel文件的对象  
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet  
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("OrderInfo");
                //给sheet1添加第一行的头部标题  
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                List<string> keys = cellHeard.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    row1.CreateCell(i).SetCellValue(cellHeard[keys[i]]); // 列名为Key的值
                }
                //将数据逐步写入sheet1各个行  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                        row2.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }

                // 5.返回下载路径
                string pathToFiles = Server.MapPath("/");
                string filename = sheetName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                string path = @"" + pathToFiles + filename;

                //写入到客户端
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);

                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;

                }
                return Content("/" + filename);//返回文件名提供下载  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable JsonToDataTable(string strJson)
        {
            DataTable tb = null;

            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');

                        dc.ColumnName = strCell[0].ToString().Replace("\"", "").Trim();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "").Replace("\"", "").Trim();
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
    }
}
