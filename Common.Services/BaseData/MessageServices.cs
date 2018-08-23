using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.IRepository;
using Common.Factory;
using Common.Utilities;
using Common.DataAccess;
using System.Net;
using Common.Entities.Other;

namespace Common.Services
{
    public class MessageServices
    {
        /// <summary>
        /// 获取客户端通知记录
        /// </summary>
        /// <param name="text"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public static Pagination GetMessageData(string text, string starttime, string endtime, int PageSize, int PageIndex)
        {
            Pagination _pagination = new Pagination();
            IMessage factory = MessageFactory.GetFactory();
            _pagination.Total = factory.GetMessageDataCount(text, starttime, endtime);
            _pagination.MessageList = factory.GetMessageData(text, starttime, endtime, PageSize, PageIndex);
            return _pagination;
        }

        public static bool Add(Message model)
        {
            if (model == null) throw new ArgumentNullException("model");
            model.RecordID = GuidGenerator.GetGuid().ToString();
            IMessage factory = MessageFactory.GetFactory(); 
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<Message>(model, OperateType.Add);
            }
            return result;
        }

        public static bool Update(Message model)
        {
            if (model == null) throw new ArgumentNullException("model");

            IMessage factory = MessageFactory.GetFactory(); 
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<Message>(model, OperateType.Update);
            }
            return result;
        }
        public static bool Delete(string recordId)
        {
            if (recordId.IsEmpty()) throw new ArgumentNullException("recordId");

            IMessage factory = MessageFactory.GetFactory();
            bool result = factory.Delete(recordId);
            if (result)
            {
                OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("recordId:{0}", recordId));
            }
            return result;
        }
    }
}
