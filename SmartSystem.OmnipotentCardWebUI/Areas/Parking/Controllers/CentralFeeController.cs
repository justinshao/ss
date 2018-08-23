using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services.Park;
using Common.Entities.Parking;
using Common.Entities;
using Common.Services;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using System.Text;
using Common.CarInOutValidation;
using Common.Entities.Validation;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    /// <summary>
    /// 中央收费
    /// </summary>
    [CheckPurview(Roles = "PK010106")]
    public class CentralFeeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QueryWaitPayRecord(string parkingId, string plateNumber, int pageIndex)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parkingId)) throw new MyException("请选择车场");

                int pageSize = 16;
                int recordTotalCount;
                List<ParkIORecord> records = ParkIORecordServices.QueryPageNotExit(parkingId, plateNumber, pageSize, pageIndex, out recordTotalCount);
                var result = from p in records
                             select new
                             {
                                 RecordID = p.RecordID,
                                 ImageUrl = GetImagePath(p.EntranceImage),
                                 PlateNumber = p.PlateNumber,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 ParkingId = p.ParkingID
                             };
                int totalPage = (recordTotalCount + pageSize - 1) / pageSize;
                string msg = string.Format("{0},{1}", recordTotalCount, totalPage);
                return Json(MyResult.Success(msg, result));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "中央缴费获取待支付记录失败");
                return Json(MyResult.Error("取待支付记录失败"));
            }

        }
        private string GetImagePath(string imgUrl, bool isBig = true, bool needShowDefault = true)
        {
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                if (needShowDefault)
                {
                    return "/Content/images/iorecord_default_not_image_big.jpg";
                }
                else
                {
                    return string.Empty;
                }

            }
            if (isBig)
            {
                string mPath = Server.MapPath("~");
                string filePath = "/Pic/" + imgUrl;
                if (System.IO.File.Exists(mPath + filePath))
                {
                    return filePath;
                }
                if (needShowDefault)
                {
                    return "/Content/images/iorecord_default_not_image_big.jpg";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                string mPath = Server.MapPath("~");
                string filePath = "/Pic/" + imgUrl.Replace("Big", "Smile");
                if (System.IO.File.Exists(mPath + filePath))
                {
                    return filePath;
                }
                if (needShowDefault)
                {
                    return "/Content/images/default_not_image_small.png";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        [HttpPost]
        public string GetParkCarModelData()
        {
            JsonResult json = new JsonResult();
            if (string.IsNullOrEmpty(Request.Params["parkingId"])) return string.Empty;
            string parkingid = Request.Params["parkingId"].ToString();
            List<ParkCarModel> carModels = ParkCarModelServices.QueryByParkingId(parkingid);
            StringBuilder strTree = new StringBuilder();
            strTree.Append("[");
            int index = 1;
            foreach (var item in carModels)
            {
                strTree.Append("{\"id\":\"" + item.CarModelID + "\",");
                if (item.IsDefault == YesOrNo.Yes)
                {
                    strTree.Append("\"selected\":true,");
                }
                else
                {
                    strTree.Append("\"selected\":false,");
                }
                strTree.Append("\"text\":\"" + item.CarModelName + "\"");
                strTree.Append("}");
                if (index != carModels.Count)
                {
                    strTree.Append(",");
                }
                index++;
            }
            strTree.Append("]");
            json.Data = strTree.ToString();
            return strTree.ToString();
        }
        public ActionResult QueryWaitPayDetail(string recordId,string carModelId)
        {
            try
            {
                string errorMsg = string.Empty;
                ParkIORecord model = ParkIORecordServices.GetIORecord(recordId, out errorMsg);
                if (!string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取该入场记录失败");
                if (model == null) throw new MyException("找不到该入场记录");
                if (model.IsExit) throw new MyException("该车辆已经出场");

                DateTime outTime = DateTime.Now;
                List<ParkGate> parkGates = ParkGateServices.QueryByParkAreaRecordIds(new List<string>(){model.AreaID});
                ParkGate outGate = parkGates.FirstOrDefault(p=>p.IoState==IoState.GoOut);
                if(outGate==null) throw new MyException("获取出口通道失败");
                ResultAgs billResult = RateProcesser.GetRateResult(model, outGate, outTime, carModelId);
                if (!string.IsNullOrWhiteSpace(billResult.ValidFailMsg)) {
                    throw new MyException(billResult.ValidFailMsg);
                }
                if (billResult.Rate == null) throw new MyException("计算停车费失败");

                List<ParkIORecord> records = new List<ParkIORecord>() { model };
                var result = from p in records
                             select new
                             {
                                 RecordID = p.RecordID,
                                 EntranceImageUrl = GetImagePath(p.EntranceImage, true, false),
                                 EntrancePlateImageUrl = GetImagePath(p.EntranceImage, false, false),
                                 PlateNumber = p.PlateNumber,
                                 EntranceTime = p.EntranceTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 OutTime = outTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                 TotalDuration = p.EntranceTime.GetParkingDuration(outTime),
                                 TotalFee = billResult.Rate.Amount.ToString("0.0"),
                                 PaySuccess = billResult.Rate.OnlinePayAmount.ToString("0.0"),
                                 WaitPay = billResult.Rate.UnPayAmount.ToString("0.0"),
                                 DiscountAmount = billResult.Rate.DiscountAmount.ToString("0.0")
                             };

                return Json(MyResult.Success(string.Empty, result));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "中央缴费获取待支付记录失败");
                return Json(MyResult.Error("取待支付记录失败"));
            }
        }
        /// <summary>
        /// 收费放行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TollRelease(decimal waitPay, string recordId, string carModelId)
        {
            try
            {
                string errorMsg = string.Empty;
                ParkIORecord model = ParkIORecordServices.GetIORecord(recordId, out errorMsg);
                if (!string.IsNullOrWhiteSpace(errorMsg)) throw new MyException("获取该入场记录失败");
                if (model == null) throw new MyException("找不到该入场记录");
                if (model.IsExit) throw new MyException("该车辆已经出场");

                DateTime outTime = DateTime.Now;
                List<ParkGate> parkGates = ParkGateServices.QueryByParkAreaRecordIds(new List<string>() { model.AreaID });
                ParkGate outGate = parkGates.FirstOrDefault(p => p.IoState == IoState.GoOut);
                if (outGate == null) throw new MyException("获取出口通道失败");
                ResultAgs billResult = RateProcesser.GetRateResult(model, outGate, outTime, carModelId);
                if (!string.IsNullOrWhiteSpace(billResult.ValidFailMsg))
                {
                    throw new MyException(billResult.ValidFailMsg);
                }
                if (billResult.Rate == null) throw new MyException("计算停车费失败");
                if (waitPay != billResult.Rate.UnPayAmount) throw new MyException("缴费金额与实际金额不匹配，请重新选择入场记录");

                bool result = CentralFeeServices.Payment(recordId, model.ParkingID, billResult, GetLoginUser.RecordID);
                if (!result) throw new MyException("缴费失败");

                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "提交收费失败");
                return Json(MyResult.Error("缴费失败"));
            }
        }
    }
}
