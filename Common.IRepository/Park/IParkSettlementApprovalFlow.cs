using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.Parking;
using Common.Entities;
namespace Common.IRepository.Park
{
    public interface IParkSettlementApprovalFlow
    {
        List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(string PKID);
        List<ParkSettlementApprovalFlowModel> GetSettlementApprovalFlows(IList<string> villIDs);
        bool AddSettlementApprovalFlows(ParkSettlementApprovalFlowModel ApprovalFlows);
        bool SaveFlowOperator(string PKID, string UserID, int FlowID);
        BaseCompany QueryParentCompanyID(string CompanyID);
    }
}
