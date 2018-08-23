namespace ClassLibrary1.PurseData
{
    public class UserInfo
    {
        public int Status { get; set; }
        public info Result { get; set; }
    }
    public class info {
        public string InviteCode { get; set; }
        public string Image { get; set; }
        public string NickName { get; set; }
        public int Sex { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public bool IsBindingQQ { get; set; }
        public bool IsBindingWx { get; set; }
        public bool IsSms { get; set; }
        public bool IsMsg { get; set; }
        public bool IsVip { get; set; }
        public bool IsSign { get; set; }
        public bool IsPayPwd { get; set; }
        public decimal Balance { get; set; }
        public int CardCount { get; set; }
        public int CouponCount { get; set; }
        public int Score { get; set; }
    }
}
