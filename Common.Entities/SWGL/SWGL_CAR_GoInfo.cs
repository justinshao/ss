using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Enum;

namespace Common.Entities.Order
{
    public class SWGL_CAR_GoInfo
    {
        #region Model
        private int _goID; 
        /// <summary>
        /// 主键
        /// </summary>
        public int GoID
        {
            set { _goID = value; }
            get { return _goID; }
        }

        private string _BID;
        /// <summary>
        ///  
        /// </summary>
        public string BID
        {
            set { _BID = value; }
            get { return _BID; }
        }


        private string _FlowID;
        /// <summary>
        ///  
        /// </summary>
        public string FlowID
        {
            set { _FlowID = value; }
            get { return _FlowID; }
        }


        private string _SSDW;
        /// <summary>
        ///  
        /// </summary>
        public string SSDW
        {
            set { _SSDW = value; }
            get { return _SSDW; }
        }


        private string _SSBM;
        /// <summary>
        ///  
        /// </summary>
        public string SSBM
        {
            set { _SSBM = value; }
            get { return _SSBM; }
        }

        private string _UseAcc;
        /// <summary>
        ///  
        /// </summary>
        public string UseAcc
        {
            set { _UseAcc = value; }
            get { return _UseAcc; }
        }



        private string _UseAccName;
        /// <summary>
        ///  
        /// </summary>
        public string UseAccName
        {
            set { _UseAccName = value; }
            get { return _UseAccName; }
        }
         
        private string _DesgoTime;
        /// <summary>
        ///  
        /// </summary>
        public string DesgoTime
        {
            set { _DesgoTime = value; }
            get { return _DesgoTime; }
        }


        private string _DesbackTime;
        /// <summary>
        ///  
        /// </summary>
        public string DesbackTime
        {
            set { _DesbackTime = value; }
            get { return _DesbackTime; }
        }


        private string _GoAddress;
        /// <summary>
        ///  
        /// </summary>
        public string GoAddress
        {
            set { _GoAddress = value; }
            get { return _GoAddress; }
        }



        private string _ReqAcc;
        /// <summary>
        ///  
        /// </summary>
        public string ReqAcc
        {
            set { _ReqAcc = value; }
            get { return _ReqAcc; }
        }


        private string _ReqAccName;
        /// <summary>
        ///  
        /// </summary>
        public string ReqAccName
        {
            set { _ReqAccName = value; }
            get { return _ReqAccName; }
        }


        private string _ReqTime;
        /// <summary>
        ///  
        /// </summary>
        public string ReqTime
        {
            set { _ReqTime = value; }
            get { return _ReqTime; }
        }

        private string _ReqWhys;
        /// <summary>
        ///  
        /// </summary>
        public string ReqWhys
        {
            set { _ReqWhys = value; }
            get { return _ReqWhys; }
        }

        private string _AppState;
        /// <summary>
        ///  
        /// </summary>
        public string AppState
        {
            set { _AppState = value; }
            get { return _AppState; }
        }


        private string _AppAcc;
        /// <summary>
        ///  
        /// </summary>
        public string AppAcc
        {
            set { _AppAcc = value; }
            get { return _AppAcc; }
        }


        private string _AppAccName;
        /// <summary>
        ///  
        /// </summary>
        public string AppAccName
        {
            set { _AppAccName = value; }
            get { return _AppAccName; }
        }
        private string _AppTime;
        /// <summary>
        ///  
        /// </summary>
        public string AppTime
        {
            set { _AppTime = value; }
            get { return _AppTime; }
        }


        private string _AppGoTime;
        /// <summary>
        ///  
        /// </summary>
        public string AppGoTime
        {
            set { _AppGoTime = value; }
            get { return _AppGoTime; }
        }

        private string _AppBackTime;
        /// <summary>
        ///  
        /// </summary>
        public string AppBackTime
        {
            set { _AppBackTime = value; }
            get { return _AppBackTime; }
        }


        private string _GIAcc;
        /// <summary>
        ///  
        /// </summary>
        public string GIAcc
        {
            set { _GIAcc = value; }
            get { return _GIAcc; }
        }
        private string _GIAccName;
        /// <summary>
        ///  
        /// </summary>
        public string GIAccName
        {
            set { _GIAccName = value; }
            get { return _GIAccName; }
        }
        private DateTime _GoTime;
        /// <summary>
        ///  
        /// </summary>
        public DateTime GoTime
        {
            set { _GoTime = value; }
            get { return _GoTime; }
        }


        private string _BIAcc;
        /// <summary>
        ///  
        /// </summary>
        public string BIAcc
        {
            set { _BIAcc = value; }
            get { return _BIAcc; }
        }



        private string _BIAccName;
        /// <summary>
        ///  
        /// </summary>
        public string BIAccName
        {
            set { _BIAccName = value; }
            get { return _BIAccName; }
        }


        private DateTime _BackTime;
        /// <summary>
        ///  
        /// </summary>
        public DateTime BackTime
        {
            set { _BackTime = value; }
            get { return _BackTime; }
        }



        private string _OutMode;
        /// <summary>
        ///  
        /// </summary>
        public string OutMode
        {
            set { _OutMode = value; }
            get { return _OutMode; }
        }



        private string _Driver;
        /// <summary>
        ///  
        /// </summary>
        public string Driver
        {
            set { _Driver = value; }
            get { return _Driver; }
        }


        private string _DriverName;
        /// <summary>
        ///  
        /// </summary>
        public string DriverName
        {
            set { _DriverName = value; }
            get { return _DriverName; }
        }



        private string _Memo;
        /// <summary>
        ///  
        /// </summary>
        public string Memo
        {
            set { _Memo = value; }
            get { return _Memo; }
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


        private DateTime _AddTime;
        /// <summary>
        ///  
        /// </summary>
        public DateTime AddTime
        {
            set { _AddTime = value; }
            get { return _AddTime; }
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
        #endregion Model
    }
}
