using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;

namespace Common.IRepository
{
    public interface IMessage
    {
        int GetMessageDataCount(string text, string starttime, string endtime); 

        List<Message> GetMessageData(string text, string starttime, string endtime, int PageSize, int PageIndex);

        bool Add(Message model); 

        bool Update(Message model);

        bool Delete(string recordId);
    }
}
