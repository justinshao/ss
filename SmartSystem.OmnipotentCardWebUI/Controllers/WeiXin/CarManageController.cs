using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Base.Common;
using SmartSystem.OmnipotentCardWebUI.Models;
using SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin;
using SmartSystem.WeiXinInerface;
using Common.Entities.WX;
using Common.Entities;
using Common.Services;
using Common.Core;
using ClassLibrary1;
using Newtonsoft.Json;
using Common.Utilities.Helpers;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    [PageBrowseRecord]
    [CheckWeiXinPurview(Roles = "Login")]
    public class CarManageController : WeiXinController
    {
        public ActionResult Index()
        {
            try
            {
                string auth = AppUserToken;
                LogerHelper.Loger.Info(AppUserToken);
                if (auth.IsEmpty())
                {
                    //没有登录
                    //
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });

                }
                else if (auth == "-1")
                {
                    return RedirectToAction("Register", "ParkingPayment");
                }

                //string auth = "D8F586C77EB73316CFB5FC8C035EF3AFD4BF2EB5DF8F150448EDB019BFE4E5814E401DEBF5C242031E53AD4858A3C1DFDF2537FD4F193CC86928F771770CF6CA";



                Car car = Carapi.bb(auth);
                    Car mycar = JsonConvert.DeserializeObject<Car>(JsonHelper.GetJsonString(car));
                 
                    return View(mycar);
              
            }
            catch (Exception e)
            {
                return View();

            }
        }
        public ActionResult AddCar()
        {
            return View();
        }
      
      
        [HttpPost]
        public ActionResult DeleteMyLicensePlate(string cid,string plateNo = "")
        {
            try
            {
                string auth = AppUserToken;

                if (auth.IsEmpty())
                {
                    //没有登录
                    //
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });

                }
                else if (auth == "-1")
                {
                    return RedirectToAction("Register", "ParkingPayment");
                }
                //string auth = "D8F586C77EB73316CFB5FC8C035EF3AFD4BF2EB5DF8F150448EDB019BFE4E5814E401DEBF5C242031E53AD4858A3C1DFDF2537FD4F193CC86928F771770CF6CA";
                string carid = cid;
                Unbundlingcar uncar = Carapi.cc(carid, auth);
                Unbundlingcar jiebangcar = JsonConvert.DeserializeObject<Unbundlingcar>(JsonHelper.GetJsonString(uncar));
                if (uncar.Result == "解绑车辆成功")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "解绑车辆成功" };
                    res.Data = Phone;//返回单个对象； 

                    //
                    if (!plateNo.IsEmpty())
                    {
                        //先查询
                        WX_CarInfo car = CarService.GetCarInfoByPlateNo(WeiXinUser.AccountID, plateNo);
                        if (car != null)
                        {
                            bool result = CarService.DelWX_CarInfo(car.RecordID);
                            if (!result)
                            {
                                TxtLogServices.WriteTxtLogEx("WXBindCarError", "用户删除车牌失败:{2},{1}，{0}", result, car.RecordID,plateNo);
                            }
                        }
                    }
                    //
                    return res;

                }
                else if (uncar.Result == "该账号已在其他设备登录,请重新登录")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "该账号已在其他设备登录,请重新登录" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                }
                else if (uncar.Result == "该车辆有未出场记录,无法解绑")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "该车辆有未出场记录,无法解绑" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                }
                else {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "解绑车辆失败" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                }
                
            }
            catch (Exception e)
            {
                return Json(MyResult.Error("解绑失败"));
            }
        }

        [HttpPost]
        public ActionResult AddMyLicensePlate(string licenseplate)
        {
            try
            {
                string auth = AppUserToken;

                if (auth.IsEmpty())
                {
                    //没有登录
                    //
                    return RedirectToAction("Index", "ErrorPrompt", new { message = "用户登录失败" });

                }
                else if (auth == "-1")
                {
                    return RedirectToAction("Register", "ParkingPayment");
                }

                //string auth = "D8F586C77EB73316CFB5FC8C035EF3AFD4BF2EB5DF8F150448EDB019BFE4E5814E401DEBF5C242031E53AD4858A3C1DFDF2537FD4F193CC86928F771770CF6CA";

                string plate = licenseplate;
                AddCar addcar = Carapi.dd(plate, auth);
                AddCar addmycar = JsonConvert.DeserializeObject<AddCar>(JsonHelper.GetJsonString(addcar));
                if (addcar.Result == "该车辆已被其它帐户绑定,是否找回?")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "该车辆已被其它帐户绑定" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                   
                }
                else if (addcar.Result == "添加车辆成功")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "添加车辆成功" };
                    res.Data = Phone;//返回单个对象； 

                    WX_CarInfo model = new WX_CarInfo();
                    model.AccountID = WeiXinUser.AccountID;
                    model.PlateNo = licenseplate.ToPlateNo();
                    model.Status = 1;
                    //
                    int result = CarService.AddWX_CarInfo(model);
                    if (result != 1)
                    {
                        TxtLogServices.WriteTxtLogEx("WXBindCarError", "用户添加车牌失败:{0}", result);
                    }
                    //
                    return res;
                   
                }
                else if (addcar.Result == "该账号已在其他设备登录,请重新登录")
                {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "该账号已在其他设备登录,请重新登录" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                }
                else {
                    JsonResult res = new JsonResult();
                    var Phone = new { Status = "添加失败" };
                    res.Data = Phone;//返回单个对象； 
                    return res;
                }
            }
            catch (Exception e)
            {
                return Json(MyResult.Error("添加失败"));
            }
        }
    }
}
