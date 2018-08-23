using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;

namespace Common.SqlRepository.Park
{
    public class ParkReserveBitDAL : BaseDAL, IParkReserveBit
    {
        public ParkReserveBit GetCanUseParkReserveBit(string parkID, string plateNumber, out string errorMsg)
        {
            ParkReserveBit parkReserveBit = null;
            errorMsg = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkReserveBit where PlateNumber=@PlateNumber and PKID=@PKID and DataStatus!=@DataStatus and Status=0");
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("PKID", parkID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            parkReserveBit = DataReaderToModel<ParkReserveBit>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            return parkReserveBit;
        }

        public bool ModifyReserveBit(string ReserveBitID, int status, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ParkReserveBit set Status=@Status,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", ReserveBitID);
                    dbOperator.AddParameter("Status", status); 
                    dbOperator.AddParameter("HaveUpdate", 1);
                    dbOperator.AddParameter("LastUpdateTime", DateTime.Now); 
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }
    }
}
