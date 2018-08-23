using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.DataAccess;
using Common.Entities.Parking;
using Common.IRepository.Park;
using System.Data.Common;
using Common.Utilities;
namespace Common.SqlRepository.Park
{
    public class ParkSettlementApprovalFlowDAL : BaseDAL,IParkSettlementApprovalFlow
    {
        public List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(string PKID)
        {
            List<ParkSettlementApprovalFlowModel> flows = new List<ParkSettlementApprovalFlowModel>();
            string strSql = "select p.*,u.username FlowUserName from ParkSettlementApprovalFlow p left join sysuser u on u.recordid=p.FlowUser ";
            if ( PKID != "ALL")
            {
                strSql = strSql + " where p.pkid=@PKID ";
            }
            strSql = strSql + " order by p.flowid asc";
            
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);               
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        flows.Add(DataReaderToModel<ParkSettlementApprovalFlowModel>.ToModel(reader));
                    }
                }
            }
            return flows;
        }
        public List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(IList<string> villIDs)
        {
            //return null;
            List<ParkSettlementApprovalFlowModel> flows = new List<ParkSettlementApprovalFlowModel>();
            //string strSql = "select p.*,u.username FlowUserName from ParkSettlementApprovalFlow p left join sysuser u on u.recordid=p.FlowUser where p.pkid=@PKID  order by p.flowid asc";
            string strSql = "select p.*,u.username FlowUserName from ParkSettlementApprovalFlow p left join sysuser u on u.recordid=p.FlowUser where 1=1 ";
            strSql += " and p.pkid in (select PKID from BaseParkInfo where VID in('" + string.Join("','", villIDs) + "'))";
            strSql += "order by p.flowid asc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                //dbOperator.AddParameter("PKID", PKID);
                //dbOperator.AddParameter("PKID", villIDs);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        flows.Add(DataReaderToModel<ParkSettlementApprovalFlowModel>.ToModel(reader));
                    }
                }
            }
            return flows;
        }
        public bool AddSettlementApprovalFlows(ParkSettlementApprovalFlowModel ApprovalFlows)
        {
            bool flag = false;
            string strSql = "insert into ParkSettlementApprovalFlow(flowname,pkid,remark,flowid) values(@flowname,@pkid,@remark,@flowid)";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("flowname", ApprovalFlows.FlowName);
                dbOperator.AddParameter("pkid", ApprovalFlows.PKID);
                dbOperator.AddParameter("remark", ApprovalFlows.Remark);
                dbOperator.AddParameter("flowid", ApprovalFlows.FlowID);
                flag =dbOperator.ExecuteNonQuery(strSql)>0 ? true:false;
            }
            return flag;
        }

        public Common.Entities.BaseCompany QueryParentCompanyID(string CompanyID)
        {
            Common.Entities.BaseCompany comany = null;
            string strSql = "select isnull(masterid,'') MasterID from basecompany where cpid=@cpid";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("cpid", CompanyID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    comany = new Entities.BaseCompany();
                    while (reader.Read())
                    {
                        comany = DataReaderToModel<Common.Entities.BaseCompany>.ToModel(reader);
                    }
                }
            }
            return comany;
        }

        public bool SaveFlowOperator(string PKID, string UserID, int FlowID)
        {
            ParkSettlementModel settlement = new ParkSettlementModel();
            string strSql = "update ParkSettlementApprovalFlow set FlowUser=@FlowUser where pkid=@PKID and flowid=@FlowID";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("FlowUser", UserID);
                dbOperator.AddParameter("PKID", PKID);
                dbOperator.AddParameter("FlowID", FlowID);
                return dbOperator.ExecuteNonQuery(strSql) > 0 ? true : false;
            }
        }
    }
}
