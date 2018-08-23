using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WXAccountDAL:IWXAccount
    {
        /// <summary>
        /// 获取微信帐户
        /// </summary>
        /// <param name="accountname">帐户名称</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<WX_Account> Search_WXAccount(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime,int PageSize,int PageIndex)
        {
            List<WX_Account> wxaccountlist = new List<WX_Account>();
            string strSql = string.Format(@"select top {0} temp.* from (select  top {1} * from wx_account where status!=1", PageSize, PageSize * PageIndex);
            if (!string.IsNullOrEmpty(accountname))
            {
                strSql += " and accountname like @accountname";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                strSql += " and mobilephone like @mobile";
            }
            if (starttime != null)
            {
                strSql += " and regtime>=@starttime";
            }
            if (endtime != null)
            {
                strSql += " and regtime<=@endtime";
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql += " and companyid = @companyid";
            }
            strSql += " order by regtime asc) temp order by temp.regtime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("accountname", "%" + accountname + "%");
                dboperator.AddParameter("mobile", "%" + mobile + "%");
                if (starttime != null)
                {
                    dboperator.AddParameter("starttime", starttime.Value);
                }
                if (endtime != null)
                {
                    dboperator.AddParameter("endtime", endtime.Value);
                }
                if (!string.IsNullOrEmpty(companyId))
                {
                    dboperator.AddParameter("companyid", companyId);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        wxaccountlist.Add(DataReaderToModel<WX_Account>.ToModel(dr));
                    }
                }
            }
            return wxaccountlist;
        }
        /// <summary>
        /// 获取微信帐户数量
        /// </summary>
        /// <param name="accountname">帐户名称</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int Search_WXAccount_Count(string companyId, string accountname, string mobile, DateTime? starttime, DateTime? endtime)
        {
            int _total = 0;
            string strSql = "select count(1) Count from wx_account where status!=1";
            if (!string.IsNullOrEmpty(accountname))
            {
                strSql += " and accountname like @accountname";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                strSql += " and mobilephone like @mobile";
            }
            if (starttime != null)
            {
                strSql += " and regtime>=@starttime";
            }
            if (endtime != null)
            {
                strSql += " and regtime<=@endtime";
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql += " and companyid = @companyid";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("accountname", "%" + accountname + "%");
                dboperator.AddParameter("mobile", "%" + mobile + "%");
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                if (!string.IsNullOrEmpty(companyId))
                {
                    dboperator.AddParameter("companyid", companyId);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _total = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _total;
        }

        public bool AddWXAccount(WX_Info model, DbOperator dbOperator)
        { 
            model.AccountID = System.Guid.NewGuid().ToString();
           string strsql = "insert into WX_Account(AccountID,AccountName,AccountModel,TradePWD,MobilePhone,Status,RegTime,OpenAnswerPhone,IsAutoLock,CompanyID)";
                strsql += "values(@AccountID,@AccountName,@AccountModel,@TradePWD,@MobilePhone,@Status,@RegTime,@OpenAnswerPhone,@IsAutoLock,@CompanyID)";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AccountID", model.AccountID);
                dbOperator.AddParameter("AccountName", model.NickName);
                dbOperator.AddParameter("AccountModel", 1);
                dbOperator.AddParameter("TradePWD", "123456");
                dbOperator.AddParameter("MobilePhone", model.MobilePhone);
                dbOperator.AddParameter("Status", 0);
                dbOperator.AddParameter("RegTime", DateTime.Now);
                dbOperator.AddParameter("OpenAnswerPhone", false);
                dbOperator.AddParameter("IsAutoLock", false);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strsql)> 0;
        }
        public bool AddWXInfo(WX_Info model, DbOperator dbOperator)
        {
           string strsql = "insert into WX_Info(OpenID,AccountID,UserType,FollowState,NickName,Language,Province,City,Country,Sex,Headimgurl,SubscribeTimes,LastSubscribeDate,LastUnsubscribeDate,LastVisitDate,CompanyID)";
                strsql += "values(@OpenID,@AccountID,@UserType,@FollowState,@NickName,@Language,@Province,@City,@Country,@Sex,@Headimgurl,@SubscribeTimes,@LastSubscribeDate,@LastUnsubscribeDate,@LastVisitDate,@CompanyID)";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("OpenID", model.OpenID);
                dbOperator.AddParameter("AccountID", model.AccountID);
                dbOperator.AddParameter("UserType", model.UserType);
                dbOperator.AddParameter("FollowState", 1);
                dbOperator.AddParameter("NickName", model.NickName);
                dbOperator.AddParameter("Language", model.Language);
                dbOperator.AddParameter("Province", model.Province);
                dbOperator.AddParameter("City", model.City);
                dbOperator.AddParameter("Country", model.Country);
                dbOperator.AddParameter("Sex", model.Sex);
                dbOperator.AddParameter("Headimgurl", model.Headimgurl);
                dbOperator.AddParameter("SubscribeTimes", 1);
                dbOperator.AddParameter("LastSubscribeDate", DateTime.Now);
                dbOperator.AddParameter("LastUnsubscribeDate", DateTime.Now);
                dbOperator.AddParameter("LastVisitDate", DateTime.Now);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strsql) > 0;
        }
        public WX_Info QueryWXInfoByMobilePhone(string mobilePhone, string companyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select a.*,b.MobilePhone from WX_Info a left join WX_Account b on a.AccountID=b.AccountID where b.MobilePhone=@MobilePhone and b.CompanyID=@CompanyID and b.status!=1");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("MobilePhone", mobilePhone);
                dbOperator.AddParameter("CompanyID", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<WX_Info>.ToModel(reader);

                    }
                    return null;
                }
            }
        }
    }
}
