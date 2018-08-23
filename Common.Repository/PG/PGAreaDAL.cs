using Common.DataAccess;
using Common.Entities;
using Common.Entities.PG;
using Common.IRepository.PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.SqlRepository.PG
{
    public class PGAreaDAL: BaseDAL, IPGArea
    {
        public bool Add(PGArea model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    bool result = Add(model, dbOperator);
                    if (!result) throw new MyException("保存优免信息失败");
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        private bool Add(PGArea model, DbOperator dbOperator)
        {
            model.Datastatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PGArea(AreaID,AreaName,PKID,AreaImgPath,LastUpdateTime,HaveUpdate)");
            strSql.Append(" values(@AreaID,@AreaName,@PKID,@AreaImgPath,@LastUpdateTime,@HaveUpdate)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("AreaID", model.AreaID);
            dbOperator.AddParameter("AreaName", model.AreaName);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("AreaImgPath", model.AreaImgPath);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(PGArea model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.Datastatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update PGArea set AreaName=@AreaName,PKID=@PKID,AreaImgPath=@AreaImgPath,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
                strSql.Append("  where AreaID=@AreaID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("AreaID", model.AreaID);
                dbOperator.AddParameter("AreaName", model.AreaName);
                dbOperator.AddParameter("PKID", model.PKID);
                dbOperator.AddParameter("AreaImgPath", model.AreaImgPath);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
    }
}
