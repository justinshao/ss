using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory.Logs;
using Common.IRepository;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Common.Utilities;
using System.Web;
using System.Web.SessionState;

namespace Common.Services
{
    public class OperateLogServices
    {
        public static void AddOperateLog<T>(T t, OperateType operateType)
        {

            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            String className = method.ReflectedType.Name;

            string remark = ObjectHelper.getProperties<T>(t);
            AddOperateLog(className, method.Name, operateType, remark);
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="remark">备注</param>
        public static void AddOperateLog(OperateType operateType, string remark)
        {
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            String className = method.ReflectedType.Name;

            AddOperateLog(className, method.Name, operateType, remark);
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="remark">备注</param>
        ///<param name="parkingId">车场编号</param>
        ///<param name="parkingName">车场名称</param>
        public static void AddOperateLog(string moduleName, string methodName, OperateType operateType, string remark)
        {
            try
            {
                OperateLog model = new OperateLog();
                model.OperateTime = DateTime.Now;
                model.OperateType = operateType;
                model.OperatorContent = SubString(remark, 4000);
                model.ModuleName = moduleName;
                model.MethodName = methodName;
                if (SystemDefaultConfig.IsCS)
                {
                    model.Operator = SystemDefaultConfig.OperatorUserAccount;
                    model.LogFrom = LogFrom.OmnipotentCard;
                }
                else
                {
                    model.LogFrom = GeLogFrom();
                    model.Operator = GetOprator();
                   
                  
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(AddOperateLog), model);
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "添加操作日志出错");
            }
        }
        public static LogFrom GeLogFrom()
        {
            try
            {
                if (SystemDefaultConfig.IsCS)
                {
                    return LogFrom.OmnipotentCard;
                }else
                if (HttpContext.Current.Session["SmartSystem_LogFrom"] != null)
                {
                    return (LogFrom)((int)HttpContext.Current.Session["SmartSystem_LogFrom"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "添加操作日志，获取日志来源失败");
            }
            return LogFrom.UnKnown;
        }
        private static string GetOprator() {
            try
            {
                if (HttpContext.Current.Session["SmartSystem_OperatorUserAccount"] != null)
                {
                    string operatorUserAccount = HttpContext.Current.Session["SmartSystem_OperatorUserAccount"].ToString();
                    if (!string.IsNullOrWhiteSpace(operatorUserAccount)) return operatorUserAccount;
                }
            }
            catch (Exception ex) {
                ExceptionsServices.AddExceptions(ex, "添加操作日志，获取日志来源失败");
            }
            return "未知";
        }

        private static string SubString(string result, int length)
        {
            if (!string.IsNullOrEmpty(result) && result.Length > length)
                result = result.Substring(0, length);
            return result;
        }
        static void AddOperateLog(object obj)
        {
            try
            {
                OperateLog model = obj as OperateLog;
                if (model != null)
                {
                    IOperateLog factory = OperateLogFactory.GetFactory();
                    factory.Add(model);
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex,"添加操作日志出错");
            }
        }
        public static Paging<OperateLog> QueryPage(OperateLogCondition condition, int pagesize, int pageindex, out int total) {
            IOperateLog factory = OperateLogFactory.GetFactory();
            return factory.QueryPage(condition,pagesize,pageindex,out total);
        }
    }
}
