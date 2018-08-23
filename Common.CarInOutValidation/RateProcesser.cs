using Common.Entities;
using Common.Entities.Parking;
using Common.Entities.Validation;
using Common.Services;
using Common.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common.Services.Park;
using Common.Services.BaseData;

namespace Common.CarInOutValidation
{
    public class RateProcesser
    {
        /// <summary>
        /// 白天黑夜最大收费参数设置
        /// </summary>
        //public static DayAndNightMaxMoneyPara DayAndNightMaxMoneyPara { get; set; }

        //private static XDocument document;
        //public static XDocument _XDocument
        //{
        //    get 
        //    {
        //        return document;
        //    }
        //    private set
        //    {
        //        document = value;
        //    }
        //}

        //static RateProcesser()
        //{
        //    try
        //    {
        //        string xmlName = "TimeShareFeeRule.xml";
        //        var dirName = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, xmlName);

        //        XDocument xdoc = XDocument.Load(dirName);
        //        foreach (var item in xdoc.Root.Elements())
        //        {
        //            if (item.Attribute("IsEnable").Value.ToBoolean())
        //            {
        //                _XDocument = xdoc;
        //            }
        //            break;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogerHelper.Loger.Error(ex);
        //    }
        //}

        //public static void  LoadCustomFeerule(string ruletext)
        //{

        //    try
        //    {
        //        using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ruletext)))
        //        {
        //            XDocument xdoc = XDocument.Load(stream);
        //            foreach (var item in xdoc.Root.Elements())
        //            {
        //                if (item.Attribute("IsEnable").Value.ToBoolean())
        //                {
        //                    _XDocument = xdoc;
        //                }
        //                break;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogerHelper.Loger.Error(ex);
        //    }
        //}
        /// <summary>
        /// 根据入场记录算费
        /// </summary>
        /// <param name="ioRecord"></param>
        /// <returns></returns>
        public static ResultAgs GetRateResult(ParkIORecord ioRecord, ParkGate outGate, DateTime exitTime, string carModeid = "")
        {
            if (outGate == null)
            {
                return null;
            }
            string errorMsg = "";
            InputAgs args = new InputAgs();
            args.AreadInfo = ParkAreaServices.QueryByRecordId(ioRecord.AreaID);
            if (args.AreadInfo.IsNestArea && args.AreadInfo.Parent == null)//内部车场时 且上级区域为空时
            {
                args.AreadInfo.Parent = ParkAreaServices.QueryByRecordId(args.AreadInfo.MasterID);
                if (args.AreadInfo.Parent == null)//还是找不到上级区域时
                {
                    LogerHelper.Loger.Error(string.Format("找不到车场[{0}]的上级车场，上级车场ID为[{1}].", args.AreadInfo.AreaName, args.AreadInfo.MasterID));
                    args.AreadInfo.MasterID = "";//找不到就至为空
                }
            }
            args.AreadInfo.Parkinfo = ParkingServices.QueryParkingByParkingID(args.AreadInfo.PKID);
            if (args.AreadInfo.Parkinfo == null)//还是找不到上级区域时
            {
                LogerHelper.Loger.Error(string.Format("找不到区域[{0}]对应的车场.", args.AreadInfo.AreaName));
                return null;
            }
            args.Plateinfo = new PlateInfo();
            args.Plateinfo.LicenseNum = ioRecord.PlateNumber;
            args.Plateinfo.TriggerTime = exitTime;
            args.CarTypeInfo = ParkCarTypeServices.QueryParkCarTypeByRecordId(ioRecord.CarTypeID);
            string str = errorMsg;
            EmployeePlate platenumber = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(args.AreadInfo.Parkinfo.VID, args.Plateinfo.LicenseNum, out errorMsg);
            if (platenumber != null)
            {
                //根据车牌获取车辆用户信息
                var pkcard = ParkGrantServices.GetParkGrantByPlateNumberID(args.AreadInfo.PKID, platenumber.PlateID, out errorMsg);
                if (pkcard.Count() > 0)
                {
                    //判断是否有区域 通道限制
                    var cardinfo = pkcard.First();
                    args.CardInfo = cardinfo;
                    args.CardInfo.OwnerPlateNumber = platenumber;
                    var usercard = BaseCardServices.GetBaseCard(args.CardInfo.CardID, out errorMsg);
                    if (usercard != null)
                    {
                        args.CardInfo.Usercard = usercard;
                    }
                    else
                    {
                        args.CardInfo = null;
                    }
                    if (!args.CardInfo.CarModelID.IsEmpty())
                    {
                        carModeid = args.CardInfo.CarModelID;
                       
                    }
                }
            }
           
            if (args.CarTypeInfo == null)
            {
                return null;
            }
            if (carModeid.IsEmpty())
            {
                args.CarModel = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
            }
            else
            {
                args.CarModel = ParkCarModelServices.QueryByRecordId(carModeid);
                if (args.CarModel==null)
                {
                    args.CarModel = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
                }
            }
            if (args.CarModel == null)
            {
                return null;
            }
            //获取默认车类型
            args.GateInfo = outGate;
            args.IORecord = ioRecord;

            ResultAgs rst = new ResultAgs();
            rst.InOutBaseCardType = BaseCarType.TempCar;
            rst.ResCode = ResultCode.OutOK;
            var rateContext = new RateTemplate();
            rateContext.Process(args, rst);
            return rst;
        }
        /// <summary>
        /// 模拟参数算费
        /// </summary>
        /// <param name="platenumber"></param>
        /// <param name="areaid"></param>
        /// <param name="intime"></param>
        /// <param name="exitTime"></param>
        /// <param name="cartypeID"></param>
        /// <param name="carModeid"></param>
        /// <returns></returns>
        public static ResultAgs GetRateResult(string platenumber, string areaid,DateTime intime, DateTime exitTime,string cartypeID, string carModeid = "")
        {
            if (areaid.IsEmpty())
            {
                return null;
            }

            string errorMsg = "";
            InputAgs args = new InputAgs();
            args.AreadInfo = ParkAreaServices.QueryByRecordId(areaid);

            args.IORecord = new ParkIORecord();
            args.IORecord.EntranceTime = intime;
            args.IORecord.ExitTime = exitTime;
            args.IORecord.PlateNumber = platenumber;
            args.IORecord.CarTypeID = cartypeID;
            args.IORecord.RecordID = "";
            if (args.AreadInfo.IsNestArea && args.AreadInfo.Parent == null)//内部车场时 且上级区域为空时
            {
                args.AreadInfo.Parent = ParkAreaServices.QueryByRecordId(args.AreadInfo.MasterID);
                if (args.AreadInfo.Parent == null)//还是找不到上级区域时
                {
                    LogerHelper.Loger.Error(string.Format("找不到车场[{0}]的上级车场，上级车场ID为[{1}].", args.AreadInfo.AreaName, args.AreadInfo.MasterID));
                    args.AreadInfo.MasterID = "";//找不到就至为空
                }
            }
            args.AreadInfo.Parkinfo = ParkingServices.QueryParkingByParkingID(args.AreadInfo.PKID);
            if (args.AreadInfo.Parkinfo == null)//还是找不到上级区域时
            {
                LogerHelper.Loger.Error(string.Format("找不到区域[{0}]对应的车场.", args.AreadInfo.AreaName));
                return null;
            }
            args.Plateinfo = new PlateInfo();
            args.Plateinfo.LicenseNum = platenumber;
            args.Plateinfo.TriggerTime = exitTime;
            args.CarTypeInfo = ParkCarTypeServices.QueryParkCarTypeByRecordId(cartypeID);

            if (args.CarTypeInfo == null)
            {
                return null;
            }
            if (carModeid.IsEmpty())
            {
                args.CarModel = ParkCarModelServices.GetDefaultCarModel(args.AreadInfo.PKID, out errorMsg);
            }
            else
            {
                args.CarModel = ParkCarModelServices.QueryByRecordId(carModeid);
            }
            if (args.CarModel == null)
            {
                return null;
            }
            //获取默认车类型
            args.GateInfo = new ParkGate();
            args.GateInfo.IoState = IoState.GoOut; 
            ResultAgs rst = new ResultAgs();
            rst.InOutBaseCardType = BaseCarType.TempCar;
            rst.ResCode = ResultCode.OutOK;
            var rateContext = new RateTemplate();
            rateContext.Process(args, rst);
            return rst;
        }
    }
}
