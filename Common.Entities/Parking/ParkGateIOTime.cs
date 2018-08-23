/// <summary>
/// ParkGateIOTime:实体类(属性说明自动提取数据库字段的描述信息)
/// </summary>
using System;
using Common.Entities;
/// <summary>
/// ParkGateIOTime:实体类(属性说明自动提取数据库字段的描述信息)
/// </summary>
[Serializable]
public partial class ParkGateIOTime
{
    public ParkGateIOTime()
    { }
    #region Model
    private int _id;
    private string _recordid;
    private string _gateid;
    private int _ruletype = 0;
    private int _weekindex;
    private DateTime _ruledate;
    private string _starttime;
    private string _endtime;
    private int _inoutstate = 0;
    private DateTime _lastupdatetime;
    private int _haveupdate;
    private DataStatus _datastatus;
    /// <summary>
    /// 
    /// </summary>
    public int ID
    {
        set { _id = value; }
        get { return _id; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string RecordID
    {
        set { _recordid = value; }
        get { return _recordid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string GateID
    {
        set { _gateid = value; }
        get { return _gateid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int RuleType
    {
        set { _ruletype = value; }
        get { return _ruletype; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int WeekIndex
    {
        set { _weekindex = value; }
        get { return _weekindex; }
    }
    /// <summary>
    /// 
    /// </summary>
    public DateTime RuleDate
    {
        set { _ruledate = value; }
        get { return _ruledate; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string StartTime
    {
        set { _starttime = value; }
        get { return _starttime; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string EndTime
    {
        set { _endtime = value; }
        get { return _endtime; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int InOutState
    {
        set { _inoutstate = value; }
        get { return _inoutstate; }
    }
    /// <summary>
    /// 
    /// </summary>
    public DateTime LastUpdateTime
    {
        set { _lastupdatetime = value; }
        get { return _lastupdatetime; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int HaveUpdate
    {
        set { _haveupdate = value; }
        get { return _haveupdate; }
    }
    /// <summary>
    /// 
    /// </summary>
    public DataStatus DataStatus
    {
        set { _datastatus = value; }
        get { return _datastatus; }
    }
    #endregion Model

}