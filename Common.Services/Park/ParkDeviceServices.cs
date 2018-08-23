using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Utilities;
using Common.Factory;
using Common.IRepository;
using Common.Entities;
using Common.IRepository.Park;
using Common.DataAccess;

namespace Common.Services
{
    public class ParkDeviceServices
    {
        public static bool Add(ParkDevice model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkDeviceDetection factoryDetection = ParkDeviceDetectionFactory.GetFactory();
            model.DeviceID = GuidGenerator.GetGuid().ToString();
            IParkDevice factory = ParkDeviceFactory.GetFactory();
            bool result = false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();

                    result = factory.Add(model, dbOperator);
                    if (!result) throw new MyException("添加设备失败");
                    if (model.DeviceType == DeviceType.NZ_CONTROL)
                    {
                        int devId = 0;
                        if (!int.TryParse(model.DeviceNo, out devId)) {
                            throw new MyException("设备编号只能输入纯数字");
                        }
                        AddDefaultParkDeviceParam(model.GateID, model.DeviceID, model.DeviceNo, dbOperator);
                    }
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDevice>(model, OperateType.Add);
            }
            return result;
        }
        private static bool AddDefaultParkDeviceParam(string gateId, string deviceId, string deviceNo, DbOperator dbOperator)
        {
            ParkGate gate = ParkGateServices.QueryByRecordId(gateId);
            ParkBox box = ParkBoxServices.QueryByRecordId(gate.BoxID);
            ParkArea area = ParkAreaServices.QueryByRecordId(box.AreaID);
            ParkDeviceParam param = CreateDefualtParam(deviceId, int.Parse(deviceNo), area.CarbitNum, (int)gate.IoState);
            IParkDevice factory = ParkDeviceFactory.GetFactory();
            ParkDeviceParam deviceParam = factory.QueryParkDeviceParamByDevID(param.DevID);
            if (deviceParam != null) throw new MyException("设备编号不能重复");

            bool result = factory.AddParam(param, dbOperator);
            if (!result) throw new MyException("添加设备默认参数失败");
            return result;
        }
        private static ParkDeviceParam CreateDefualtParam(string DeviceID,int DeviceNo,int CarbitNum,int Iostate)
        {
            ParkDeviceParam model = new ParkDeviceParam();
            model.RecordID = GuidGenerator.GetGuid().ToString();
            model.VipMode = 1;
            model.TempMode = 1;
            model.NetOffMode = 1;
            model.VipDevMultIn = 1;
            model.PloicFree = 1;
            model.VipDutyDay = 7;
            model.OverDutyYorN =1;
            model.OverDutyDay = 0;
            model.SysID = 1;
            model.DevID = DeviceNo;
            model.SysInDev = 1;
            model.SysOutDev = 1;
            model.SysParkNumber = CarbitNum;
            model.SwipeInterval = 10;
            model.UnKonwCardType = 1;
            model.LEDNumber = 4;
            model.DevInorOut = Iostate - 1;
            model.DeviceID = DeviceID;
            return model;
        }

        private static ParkDeviceDetection GetParkDeviceDetectionModel(ParkDevice device)
        {
            ParkDeviceDetection model = new ParkDeviceDetection();
            model.RecordID = GuidGenerator.GetGuid().ToString();
            model.DeviceID = device.DeviceID;
            model.ConnectionState = 0;
            IParkGate factory = ParkGateFactory.GetFactory();
            model.PKID = factory.QueryParkingIdByGateId(device.GateID);
            if (string.IsNullOrWhiteSpace(model.PKID))
                throw new MyException("获取车场编号失败");

            return model;
        }
        public static bool Update(ParkDevice model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IParkDevice factory = ParkDeviceFactory.GetFactory();
            IParkDeviceDetection factoryDetection = ParkDeviceDetectionFactory.GetFactory();
            bool result = false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();

                    result = factory.Update(model, dbOperator);
                    if (!result) throw new MyException("修改设备失败");

                    if (model.DeviceType == DeviceType.NZ_CONTROL)
                    {
                       ParkDeviceParam deviceParam = factory.QueryParkDeviceParamByDID(model.DeviceID);
                       if (deviceParam == null)
                       {
                           AddDefaultParkDeviceParam(model.GateID, model.DeviceID, model.DeviceNo, dbOperator);
                       }
                       else
                       {
                           factory.UpdateParam(model.DeviceID, model.DeviceNo, dbOperator);
                       }
                    }

                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDevice>(model, OperateType.Update);
            }
            return result;
        }

       
        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkDevice factory = ParkDeviceFactory.GetFactory();
            IParkDeviceDetection factoryDetection = ParkDeviceDetectionFactory.GetFactory();
            bool result = false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();

                    result = factory.Delete(recordId, dbOperator);
                    if (!result) throw new MyException("删除设备失败");

                    factoryDetection.Delete(recordId,dbOperator);

                    factory.DeleParam(recordId, dbOperator);
                    
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }

        public static List<ParkDevice> QueryParkDeviceByGateRecordId(string gateRecordId)
        {
            if (string.IsNullOrWhiteSpace(gateRecordId)) throw new ArgumentNullException("gateRecordId");

            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceByGateRecordId(gateRecordId);
        }

        public static ParkDevice QueryParkDeviceByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceByRecordId(recordId);
        }

        public static List<ParkDevice> QueryParkDeviceByIPAddress(string ipaddress)
        {
            if (string.IsNullOrWhiteSpace(ipaddress)) throw new ArgumentNullException("ipaddress");

            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceByIPAddress(ipaddress);
        }

        public static List<ParkDevice> QueryParkDeviceDetectionByParkingID(string parkingid)
        {
            if (string.IsNullOrWhiteSpace(parkingid)) throw new ArgumentNullException("parkingid");
            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceDetectionByParkingID(parkingid);
        }

        public static List<ParkDevice> QueryParkDeviceAll()
        {
            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceAll();
        }

        public static ParkDeviceParam QueryParkDeviceParamByDID(string DeviceID)
        {
            IParkDevice factory = ParkDeviceFactory.GetFactory();
            return factory.QueryParkDeviceParamByDID(DeviceID);
        }

        public static bool AddParam(ParkDeviceParam model)
        {
            if (model == null) throw new ArgumentNullException("model");
            model.RecordID = GuidGenerator.GetGuid().ToString();
            IParkDevice factory = ParkDeviceFactory.GetFactory();

            ParkDeviceParam deviceParam = factory.QueryParkDeviceParamByDevID(model.DevID);
            if (deviceParam != null) throw new MyException("设备编号不能重复");

            bool result = false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();

                    result = factory.AddParam(model, dbOperator);
                    if (!result) throw new MyException("添加设备参数失败");
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDeviceParam>(model, OperateType.Add);
            }
            return result;
        }

        public static bool UpdateParam(ParkDeviceParam model)
        {
             if (model == null) throw new ArgumentNullException("model");
            IParkDevice factory = ParkDeviceFactory.GetFactory();

            ParkDeviceParam deviceParam = factory.QueryParkDeviceParamByDevID(model.DevID);
            if (deviceParam != null && deviceParam.DeviceID!=model.DeviceID) throw new MyException("设备编号不能重复");

            bool result = false;
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();

                    result = factory.UpdateParam(model, dbOperator);
                    if (!result) throw new MyException("修改设备参数失败");
                    dbOperator.CommitTransaction();
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDeviceParam>(model, OperateType.Add);
            }
            return result;
        }
    }
}
