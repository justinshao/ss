using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Statistics
{
   [Serializable] 
   public partial class tgPerson
   {
       public tgPerson(){}
       private int _id;
       private string _name;
       private string _phone;
       private string _bz;
       private string _count;
        private string _PKID;
        private string _PKName;

        public int id
       {
           get { return _id; }
           set { _id = value; }
       }

       public string name {
           get { return _name; }
           set { _name = value; }
       }

       public string phone
       {
           get { return _phone; }
           set { _phone = value; }
       }

       public string bz {
           get { return _bz; }
           set { _bz = value; }
       }

       public string count
       {
           get { return _count; }
           set { _count = value; }
       }
        public string PKID
        {
            get { return _PKID; }
            set { _PKID = value; }
        }

        public string PKName
        {
            get { return _PKName; }
            set { _PKName = value; }
        }

    }
}
