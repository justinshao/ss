using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataAccess;
using Common.Entities;
using Common.IRepository;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository
{
    public class MessageDAL : BaseDAL, IMessage
    {

        /// <summary>
        /// 获取客户端通知记录数
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public int GetMessageDataCount(string text, string starttime, string endtime)
        {
            int _total = 0;
            string strSql = "select count(1) Count from Message where DataStatus!=2";
            if (!string.IsNullOrEmpty(text))
            {
                strSql += " and (MessageTitle like '%" + text + "%' or MessageTxt like '%" + text + "%')";
            }
            if (starttime != null)
            {
                DateTime Startworktime = DateTime.Parse(starttime);
                strSql += " and LastUpdateTime>='"+ DateTime.Parse(starttime) + "'";
            }
            if (endtime != null)
            {
                DateTime EndWorkTime = DateTime.Parse(endtime);
                strSql += " and LastUpdateTime<='"+ DateTime.Parse(endtime)+"'";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            { 
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

        /// <summary>
        /// 获取客户端通知记录
        /// </summary>
        /// <param name="text"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public List<Message> GetMessageData(string text, string starttime, string endtime, int PageSize, int PageIndex)
        {
            List<Message> Messagelist = new List<Message>();
            string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum,* from Message where DataStatus!=2", PageIndex * PageSize);
            if (!string.IsNullOrEmpty(text))
            {
                strSql += " and (MessageTitle like '%" + text + "%' or MessageTxt like '%" + text + "%')";
            }
            if (starttime != null)
            {
                DateTime Startworktime = DateTime.Parse(starttime);
                strSql += " and LastUpdateTime>='" + DateTime.Parse(starttime) + "'";
            }
            if (endtime != null)
            {
                DateTime EndWorkTime = DateTime.Parse(endtime);
                strSql += " and LastUpdateTime<='" + DateTime.Parse(endtime) + "'";
            }

            strSql += string.Format(" order by LastUpdateTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc", PageSize * (PageIndex - 1) + 1, PageSize * PageIndex);
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                 
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        Messagelist.Add(DataReaderToModel<Message>.ToModel(dr));
                    }
                }
            }
            return Messagelist;
        }

        public bool Add(Message model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {  
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Message(RecordID,MessageTitle,MessageTxt,LastUpdateTime,MessageStates,PostStates,DataStatus,UserID,UserName)");
                strSql.Append(" values(@RecordID,@MessageTitle,@MessageTxt,@LastUpdateTime,@MessageStates,@PostStates,@DataStatus,@UserID,@UserName)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("MessageTitle", model.MessageTitle);
                dbOperator.AddParameter("MessageTxt", model.MessageTxt);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("MessageStates", 0);
                dbOperator.AddParameter("PostStates", 0);
                dbOperator.AddParameter("DataStatus", DataStatus.Normal);
                dbOperator.AddParameter("UserID", model.UserID);
                dbOperator.AddParameter("UserName", model.UserName);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(Message model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Message set MessageTitle=@MessageTitle,MessageTxt=@MessageTxt,LastUpdateTime=@LastUpdateTime,MessageStates=@MessageStates");
                strSql.Append(",PostStates=@PostStates,DataStatus=@DataStatus,UserID=@UserID,UserName=@UserName where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("MessageTitle", model.MessageTitle);
                dbOperator.AddParameter("MessageTxt", model.MessageTxt);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("MessageStates", 0);
                dbOperator.AddParameter("PostStates", 0);
                dbOperator.AddParameter("DataStatus", DataStatus.Normal);
                dbOperator.AddParameter("UserID", model.UserID); 
                dbOperator.AddParameter("UserName", model.UserName);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        { 
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            { 
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("update Message set DataStatus=2,LastUpdateTime='{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                strSql.AppendFormat("  where recordId='{0}'", recordId);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            } 
        }
    }
}
