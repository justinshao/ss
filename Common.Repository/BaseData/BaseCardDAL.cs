using Common.IRepository.BaseData;
using System;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.Core;
using System.Collections.Generic;

namespace Common.SqlRepository.BaseData
{
    public class BaseCardDAL : BaseDAL, IBaseCard
    {
        public BaseCard GetBaseCard(string cardID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                string sql = @"select * from BaseCard where CardID =@CardID and DataStatus!=@DataStatus;";
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("CardID", cardID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<BaseCard>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public bool ModifyBaseCard(BaseCard model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update BaseCard set Balance=@Balance,CardID=@CardID,CardNo=@CardNo,
                                    CardNumb=@CardNumb,CardSystem=@CardSystem,CardType=@CardType,
                                    Deposit=@Deposit,EmployeeID=@EmployeeID,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,
                                    OperatorID=@OperatorID,State=@State,VID=@VID");
                    strSql.Append(" where CardID=@CardID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("Balance", model.Balance);
                    dbOperator.AddParameter("CardID", model.CardID);
                    dbOperator.AddParameter("CardNo", model.CardNo);
                    dbOperator.AddParameter("CardNumb", model.CardNumb);
                    dbOperator.AddParameter("CardSystem", (int)model.CardSystem);
                    dbOperator.AddParameter("CardType", (int)model.CardType);  
                    dbOperator.AddParameter("Deposit", model.Deposit);
                    dbOperator.AddParameter("EmployeeID", model.EmployeeID);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("OperatorID", model.OperatorID);
                    dbOperator.AddParameter("State", (int)model.State);
                    dbOperator.AddParameter("VID", model.VID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public List<BaseCard> QueryBaseCardByVillageId(string villageId)
        {
            List<BaseCard> basecards =new List<BaseCard>();
            string sql = @"select * from BaseCard where VID =@VID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        basecards.Add( DataReaderToModel<BaseCard>.ToModel(reader));
                    }
                }
            }
            return basecards;
        }

        public bool Add(BaseCard model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(BaseCard model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BaseCard(CardID,EmployeeID,CardNo,CardNumb,CardType,Balance,State,Deposit,RegisterTime ,OperatorID,CardSystem,VID,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@CardID,@EmployeeID,@CardNo,@CardNumb,@CardType,@Balance,@State,@Deposit,@RegisterTime ,@OperatorID,@CardSystem,@VID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CardID", model.CardID);
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("CardNo", model.CardNo);
            dbOperator.AddParameter("CardNumb", model.CardNumb);
            dbOperator.AddParameter("CardType", (int)model.CardType);
            dbOperator.AddParameter("Balance", model.Balance);
            dbOperator.AddParameter("State", (int)model.State);
            dbOperator.AddParameter("Deposit", model.Deposit);
            dbOperator.AddParameter("RegisterTime", model.RegisterTime);
            dbOperator.AddParameter("OperatorID",model.OperatorID);
            dbOperator.AddParameter("CardSystem", (int)model.CardSystem);
            dbOperator.AddParameter("VID",model.VID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(BaseCard model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set Balance=@Balance,CardNo=@CardNo,
                                    CardNumb=@CardNumb,CardSystem=@CardSystem,CardType=@CardType,
                                    Deposit=@Deposit,EmployeeID=@EmployeeID,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,
                                    OperatorID=@OperatorID,RegisterTime=@RegisterTime,State=@State,VID=@VID");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CardID", model.CardID);
            dbOperator.AddParameter("Balance", model.Balance);
            dbOperator.AddParameter("CardNo", model.CardNo);
            dbOperator.AddParameter("CardNumb", model.CardNumb);
            dbOperator.AddParameter("CardSystem", (int)model.CardSystem);
            dbOperator.AddParameter("CardType", (int)model.CardType);
            dbOperator.AddParameter("Deposit", model.Deposit);
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("OperatorID", model.OperatorID);
            dbOperator.AddParameter("RegisterTime", model.RegisterTime);
            dbOperator.AddParameter("State", (int)model.State);
            dbOperator.AddParameter("VID", model.VID);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }


        public bool CancelEmployeeIdByCardId(string cardId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set EmployeeID=@EmployeeID,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CardID", cardId);
            dbOperator.AddParameter("EmployeeID",DBNull.Value);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool CancelEmployeeIdByEmployeeId(string employeeId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set EmployeeID=@EmployeeID,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where EmployeeID=@EmployeeID1");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("EmployeeID1", employeeId);
            dbOperator.AddParameter("EmployeeID", DBNull.Value);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool LogOut(string cardId, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set State=@State,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CardID", cardId);
            dbOperator.AddParameter("State", (int)CardStatus.LogOut);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool IssueCard(string employeeId, string cardId, string carNo, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set EmployeeId=@EmployeeId,CarNo=@CarNo,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("CardID", cardId);
            dbOperator.AddParameter("EmployeeId", employeeId);
            dbOperator.AddParameter("CarNo", carNo);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Recharge(string cardId, decimal balance, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update BaseCard set Balance=Balance+@Balance,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("Balance", balance);
            dbOperator.AddParameter("CardID", cardId);
           
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool SetEndDate(string cardId, DateTime enddate, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ParkGrant set EndDate=@EndDate,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime");
            strSql.Append(" where CardID=@CardID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("EndDate", enddate);
            dbOperator.AddParameter("CardID", cardId);

            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool Delete(string cardId)
        {
            return CommonDelete("BaseCard", "CardID", cardId);
        }

        public bool Delete(string cardId, DbOperator dbOperator)
        {
            return CommonDelete("BaseCard", "CardID", cardId,dbOperator);
        }

        public BaseCard QueryBaseCard(string villageId, string cardNo)
        {
            string sql = @"select * from BaseCard where  VID =@VID and CardNo =@CardNo and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("CardNo", cardNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCard>.ToModel(reader);
                    }
                }
            }
            return null;
        }
        public BaseCard QueryBaseCardByParkingId(string parkingId, string cardNo)
        {
            string sql = @"select c.* from BaseCard c inner join ParkGrant g on c.CardID=g.CardID  where  g.PKID =@PKID and c.CardNo =@CardNo  and c.DataStatus!=@DataStatus and g.DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("CardNo", cardNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCard>.ToModel(reader);
                    }
                }
            }
            return null;
        
        }
        public BaseCard QueryBaseCard(string villageId, string cardNo, CardType cardType)
        {
            string sql = @"select * from BaseCard where  VID =@VID and CardNo =@CardNo and CardType=@CardType and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("CardNo", cardNo);
                dbOperator.AddParameter("CardType", (int)cardType);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCard>.ToModel(reader);
                    }
                }
            }
            return null;
        }

        public BaseCard QueryBaseCard(string villageId, CardType cardType, string cardNumber)
        {
            string sql = @"select * from BaseCard where  VID =@VID and CardNumb =@CardNumb and CardType=@CardType and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("CardNumb", cardNumber);
                dbOperator.AddParameter("CardType", (int)cardType);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCard>.ToModel(reader);
                    } 
                }
            }
            return null;
        }

        public BaseCard QueryBaseCardByCardNumber(string villageId, string cardNumber)
        {
            string sql = @"select * from BaseCard where  VID =@VID and CardNumb =@CardNumb  and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", villageId);
                dbOperator.AddParameter("CardNumb", cardNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BaseCard>.ToModel(reader);
                    }
                }
            }
            return null;
        }

        public List<BaseCard> QueryBaseCardByEmployeeId(string employeeId)
        {
            List<BaseCard> basecards = new List<BaseCard>();
            string sql = @"select * from BaseCard where EmployeeID =@EmployeeID and DataStatus!=@DataStatus;";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("EmployeeID", employeeId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                { 
                    while (reader.Read())
                    {
                        basecards.Add( DataReaderToModel<BaseCard>.ToModel(reader));
                    }
                }
            }
            return basecards;
        }

        public List<BaseCard> QueryPage(string villageId, string employeeId, string cardNo, CardType? cardType, int pagesize, int pageindex, out int total)
        {
            List<BaseCard> basecards = new List<BaseCard>();
             
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from BaseCard where DataStatus!=@DataStatus");
                if (!string.IsNullOrWhiteSpace(villageId))
                {
                    strSql.Append(" AND VID=@VID");
                    dbOperator.AddParameter("VID", villageId);
                }
                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    strSql.Append(" AND EmployeeID=@EmployeeID");
                    dbOperator.AddParameter("EmployeeID", employeeId);
                }
                if (!string.IsNullOrWhiteSpace(cardNo))
                {
                    strSql.Append(" AND CardNo=@CardNo");
                    dbOperator.AddParameter("CardNo", cardNo);
                }
                if (cardType.HasValue)
                {
                    strSql.Append(" AND CardType=@CardType");
                    dbOperator.AddParameter("CardType", (int)cardType.Value);
                }
               
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<BaseVillage> models = new List<BaseVillage>();
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " ID DESC", pageindex, pagesize, out total))
                {
                    while (reader.Read())
                    {
                        basecards.Add(DataReaderToModel<BaseCard>.ToModel(reader));
                    } 
                }
            }
            return basecards;
        }

        public List<BaseCard> QueryBaseCardByEmployeeInfo(string villageId, string condition)
        {
            List<BaseCard> basecards = new List<BaseCard>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select c.*,e.EmployeeName,e.MobilePhone from BaseCard c left join BaseEmployee e on c.EmployeeID= e.EmployeeID ");
                strSql.Append(" where c.VID=@VID and c.DataStatus!=@DataStatus and e.DataStatus!=@DataStatus");

                dbOperator.AddParameter("VID", villageId);

                if (!string.IsNullOrWhiteSpace(condition))
                {
                    strSql.Append(" and (e.EmployeeName like @Condition or e.MobilePhone like @Condition)");
                    dbOperator.AddParameter("Condition", "%" + condition + "%");
                }
               
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        basecards.Add(DataReaderToModel<BaseCard>.ToModel(reader));
                    }
                }
            }
            return basecards;
        }

        public List<BaseCard> QueryBaseCardByEmployeeMobile(string villageId,string mobile)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select c.*,e.EmployeeName,e.MobilePhone from BaseCard c inner join BaseEmployee e on c.EmployeeID= e.EmployeeID ");
                strSql.Append(" where c.VID=@VID and e.MobilePhone=@MobilePhone and c.DataStatus!=@DataStatus and e.DataStatus!=@DataStatus");

                dbOperator.AddParameter("VID", villageId);

                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    strSql.Append(" and MobilePhone=@MobilePhone");
                    dbOperator.AddParameter("MobilePhone", mobile);
                }

                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    return DataReaderToModel<List<BaseCard>>.ToModel(reader);
                }
            }
        }
         
    }
}
