using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common; 
using Common.Utilities;

namespace Common.SqlRepository
{
    public class EmployeePlateDAL : BaseDAL, IEmployeePlate
    {
        public List<EmployeePlate> GetGrantPlateNumberListByLike(string villageid, string plateNumber, out string ErrorMessage)
        {
            List<EmployeePlate> _employeePlates = new List<EmployeePlate>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID where e.PlateNo like @PlateNo and e.DataStatus!=@DataStatus and b.VID=@VID");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNo", "%" + plateNumber + "%");
                    dbOperator.AddParameter("VID", villageid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            _employeePlates.Add(DataReaderToModel<EmployeePlate>.ToModel(reader));
                        } 
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return _employeePlates;
        }
        public EmployeePlate GetGrantPlateNumberByLike(string villageid, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID where e.PlateNo like @PlateNo and e.DataStatus!=@DataStatus and b.VID=@VID");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNo","%"+ plateNumber+"%");
                    dbOperator.AddParameter("VID", villageid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<EmployeePlate>.ToModel(reader);
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

        public EmployeePlate GetEmployeePlateNumberByPlateNumber(string villageid, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID where e.PlateNo=@PlateNo and b.VID=@VID and e.DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNo", plateNumber);
                    dbOperator.AddParameter("VID", villageid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<EmployeePlate>.ToModel(reader);
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

        public EmployeePlate GetPlateNumber(string villageid, string plateID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID  where e.PlateID=@PlateID and b.VID=@VID and e.DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateID", plateID);
                    dbOperator.AddParameter("VID", villageid);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<EmployeePlate>.ToModel(reader);
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

        public bool Add(EmployeePlate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(EmployeePlate model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into EmployeePlate(PlateID,EmployeeID,PlateNo,Color,CarBrand,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@PlateID,@EmployeeID,@PlateNo,@Color,@CarBrand,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("PlateID", model.PlateID);
            dbOperator.AddParameter("EmployeeID",model.EmployeeID);
            dbOperator.AddParameter("PlateNo", model.PlateNo);
            dbOperator.AddParameter("Color", (int)model.Color);
            dbOperator.AddParameter("CarBrand",model.CarBrand);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(EmployeePlate model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update EmployeePlate set EmployeeID=@EmployeeID,PlateNo=@PlateNo,Color=@Color,CarBrand=@CarBrand,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
                strSql.Append(" where PlateID=@PlateID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateID", model.PlateID);
                dbOperator.AddParameter("EmployeeID",model.EmployeeID);
                dbOperator.AddParameter("PlateNo", model.PlateNo);
                dbOperator.AddParameter("Color", (int)model.Color);
                dbOperator.AddParameter("CarBrand", model.CarBrand);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(EmployeePlate model, DbOperator dbOperator)
        {
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update EmployeePlate set EmployeeID=@EmployeeID,PlateNo=@PlateNo,Color=@Color,CarBrand=@CarBrand,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where PlateID=@PlateID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("PlateID", model.PlateID);
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("PlateNo", model.PlateNo);
            dbOperator.AddParameter("Color", (int)model.Color);
            dbOperator.AddParameter("CarBrand", model.CarBrand);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Update(string plateId, string plateNumber, PlateColor color, DbOperator dbOperator)
        {
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update EmployeePlate set PlateNo=@PlateNo,Color=@Color,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where PlateID=@PlateID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("PlateID", plateId);
            dbOperator.AddParameter("PlateNo",plateNumber);
            dbOperator.AddParameter("Color", (int)color);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string plateId, DbOperator dbOperator)
        {
            return CommonDelete("EmployeePlate", "PlateID", plateId, dbOperator);
        }

        public bool DeleteByEmployeeId(string employeeId, DbOperator dbOperator)
        {
            return CommonDelete("EmployeePlate", "EmployeeID", employeeId, dbOperator);
        }

        public List<EmployeePlate> QueryByEmployeeId(string employeeId)
        {
            List<EmployeePlate> _employeePlates = new List<EmployeePlate>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID where b.EmployeeID= @EmployeeID and e.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("EmployeeID", employeeId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        _employeePlates.Add( DataReaderToModel<EmployeePlate>.ToModel(reader));
                    }
                }
            }
            return _employeePlates;
        }

        public EmployeePlate Query(string plateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from EmployeePlate  where PlateID= @PlateID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateID", plateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<EmployeePlate>.ToModel(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public EmployeePlate QueryByEmployeeIdAndPlateNumber(string employeeId, string plateNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from EmployeePlate e LEFT JOIN BaseEmployee b on e.EmployeeID =b.EmployeeID where b.EmployeeID= @EmployeeID and PlateNo=@PlateNo and e.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("EmployeeID", employeeId);
                dbOperator.AddParameter("PlateNo", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<EmployeePlate>.ToModel(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

    }
}
