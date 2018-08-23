using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.Park;
using Common.Entities;
using System.Text;
using Common.Services;
using Common.Entities.Parking;
using SmartSystem.OmnipotentCardWebUI.Models;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010302")]
    public class ParkDerateController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetSellerDerateData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["sellerId"])) return json;

                string sellerId = Request.Params["sellerId"].ToString();
                List<ParkDerate> derates = ParkDerateServices.QueryBySellerID(sellerId);
                var result = from p in derates select new {
                    DerateID = p.DerateID,
                    SellerID = p.SellerID,
                    Name = p.Name,
                    DerateSwparate = (int)p.DerateSwparate,
                    DerateSwparateDes = p.DerateSwparate.GetDescription(),
                    DerateType = (int)p.DerateType,
                    DerateTypeDes = p.DerateType.GetDescription(),
                    DerateMoney = p.DerateMoney,
                    FreeTime = p.FreeTime,
                    StartTime = p.StartTime!=DateTime.MinValue?p.StartTime.ToString("HH:mm"):"00:00",
                    EndTime = p.EndTime != DateTime.MinValue ? p.EndTime.ToString("HH:mm") : "00:00",
                    FeeRuleID = p.FeeRuleID,
                    DerateIntervar = GetDerateIntervar(p.DerateIntervar),
                };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "根据商家编号获取优免信息失败");
            }
            return json;

        }
        private string GetDerateIntervar(List<ParkDerateIntervar> derateIntervars)
        {
            if (derateIntervars == null || derateIntervars.Count == 0) return string.Empty;

            StringBuilder str = new StringBuilder();
            foreach (var item in derateIntervars) {
                str.AppendFormat("{0},{1}|",item.FreeTime,item.Monetry);
            }
            return str.ToString().TrimEnd('|');
        }
        public JsonResult GetParkFeeRuleTreeData()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["sellerId"]))
                return json;

            string sellerId = Request.Params["sellerId"].ToString();
            ParkSeller seller = ParkSellerServices.QueryBySellerId(sellerId);
            if (seller == null) throw new MyException("获取商家信息失败");

            List<BaseParkinfo> parkings = ParkingServices.QueryParkingByVillageId(seller.VID);
            if (parkings.Count == 0) return json;

            List<ParkFeeRule> rules = new List<ParkFeeRule>();
            foreach (var item in parkings)
            {
                List<ParkFeeRule> rule = ParkFeeRuleServices.QueryParkFeeRuleByParkingId(item.PKID);
                if (rule.Count > 0){
                    rules.AddRange(rule);
                }
            }
            json.Data = rules;
            return json;
          
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030201,PK01030202")]
        public JsonResult SaveParkDerate(ParkDerate model)
        {
            try
            {
                model = CheckParkDerate(model);
                List<ParkDerateIntervar> derateintervars = CheckParkDerateIntervar(model.DerateType, model.DerateID);
                model.DerateIntervar = derateintervars;
                if (string.IsNullOrWhiteSpace(model.DerateID))
                {
                    bool result = ParkDerateServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                }
                else
                {
                    bool result = ParkDerateServices.Update(model);
                    if (!result) throw new MyException("修改失败");
                }
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存商家优免信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01030203")]
        public JsonResult Delete(string derateId)
        {
            try
            {

                bool result = ParkDerateServices.Delete(derateId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除商家优免信息失败");
                return Json(MyResult.Error("删除失败"));
            }

           
        }
        private List<ParkDerateIntervar> CheckParkDerateIntervar(DerateType derateType, string derateId)
        {

            if (derateType != DerateType.TimePeriodPayment && derateType != DerateType.VoteSpecialPayment)
                return new List<ParkDerateIntervar>();

            if (string.IsNullOrWhiteSpace(Request.Params["DerateIntervar"]))
                throw new MyException("请添加时间和金额的规则");

            string strDerateIntervar = Request.Params["DerateIntervar"].ToString().TrimEnd('|');
            List<ParkDerateIntervar> derateintervars = new List<ParkDerateIntervar>();
            string[] strRule = strDerateIntervar.Split('|');
            for (int i = 0; i < strRule.Length; i++)
            {
                string[] detail = strRule[i].Split(',');
                ParkDerateIntervar derateintervar = new ParkDerateIntervar();
                derateintervar.DerateID = derateId;
                derateintervar.FreeTime = int.Parse(detail[0]);
                derateintervar.Monetry = decimal.Parse(detail[1]);
                derateintervars.Add(derateintervar);
            }
            if (derateintervars.Count == 0) throw new MyException("请添加时间和金额的规则");
            return derateintervars;
        }
        private ParkDerate CheckParkDerate(ParkDerate derate)
        {
            if (string.IsNullOrWhiteSpace(derate.Name)) throw new MyException("优免名称不能为空");
            if (string.IsNullOrWhiteSpace(derate.SellerID)) throw new MyException("您未选择商家");

            ParkDerate newDerate = new ParkDerate();
            newDerate.ID = derate.ID;
            newDerate.DerateID = derate.DerateID;
            newDerate.Name = derate.Name;
            newDerate.SellerID = derate.SellerID;
            newDerate.DerateType = derate.DerateType;
            switch (derate.DerateType)
            {
                case DerateType.PaymentOnTime:
                case DerateType.NoPayment:
                case DerateType.SpecialTimePeriodPayment:
                    {
                        break;
                    }
                case DerateType.TimesPayment:
                    {
                        if (string.IsNullOrWhiteSpace(derate.FeeRuleID))
                            throw new MyException("请选择收费标准");

                        if (derate.DerateMoney <= 0)
                            throw new MyException("一次消费收消费场所金额不正确");

                        newDerate.DerateSwparate = derate.DerateSwparate;
                        newDerate.FeeRuleID = derate.FeeRuleID;
                        newDerate.DerateMoney = derate.DerateMoney;
                        newDerate.FreeTime = (int)(float.Parse(Request.Params["FreeTimeHour"].ToString()) * 60);
                        break;
                    }
                case DerateType.VotePayment:
                case DerateType.ReliefTime:
                    {
                        newDerate.DerateSwparate = derate.DerateSwparate;
                        break;
                    }
                case DerateType.TimePeriodPayment:
                    {
                        newDerate.DerateSwparate = derate.DerateSwparate;
                        if (string.IsNullOrWhiteSpace(Request.Params["MaxFreeHour"]))
                        {
                            throw new MyException("车主最高免时间不能为空");
                        }
                        newDerate.FreeTime = (int)(float.Parse(Request.Params["MaxFreeHour"].ToString()) * 60);
                        break;
                    }
                case DerateType.StandardPayment:
                    {
                        if (string.IsNullOrWhiteSpace(derate.FeeRuleID))
                            throw new MyException("请选择收费标准");

                        newDerate.FeeRuleID = derate.FeeRuleID;
                        break;
                    }
                case DerateType.TimePeriodAndTimesPayment:
                    {
                        newDerate.DerateSwparate = derate.DerateSwparate;
                        newDerate.DerateMoney = derate.DerateMoney;
                        newDerate.FreeTime = (int)(float.Parse(Request.Params["FreeTimeHour"].ToString()) * 60);
                        newDerate.StartTime = derate.StartTime;
                        newDerate.EndTime = derate.EndTime;
                        break;
                    }
                case DerateType.VoteSpecialPayment:
                    {
                        newDerate.DerateSwparate = derate.DerateSwparate;
                        break;
                    }
                case DerateType.DayFree:
                    {
                        newDerate.DerateMoney = derate.DayMoney;
                        break;
                    }
                default: throw new MyException("优免类型不存在");
            }
            return newDerate;
        }
        /// <summary>
        /// 获取商家优免操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSellerDerateOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010302").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01030201":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01030202":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01030203":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 6;
            options.Add(roption);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
