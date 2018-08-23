using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSystem.OmnipotentCardWebUI.Models;
using Common.Entities.WX;
using SmartSystem.WeiXinInerface;
using Common.Entities;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.Html5
{
    [H5LoginPurview(Roles = "Login")]
    public class H5CarManageController : H5WeiXinController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMyLicenseplate()
        {
            try
            {
                List<WX_CarInfo> plates = CarService.GetCarInfoByAccountID(UserAccount.AccountID);
                return Json(MyResult.Success("获取成功", plates));

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CarManageError", "根据账号编号获取车牌信息失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("获取车辆信息失败"));
            }
        }
        [HttpPost]
        public ActionResult DeleteMyLicensePlate(string id)
        {
            try
            {
                bool result = CarService.DelWX_CarInfo(id);
                if (!result) throw new MyException("删除失败");
                return Json(MyResult.Success("删除成功"));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CarManageError", "删除车牌失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("删除失败"));
            }
        }

        [HttpPost]
        public ActionResult AddMyCar(string licenseplate)
        {
            try
            {
                WX_CarInfo model = new WX_CarInfo();
                model.AccountID = UserAccount.AccountID;
                model.PlateNo = licenseplate.ToPlateNo();
                model.Status = 2;
                int result = CarService.AddWX_CarInfo(model);
                if (result == 1)
                {
                    return Json(MyResult.Success("添加成功"));
                }
                if (result == 0)
                {
                    return Json(MyResult.Error("车牌号重复"));
                }
                return Json(MyResult.Error("添加失败"));

            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5CarManageError", "添加车牌信息失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("添加失败"));
            }
        }
    }
}
