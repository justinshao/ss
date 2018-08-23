using System.ComponentModel;

namespace Common.Utilities
{
    public class EnumContext : INotifyPropertyChanged
    {
        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion   
        
        private string _enumString;
        public string EnumString
        {
            get { return _enumString; }
            set
            {

                if (_enumString != value)
                {
                    _enumString = value;
                    Notify("EnumString");
                }
            }
        }
        private int _enumValue;
        public int EnumValue
        {
            get { return _enumValue; }
            set
            {

                if (_enumValue != value)
                {
                    _enumValue = value;
                    Notify("EnumValue");
                }
            }
        }

        private string _description; 
        public string Description
        {
            get { return _description; }
            set
            {

                if (_description != value)
                {
                    _description = value;
                    Notify("EnumValue");
                }
            }
        }
    }
}
