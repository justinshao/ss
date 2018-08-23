using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Factory.Logs;
using Common.IRepository;

namespace Common.Services
{
    public class LoginLogServies
    {
        public static void Add(LoginLog model) {
            if (model == null) throw new ArgumentNullException("model");

            ILoginLog factory = LoginLogFactory.GetFactory();
            factory.Add(model);
        }

        public static void UpdateLogoutTime(string loginAccount) {
            if (string.IsNullOrWhiteSpace(loginAccount)) throw new ArgumentNullException("loginAccount");

            ILoginLog factory = LoginLogFactory.GetFactory();
            factory.UpdateLogoutTime(loginAccount);
        }

        public static Paging<LoginLog> QueryPage(LoginLogCondition condition, int pagesize, int pageindex, out int total)
        {
            ILoginLog factory = LoginLogFactory.GetFactory();
            return factory.QueryPage(condition,pagesize,pageindex,out total);
        }
    }
}
