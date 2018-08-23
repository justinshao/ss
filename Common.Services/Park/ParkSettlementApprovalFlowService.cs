using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.IRepository.Park;
using Common.Factory.Park;
using Common.Entities;
namespace Common.Services.Park
{
    public class ParkSettlementApprovalFlowService
    {
        /// <summary>
        /// 根据车场编号  获取所有公司
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public static List<string> GetParentCompany()
        {
            List<string> companys = new List<string>();
            //获得当前车场编号编号
            var company = new Common.SqlRepository.CompanyDAL().QueryTopCompany();
            if (company != null)
            {
                companys = company.Select(u => u.CPID).ToList();
 
            }
            return companys;
        }

       
        public static bool SaveFlowOperator(string PKID, string UserID, int FlowID)
        {
            IParkSettlementApprovalFlow factory = ParkSettlementApprovalFlowFactory.GetFactory();
            return factory.SaveFlowOperator(PKID, UserID, FlowID);
        }
        public static List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(string PKID)
        {
            IParkSettlementApprovalFlow factory = ParkSettlementApprovalFlowFactory.GetFactory();
            List<ParkSettlementApprovalFlowModel> approvals = factory.GetSettlementApprovalFlows(PKID);
            if( approvals== null || approvals.Count == 0)
            {
                ParkSettlementApprovalFlowModel psafF1 = new ParkSettlementApprovalFlowModel ();
                psafF1.FlowID = -1;
                psafF1.FlowName = "已撤销";
                psafF1.PKID = PKID;
                psafF1.Remark = "流程已被发起人取消";
                factory.AddSettlementApprovalFlows(psafF1);

                ParkSettlementApprovalFlowModel psaf2 = new ParkSettlementApprovalFlowModel ();
                psaf2.FlowID = 0;
                psaf2.FlowName = "运营商待转款";
                psaf2.PKID = PKID;
                psaf2.Remark = "运营商待转款";
                factory.AddSettlementApprovalFlows(psaf2);

                ParkSettlementApprovalFlowModel psaf3 = new ParkSettlementApprovalFlowModel ();
                psaf3.FlowID = 1;
                psaf3.FlowName = "待收款";
                psaf3.PKID = PKID;
                psaf3.Remark = "车场待收款";
                factory.AddSettlementApprovalFlows(psaf3);

                ParkSettlementApprovalFlowModel psaf4 = new ParkSettlementApprovalFlowModel ();
                psaf4.FlowID = 2;
                psaf4.FlowName = "完成";
                psaf4.PKID = PKID;
                psaf4.Remark = "车场确认收款 流程完毕";
                factory.AddSettlementApprovalFlows(psaf4);
                approvals = factory.GetSettlementApprovalFlows(PKID);
            }
            return approvals;
        }
        public static List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(IList<string> villIDs)
        {

            return null;
            //IParkSettlementApprovalFlow factory = ParkSettlementApprovalFlowFactory.GetFactory();
            //List<ParkSettlementApprovalFlowModel> approvals = factory.GetSettlementApprovalFlows(villIDs);
            //if (approvals == null || approvals.Count == 0)
            //{
            //    ParkSettlementApprovalFlowModel psafF1 = new ParkSettlementApprovalFlowModel();
            //    psafF1.FlowID = -1;
            //    psafF1.FlowName = "已撤销";
            //    psafF1.PKID = villIDs;
            //    psafF1.Remark = "流程已被发起人取消";
            //    factory.AddSettlementApprovalFlows(psafF1);

            //    ParkSettlementApprovalFlowModel psaf2 = new ParkSettlementApprovalFlowModel();
            //    psaf2.FlowID = 0;
            //    psaf2.FlowName = "运营商待转款";
            //    psaf2.PKID = villIDs;
            //    psaf2.Remark = "运营商待转款";
            //    factory.AddSettlementApprovalFlows(psaf2);

            //    ParkSettlementApprovalFlowModel psaf3 = new ParkSettlementApprovalFlowModel();
            //    psaf3.FlowID = 1;
            //    psaf3.FlowName = "待收款";
            //    psaf3.PKID = villIDs;
            //    psaf3.Remark = "车场待收款";
            //    factory.AddSettlementApprovalFlows(psaf3);

            //    ParkSettlementApprovalFlowModel psaf4 = new ParkSettlementApprovalFlowModel();
            //    psaf4.FlowID = 2;
            //    psaf4.FlowName = "完成";
            //    psaf4.PKID = villIDs;
            //    psaf4.Remark = "车场确认收款 流程完毕";
            //    factory.AddSettlementApprovalFlows(psaf4);
            //    approvals = factory.GetSettlementApprovalFlows(villIDs);
            //}
            //return approvals;
        }

    }
}
