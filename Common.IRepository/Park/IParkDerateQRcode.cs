using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities.Enum;

namespace Common.IRepository.Park
{
    public interface IParkDerateQRcode
    {
        bool Add(ParkDerateQRcode model);

        bool Update(ParkDerateQRcode model);
        /// <summary>
        /// 优免券二维码规则
        /// </summary>
        /// <param name="recordId">二维码规则编号</param>
        /// <param name="derateQRcodeZipFilePath">优免券二维码压缩包路径</param>
        /// <param name="totalNumbers">已发放总数量</param>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        bool Update(string recordId, string derateQRcodeZipFilePath, int totalNumbers, DbOperator dbOperator);

        bool Delete(string recordId, DbOperator dbOperator);

        List<ParkDerateQRcode> QueryByDerateID(string derateId);

        ParkDerateQRcode QueryByRecordId(string recordId);

        bool UpdateAlreadyUseTimes(string recordId, int alreadyUseTimes, DbOperator dbOperator);

        List<ParkDerateQRcode> QueryPage(string sellerId, string derateId, int derateQRcodeType, int? status, DerateQRCodeSource? source, int pagesize, int pageindex, out int total);
    }
}
