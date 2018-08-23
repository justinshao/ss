using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Park;
using Common.Services;
using Common.Entities;

namespace SmartSystem.OmnipotentCardWebUI.Controllers.WeiXin
{
    public class ParkCommonDataController : WeiXinController
    {
        #region 基础数据
        /// <summary>
        /// 获得所有车场
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSellers()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkSellerServices.QueryByVillageIds(GetLoginUserVillages.Select(p => p.VID).ToList());
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得所有车场
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParks()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkingServices.QueryParkingByVillageIds(base.GetLoginUserVillages.Select(u => u.VID).ToList());
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 获得所有车场
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllParks()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkingServices.QueryAllParking();
            }
            catch (Exception ex)
            { }
            return json;
        }
        /// <summary>
        /// 车辆进场类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCardEntranceType()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 进通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEntranceGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingIdAndIoState(parkingid, IoState.GoIn);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得车场所有通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 出通道
        /// </summary>
        /// <returns></returns>
        public JsonResult GetExitGates()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkGateServices.QueryByParkingIdAndIoState(parkingid, IoState.GoOut);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得车形
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCarTypes()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkCarModelServices.QueryByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAreas()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkAreaServices.GetParkAreaByParkingId(parkingid);
            }
            catch { }
            return json;
        }
        /// <summary>
        /// 获得岗亭
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBoxes()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkBoxServices.QueryByParkingID(parkingid);
            }
            catch
            { }
            return json;
        }
        /// <summary>
        /// 当班人
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnDutys()
        {
            string parkingid = Request.Params["parkingid"];
            JsonResult json = new JsonResult();
            try
            {
                json.Data = SysUserServices.QuerySysUserByParkingId(parkingid);
            }
            catch
            { }
            return json;
        }
        /// <summary>
        /// 获取事件类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEventType()
        {
            JsonResult json = new JsonResult();
            try
            {
                json.Data = ParkEventServices.GetEventType();
            }
            catch
            { }
            return json;
        }
        #endregion

    }
}
