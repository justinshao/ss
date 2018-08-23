using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.PG
{
    [Serializable]
     public partial class goverment
    {
        public goverment() { }
        private string _pknname;
        private string _pkid;
        private decimal _ys;
        private decimal _ss;
        private decimal _xj;
        private decimal _xs;
        private decimal _bl;
        private decimal _fxcs;
        private decimal _mfcs;
        private decimal _sffxcs;
        private decimal _ssjfcs;
        private decimal _zs;
       //private string _pkids;

        public string PKName
        {
            get {
                return _pknname;
            }
            set {
                _pknname= value;
            }
        }
        public string pkid
        {
            get
            {
                return _pkid;
            }
            set
            {
                _pkid = value;
            }
        }
        public decimal ys
        {
            get { return _ys; }
            set { _ys = value; }
        }
        public decimal ss
        {
            get { return _ss; }
            set { _ss = value; }
        }
        public decimal xj
        {
            get { return _xj; }
            set { _xj = value; }
        }
        public decimal xs
        {
            get { return _xs; }
            set { _xs = value; }
        }
        public decimal bl
        {
            get { return _bl; }
            set { _bl = value; }
        }
        public decimal fxcs
        {
            get { return _fxcs; }
            set { _fxcs = value; }
        }
        public decimal mfcs
        {
            get { return _mfcs; }
            set { _mfcs = value; }
        }
        public decimal sffxcs
        {
            get { return _sffxcs; }
            set { _sffxcs = value; }
        }
        public decimal ssjfcs
        {
            get { return _ssjfcs; }
            set { _ssjfcs = value; }
        }
        public decimal zs
        {
            get { return _zs; }
            set { _zs = value; }
        }
      

    }
}
