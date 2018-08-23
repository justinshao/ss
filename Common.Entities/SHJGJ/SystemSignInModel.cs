using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SHJGJ
{
    public class SystemSignInModel
    {
        public int seqno { set; get; }
        public string code { set; get; }
        public CommRequest commRequest { set; get; }
        public string platformId { set; get; }
        public string batchCode { set; get; }
        public string platformName { set; get; }
        public string operationUnit { set; get; }
        public string contact { set; get; }
        public string phoneno { set; get; }
        public int parkNum { set; get; }
    }
}
