using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Utilities;
using Common.Utilities.Helpers;
using Common.Entities.Parking;
using Common.Services.Park;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
     [CheckPurview(Roles = "PK010201")]
    public class ParkCarTypeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetParkCarTypeData()
        {
            JsonResult json = new JsonResult();
            try
            {
                if (string.IsNullOrEmpty(Request.Params["parkingId"])) return json;

                string parkingid = Request.Params["parkingid"].ToString();
                BaseParkinfo parking = ParkingServices.QueryParkingByRecordId(parkingid);
                List<ParkCarType> carTypes = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
                foreach (var item in carTypes)
                {
                    ProcessListShowData(item);
                }
                var result = from p in carTypes select new {
                    CarTypeID = p.CarTypeID,
                    CarTypeName = p.CarTypeName,
                    PKID = p.PKID,
                    BaseTypeID = (int)p.BaseTypeID,
                    BaseTypeDes = p.BaseTypeID.GetDescription(),
                    RepeatIn = p.RepeatIn,
                    ParkName=parking.PKName,
                    RepeatOut = p.RepeatOut,
                    AffirmIn = p.AffirmIn,
                    AffirmOut = p.AffirmOut,
                    InBeginTime = p.InBeginTime != DateTime.MinValue ? p.InBeginTime.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    InEdnTime = p.InEdnTime != DateTime.MinValue ? p.InEdnTime.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    MaxUseMoney = p.MaxUseMoney,
                    AllowLose = p.AllowLose,
                    LpDistinguish = (int)p.LpDistinguish,
                    LpDistinguishDes = p.LpDistinguish.GetDescription(),
                    InOutEditCar = p.InOutEditCar,
                    InOutTime = p.InOutTime,
                    CarNoLike = p.CarNoLike,
                    IsAllowOnlIne = p.IsAllowOnlIne,
                    Amount = p.Amount,
                    MaxMonth = p.MaxMonth,
                    MaxValue = p.MaxValue,
                    OnlineUnit = p.OnlineUnit,
                    OverdueToTemp = p.OverdueToTemp,
                    OverdueToTempDes = p.OverdueToTemp.GetDescription(),
                    LotOccupy = p.LotOccupy,
                    LotOccupyDes = p.LotOccupy.GetDescription(),
                    Deposit = p.Deposit,
                    MonthCardExpiredEnterDay = p.MonthCardExpiredEnterDay,
                    AffirmBegin = string.IsNullOrWhiteSpace(p.AffirmBegin)?"00:00":p.AffirmBegin,
                    AffirmEnd = string.IsNullOrWhiteSpace(p.AffirmEnd) ? "23:59" : p.AffirmEnd,
                    IsDispatch=p.IsDispatch
                };
                json.Data = result;
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取车类型集合失败");
            }
            return json;

        }
        private void ProcessListShowData(ParkCarType carType)
        {
            if (carType.BaseTypeID == BaseCarType.VIPCar)
            {

                carType.MaxValue = -1;
                carType.Amount = -1;
                carType.MaxMonth = -1;
                carType.MaxUseMoney = -1;
            }
            if (carType.BaseTypeID == BaseCarType.StoredValueCar)
            {
                carType.Amount = -1;
                carType.MaxMonth = -1;
                carType.OverdueToTemp = OverdueToTemp.ExpiredToTemp;
                carType.LotOccupy = LotOccupy.ChangeToTemp;
            }
            if (carType.BaseTypeID == BaseCarType.MonthlyRent)
            {
                carType.MaxValue = -1;
                carType.MaxUseMoney = -1;
            }
            if (carType.BaseTypeID == BaseCarType.TempCar)
            {
                carType.MaxValue = -1;
                carType.Amount = -1;
                carType.MaxMonth = -1;
                carType.OverdueToTemp = OverdueToTemp.ExpiredToTemp;
                carType.LotOccupy = LotOccupy.ChangeToTemp;
            }
            if (carType.BaseTypeID == BaseCarType.SeasonRent)
            {
                carType.MaxValue = -1;
                carType.MaxUseMoney = -1;
            }
            if (carType.BaseTypeID == BaseCarType.YearRent)
            {
                carType.MaxValue = -1;
                carType.MaxUseMoney = -1;
            }
            if (carType.BaseTypeID == BaseCarType.CustomRent)
            {
                carType.MaxValue = -1;
                carType.MaxUseMoney = -1;
            }
            //if (carType.BaseTypeID == BaseCarType.WorkCar)
            //{
            //    carType.MaxValue = -1;
            //    carType.Amount = -1;
            //    carType.MaxMonth = -1;
            //    carType.OverdueToTemp = OverdueToTemp.ExpiredToTemp;
            //    carType.LotOccupy = LotOccupy.ChangeToTemp;
            //    carType.MaxUseMoney = -1;
            //}

        }
        [HttpPost]
        [CheckPurview(Roles = "PK01020101,PK01020102")]
        public JsonResult SaveUpdate(ParkCarType model)
        {
            try
            {
                UpdateParkCarTypeDefault(model);
                bool result = false;
                if (string.IsNullOrWhiteSpace(model.CarTypeID))
                {
                    result = ParkCarTypeServices.Add(model);
                    if (!result) throw new MyException("添加失败");
                    return Json(MyResult.Success());
                }
                else
                {
                    result = ParkCarTypeServices.Update(model);
                    if (!result) throw new MyException("修改失败");
                    return Json(MyResult.Success());
                }
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存车类型信息失败");
                return Json(MyResult.Error("保存车类型信息失败"));
            }

        }
        private void UpdateParkCarTypeDefault(ParkCarType model)
        {
            if (model.BaseTypeID == BaseCarType.VIPCar)
            {

                model.MaxValue = 0;
                model.Amount = 0;
                model.MaxMonth = 0;
                model.MaxUseMoney = 0;
            }
            if (model.BaseTypeID == BaseCarType.StoredValueCar)
            {
                model.Amount = 0;
                model.MaxMonth = 0;
                model.OverdueToTemp = 0;
                model.LotOccupy = 0;
            }
            if (model.BaseTypeID == BaseCarType.MonthlyRent)
            {
                model.MaxValue = 0;
                model.MaxUseMoney = 0;
            }
            if (model.BaseTypeID == BaseCarType.TempCar)
            {
                model.MaxValue = 0;
                model.Amount = 0;
                model.MaxMonth = 0;
                model.OverdueToTemp = 0;
                model.LotOccupy = 0;
            }
            //if (model.BaseTypeID == BaseCarType.WorkCar)
            //{
            //    model.MaxValue = 0;
            //    model.Amount = 0;
            //    model.MaxMonth = 0;
            //    model.OverdueToTemp = 0;
            //    model.LotOccupy = 0;
            //    model.MaxUseMoney = 0;
            //}
            if (model.BaseTypeID == BaseCarType.SeasonRent)
            {
                model.MaxValue = 0;
                model.MaxUseMoney = 0;
            }
            if (model.BaseTypeID == BaseCarType.YearRent)
            {
                model.MaxValue = 0;
                model.MaxUseMoney = 0;
            }
            if (model.BaseTypeID == BaseCarType.CustomRent)
            {
                model.MaxValue = 0;
                model.MaxUseMoney = 0;
            }
            if (model.BaseTypeID != BaseCarType.MonthlyRent || model.OverdueToTemp != OverdueToTemp.ExpiredToTemp)
            {
                model.MonthCardExpiredEnterDay = 0;
            }
        }

        [HttpPost]
        [CheckPurview(Roles = "PK01020104")]
        public JsonResult Deletetype(string recordId)
        {
            try
            { 
                bool result =  ParkCarTypeServices.Delete(recordId);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "删除车类型信息失败");
                return Json(MyResult.Error("删除车类型信息失败"));
            }

        }
        public JsonResult GetCarTypeBaseCarType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(BaseCarType));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "车类型枚举转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetLpDistinguish()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(LpDistinguish));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "进出方式枚举转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetOverdueToTemp()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(OverdueToTemp));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "过去转临停枚举转换对象集合失败");
            }
            return json;
        }
        public JsonResult GetLotOccupy()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = EnumHelper.GetEnumContextList(typeof(LotOccupy));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "车位占用枚举转换对象集合失败");
            }
            return json;
        }
        [HttpPost]
        //[CheckPurview(Roles = "PK01020103")]
        public JsonResult SaveCarTypeSingle(ParkCarTypeSingle model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.CarTypeID)) throw new MyException("获取车类编号失败");
                if (string.IsNullOrWhiteSpace(model.SingleID)) throw new MyException("获取编号失败");

                bool result = ParkCarTypeSingleServices.Update(model);
                if (!result) throw new MyException("保存失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "保存单双车牌配置失败");
                return Json(MyResult.Error("保存单双车牌配置失败"));
            }
        }
        [HttpPost]
        //[CheckPurview(Roles = "PK01020103")]
        public JsonResult GetCarTypeSingle(string carTypeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(carTypeId)) throw new MyException("获取车类编号失败");
                List<ParkCarTypeSingle> models = ParkCarTypeSingleServices.QueryParkCarTypeByCarTypeID(carTypeId);
                if (models.Count == 0) {
                    ParkCarTypeSingleServices.AddDefault(carTypeId);
                }
                models = ParkCarTypeSingleServices.QueryParkCarTypeByCarTypeID(carTypeId);
                if (models.Count == 0) throw new MyException("获取单双车牌配置失败");

               return Json(MyResult.Success("获取成功", models));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取单双车牌配置失败");
                return Json(MyResult.Error("获取单双车牌配置失败"));
            }

        }
        /// <summary>
        /// 获取车辆类型操作权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParkCarTypeOperatePurview()
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
                            option.text = "添加";
                            option.handler = "Add";
                            option.sort = 1;
                            options.Add(option);
                            break;
                        }
                    case "PK01020102":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "修改";
                            option.handler = "Update";
                            option.sort = 2;
                            options.Add(option);
                            break;
                        }
                    case "PK01020104":
                        {
                            SystemOperatePurview option = new SystemOperatePurview();
                            option.text = "删除";
                            option.handler = "Deletetype";
                            option.sort = 3;
                            options.Add(option);
                            break;
                        }
                }
            }
            SystemOperatePurview option1 = new SystemOperatePurview();
            option1.text = "单双车牌配置";
            option1.iconCls = "icon-add";
            option1.handler = "AddCarTypeSingle";
            option1.sort = 3;
            options.Add(option1);

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
