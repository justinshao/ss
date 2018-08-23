using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.Entities;
using Common.Utilities;
using Common.IRepository.WeiXin;

namespace Common.SqlRepository.WeiXin
{
    public class WX_LockCarDAL : IWX_LockCar
    {
        public WX_LockCar GetLockCar(string parkingID, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from WX_LockCar where PKID=@PKID and DataStatus!=@DataStatus and PlateNumber=@PlateNumber");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<WX_LockCar>.ToModel(reader);
                        }
                    }
                }
            }
            catch
            {
                ErrorMessage = "";
            }
            return null;
        }

        public bool ReleaseLockCar(string parkingID, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update WX_LockCar set Status=@Status where PKID=@PKID and PlateNumber=@PlateNumber");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("Status", 0);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch(Exception e)
            {
                ErrorMessage = e.StackTrace;
            }
            return false;
        }
    }
}
