using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.SWGL
{
    public class SWGL_CAR_BaseInfo
    { 
        private int _ID;
        /// <summary>
        /// 主键
        /// </summary>
        public int ID
        {
            set { _ID = value; }
            get { return _ID; }
        }


        private string _CarSign;
        /// <summary>
        /// 主键
        /// </summary>
        public string CarSign
        {
            set { _CarSign = value; }
            get { return _CarSign; }
        }


        private string _CarName;
        /// <summary>
        ///  
        /// </summary>
        public string CarName
        {
            set { _CarName = value; }
            get { return _CarName; }
        }



        private string _AddAcc;
        /// <summary>
        ///  
        /// </summary>
        public string AddAcc
        {
            set { _AddAcc = value; }
            get { return _AddAcc; }
        }


        private string _AddAccName;
        /// <summary>
        ///  
        /// </summary>
        public string AddAccName
        {
            set { _AddAccName = value; }
            get { return _AddAccName; }
        }


        private DateTime _AddTime;
        /// <summary>
        ///  
        /// </summary>
        public DateTime AddTime
        {
            set { _AddTime = value; }
            get { return _AddTime; }
        }



        private string _AddIP;
        /// <summary>
        ///  
        /// </summary>
        public string AddIP
        {
            set { _AddIP = value; }
            get { return _AddIP; }
        }



        private string _ChgAcc;
        /// <summary>
        ///  
        /// </summary>
        public string ChgAcc
        {
            set { _ChgAcc = value; }
            get { return _ChgAcc; }
        }



        private string _ChgAccName;
        /// <summary>
        ///  
        /// </summary>
        public string ChgAccName
        {
            set { _ChgAccName = value; }
            get { return _ChgAccName; }
        }



        private DateTime _ChgTime;
        /// <summary>
        ///  
        /// </summary>
        public DateTime ChgTime
        {
            set { _ChgTime = value; }
            get { return _ChgTime; }
        }


        private string _ChgIP;
        /// <summary>
        ///  
        /// </summary>
        public string ChgIP
        {
            set { _ChgIP = value; }
            get { return _ChgIP; }
        }

        private int _Deleted;
        /// <summary>
        ///  
        /// </summary>
        public int Deleted
        {
            set { _Deleted = value; }
            get { return _Deleted; }
        }
    }
}
