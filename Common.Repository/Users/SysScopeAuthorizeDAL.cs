using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;

namespace Common.SqlRepository
{
    public class SysScopeAuthorizeDAL : BaseDAL, ISysScopeAuthorize
    {

        public List<SysScopeAuthorize> QuerySysScopeAuthorizeByScopeId(string scopeId)
        {
            string sql = "select ID,ASID,ASDID,TagID,ASType,CPID,LastUpdateTime,HaveUpdate,DataStatus from SysScopeAuthorize where ASID=@ASID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ASID", scopeId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    List<SysScopeAuthorize> models = new List<SysScopeAuthorize>();
                    while (reader.Read())
                    {
                        models.Add(new SysScopeAuthorize()
                        {
                            ID = reader.GetInt32DefaultZero(0),
                            ASID = reader.GetStringDefaultEmpty(1),
                            ASDID = reader.GetStringDefaultEmpty(2),
                            TagID = reader.GetStringDefaultEmpty(3),
                            ASType = (ASType)reader.GetInt32DefaultZero(4),
                            CPID = reader.GetStringDefaultEmpty(5),
                            LastUpdateTime = reader.GetDateTimeDefaultMin(6),
                            HaveUpdate = reader.GetInt32DefaultZero(7),
                            DataStatus = (DataStatus)reader.GetInt32DefaultZero(8)
                        });
                    }
                    return models;
                }
            }
        }

        public bool Add(List<SysScopeAuthorize> models, DbOperator dbOperator)
        {
            foreach (var item in models) {
                item.DataStatus = DataStatus.Normal;
                item.LastUpdateTime = DateTime.Now;
                item.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SysScopeAuthorize(ASID,ASDID,TagID,ASType,CPID,LastUpdateTime,HaveUpdate,DataStatus)");
                strSql.Append(" values(@ASID,@ASDID,@TagID,@ASType,@CPID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ASID", item.ASID);
                dbOperator.AddParameter("ASDID", item.ASDID);
                dbOperator.AddParameter("TagID", item.TagID);
                dbOperator.AddParameter("ASType", (int)item.ASType);
                dbOperator.AddParameter("CPID", item.CPID);
                dbOperator.AddParameter("LastUpdateTime", item.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", item.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)item.DataStatus);
                bool result = dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                if (!result) return false;
            }
            return true;
        }

        public bool DeleteByScopeId(string scopeId, DbOperator dbOperator)
        {
            return CommonDelete("SysScopeAuthorize", "ASID", scopeId,dbOperator);
        }
    }
}
