using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;
using Common.Factory;
using Common.Utilities;
using Common.DataAccess;
using System.Net;

namespace Common.Services
{
    public class CompanyServices
    {
        public static bool Add(BaseCompany model) {
            if (model == null) throw new ArgumentNullException("model");

            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    model.CPID = GuidGenerator.GetGuid().ToString();
                    ICompany factory = CompanyFactory.GetFactory();
                    BaseCompany dbModel = factory.QueryCompanyByCompanyName(model.CPName);
                    if (dbModel != null) throw new MyException("系统已存在相同单位名称的单位");

                    bool result = factory.Add(model,dbOperator);

                    result = SysUserServices.AddCompanyDefaultUser(model, dbOperator);
                    if (!result) throw new MyException("添加默认用户失败");

                    dbOperator.CommitTransaction();
                    if (result) {
                        OperateLogServices.AddOperateLog<BaseCompany>(model, OperateType.Add);
                    }
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }


        public static bool Update(BaseCompany model)
        {
            if (model == null) throw new ArgumentNullException("model");

            ICompany factory = CompanyFactory.GetFactory();
            BaseCompany dbModel = factory.QueryCompanyByCompanyName(model.CPName);
            if (dbModel != null && dbModel.CPID!=model.CPID) throw new MyException("系统已存在相同单位名称的单位");

            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<BaseCompany>(model, OperateType.Update);
            }
            return result;
        }


        public static bool Delete(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ICompany factory = CompanyFactory.GetFactory();
            BaseCompany model = factory.QueryCompanyByRecordId(recordId);
            if (model == null) throw new MyException("待删除的单位不存在");

            List<BaseCompany> models = factory.QueryCompanysByMasterID(model.CPID);
            if (models.Count > 0) throw new MyException("请先删除该公司下的所有下级公司");

            List<BaseVillage> villages =  VillageServices.QueryVillageByCompanyId(recordId);
            if (villages.Count != 0) throw new MyException("请先删除该单位下的所有的小区");

            ISysUser factoryUser = SysUserFactory.GetFactory();
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    bool result = factory.Delete(recordId, dbOperator);
                    if (!result) throw new MyException("移除单位信息失败");

                    result = factoryUser.DeleteByCompanyId(recordId, dbOperator);
                    if (!result) throw new MyException("移除用户信息失败");

                    dbOperator.CommitTransaction();
                    if (result)
                    {
                        OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
                    }
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }

       
          
        }

        public static List<BaseCompany> QueryCompanyByRecordIds(List<string> recordIds) {
            if (recordIds == null || recordIds.Count == 0) throw new ArgumentNullException("recordIds");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryCompanyByRecordIds(recordIds);
        }

        public static List<BaseCompany> QueryCompanysByMasterID(string masterId)
        {
            if (string.IsNullOrWhiteSpace(masterId)) throw new ArgumentNullException("masterId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryCompanysByMasterID(masterId);
        }

        public static BaseCompany QueryCompanyByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryCompanyByRecordId(recordId);
        }
        /// <summary>
        /// 查询单位包含所有下级单位
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static List<BaseCompany> QueryCompanyAndSubordinateCompany(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryCompanyAndSubordinateCompany(recordId);
        }
        /// <summary>
        /// 根据单位名称查询上下级所有单位
        /// </summary>
        /// <param name="recordId">用户所在单位单位ID</param>
        /// <param name="str">单位名称</param>
        /// <returns></returns>
        public static List<BaseCompany> QueryAllCompanyByName(string recordId, string str)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");
            ICompany factory = CompanyFactory.GetFactory();
            List<BaseCompany> result1, result2;

            List<BaseCompany> models = factory.QueryAllCompanyByName(str); //根据名称模糊查询单位

            List<BaseCompany> querycompany = copylist(models);
            if (models.Count == 0)
                throw new MyException("未找到相关单位，请确保关键字正确");
            result1 = factory.QuerymasterCompanyBymodels(models); //查询所有父单位  
            result2 = factory.QuerySubordinateCompanyBymodels(querycompany); //查询所有子单位 
            List<BaseCompany> companys = result1.Union(result2).ToList<BaseCompany>(); //合并   
            companys = SameRemove(companys);//剔除重复项
            return companys;

        }
        public static List<BaseCompany> SameRemove(List<BaseCompany> result)
        {
            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = result.Count - 1; j > i; j--)
                {
                    if (result[i].CPID == result[j].CPID)
                    {
                        result.RemoveAt(j);
                    }
                }
            }
            return result;
        }
        public static List<BaseCompany> copylist(List<BaseCompany> b)
        {
            List<BaseCompany> a = new List<BaseCompany>();
            foreach (var item in b)
            {
                a.Add(item);
            }
            return a;
        }
        /// <summary>
        /// 查询顶级单位
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static BaseCompany QueryTopCompanyByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentNullException("recordId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryTopCompanyByRecordId(recordId);
        }
        public static BaseCompany QueryByParkingId(string parkingId) {
            if (string.IsNullOrWhiteSpace(parkingId)) throw new ArgumentNullException("parkingId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryByParkingId(parkingId);
        }
        public static BaseCompany QueryByBoxID(string boxId)
        {
            if (string.IsNullOrWhiteSpace(boxId)) throw new ArgumentNullException("boxId");

            ICompany factory = CompanyFactory.GetFactory();
            return factory.QueryByBoxID(boxId);
        }
        /// <summary>
        /// 系统初始化默认单位
        /// </summary>
        public static void InitSystemDefaultCompany()
        {
            try
            {
                ICompany factory = CompanyFactory.GetFactory();
                bool hasCompany = factory.SystemExistCompany();
                if (!hasCompany)
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                    {
                        try
                        {
                            dbOperator.BeginTransaction();
                            BaseCompany compamy = GetCompanyModel();
                            bool result = factory.Add(compamy, dbOperator);
                            if (!result) throw new MyException("添加默认单位失败");

                            result = SysUserServices.AddCompanyDefaultUser(compamy, dbOperator);

                            if (!result) throw new MyException("添加默认用户失败");

                            dbOperator.CommitTransaction();
                        }
                        catch
                        {
                            dbOperator.RollbackTransaction();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "添加系统默认单用户失败");
            }
        }

        /// <summary>
        /// 是否需要初始化数据
        /// </summary>
        /// <returns></returns>
        public static bool HasInit()
        {
            ICompany factory = CompanyFactory.GetFactory();
            return factory.SystemExistCompany();
        }

        /// <summary>
        /// 系统初始化默认单位CS
        /// </summary>
        public static bool InitSystemDefaultCompanyCS(string VName,string CPName,string userno,string pwd,string systemmodelpath)
        {
            try
            {
                ICompany factory = CompanyFactory.GetFactory();
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    try
                    {
                        dbOperator.BeginTransaction();
                        BaseCompany compamy = GetCompanyModel();
                        compamy.CPName = CPName;
                        compamy.UserAccount = userno;
                        compamy.UserPassword = pwd;
                        bool result = factory.Add(compamy, dbOperator);
                        if (!result) throw new MyException("添加默认单位失败");
                        BaseVillage village = GetVillage(compamy.CPID);
                        village.VName = VName;
                        result = VillageServices.AddVillageDefaultUser(village, dbOperator);
                        if (!result) throw new MyException("添加默认小区失败");
                        result = SysUserServices.AddCompanyDefaultUserCS(compamy, village, dbOperator, systemmodelpath);
                        if (!result) throw new MyException("添加默认用户失败");

                        BaseParkinfo parkinfo = GetParkinfo(village.VID);
                        result = ParkingServices.AddParkinfoDefault(parkinfo, dbOperator);
                        if (!result) throw new MyException("添加默认车场失败");
                        dbOperator.CommitTransaction();

                        ParkArea area = GetParkArea(parkinfo.PKID);
                        result = ParkAreaServices.AddDefualt(area);
                        if (result)
                        {
                            ParkBox box = GetParkBox(area.AreaID);
                            result = ParkBoxServices.AddDefault(box);
                            if (result)
                            {

                                result = ParkGateServices.AddDefault(GetParkGate(box.BoxID, 1));
                                result = ParkGateServices.AddDefault(GetParkGate(box.BoxID, 2));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbOperator.RollbackTransaction();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "添加系统默认单用户失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加通道信息
        /// </summary>
        /// <param name="BoxID"></param>
        /// <param name="inout"></param>
        public static ParkGate GetParkGate(string BoxID, int inout)
        {
            ParkGate model = new ParkGate();
            model.BoxID = BoxID;
            model.GateNo = inout.ToString();
            if (inout == 1)
            {
                model.GateName = "进通道";
                model.IoState =  IoState.GoIn;
            }
            else
            {
                model.GateName = "出通道";
                model.IoState = IoState.GoOut;
            }
            model.DataStatus = DataStatus.Normal;
            model.GateID = GuidGenerator.GetGuidString();
            model.HaveUpdate = 3;
            model.IsEnterConfirm = YesOrNo.Yes;
            model.IsTempInOut = YesOrNo.Yes;
            model.LastUpdateTime = DateTime.Now;
            model.OpenPlateBlurryMatch = YesOrNo.No;
            model.IsNeedCapturePaper = false;
            return model;
        }

        /// <summary>
        /// 增加岗亭信息
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        public static ParkBox GetParkBox(string AreaID)
        {
            ParkBox model = new ParkBox();
            model.AreaID = AreaID;
            model.BoxID = GuidGenerator.GetGuidString();
            model.BoxName = "收费岗亭";
            model.BoxNo = "0001";
            model.DataStatus = DataStatus.Normal;
            model.HaveUpdate = 3;
            model.IsCenterPayment = YesOrNo.No;
            model.LastUpdateTime = DateTime.Now;
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            if (localhost.AddressList.Length > 0)
            {
                model.ComputerIP = localhost.AddressList[0].ToString();
            }
            else
            {
                model.ComputerIP = "127.0.0.1";
            }
            return model;

        }

        /// <summary>
        /// 增加停车区域
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        public static ParkArea GetParkArea(string PKID)
        {
            ParkArea model = new ParkArea();
            model.AreaID = GuidGenerator.GetGuidString();
            model.AreaName = "停车区域";
            model.CameraWaitTime = 1;
            model.CarbitNum = 1000;
            model.DataStatus = DataStatus.Normal;
            model.HaveUpdate = 3;
            model.LastUpdateTime = DateTime.Now;
            model.MasterID = String.Empty;
            model.NeedToll = YesOrNo.Yes;
            model.PKID = PKID;
            model.TwoCameraWait = YesOrNo.Yes;
            return model;
        }

        public static List<BaseCompany> GetCurrLoginUserRoleCompany(string companyId, string userId)
        {
            List<BaseVillage> villages = VillageServices.QueryVillageByUserId(userId);
            List<BaseCompany> validCompanys = new List<BaseCompany>();
            List<BaseCompany> allComapnys = CompanyServices.QueryCompanyAndSubordinateCompany(companyId);
            foreach (var item in villages)
            {
                CompanyServices.GetCurrLoginUserRoleCompany(validCompanys, allComapnys, item.CPID);
            }
            return validCompanys;
        }

        private static void GetCurrLoginUserRoleCompany(List<BaseCompany> companys, List<BaseCompany> allComapnys, string companyId)
        {
            BaseCompany company = allComapnys.FirstOrDefault(p => p.CPID == companyId);
            if (company != null)
            {
                if (companys.Count(p => p.CPID == company.CPID) == 0)
                {
                    companys.Add(company);
                }
                GetCurrLoginUserRoleCompany(companys, allComapnys, company.MasterID);
            }
        }

        private static BaseCompany GetCompanyModel()
        {
            BaseCompany model = new BaseCompany();
            model.CPID = GuidGenerator.GetGuidString();
            model.CPName = "系统默认单位";
            model.UserAccount = "admin";
            model.UserPassword = "888888";
            return model;
        }

        public static BaseVillage GetVillage(string CPID)
        {
            BaseVillage model = new BaseVillage();
            model.VID = GuidGenerator.GetGuidString();
            model.ProxyNo = model.VID;
            model.CPID = CPID;
            model.VName = "默认小区";
            model.VNo = "0001";
            return model;
        }

        public static BaseParkinfo GetParkinfo(string VID)
        {
            BaseParkinfo model = new BaseParkinfo();
            model.CenterTime = 15;
            model.DataSaveDays = 365;
            model.ExpiredAdvanceRemindDay = 7;
            model.IsOnLineGathe = 0;
            model.IsLine = YesOrNo.No;
            model.NeedFee = YesOrNo.Yes;
            model.PictureSaveDays = 90;
            model.PKID = GuidGenerator.GetGuidString();
            model.PKName = "停车场";
            model.PKNo="0001";
            model.VID = VID;
            model.PoliceFree = true;
            return model;
        }
        public static string GetCompanyId(string parkingId, string boxId)
        {
            if (!string.IsNullOrWhiteSpace(parkingId))
            {
                BaseCompany company = CompanyServices.QueryByParkingId(parkingId);
                if (company != null)
                {
                    return company.CPID;
                }
            }
            if (!string.IsNullOrWhiteSpace(boxId))
            {
                BaseCompany company = CompanyServices.QueryByBoxID(boxId);
                if (company != null)
                {
                    return company.CPID;
                }
            }
            return string.Empty;
        }
    }
}
