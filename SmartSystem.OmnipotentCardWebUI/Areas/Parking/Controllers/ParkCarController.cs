using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Services;
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Entities.Condition;
using Common.Utilities;
using Common.Services.BaseData;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities.Helpers;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    [CheckPurview(Roles = "PK010204")]
    public class ParkCarController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetParkCarData()
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return string.Empty;
                string parkingid = Request.Params["parkingId"].ToString();

                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
                List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingid);
                //List<ParkCarBitGroup> bitGroups = ParkGrantServices.QueryCarBitGroupByParkingId(parkingid);
                
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                string sort = Request.Params["sort"];
                int total = 0;

                Dictionary<string, DateTime> dicIoRecords = new Dictionary<string, DateTime>();
                List<ParkGrantView> parkGrants = ParkGrantServices.QueryPage(GetQueryCondition(), rows, page, out total);
                if (parkGrants.Count > 0)
                {
                    List<string> plateNumbers = parkGrants.Where(p => !string.IsNullOrWhiteSpace(p.PlateNo)).Select(p => p.PlateNo).ToList();
                    dicIoRecords = ParkIORecordServices.QueryLastNotExitIORecord(parkingid, plateNumbers);
                }
                List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(parkingid);
                List<ParkGate> gates = ParkGateServices.QueryByParkingId(parkingid);

                List<ParkCardSuspendPlan> suspendPlans = ParkCardSuspendPlanServices.QueryByGrantIds(parkGrants.Select(p => p.GID).ToList());
                var result = from p in parkGrants
                             select new
                             {
                                 ID = p.ID,
                                 CardID = p.CardID,
                                 GID = p.GID,
                                 PKID = p.PKID,
                                 BeginDate = GetBeginDate(p),
                                 EndDate = p.EndDate == DateTime.MinValue ? string.Empty : p.EndDate.ToString("yyyy-MM-dd"),
                                 PlateNo = p.PlateNo,
                                 PlateID = p.PlateID,
                                 CarModelID = p.CarModelID,
                                 CarTypeID = p.CarTypeID,
                                 PKLot = p.PKLot,
                                 EmployeeName = p.EmployeeName,
                                 Phone = string.IsNullOrWhiteSpace(p.MobilePhone) ? p.HomePhone : p.MobilePhone,
                                 CardNo = p.CardNo,
                                 CardNumber = p.CardNumber,
                                 FamilyAddr = p.FamilyAddr,
                                 Remark = p.Remark,
                                 Balance = p.Balance,
                                 EmployeeID = p.EmployeeID,
                                 CarBaseTypeID = GetCarBaseTypeID(carTypes, p.CarTypeID),
                                 CarModelName = GetCarModelName(carModels, p.CarModelID),
                                 CarTypeName = GetCarTypeName(carTypes, p.CarTypeID),
                                 CarMonthlyRentAmount = GetCarMonthlyRentAmount(carTypes, p.CarTypeID),
                                 CarSeasonRentAmount = GetCarMonthlyRentAmount(carTypes, p.CarTypeID),
                                 CarYearRentAmount = GetCarMonthlyRentAmount(carTypes, p.CarTypeID),
                                 Color = p.Color,
                                 ColorDes = p.Color.GetDescription(),
                                 AreaID = FillInvalidArea(areas, p.AreaIDS),
                                 GateID = FillInvalidGate(gates, p.GateID),
                                 State = (int)p.State,
                                 StateDes = GetStateDescription(p.State, p.EndDate),
                                 Due = (bool)p.Due,
                                 SuspendPlanDate = GetCardSuspendPlanData(suspendPlans, p.GID),
                                 DefaultSelectStartDate = DefaultSelectStartDate(p.BeginDate, p.EndDate, p.PlateNo, dicIoRecords),
                                 MaxCanSelectStartDate = MaxCanSelectStartDate(p.BeginDate, p.EndDate, p.PlateNo, dicIoRecords),
                                 PKLotDes = GetParkCarBitGroupDes(p.PKLot, p.PKLotNum),
                                 PKLotNum = p.PKLotNum,
                                 OnlineUnit = p.OnlineUnit,
                                 OnlineUnitDes= p.OnlineUnit + "个月"
                             };

                strData.Append("{");
                strData.Append("\"total\":" + total + ",");
                strData.Append("\"rows\":" + JsonHelper.GetJsonString(result) + ",");
                strData.Append("\"index\":" + page);
                strData.Append("}");

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "查询车辆信息失败");
            }
            return strData.ToString();

        }

        public string GetParkCarData2() {

            StringBuilder sb = new StringBuilder();
            try { 
                
            }
            catch(Exception ex) { 
                
            }
            return "";
        }


        private string GetParkCarBitGroupRecordID(List<ParkCarBitGroup> bitGroups, string pkLot)
        {
            if (string.IsNullOrWhiteSpace(pkLot)) return string.Empty;

            ParkCarBitGroup model = bitGroups.FirstOrDefault(p => p.CarBitName == pkLot);
            if (model == null) return string.Empty;

            return model.RecordID;
        }
        private string GetParkCarBitGroupDes(string pkLot, int pkLotNum)
        {
            if (string.IsNullOrWhiteSpace(pkLot)) return string.Empty;

            return string.Format("{0}/{1}个", pkLot, pkLotNum);
        }
        private string GetBeginDate(ParkGrantView model)
        {
            if (model.BeginDate != DateTime.MinValue)
            {
                return model.BeginDate.ToString("yyyy-MM-dd");
            }
            if (model.EndDate != DateTime.MinValue)
            {
                if (model.EndDate.Date < DateTime.Now.Date)
                {
                    return model.EndDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                return string.Empty;
            }
        }
        private string FillInvalidArea(List<ParkArea> areaList, string areas)
        {
            StringBuilder strAreas = new StringBuilder();
            if (string.IsNullOrWhiteSpace(areas))
                return strAreas.ToString();

            if (areaList == null || areaList.Count == 0)
                return strAreas.ToString();

            string[] areaslist = areas.Split(',');
            for (var i = 0; i < areaslist.Length; i++)
            {
                if (areaList.Count(p => p.AreaID == areaslist[i]) > 0)
                {
                    strAreas.AppendFormat("{0},", areaslist[i]);
                }
            }
            if (string.IsNullOrWhiteSpace(strAreas.ToString()))
                return string.Empty;

            return strAreas.ToString().TrimEnd(',');
        }
        private string FillInvalidGate(List<ParkGate> gateList, string gates)
        {
            StringBuilder strGates = new StringBuilder();
            if (string.IsNullOrWhiteSpace(gates))
                return strGates.ToString();

            if (gateList == null || gateList.Count == 0)
                return strGates.ToString();

            string[] areaslist = gates.Split(',');
            for (var i = 0; i < areaslist.Length; i++)
            {
                if (gateList.Count(p => p.GateID == areaslist[i]) > 0)
                {
                    strGates.AppendFormat("{0},", areaslist[i]);
                }
            }
            if (string.IsNullOrWhiteSpace(strGates.ToString()))
                return string.Empty;

            return strGates.ToString().TrimEnd(',');
        }
        private string GetStateDescription(ParkGrantState state, DateTime endDate)
        {
            if (state == ParkGrantState.Normal && endDate != DateTime.MinValue && endDate.Date < DateTime.Now.Date)
            {
                return "过期";
            }
            return state.GetDescription();
        }
        private ParkGrantCondition GetQueryCondition()
        {
            ParkGrantCondition condition = new ParkGrantCondition();
            condition.ParkingId = Request.Params["parkingId"].ToString();

            if (!string.IsNullOrWhiteSpace(Request.Params["EmployeeNameOrMoblie"]))
            {
                condition.EmployeeNameOrMoblie = Request.Params["EmployeeNameOrMoblie"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request.Params["PlateNo"]))
            {
                condition.PlateNumber = Request.Params["PlateNo"].ToString().ToPlateNo();
            }

            if (!string.IsNullOrWhiteSpace(Request["CarTypeId"]))
            {
                condition.CarTypeId = Request.Params["CarTypeId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request["CarModelId"]))
            {
                condition.CarModelId = Request.Params["CarModelId"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request["HomeAddress"]))
            {
                condition.HomeAddress = Request.Params["HomeAddress"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request["State"]))
            {
                condition.State = int.Parse(Request.Params["State"].ToString());
            }
            if (!string.IsNullOrWhiteSpace(Request["PKLot"]))
            {
                condition.ParkingLot = Request.Params["PKLot"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request["Due"]))
            {
                condition.Due = bool.Parse(Request["Due"].ToString());
            }
            return condition;
        }
        /// <summary>
        /// 能选择的最大开始有效期
        /// </summary>
        /// <param name="cardGrant"></param>
        /// <returns></returns>
        private string MaxCanSelectStartDate(DateTime? beginDate, DateTime? endDate, string plateNo, Dictionary<string, DateTime> dicIORecord)
        {
            //首次续期
            if (!beginDate.HasValue || !endDate.HasValue)
                return DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

            //已过期
            if (endDate.Value.Date < DateTime.Now.Date)
            {
                if (dicIORecord.ContainsKey(plateNo))
                {
                    return dicIORecord[plateNo].ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            //未过期
            return beginDate.Value.ToString("yyyy-MM-dd");
        }
        private string DefaultSelectStartDate(DateTime? beginDate, DateTime? endDate, string plateNo, Dictionary<string, DateTime> dicIORecord)
        {
            //首次续期
            if (!beginDate.HasValue || !endDate.HasValue)
                return DateTime.Now.ToString("yyyy-MM-dd");

            //已过期
            if (endDate.Value.Date < DateTime.Now.Date)
            {
                if (dicIORecord.ContainsKey(plateNo))
                {
                    return dicIORecord[plateNo].ToString("yyyy-MM-dd");
                }
                else
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            //未过期
            return beginDate.Value.ToString("yyyy-MM-dd");
        }
        private string GetCardSuspendPlanData(List<ParkCardSuspendPlan> suspendPlans, string grantId)
        {
            ParkCardSuspendPlan suspendPlan = suspendPlans.LastOrDefault(p => p.GrantID == grantId);
            if (suspendPlan == null)
            {
                return string.Format("{0}|", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            return string.Format("{0}|{1}", suspendPlan.StartDate.ToString("yyyy-MM-dd"), suspendPlan.EndDate != DateTime.MinValue ? suspendPlan.EndDate.ToString("yyyy-MM-dd") : string.Empty);
        }
        private string GetCarTypeName(List<ParkCarType> carTypes, string carTypeId)
        {
            ParkCarType carType = carTypes.FirstOrDefault(p => p.CarTypeID == carTypeId);
            return carType == null ? string.Empty : carType.CarTypeName;
        }
        private string GetCarModelName(List<ParkCarModel> carModels, string carModelId)
        {
            ParkCarModel carModel = carModels.FirstOrDefault(p => p.CarModelID == carModelId);
            return carModel == null ? string.Empty : carModel.CarModelName;
        }
        private string GetCarBaseTypeID(List<ParkCarType> carTypes, string carTypeId)
        {
            ParkCarType carType = carTypes.FirstOrDefault(p => p.CarTypeID == carTypeId);
            if (carType != null)
            {
                return ((int)carType.BaseTypeID).ToString();
            }
            return string.Empty;
        }
        private decimal GetCarMonthlyRentAmount(List<ParkCarType> carTypes, string carTypeID)
        {
            ParkCarType carType = carTypes.FirstOrDefault(p => p.CarTypeID == carTypeID);
            if (carType != null && carType.BaseTypeID == BaseCarType.MonthlyRent|| carType != null && carType.BaseTypeID == BaseCarType.CustomRent)
            {
                return carType.Amount;
            }
            if (carType != null && carType.BaseTypeID == BaseCarType.SeasonRent)
            {
                return carType.Amount * 3;
            }
            if (carType != null && carType.BaseTypeID == BaseCarType.YearRent)
            {
                return carType.Amount * 12;
            }
            return 0;
        }
        [HttpPost]
        public JsonResult GetParkCarModelData()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;
            string parkingid = Request.Params["parkingId"].ToString();
            List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingid);
            json.Data = carModels;
            return json;
        }
        [HttpPost]
        public JsonResult GetParkCarTypeData()
        {
            JsonResult json = new JsonResult();
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;
            string parkingid = Request.Params["parkingId"].ToString();
            List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
            json.Data = carTypes;
            return json;
        }
        [HttpPost]
        public JsonResult GetParkCarStateData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(ParkGrantState));
            return json;
        }
        [HttpPost]
        public string GetParkCarBitGroupData()
        {
            JsonResult json = new JsonResult();
            string errorMsg = string.Empty;
            string parkingId = Request.Params["parkingId"].ToString();
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"\",");
            strTree.Append("\"text\":\"无\",\"selected\":true");
            strTree.Append("}");
            if (!string.IsNullOrWhiteSpace(parkingId))
            {
                List<ParkCarBitGroup> models = ParkGrantServices.QueryCarBitGroupByParkingId(parkingId);
                foreach (var item in models)
                {
                    string text = string.Format("{0}/{1}个车位", item.CarBitName, item.CarBitNum);
                    strTree.Append(",{\"id\":\"" + item.CarBitName + "\",");
                    strTree.Append("\"text\":\"" + text + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public JsonResult GetPlateColorData()
        {
            JsonResult json = new JsonResult();
            json.Data = EnumHelper.GetEnumContextList(typeof(PlateColor));
            return json;
        }

        /// <summary>
        /// 编辑车辆信息
        /// </summary>
        [HttpPost]
        [CheckPurview(Roles = "PK01020401,PK01020402")]
        public JsonResult SaveParkCar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request["PKID"])) throw new MyException("获取车场编号失败");

                string parkingId = Request["PKID"].ToString();
                BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(parkingId);
                if (parking == null) throw new MyException("车场信息不存在");

                BaseEmployee employee = GenerateBaseEmployeeModel(parking.VID);
                EmployeePlate plate = GenerateEmployeePlateModel(employee, parking.VID);
                BaseCard card = GenerateCardModel(parking, employee, plate);

                ParkGrant parkGrant = GenerateParkGrantModel(parking, plate, card);
                bool result = ParkGrantServices.Add(employee, plate, card, parkGrant);
                if (!result) throw new MyException("保存失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车辆信息失败");
                return Json(MyResult.Error("保存车辆信息失败"));
            }
        }
        /// <summary>
        /// 编辑车辆信息
        /// </summary>
        [HttpPost]
        public JsonResult SaveParkCarBitGroup(ParkCarBitGroup model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.PKID)) throw new MyException("获取车场编号失败");
                if (string.IsNullOrWhiteSpace(model.CarBitName)) throw new MyException("车位组名称不能为空");
                if (model.CarBitNum < 0) throw new MyException("车位数量不能小于0");

                bool isAdd = false;
                bool result = ParkGrantServices.AddOrUpdateCarBitGroup(model, out isAdd);
                if (!result) throw new MyException("保存车位组失败");

                string data = isAdd ? string.Format("{0}/{1}个车位", model.CarBitName, model.CarBitNum) : string.Empty;
                return Json(MyResult.Success(model.CarBitName, data));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车位组失败");
                return Json(MyResult.Error("保存车位组失败"));
            }
        }
        private BaseEmployee GenerateBaseEmployeeModel(string villageId)
        {
            if (string.IsNullOrWhiteSpace(Request["EmployeeName"])) throw new MyException("获取姓名失败");
            if (string.IsNullOrWhiteSpace(Request["MobilePhone"])) throw new MyException("获取电话失败");

            string mobilePhone = Request["MobilePhone"].ToString().Trim();
            if (!string.IsNullOrWhiteSpace(Request["CardID"]))
            {
                string errorMsg = string.Empty;
                BaseCard card = BaseCardServices.GetBaseCard(Request["CardID"].ToString(), out errorMsg);
                if (card == null || !string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取卡失败");

                //如果这个员工只有一条卡信息
                List<BaseCard> employeeCards = BaseCardServices.QueryBaseCardByEmployeeId(card.EmployeeID);
                if (employeeCards != null && employeeCards.Count == 1)
                {
                    BaseEmployee oldBaseEmployee = BaseEmployeeServices.QueryByEmployeeId(card.EmployeeID);
                    if (oldBaseEmployee != null)
                    {
                        oldBaseEmployee.MobilePhone = mobilePhone;
                        if (!string.IsNullOrWhiteSpace(Request["FamilyAddr"]))
                        {
                            oldBaseEmployee.FamilyAddr = Request["FamilyAddr"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(Request["Remark"]))
                        {
                            oldBaseEmployee.Remark = Request["Remark"].ToString();
                        }
                        oldBaseEmployee.EmployeeName = Request["EmployeeName"].ToString().Trim();
                        return oldBaseEmployee;
                    }
                }
            }

            BaseEmployee model = new BaseEmployee();
            model.EmployeeName = Request["EmployeeName"].ToString().Trim();
            model.RegTime = DateTime.Now;
            model.MobilePhone = mobilePhone;
            model.VID = villageId;
            model.EmployeeType = EmployeeType.Owner;
            model.EmployeeID = GuidGenerator.GetGuidString();
            if (!string.IsNullOrWhiteSpace(Request["FamilyAddr"]))
            {
                model.FamilyAddr = Request["FamilyAddr"].ToString();
            }
            if (!string.IsNullOrWhiteSpace(Request["Remark"]))
            {
                model.Remark = Request["Remark"].ToString();
            }
            return model;
        }
        private EmployeePlate GenerateEmployeePlateModel(BaseEmployee employee, string villageId)
        {
            if (string.IsNullOrWhiteSpace(Request["PlateNo"])) throw new MyException("获取车牌号失败");
            if (string.IsNullOrWhiteSpace(Request["Color"])) throw new MyException("获取车牌颜色失败");

            EmployeePlate model = new EmployeePlate();
            model.PlateNo = Request["PlateNo"].Trim().ToPlateNo();

            string errorMsg = string.Empty;
            EmployeePlate dbPlate = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(villageId, model.PlateNo, out errorMsg);
            if (!string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取车牌失败");
            if (dbPlate != null)
            {
                dbPlate.Color = (PlateColor)int.Parse(Request["Color"]);
                dbPlate.EmployeeID = employee.EmployeeID;
                return dbPlate;
            }
            model.Color = (PlateColor)int.Parse(Request["Color"]);
            model.PlateID = GuidGenerator.GetGuidString();
            model.EmployeeID = employee.EmployeeID;
            return model;
        }
        private BaseCard GenerateCardModel(BaseParkinfo parking, BaseEmployee employee, EmployeePlate plate)
        {
            BaseCard model = new BaseCard();
            model.CardID = GuidGenerator.GetGuidString();
            model.CardNo = plate.PlateNo;
            model.CardNumb = model.CardNo;
            model.VID = parking.VID;

            BaseCard oldCard = BaseCardServices.QueryBaseCardByParkingId(parking.PKID, model.CardNo);
            if (!string.IsNullOrWhiteSpace(Request["CardID"]))
            {
                model.CardID = Request["CardID"].ToString();
                if (oldCard != null && oldCard.CardID != model.CardID) throw new MyException("车牌号已存在，不能重复添加");

                string errorMsg = string.Empty;
                model = BaseCardServices.GetBaseCard(model.CardID, out errorMsg);
                if (model == null || !string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取卡信息失败");

                model.CardNo = plate.PlateNo;
                model.CardNumb = model.CardNo;
                model.VID = parking.VID;
            }
            else
            {
                if (oldCard != null) throw new MyException("车牌号已存在，不能重复添加");
                model.RegisterTime = DateTime.Now;
            }

            model.OperatorID = GetLoginUser.RecordID;
            model.CardSystem = CardSystem.Park;
            model.EmployeeID = employee.EmployeeID;
            model.CardType = CardType.Plate;
            return model;
        }
        private ParkGrant GenerateParkGrantModel(BaseParkinfo parking, EmployeePlate plate, BaseCard card)
        {
            ParkGrant model = new ParkGrant();
            model.PKID = parking.PKID;
            model.CardID = card.CardID;
            model.PlateID = plate.PlateID;
            model.PlateNo = plate.PlateNo;
            if (!string.IsNullOrWhiteSpace(Request["PKLot"]))
            {
                model.PKLot = Request["PKLot"].ToString();
            }
            int lotNumber = 0;
            if (!string.IsNullOrWhiteSpace(Request["PKLotNum"]) && int.TryParse(Request["PKLotNum"], out lotNumber))
            {
                model.PKLotNum = lotNumber;
            }
            if (string.IsNullOrWhiteSpace(Request["CarModelID"])) throw new MyException("获取选择的车型失败");
            string carModelId = Request["CarModelID"].ToString();
            ParkCarModel carModel = ParkCarModelServices.QueryByRecordId(carModelId);
            if (carModel == null) throw new MyException("选择的车型不存在");
            model.CarModelID = carModel.CarModelID;

            if (string.IsNullOrWhiteSpace(Request["CarTypeID"])) throw new MyException("获取选择的车类失败");
            string carTypeId = Request["CarTypeID"].ToString();
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) throw new MyException("选择的车类不存在");
            model.CarTypeID = carType.CarTypeID;

            if (string.IsNullOrWhiteSpace(Request["AreaIDS"])) throw new MyException("获取选择的车场区域失败");

            string areaIds = Request["AreaIDS"].ToString();
            model.AreaIDS = areaIds == "-1" ? string.Empty : areaIds;

            if (string.IsNullOrWhiteSpace(Request["GateID"])) throw new MyException("获取选择的车场通道失败");

            string gateIds = Request["GateID"].ToString();
            model.GateID = gateIds == "-1" ? string.Empty : gateIds;
            return model;
        }
        public string GetAreaDataByParkingId(string parkingId)
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");

            if (!string.IsNullOrWhiteSpace(parkingId))
            {
                List<ParkArea> areas = ParkAreaServices.GetParkAreaByParkingId(parkingId);
                foreach (var item in areas)
                {
                    strTree.Append(",{\"id\":\"" + item.AreaID + "\",");
                    strTree.Append("\"text\":\"" + item.AreaName + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetGateDataByAreaIds(string areaIds)
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");

            if (!string.IsNullOrWhiteSpace(areaIds))
            {
                List<string> areaIdlist = areaIds.Split(',').ToList();
                List<ParkGate> gates = ParkGateServices.QueryByParkAreaRecordIds(areaIdlist).ToList();
                foreach (var item in gates)
                {
                    strTree.Append(",{\"id\":\"" + item.GateID + "\",");
                    strTree.Append("\"text\":\"" + item.GateName + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }
        public string GetGateDataByParkingId(string parkingId)
        {
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            strTree.Append("{\"id\":\"-1\",");
            strTree.Append("\"text\":\"所有\",\"selected\":true");
            strTree.Append("}");

            if (!string.IsNullOrWhiteSpace(parkingId))
            {
                List<ParkGate> gates = ParkGateServices.QueryByParkingId(parkingId);
                foreach (var item in gates)
                {
                    strTree.Append(",{\"id\":\"" + item.GateID + "\",");
                    strTree.Append("\"text\":\"" + item.GateName + "\"");
                    strTree.Append("}");
                }
            }
            strTree.Append("]");
            return strTree.ToString();
        }

        public JsonResult CheckSubmitPkLot(string parkingId, string pkLots, string grantId, string carTypeId)
        {
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(carTypeId);
            if (carType == null) return Json(MyResult.Error("获取车类失败"));

            if (carType.BaseTypeID != BaseCarType.MonthlyRent) return Json(MyResult.Success());

            List<ParkGrant> oldGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(parkingId, pkLots, BaseCarType.MonthlyRent, grantId);

            DateTime? start = null; DateTime? end = null;
            if (!string.IsNullOrWhiteSpace(grantId))
            {
                ParkGrant cardGrant = ParkGrantServices.QueryByGrantId(grantId);
                if (cardGrant != null && cardGrant.BeginDate != DateTime.MinValue && cardGrant.EndDate != DateTime.MinValue)
                {
                    start = cardGrant.BeginDate;
                    end = cardGrant.EndDate;
                }
            }

            foreach (var item in oldGrants)
            {
                int oldLotLen = item.PKLot.Split(',').Length;
                int newLotLen = pkLots.Split(',').Length;
                if (oldLotLen != newLotLen)
                {

                    return Json(MyResult.Error(string.Format("车位号无效：车位号[{0}]与已存在的车位号[{1}]不完全一致", pkLots, item.PKLot)));
                }
                if (!start.HasValue && !end.HasValue)
                {
                    return Json(MyResult.Success(string.Format("车位已存在，如果是多车多位系统自动将该车辆的有效期修改为{0}至{1}", item.BeginDate.ToString("yyyy-MM-dd"), item.EndDate.ToString("yyyy-MM-dd"))));
                }
            }
            return Json(MyResult.Success());
        }

        /// <summary>
        /// 获取原来月卡有效结束时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CalculateOriginalEndDate(string grantId, DateTime start)
        {
            try
            {
                ParkGrant grant = ParkGrantServices.QueryByGrantId(grantId);
                if (grant == null) throw new MyException("车辆信息不存在");

                if (grant.BeginDate == DateTime.MinValue || grant.EndDate == DateTime.MinValue)
                    return Json(MyResult.Success());

                if (grant.EndDate != DateTime.MinValue && grant.EndDate.Date < DateTime.Now.Date)
                {
                    return Json(MyResult.Success(string.Empty, grant.EndDate.ToString("yyyy-MM-dd")));
                }
                if (grant.BeginDate.Date != start.Date)
                {
                    int day = (int)(start.Date - grant.BeginDate.Date).TotalDays;
                    DateTime newLimitEnd = grant.EndDate.AddDays(day);
                    return Json(MyResult.Success(string.Empty, newLimitEnd.ToString("yyyy-MM-dd")));
                }
                return Json(MyResult.Success(string.Empty, grant.EndDate.ToString("yyyy-MM-dd")));

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(MyResult.Error("获取原始有效期失败"));
            }
        }
        [HttpPost]
        public JsonResult CalculateNewEndDate(DateTime startDate, DateTime? endDate, int month)
        {
            try
            {
                string newEndDate = BaseCardServices.CalculateNewEndDate(startDate, endDate, month).ToString("yyyy-MM-dd");
                return Json(MyResult.Success(string.Empty, newEndDate));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(MyResult.Error("技术新的结束日期失败"));
            }

        }
        [HttpPost]
        public JsonResult GetMonthlyRentCarManyLot(string grantId, BaseCarType carType)
        {
            try
            {
                ParkGrant grant = ParkGrantServices.QueryByGrantId(grantId);
                if (grant == null) throw new MyException("车辆信息不存在");

                List<ParkGrant> monthCardGrants = new List<ParkGrant>();
                if (!string.IsNullOrWhiteSpace(grant.PKLot))
                {
                   if (carType == BaseCarType.MonthlyRent)
                    {
                        monthCardGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.MonthlyRent, grant.GID);
                    }
                    if (carType == BaseCarType.SeasonRent)
                    {
                        monthCardGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.SeasonRent, grant.GID);
                    }
                    if (carType == BaseCarType.YearRent)
                    {
                        monthCardGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.YearRent, grant.GID);
                    }
                    if (carType == BaseCarType.CustomRent)
                    {
                        monthCardGrants = ParkGrantServices.QueryByParkingAndLotAndCarType(grant.PKID, grant.PKLot, BaseCarType.CustomRent, grant.GID);
                    }
                }
                var result = from p in monthCardGrants
                             select new
                             {
                                 GID = p.GID,
                                 PlateNumber = GetPlateNoByPlateId(p.PlateID),
                                 EndDate = p.EndDate != DateTime.MinValue ? p.EndDate.ToString("yyyy-MM-dd") : string.Empty
                             };
                return Json(MyResult.Success(string.Empty, result));

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(MyResult.Error("获取多车多位数据失败"));
            }
        }
        private string GetPlateNoByPlateId(string plateId)
        {
            EmployeePlate model = EmployeePlateServices.Query(plateId);
            if (model != null)
            {
                return model.PlateNo;
            }
            return string.Empty;
        }
        [HttpPost]
        public JsonResult CalculateMonthlyRentExpiredWaitPayAmount(string grantId, DateTime start)
        {
            try
            {
                decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
                return Json(MyResult.Success(string.Empty, amount));

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return Json(MyResult.Error("计算月卡过期转临停已产生未支付金额失败"));
            }

        }
        //[HttpPost]
        //[CheckPurview(Roles = "PK01020403")]
        //public JsonResult CarRenewal()
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(Request.Params["GID"])) throw new MyException("获取车辆编号失败");
        //        if (string.IsNullOrWhiteSpace(Request.Params["CarBaseTypeID"])) throw new MyException("获取车辆基础类型失败");

        //        string grantId = Request.Params["GID"].ToString();
        //        BaseCarType baseType = (BaseCarType)Request.Params["CarBaseTypeID"].ToString().ToInt();

        //        int renewalMonth = 0;
        //        decimal monthlyRentTotalMoney = 0, needPayTotalMoney = 0, monthlyRentToTempNoPayAmount = 0;
        //        DateTime start = DateTime.MinValue, end = DateTime.MinValue;

        //        switch (baseType)
        //        {
        //            case BaseCarType.SeasonRent:
        //            case BaseCarType.YearRent:
        //            case BaseCarType.MonthlyRent:
        //            case BaseCarType.CustomRent:
        //            //case BaseCarType.WorkCar:
        //            case BaseCarType.VIPCar:
        //                {
        //                    if (string.IsNullOrWhiteSpace(Request.Params["RenewalMonth"])
        //                        || !int.TryParse(Request.Params["RenewalMonth"].ToString(), out renewalMonth)
        //                        || renewalMonth < 1)
        //                    {
        //                        throw new MyException("获取续费月数失败");
        //                    }
        //                    if (string.IsNullOrWhiteSpace(Request.Params["RenewalSeason"])
        //                        || !int.TryParse(Request.Params["RenewalSeason"].ToString(), out renewalMonth)
        //                        || renewalMonth < 1)
        //                    {
        //                        throw new MyException("获取续费季数失败");
        //                    }
        //                    if (string.IsNullOrWhiteSpace(Request.Params["RenewalYear"])
        //                        || !int.TryParse(Request.Params["RenewalYear"].ToString(), out renewalMonth)
        //                        || renewalMonth < 1)
        //                    {
        //                        throw new MyException("获取续费年数失败");
        //                    }

        //                    DateTime time;
        //                    if (string.IsNullOrWhiteSpace(Request.Params["NewEndDate"]) || !DateTime.TryParse(Request.Params["NewEndDate"].ToString(), out time))
        //                    {
        //                        throw new MyException("获取有效结束日期失败");
        //                    }
        //                    end = time;
        //                    if (string.IsNullOrWhiteSpace(Request.Params["BeginDate"]) || !DateTime.TryParse(Request.Params["BeginDate"].ToString(), out time))
        //                    {
        //                        throw new MyException("获取有效结开始日期失败");
        //                    }
        //                    start = time;

        //                    if (baseType == BaseCarType.MonthlyRent|| baseType == BaseCarType.SeasonRent|| baseType == BaseCarType.YearRent||baseType==BaseCarType.CustomRent)
        //                    {
        //                        if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentTotalMoney"]) || !decimal.TryParse(Request.Params["MonthlyRentTotalMoney"].ToString(), out monthlyRentTotalMoney))
        //                        {
        //                            throw new MyException("获取续期金额失败");
        //                        }

        //                        if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentToTempNoPayAmount"]) || !decimal.TryParse(Request.Params["MonthlyRentToTempNoPayAmount"].ToString(), out monthlyRentToTempNoPayAmount))
        //                        {
        //                            throw new MyException("获取月卡转临停金额失败");
        //                        }

        //                        if (string.IsNullOrWhiteSpace(Request.Params["NeedPayTotalMoney"]) || !decimal.TryParse(Request.Params["NeedPayTotalMoney"].ToString(), out needPayTotalMoney))
        //                        {
        //                            throw new MyException("获取待缴费总金额失败");
        //                        }
        //                        decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
        //                        if (monthlyRentToTempNoPayAmount != amount)
        //                        {
        //                            throw new MyException("计算月卡转临停金额异常");
        //                        }
        //                        if (needPayTotalMoney < 0 || needPayTotalMoney != (monthlyRentTotalMoney + monthlyRentToTempNoPayAmount))
        //                        {
        //                            throw new MyException("缴费金额计算错误");
        //                        }
        //                    }

        //                    break;
        //                }
        //            case BaseCarType.StoredValueCar:
        //                {
        //                    DateTime time;
        //                    if (!string.IsNullOrWhiteSpace(Request.Params["EndDate"]) && DateTime.TryParse(Request.Params["EndDate"].ToString(), out time))
        //                    {
        //                        end = time;
        //                    }

        //                    if (string.IsNullOrWhiteSpace(Request.Params["RechargeMoney"])
        //                        || !decimal.TryParse(Request.Params["RechargeMoney"], out needPayTotalMoney)
        //                        || needPayTotalMoney < 0)
        //                    {
        //                        throw new MyException("获取充值金额失败");
        //                    }
        //                    break;
        //                }
        //            default: throw new MyException("选择的车辆不能续期和充值");
        //        }
        //        bool result = ParkGrantServices.RenewalsOrRecharge(grantId, renewalMonth, needPayTotalMoney, GetLoginUser.RecordID, start, end);
        //        if (!result) throw new MyException("操作失败");
        //        return Json(MyResult.Success());

        //    }
        //    catch (MyException ex)
        //    {
        //        return Json(MyResult.Error(ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionsServices.AddExceptions(ex, "车辆续期或充值失败");
        //        return Json(MyResult.Error("操作失败"));
        //    }
        //}
        [HttpPost]
        [CheckPurview(Roles = "PK01020403")]
        public JsonResult CarRenewal()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request.Params["GID"])) throw new MyException("获取车辆编号失败");
                if (string.IsNullOrWhiteSpace(Request.Params["CarBaseTypeID"])) throw new MyException("获取车辆基础类型失败");

                string grantId = Request.Params["GID"].ToString();
                BaseCarType baseType = (BaseCarType)Request.Params["CarBaseTypeID"].ToString().ToInt();

                int renewalMonth = 0;
                decimal monthlyRentTotalMoney = 0, needPayTotalMoney = 0, monthlyRentToTempNoPayAmount = 0;
                DateTime start = DateTime.MinValue, end = DateTime.MinValue;

                switch (baseType)
                {
                    case BaseCarType.SeasonRent:
                        {
                            if (string.IsNullOrWhiteSpace(Request.Params["RenewalSeason"])
                                || !int.TryParse(Request.Params["RenewalSeason"].ToString(), out renewalMonth)
                                || renewalMonth < 1)
                            {
                                throw new MyException("获取续费季数失败");
                            }
                            DateTime time;
                            if (string.IsNullOrWhiteSpace(Request.Params["NewEndDate"]) || !DateTime.TryParse(Request.Params["NewEndDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结束日期失败");
                            }
                            end = time;
                            if (string.IsNullOrWhiteSpace(Request.Params["BeginDate"]) || !DateTime.TryParse(Request.Params["BeginDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结开始日期失败");
                            }
                            start = time;

                            if (baseType == BaseCarType.MonthlyRent || baseType == BaseCarType.SeasonRent || baseType == BaseCarType.YearRent || baseType == BaseCarType.CustomRent)
                            {
                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentTotalMoney"]) || !decimal.TryParse(Request.Params["MonthlyRentTotalMoney"].ToString(), out monthlyRentTotalMoney))
                                {
                                    throw new MyException("获取续期金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentToTempNoPayAmount"]) || !decimal.TryParse(Request.Params["MonthlyRentToTempNoPayAmount"].ToString(), out monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("获取月卡转临停金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["NeedPayTotalMoney"]) || !decimal.TryParse(Request.Params["NeedPayTotalMoney"].ToString(), out needPayTotalMoney))
                                {
                                    throw new MyException("获取待缴费总金额失败");
                                }
                                decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
                                if (monthlyRentToTempNoPayAmount != amount)
                                {
                                    throw new MyException("计算月卡转临停金额异常");
                                }
                                if (needPayTotalMoney < 0 || needPayTotalMoney != (monthlyRentTotalMoney + monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("缴费金额计算错误");
                                }
                            }

                            break;
                        }
                    case BaseCarType.YearRent:
                        {
                            if (string.IsNullOrWhiteSpace(Request.Params["RenewalYear"])
                                || !int.TryParse(Request.Params["RenewalYear"].ToString(), out renewalMonth)
                                || renewalMonth < 1)
                            {
                                throw new MyException("获取续费年数失败");
                            }
                            DateTime time;
                            if (string.IsNullOrWhiteSpace(Request.Params["NewEndDate"]) || !DateTime.TryParse(Request.Params["NewEndDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结束日期失败");
                            }
                            end = time;
                            if (string.IsNullOrWhiteSpace(Request.Params["BeginDate"]) || !DateTime.TryParse(Request.Params["BeginDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结开始日期失败");
                            }
                            start = time;

                            if (baseType == BaseCarType.MonthlyRent || baseType == BaseCarType.SeasonRent || baseType == BaseCarType.YearRent || baseType == BaseCarType.CustomRent)
                            {
                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentTotalMoney"]) || !decimal.TryParse(Request.Params["MonthlyRentTotalMoney"].ToString(), out monthlyRentTotalMoney))
                                {
                                    throw new MyException("获取续期金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentToTempNoPayAmount"]) || !decimal.TryParse(Request.Params["MonthlyRentToTempNoPayAmount"].ToString(), out monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("获取月卡转临停金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["NeedPayTotalMoney"]) || !decimal.TryParse(Request.Params["NeedPayTotalMoney"].ToString(), out needPayTotalMoney))
                                {
                                    throw new MyException("获取待缴费总金额失败");
                                }
                                decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
                                if (monthlyRentToTempNoPayAmount != amount)
                                {
                                    throw new MyException("计算月卡转临停金额异常");
                                }
                                if (needPayTotalMoney < 0 || needPayTotalMoney != (monthlyRentTotalMoney + monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("缴费金额计算错误");
                                }
                            }

                            break;
                        }
                    case BaseCarType.MonthlyRent:
                        {
                            if (string.IsNullOrWhiteSpace(Request.Params["RenewalMonth"])
                                || !int.TryParse(Request.Params["RenewalMonth"].ToString(), out renewalMonth)
                                || renewalMonth < 1)
                            {
                                throw new MyException("获取续费月数失败");
                            }
                            DateTime time;
                            if (string.IsNullOrWhiteSpace(Request.Params["NewEndDate"]) || !DateTime.TryParse(Request.Params["NewEndDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结束日期失败");
                            }
                            end = time;
                            if (string.IsNullOrWhiteSpace(Request.Params["BeginDate"]) || !DateTime.TryParse(Request.Params["BeginDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结开始日期失败");
                            }
                            start = time;

                            if (baseType == BaseCarType.MonthlyRent || baseType == BaseCarType.SeasonRent || baseType == BaseCarType.YearRent || baseType == BaseCarType.CustomRent)
                            {
                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentTotalMoney"]) || !decimal.TryParse(Request.Params["MonthlyRentTotalMoney"].ToString(), out monthlyRentTotalMoney))
                                {
                                    throw new MyException("获取续期金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentToTempNoPayAmount"]) || !decimal.TryParse(Request.Params["MonthlyRentToTempNoPayAmount"].ToString(), out monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("获取月卡转临停金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["NeedPayTotalMoney"]) || !decimal.TryParse(Request.Params["NeedPayTotalMoney"].ToString(), out needPayTotalMoney))
                                {
                                    throw new MyException("获取待缴费总金额失败");
                                }
                                decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
                                if (monthlyRentToTempNoPayAmount != amount)
                                {
                                    throw new MyException("计算月卡转临停金额异常");
                                }
                                if (needPayTotalMoney < 0 || needPayTotalMoney != (monthlyRentTotalMoney + monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("缴费金额计算错误");
                                }
                            }

                            break;
                        }
                    case BaseCarType.CustomRent:
                    //case BaseCarType.WorkCar:
                    case BaseCarType.VIPCar:
                        {
                            
                            
                            

                            DateTime time;
                            if (string.IsNullOrWhiteSpace(Request.Params["NewEndDate"]) || !DateTime.TryParse(Request.Params["NewEndDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结束日期失败");
                            }
                            end = time;
                            if (string.IsNullOrWhiteSpace(Request.Params["BeginDate"]) || !DateTime.TryParse(Request.Params["BeginDate"].ToString(), out time))
                            {
                                throw new MyException("获取有效结开始日期失败");
                            }
                            start = time;

                            if (baseType == BaseCarType.MonthlyRent || baseType == BaseCarType.SeasonRent || baseType == BaseCarType.YearRent || baseType == BaseCarType.CustomRent)
                            {
                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentTotalMoney"]) || !decimal.TryParse(Request.Params["MonthlyRentTotalMoney"].ToString(), out monthlyRentTotalMoney))
                                {
                                    throw new MyException("获取续期金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["MonthlyRentToTempNoPayAmount"]) || !decimal.TryParse(Request.Params["MonthlyRentToTempNoPayAmount"].ToString(), out monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("获取月卡转临停金额失败");
                                }

                                if (string.IsNullOrWhiteSpace(Request.Params["NeedPayTotalMoney"]) || !decimal.TryParse(Request.Params["NeedPayTotalMoney"].ToString(), out needPayTotalMoney))
                                {
                                    throw new MyException("获取待缴费总金额失败");
                                }
                                decimal amount = ParkOrderServices.CalculateMonthlyRentExpiredWaitPayAmount(start, grantId);
                                if (monthlyRentToTempNoPayAmount != amount)
                                {
                                    throw new MyException("计算月卡转临停金额异常");
                                }
                                if (needPayTotalMoney < 0 || needPayTotalMoney != (monthlyRentTotalMoney + monthlyRentToTempNoPayAmount))
                                {
                                    throw new MyException("缴费金额计算错误");
                                }
                            }

                            break;
                        }
                    case BaseCarType.StoredValueCar:
                        {
                            DateTime time;
                            if (!string.IsNullOrWhiteSpace(Request.Params["EndDate"]) && DateTime.TryParse(Request.Params["EndDate"].ToString(), out time))
                            {
                                end = time;
                            }

                            if (string.IsNullOrWhiteSpace(Request.Params["RechargeMoney"])
                                || !decimal.TryParse(Request.Params["RechargeMoney"], out needPayTotalMoney)
                                || needPayTotalMoney < 0)
                            {
                                throw new MyException("获取充值金额失败");
                            }
                            break;
                        }
                    default: throw new MyException("选择的车辆不能续期和充值");
                }
                bool result = ParkGrantServices.RenewalsOrRecharge(grantId, renewalMonth, needPayTotalMoney, GetLoginUser.RecordID, start, end);
                if (!result) throw new MyException("操作失败");
                return Json(MyResult.Success());

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "车辆续期或充值失败");
                return Json(MyResult.Error("操作失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020404")]
        public JsonResult CancelParkGrant(string grantId)
        {
            try
            {
                bool result = ParkGrantServices.CancelParkGrant(grantId);
                if (!result) throw new MyException("取消授权失败");
                return Json(MyResult.Success());

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "取消授权失败");
                return Json(MyResult.Error("取消授权失败"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020404")]
        public JsonResult CancelAllParkGrant(string grantId)
        {
            try
            {
                bool result = ParkGrantServices.CancelAllParkGrant(grantId);
                if (!result) throw new MyException("批量删除失败");
                return Json(MyResult.Success());

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "批量删除失败");
                return Json(MyResult.Error("批量删除失败"));
            }
        }
        /// <summary>
        /// 暂停使用
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="grantId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01020405")]
        public JsonResult CarSuspendUse(DateTime start, DateTime? end, string grantId,DateTime? EndDate)
        {
            try
            {
                DateTime endDate = end.HasValue ? end.Value : DateTime.MinValue;
                DateTime nEndDate = EndDate.HasValue ? EndDate.Value : DateTime.MinValue;
                DateTime NewEndDate = GetNewEndDate(start, endDate, nEndDate);
                bool result = ParkGrantServices.CarSuspendUse(start, endDate, grantId, NewEndDate);
                if (!result) throw new MyException("暂停使用失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存暂停失败");
                return Json(MyResult.Error("暂停失败"));
            }
        }
        public static DateTime GetNewEndDate(DateTime start, DateTime? endDate, DateTime EndDate)
        {
            if (EndDate == DateTime.MinValue) {
                return EndDate;
            }
            DateTime NewEndDate = EndDate;
            if (endDate.Value.Year != start.Year)
            {
                NewEndDate=NewEndDate.AddYears(endDate.Value.Year - start.Year);
            }
            if (endDate.Value.Month != start.Month) {
                NewEndDate=NewEndDate.AddMonths(endDate.Value.Month - start.Month);
            }
            if (endDate.Value.Day != start.Day)
            {
                NewEndDate=NewEndDate.AddDays(endDate.Value.Day - start.Day);

            }
            return NewEndDate;
        }
        /// <summary>
        /// 恢复使用
        /// </summary>
        /// <param name="grantId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01020406")]
        public JsonResult CarRestoreUse(string grantId,DateTime? EndDate)
        {
            try
            {
                DateTime nEndDate = EndDate.HasValue ? EndDate.Value : DateTime.MinValue;
                DateTime NewRestoreDate = GetNewRestoreDate(grantId,nEndDate);
                bool result = ParkGrantServices.CarRestoreUse(grantId, NewRestoreDate);
                if (!result) throw new MyException("保存设置失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车辆恢复使用设置失败");
                return Json(MyResult.Error("保存设置失败"));
            }
        }
        public static DateTime GetNewRestoreDate(string grantId,DateTime date)
        {
            if (date == DateTime.MinValue) {
                return date;
            }
            ParkCardSuspendPlan plan = ParkGrantServices.QueryByGrantID(grantId);
            DateTime NewEndDate = date;
            if (DateTime.Now.Year != plan.EndDate.Year)
            {
                NewEndDate= NewEndDate.AddYears(DateTime.Now.Year - plan.EndDate.Year);
            }
            if (DateTime.Now.Month != plan.EndDate.Month)
            {
                NewEndDate = NewEndDate.AddMonths(DateTime.Now.Month - plan.EndDate.Month);
            }
            if (DateTime.Now.Day != plan.EndDate.Day)
            {
                NewEndDate = NewEndDate.AddDays(DateTime.Now.Day - plan.EndDate.Day);
            }
            return NewEndDate;
        }
        /// <summary>
        /// 重新启用
        /// </summary>
        /// <param name="cardGrantId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01020408")]
        public JsonResult CarAgainEnabledUse(string grantId, DateTime? EndDate)
        {
            try
            {
                DateTime nEndDate = EndDate.HasValue ? EndDate.Value : DateTime.MinValue;
                bool result = ParkGrantServices.CarAgainEnabledUse(grantId,nEndDate);
                if (!result) throw new MyException("保存设置失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车辆重新启用设置失败");
                return Json(MyResult.Error("保存设置失败"));
            }
        }
        /// <summary>
        /// 月卡停用
        /// </summary>
        /// <param name="grantId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPurview(Roles = "PK01020407")]
        public JsonResult CarStopUse(string grantId, DateTime? EndDate)
        {
            try
            {
                DateTime nEndDate = EndDate.HasValue ? EndDate.Value : DateTime.MinValue;
                bool result = ParkGrantServices.CarStopUse(grantId, nEndDate);
                if (!result) throw new MyException("保存设置失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "月卡停用设置失败");
                return Json(MyResult.Error("保存设置失败"));
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveExeclFile()
        {
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    string fileExtension = System.IO.Path.GetExtension(file.FileName);
                    if (fileExtension != ".xlsx" && fileExtension != ".xls")
                    {
                        return Json(MyResult.Error("请选择Execl文件"));
                    }
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    string filePath = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), "TempFile", GetLoginUser.RecordID);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string path = System.IO.Path.Combine(filePath, fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    file.SaveAs(path);
                    return Json(MyResult.Success(string.Empty, string.Format("{0}/{1}/{2}", "/TempFile", GetLoginUser.RecordID, fileName)));

                }
                return Json(MyResult.Error("请选择Execl文件"));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "批量导入车辆时获取导入文件失败");
                return Json(MyResult.Error("导入异常"));
            }
        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020401")]
        public JsonResult SaveImportCar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Request["PKID"])) throw new MyException("获取车场信息失败");

                string pkId = Request["PKID"].ToString();
                BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(pkId);
                if (parking == null) throw new MyException("获取车场信息失败");

                if (string.IsNullOrWhiteSpace(Request["CarFilePath"])) throw new MyException("获取导入文件失败");
                string carFilePath = Request["CarFilePath"].ToString();
                string filepath = System.Web.HttpContext.Current.Server.MapPath(carFilePath);
                if (!System.IO.File.Exists(filepath)) throw new MyException("上传的文件不存在");

                StringBuilder strError = new StringBuilder();
                DataTable table = ExcelToDataTable(filepath, "ParkCarData");
                int rowIndex = 1;
                foreach (DataRow dr in table.Rows)
                {
                    try
                    {
                        BaseEmployee employee = GetBatchImportBaseEmployeeModel(dr, parking.VID);
                        EmployeePlate plate = GetBatchImportEmployeePlateModel(employee, dr);
                        BaseCard card = GetBatchImportBaseCardModel(parking, employee, plate, dr);
                        ParkGrant parkGrant = GetBatchImportParkGrantModel(parking, plate, card, dr);

                        bool result = ParkGrantServices.Add(employee, plate, card, parkGrant);
                        if (!result) throw new MyException("保存失败");
                    }
                    catch (MyException ex)
                    {
                        strError.AppendFormat("第{0}行添加失败，原因：{1}<br>", rowIndex, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        strError.AppendFormat("第{0}行添加失败，原因：{1}<br>", rowIndex, ex.Message);
                    }
                    rowIndex++;
                }
                return Json(MyResult.Success(strError.ToString()));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "导入车辆信息失败");
                return Json(MyResult.Error("保存失败"));
            }
        }
        private ParkGrant GetBatchImportParkGrantModel(BaseParkinfo parking, EmployeePlate plate, BaseCard card, DataRow dr)
        {
            ParkGrant model = new ParkGrant();
            model.PKID = parking.PKID;
            model.CardID = card.CardID;
            model.PlateID = plate.PlateID;
            model.PlateNo = plate.PlateNo;

            if (!string.IsNullOrWhiteSpace(dr["车位组"].ToString()))
            {
                model.PKLot = dr["车位组"].ToString().Trim();
            }
            int pkLotNum = 0;
            if (!string.IsNullOrWhiteSpace(dr["车位数量"].ToString()) && int.TryParse(dr["车位数量"].ToString().Trim(), out pkLotNum))
            {
                model.PKLotNum = pkLotNum;
            }
            if (string.IsNullOrWhiteSpace(Request["CarTypeID"])) throw new MyException("获取车类失败");
            ParkCarType carType = ParkCarTypeServices.QueryParkCarTypeByRecordId(Request["CarTypeID"].ToString());
            if (carType == null) throw new MyException("车类不存在");
            model.CarTypeID = carType.CarTypeID;
            model.CardType = carType;

            if (string.IsNullOrWhiteSpace(Request["CarModelID"])) throw new MyException("获取选择的车型失败");
            string carModelId = Request["CarModelID"].ToString();
            ParkCarModel carModel = ParkCarModelServices.QueryByRecordId(carModelId);
            if (carModel == null) throw new MyException("选择的车型不存在");
            model.CarModelID = carModel.CarModelID;

            if (string.IsNullOrWhiteSpace(Request["AreaIDS"])) throw new MyException("获取选择的车场区域失败");
            string areaIds = Request["AreaIDS"].ToString();
            model.AreaIDS = areaIds == "-1" ? string.Empty : areaIds;

            if (string.IsNullOrWhiteSpace(Request["GateID"])) throw new MyException("获取选择的车场通道失败");
            string gateIds = Request["GateID"].ToString();
            model.GateID = gateIds == "-1" ? string.Empty : gateIds;

            if (!string.IsNullOrWhiteSpace(dr["有效开始日期"].ToString()))
            {
                DateTime start;
                if (!DateTime.TryParse(dr["有效开始日期"].ToString().Trim(), out start))
                {
                    throw new MyException("有效开始日期错误");
                }
                model.BeginDate = start;
            }
            if (!string.IsNullOrWhiteSpace(dr["有效结束日期"].ToString()))
            {
                DateTime end;
                if (!DateTime.TryParse(dr["有效结束日期"].ToString().Trim(), out end))
                {
                    throw new MyException("有效结束日期错误");
                }
                model.EndDate = end;
            }
            return model;
        }
        private EmployeePlate GetBatchImportEmployeePlateModel(BaseEmployee employee, DataRow dr)
        {
            if (string.IsNullOrWhiteSpace(dr["车牌号"].ToString())) throw new MyException("获取车牌号失败");

            EmployeePlate model = new EmployeePlate();
            model.PlateNo = dr["车牌号"].ToString().ToPlateNo();

            string errorMsg = string.Empty;
            EmployeePlate dbPlate = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(employee.VID, model.PlateNo, out errorMsg);
            if (!string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取车牌失败");
            if (dbPlate != null)
            {
                dbPlate.EmployeeID = employee.EmployeeID;
                return dbPlate;
            }
            model.Color = GetImportEmployeePlateColor(dr["车牌颜色"].ToString());
            model.PlateID = GuidGenerator.GetGuidString();
            model.EmployeeID = employee.EmployeeID;
            return model;
        }
        private PlateColor GetImportEmployeePlateColor(string color)
        {
            List<EnumContext> models = EnumHelper.GetEnumContextList(typeof(PlateColor));
            foreach (var item in models)
            {
                if (item.Description == color.Trim())
                {
                    return (PlateColor)item.EnumValue;
                }
            }
            return PlateColor.Blue;

        }
        private BaseCard GetBatchImportBaseCardModel(BaseParkinfo parking, BaseEmployee employee, EmployeePlate plate, DataRow dr)
        {
            BaseCard model = new BaseCard();
            model.CardID = GuidGenerator.GetGuidString();
            model.CardNo = plate.PlateNo;
            model.CardNumb = model.CardNo;
            model.VID = parking.VID;
            model.RegisterTime = DateTime.Now;

            BaseCard oldCard = BaseCardServices.QueryBaseCard(parking.VID, model.CardNo);
            if (oldCard != null)
            {
                oldCard.OperatorID = GetLoginUser.RecordID;
                oldCard.CardSystem = CardSystem.Park;
                oldCard.EmployeeID = employee.EmployeeID;
                oldCard.CardType = CardType.Plate;
                return oldCard;
            }

            model.OperatorID = GetLoginUser.RecordID;
            model.CardSystem = CardSystem.Park;
            model.EmployeeID = employee.EmployeeID;
            model.CardType = CardType.Plate;

            return model;
        }
        private BaseEmployee GetBatchImportBaseEmployeeModel(DataRow dr, string villageId)
        {
            if (string.IsNullOrWhiteSpace(dr["车主姓名"].ToString())) throw new MyException("车主姓名格式不正确");
            if (string.IsNullOrWhiteSpace(dr["车主电话"].ToString())) throw new MyException("车主电话格式不正确");

            string phone = dr["车主电话"].ToString().Trim();
            string name = dr["车主姓名"].ToString().Trim();
            string familyAddr = string.Empty;
            if (!string.IsNullOrWhiteSpace(dr["家庭地址"].ToString()))
            {
                familyAddr = dr["家庭地址"].ToString();
            }
            string remark = string.Empty;
            if (!string.IsNullOrWhiteSpace(dr["备注"].ToString()))
            {
                remark = dr["备注"].ToString();
            }
            BaseEmployee model = new BaseEmployee();
            model.EmployeeName = name;
            model.RegTime = DateTime.Now;
            model.MobilePhone = phone;
            model.VID = villageId;
            model.EmployeeType = EmployeeType.Owner;
            model.EmployeeID = GuidGenerator.GetGuidString();
            model.FamilyAddr = familyAddr;
            model.Remark = remark;
            return model;
        }
        private DataTable ExcelToDataTable(string fileUrl, string tableName)
        {
            const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tableName);
                return ds.Tables[0];
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }
        [ValidateInput(false)]
        public ActionResult DownLoadExcel(DataTable dt)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("车辆信息");
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                    row2.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
            }
            string pathToFiles = Server.MapPath("/");
            string filename= DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            string path = @"" + pathToFiles + "\\" + filename;

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
        /// <summary>
        /// 导出全部数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Export_AllCar()
        {
            //if (string.IsNullOrEmpty(Request.Params["parkingId"])) return string.Empty;
            string parkingid = Request.Params["parkingId"].ToString();
            List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
            List<ParkGrantView> pgw = ParkGrantServices.QueryPage1(GetQueryCondition());
            List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingid);
            var result = from p in pgw
                         select new
                         {

                             //BeginDate = GetBeginDate(p),
                             //EndDate = p.EndDate == DateTime.MinValue ? string.Empty : p.EndDate.ToString("yyyy-MM-dd"),

                             车牌号 = p.PlateNo,
                             车牌颜色 = p.Color.GetDescription(),
                             状态 = GetStateDescription(p.State, p.EndDate),
                             车型 = GetCarModelName(carModels, p.CarModelID),
                             车类 = GetCarTypeName(carTypes, p.CarTypeID),
                             车主名称= p.EmployeeName,
                             车主电话 = string.IsNullOrWhiteSpace(p.MobilePhone) ? p.HomePhone : p.MobilePhone,
                             余额 = p.Balance,
                             车位组 = GetParkCarBitGroupDes(p.PKLot, p.PKLotNum),
                             有效期 = GetUsefulTime(p.BeginDate, p.EndDate),
                             家庭地址 = p.FamilyAddr,
                             备注 = p.Remark,
                         };
            StringBuilder sb = new StringBuilder();
            string str = JsonHelper.GetJsonString(result);
            sb.Append(str);
            var dt = JsonToDataTable(sb.ToString());
            var dl = DownLoadExcel(dt);
            return dl;
        }
        public string GetUsefulTime(DateTime beginTime, DateTime endTime)
        {
            if (beginTime != null && endTime != null)
            {
                if (beginTime != DateTime.MinValue && endTime != DateTime.MinValue)
                {
                    if (endTime.Date < DateTime.Now.Date)
                    {
                        return beginTime.ToString("yyyy-MM-dd") + "至" + endTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        return beginTime.ToString("yyyy-MM-dd") + "至" + DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    return string.Empty;
                }

            }
            else
            {
                return string.Empty;
            }
        }
        public static DataTable JsonToDataTable(string strJson)
        {
            ////取出表名  
            //Regex rg = new Regex(@"(?<={)[^:]+(?=:/[)", RegexOptions.IgnoreCase);
            //string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            ////去除表名  
            //strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            //strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                List<string> list = new List<string>(strRows);
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[9].IndexOf("有效期") < 0)
                    {
                        var a1 = list[8];
                        var a2 = list[9];
                        var a3 = a1 + "," + a2;
                        list.RemoveAt(8);
                        list.RemoveAt(8);
                        list.Insert(8, a3);
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[11].IndexOf("备注") < 0)
                    {
                        var b1 = list[10];
                        var b2 = list[11];
                        var b3 = b1 + "," + b2;
                        list.RemoveAt(10);
                        list.RemoveAt(10);
                        list.Insert(10, b3);
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    if (list.Count > 12)
                    {
                        var c1 = list[11];
                        var c2 = list[12];
                        var c3 = c1 + "," + c2;
                        list.RemoveAt(11);
                        list.RemoveAt(11);
                        list.Insert(11, c3);
                    }
                }
                string[] strRows1 = list.ToArray();
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "";
                    foreach (string str in strRows1)
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
                for (int r = 0; r < strRows1.Length; r++)
                {
                    if (strRows1[r].Split(':')[1].Trim().Replace("\"", "").Trim() == "null")
                    {
                        dr[r] = "";
                    }
                    else
                    {
                        dr[r] = strRows1[r].Split(':')[1].Trim().Replace("\"", "").Trim();
                    }
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        /// <summary>
        /// 获取车辆操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkCarOperatePurview()
        {
            JsonResult result = new JsonResult();
            List<SystemOperatePurview> options = new List<SystemOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010204").ToList();

            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01020401":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 0;
                            options.Add(option);

                            SystemOperatePurview option1 = new SystemOperatePurview();
                            option1.id = "btnimport";
                            option1.iconCls = "icon-import";
                            option1.text = "批量导入";
                            option1.handler = "Import";
                            option1.sort = 1;
                            options.Add(option1);
                            break;
                        }
                    case "PK01020402":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 0;
                            options.Add(option);
                            break;
                        }
                    case "PK01020403":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnrenewal";
                            option.iconCls = "icon-renewal";
                            option.text = "续期";
                            option.handler = "Renewal";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                    case "PK01020404":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Delete";
                            option.sort = 4;
                            options.Add(option);
                            break;
                        }
                    case "PK01020405":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnsuspend";
                            option.iconCls = "icon-uspend";
                            option.text = "暂停";
                            option.handler = "Suspend";
                            option.sort = 6;
                            options.Add(option);
                            break;
                        }
                    case "PK01020406":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnrestore";
                            option.iconCls = "icon-restore";
                            option.text = "恢复";
                            option.handler = "Restore";
                            option.sort = 7;
                            options.Add(option);
                            break;
                        }
                    case "PK01020407":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnstop";
                            option.iconCls = "icon-stop";
                            option.text = "停用";
                            option.handler = "Stop";
                            option.sort = 8;
                            options.Add(option);
                            break;
                        }
                    case "PK01020408":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.id = "btnenabled";
                            option.iconCls = "icon-enabled";
                            option.text = "启用";
                            option.handler = "Enabled";
                            option.sort = 9;
                            options.Add(option);
                            break;
                        }
                }
            }

            SystemOperatePurview roption = new SystemOperatePurview();
            roption.text = "刷新";
            roption.handler = "Refresh";
            roption.sort = 11;
            options.Add(roption);

            SystemOperatePurview roption2 = new SystemOperatePurview();
            roption2.id = "btnexport";
            roption2.text = "导出";
            roption2.handler = "Export";
            roption2.iconCls = "icon-print";
            roption2.sort = 12;
            options.Add(roption2);

            result.Data = options.OrderBy(p => p.sort);
            return result;
        }
    }
}
