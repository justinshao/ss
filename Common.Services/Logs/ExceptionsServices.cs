using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory;
using Common.IRepository;

namespace Common.Services
{
    public class ExceptionsServices
    {
       
        public static void AddExceptions(Exception ex, string description,LogFrom? logFrom = null)
        {
            AddExceptions(new Exceptions()
            {
                DateTime = DateTime.Now,
                Description = description,
                Detail = ex.Message,
                Server = Common.Utilities.AddressLoader.GetServerIp(),
                Source = ex.Source,
                Track = ex.StackTrace,
                LogFrom = logFrom.HasValue ? logFrom.Value : OperateLogServices.GeLogFrom()
            });
            if (ex.InnerException == null) return;
            AddExceptions(ex.InnerException, "InnerException");
        }

        public static void AddException(string source, string description, string detail, LogFrom? logFrom = null)
        {
            AddExceptions(new Exceptions()
            {
                DateTime = DateTime.Now,
                Description = description,
                Source = source,
                Detail = detail,
                Track = string.Empty,
                Server = Common.Utilities.AddressLoader.GetServerIp(),
                LogFrom = logFrom.HasValue ? logFrom.Value : OperateLogServices.GeLogFrom()
            });
        }

        public static void AddException(string source, string description, Exception e, LogFrom? logFrom = null)
        {
            AddExceptions(new Exceptions()
            {
                DateTime = DateTime.Now,
                Description = description,
                Source = source,
                Detail = e.Message,
                Track = e.StackTrace,
                Server = Common.Utilities.AddressLoader.GetServerIp(),
                LogFrom = logFrom.HasValue ? logFrom.Value : OperateLogServices.GeLogFrom()
            });
        }

        public static void AddExceptions(Exceptions item)
        {
            try
            {
                item.Track = SubString(item.Track, 2000);
                item.Source = SubString(item.Source, 100);
                item.Description = SubString(item.Description, 400);
                item.Detail = SubString(item.Detail, 1000);
                item.DateTime = DateTime.Now;
                IExceptions factory = ExceptionsFactory.GetFactory();
                factory.AddExceptions(item);
            }
            catch (Exception ex)
            {
                TxtLogServices.WriteTxtLogEx("AddExceptionsLogError",ex);
            }
        }

        private static string SubString(string result, int length)
        {
            if (!string.IsNullOrEmpty(result) && result.Length > length)
                result = result.Substring(0, length);
            return result;
        }

        public static List<Exceptions> LoadExceptions(ExceptionCondition condition, int pageIndex, int pageSize, out int recordTotalCount) {
            IExceptions factory = ExceptionsFactory.GetFactory();
            return factory.LoadExceptions(condition,pageIndex,pageSize,out recordTotalCount);
        }
        public static void AddExceptionToDbAndTxt(string fileName, string description, Exception ex, LogFrom? logFrom = null)
        {
            AddExceptions(ex,description,logFrom);
            TxtLogServices.WriteTxtLogEx(fileName,description,ex);
        }
        public static void AddExceptionToDbAndTxt(string source, string description, string detail, LogFrom? logFrom = null)
        {
            AddExceptions(new Exceptions()
            {
                DateTime = DateTime.Now,
                Description = description,
                Source = source,
                Detail = detail,
                Track = string.Empty,
                Server = Common.Utilities.AddressLoader.GetServerIp(),
                LogFrom = logFrom.HasValue ? logFrom.Value : OperateLogServices.GeLogFrom()
            });
            TxtLogServices.WriteTxtLogEx(source, "description:{0},detail:{1}",description,detail);
        }
    }
}
