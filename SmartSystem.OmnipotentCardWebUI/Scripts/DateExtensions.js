///<summary>把josn时间转化为日期格式</summary>
///<param name="str">要转化的json日期</param>
///<returns>返回日期格式 如:2011-01-01</returns>
String.getjosntoDate = function (jsondate) {
    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }
    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate;
};
///<summary>把josn时间转化为日期格式</summary>
///<param name="str">要转化的json日期</param>
///<returns>返回日期格式 如:12:12</returns>
String.getJsonHourAndMinutes = function (jsondate) {
    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }
    var date = new Date(parseInt(jsondate, 10));
    var Hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    var Minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
    return Hours + ":" + Minutes;
};
String.GetDateToyyyyMMdd = function(value) {
    if ($.trim(value) == "") {
        return "";
    }
    var date = new Date(value);
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate;
}
//转换日期格式
String.GetDateyyyyMMddHHmmss = function (value) {
    var date = new Date(value);
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hh = date.getHours() < 10 ? "0" + date.getHours() : date.getHours(); ; //截取小时 
    var mm = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes(); ; //截取分钟
    var ss = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds(); ; //截取秒
    return date.getFullYear() + "-" + month + "-" + currentDate + " " + hh + ":" + mm + ":" + ss;
}
String.prototype.GetDateyyyyMMddHHmmss = function () {
    return String.GetDateyyyyMMddHHmmss(this);
};