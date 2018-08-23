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
    public class H5LockCarController : H5WeiXinController
    {
        public ActionResult Index()
        {
            List<WX_LockCar> cars = CarService.GetLockCarByAccountID(UserAccount.AccountID);
            return View(cars);
        }
        [HttpPost]
        public ActionResult LockOrUnLockCar(string parkingid, string platenumber, int status, string villageid)
        {
            try
            {
                platenumber = platenumber.ToPlateNo();
                bool canConnection = CarService.WXTestClientProxyConnectionByVID(villageid);
                if (!canConnection)
                {
                    throw new MyException("车场网络异常，暂无法解锁车，请稍后再试！");
                }

                if (status == 1)
                {
                    int result = CarService.WXUlockCarInfo(UserAccount.AccountID, parkingid, platenumber);
                    if (result == 0)
                    {
                        return Json(MyResult.Success("解锁成功"));
                    }
                    if (result == 1)
                    {
                        return Json(MyResult.Error("车场网络异常，暂无法解锁车，请稍后再试！"));
                    }
                    return Json(MyResult.Error("锁车失败！"));
                }
                else
                {
                    int result = CarService.WXLockCarInfo(UserAccount.AccountID, parkingid, platenumber);
                    if (result == 0)
                    {
                        return Json(MyResult.Success("锁车成功"));
                    }
                    if (result == 1)
                    {
                        return Json(MyResult.Error("车场网络异常，暂无法解锁车，请稍后再试！"));
                    }
                    return Json(MyResult.Error("锁车失败！"));
                }
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptionToDbAndTxt("H5LockCarError", "锁车或解锁失败", ex, LogFrom.WeiXin);
                return Json(MyResult.Error("操作失败"));
            }
        }
    }
}
