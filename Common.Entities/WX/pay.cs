using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WX
{
    public class pay
    {
        private string _body = "";
        private string _fee = "";
        private string _auth_code = "";
        public string body
        {
            get { return _body; }
            set { _body = value; }
        }
        public string fee {
            get { return _fee; }
            set { _fee = value; }
        }
        public string auth_code {
            get { return _auth_code; }
            set { _auth_code = value; }
        }
    }
}
