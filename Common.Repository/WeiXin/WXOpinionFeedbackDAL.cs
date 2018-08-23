using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.WeiXin;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;

namespace Common.SqlRepository.WeiXin
{
    public class WXOpinionFeedbackDAL : IOpinionFeedback
    {
        public bool Create(WX_OpinionFeedback model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO WX_OpinionFeedback(OpenId,FeedbackContent,CreateTime,CompanyID) VALUES (@OpenId,@FeedbackContent,@CreateTime,@CompanyID);");
                dbOperator.AddParameter("OpenId", model.OpenId);
                dbOperator.AddParameter("FeedbackContent", model.FeedbackContent);
                dbOperator.AddParameter("CreateTime",DateTime.Now);
                dbOperator.AddParameter("CompanyID", model.CompanyID);
                return dbOperator.ExecuteNonQuery(strSql.ToString())>0;
            }
        }

        public List<WX_OpinionFeedback> QueryPage(string companyId,DateTime start, DateTime end, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT o.id,o.FeedbackContent,o.CreateTime,w.NickName FROM WX_OpinionFeedback o inner join WX_Info w on o.Openid=w.openid WHERE  o.CreateTime>=@STARTCREATETIME  AND o.CreateTime<=@ENDCREATETIME and o.CompanyID=@CompanyID");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.AddParameter("STARTCREATETIME", start.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("ENDCREATETIME", end.ToString("yyyy-MM-dd HH:mm:ss"));
                dbOperator.AddParameter("CompanyID", companyId);

                List<WX_OpinionFeedback> models = new List<WX_OpinionFeedback>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "id DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(new WX_OpinionFeedback()
                        {
                            Id = reader.GetInt32DefaultZero(0),
                            FeedbackContent = reader.GetStringDefaultEmpty(1),
                            CreateTime = reader.GetDateTimeDefaultMin(2),
                            OpenId = reader.GetStringDefaultEmpty(3),
                           
                        });
                    }
                }
                return models;
            }
        }
    }
}
