function flow(mh, mv) {//参数mh和mv是定义数据块之间的间距，mh是水平距离，mv是垂直距离
    var w = $("#divleft").css("width").replace("px", ""); //计算页面宽度
    var ul = document.getElementById("flow-box");
    var li = ul.getElementsByTagName("li");
    var iw = li[0].offsetWidth + mh; //计算数据块的宽度
    var c = Math.floor(w / iw); //计算列数
    ul.style.width = iw * c - mh + "px"; //设置ul的宽度至适合便可以利用css定义的margin把所有内容居中

    var liLen = li.length;
    var lenArr = [];
    for (var i = 0; i < liLen; i++) {//遍历每一个数据块将高度记入数组
        lenArr.push(li[i].offsetHeight);
    }

    var oArr = [];
    for (var i = 0; i < c; i++) {//把第一行排放好，并将每一列的高度记入数据oArr
        li[i].style.top = "0";
        li[i].style.left = iw * i + "px";
        li[i].style.opacity = "1";
        li[i].style["-moz-opacity"] = "1";
        li[i].style["filter"] = "alpha(opacity=100)";
        oArr.push(lenArr[i]);
    }

    for (var i = c; i < liLen; i++) {//将其他数据块定位到最短的一列后面，然后再更新该列的高度
        var x = _getMinKey(oArr); //获取最短的一列的索引值
        li[i].style.top = oArr[x] + mv + "px";
        li[i].style.left = iw * x + "px";
        li[i].style.opacity = "1";
        li[i].style["-moz-opacity"] = "1";
        li[i].style["filter"] = "alpha(opacity=100)";
        oArr[x] = lenArr[i] + oArr[x] + mv; //更新该列的高度
    }
    document.getElementById("loadimg").style.top = _getMaxValue(oArr) + 50 + "px"; //将loading移到下面

    function scroll() {//滚动加载数据
        var st = oArr[_getMinKey(oArr)];
        var scrollTop = document.documentElement.scrollTop > document.body.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;
        if (scrollTop >= st - document.documentElement.clientHeight) {
            window.onscroll = null; //为防止重复执行，先清除事件
            _request(null, "GetList.php", function (data) {//当滚动到达最短的一列的距离时便发送ajax请求新的数据，然后执行回调函数
                _addItem(data.d, function () {//追加数据
                    var liLenNew = li.length;
                    for (var i = liLen; i < liLenNew; i++) {
                        lenArr.push(li[i].offsetHeight);
                    }
                    for (var i = liLen; i < liLenNew; i++) {
                        var x = _getMinKey(oArr);
                        li[i].style.top = oArr[x] + 10 + "px";
                        li[i].style.left = iw * x + "px";
                        li[i].style.opacity = "1";
                        li[i].style["-moz-opacity"] = "1";
                        li[i].style["filter"] = "alpha(opacity=100)";
                        oArr[x] = lenArr[i] + oArr[x] + 10;
                    }
                    document.getElementById("loadimg").style.top = _getMaxValue(oArr) + 50 + "px"; //loading向下移位
                    liLen = liLenNew;
                    window.onscroll = scroll; //执行完成，恢愎onscroll事件
                });
            })
        }
    }
    window.onscroll = scroll;
}
//图片加载完成后执行
window.onload = function () { flow(10, 10) };
//改变窗口大小时重新布局
var re;
window.onresize = function () {
    clearTimeout(re);
    re = setTimeout(function () { flow(10, 10); }, 200);
}
//追加项
function _addItem(arr, callback) {
    var _html = "";
    var a = 0;
    var l = arr.length;
    (function loadimg() {
        var img = new Image();
        img.onload = function () {
            a += 1;
            if (a == l) {
                for (var k in arr) {
                    var img = new Image();
                    img.src = arr[k].img;
                    _html += '<li><img src="' + arr[k].img + '" /><a href="#">' + arr[k].title + '</a></li>';
                }
                _appendhtml(document.getElementById("flow-box"), _html);
                callback();
            }
            else {
                loadimg();
            }
        }
        img.src = arr[a].img;
    })()
}
//ajax请求
function _request(reqdata, url, callback) {
    var xmlhttp;
    if (window.XMLHttpRequest) {
        xmlhttp = new XMLHttpRequest();
    }
    else {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var data = eval("(" + xmlhttp.responseText + ")");
            callback(data);
        }
    }
    xmlhttp.open("POST", url);
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send(reqdata);
}
//追加html
function _appendhtml(parent, child) {
    if (typeof (child) == "string") {
        var div = document.createElement("div");
        div.innerHTML = child;
        var frag = document.createDocumentFragment();
        (function () {
            if (div.firstChild) {
                frag.appendChild(div.firstChild);
                arguments.callee();
            }
            else {
                parent.appendChild(frag);
            }
        })();
    }
    else {
        parent.appendChild(child);
    }
}
//获取数字数组的最大值
function _getMaxValue(arr) {
    var a = arr[0];
    for (var k in arr) {
        if (arr[k] > a) {
            a = arr[k];
        }
    }
    return a;
}
//获取数字数组最小值的索引
function _getMinKey(arr) {
    var a = arr[0];
    var b = 0;
    for (var k in arr) {
        if (arr[k] < a) {
            a = arr[k];
            b = k;
        }
    }
    return b;
}
function btnSubmitQueryPage() {
    QueryPage(1);
}
function QueryPage(pageIndex) {
    $("#flow-box").html("");
    var parkingId = $("#cmbParkingId").combobox("getValue");
    var plateNumber = $("#txtPlateNumber").textbox("getValue");
    $.ajax({
        type: "post",
        url: '/p/CentralFee/QueryWaitPayRecord',
        data: "parkingId=" + parkingId + "&plateNumber=" + plateNumber + "&pageIndex=" + pageIndex + "",
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                var pages = data.msg.split(',');
                $("#spanRecordTotalCount").text(pages[0]);

                $("#spanTotalPageSize").text(pages[1]);
                if (pages[1] == "0") {
                    $("#spanPageIndex").text("0");
                } else {
                    $("#spanPageIndex").text(pageIndex);
                }
                SetPageButtonState();
                if (data.data.length > 0) {
                    for (var i = 0; i < data.data.length; i++) {
                        AddHtmlRecordData(data.data[i].RecordID, data.data[i].ImageUrl, data.data[i].EntranceTime, data.data[i].PlateNumber, data.data[i].ParkingId);
                    }
                }
                flow(10, 10);
            } else {
                $("#spanRecordTotalCount").text("0");
                $("#spanPageIndex").text("0");
                $("#spanTotalPageSize").text("0");
                SetPageButtonState();
                $.messager.alert('系统提示', data.msg, 'error');
            }


        }
    });
}
function SetPageButtonState() {
    var totalRow = $("#spanRecordTotalCount").text();
    var pageIndex = $("#spanPageIndex").text();
    var toatlPage = $("#spanTotalPageSize").text();
    if (parseInt(pageIndex) <= 1) {
        $("#aPrevPage").linkbutton("disable");
    } else {
        $("#aPrevPage").linkbutton("enable");
    }
    if (parseInt(toatlPage) <= parseInt(pageIndex)) {
        $("#aNextPage").linkbutton("disable");
    } else {
        $("#aNextPage").linkbutton("enable");
    }
}
$(function () {
    $("#cmbParkingId").combobox({
        url: '/ParkingData/GetOnlyParkingTreeData',
        valueField: 'id',
        textField: 'text'
    });
    $("#aPrevPage").click(function () {
        var pageIndex = $("#spanPageIndex").text();
        var index = parseInt(pageIndex) - 1;
        if (index <= 0) {
            index = 1;
            $("#spanPageIndex").text("1");
            return false;
        }
        QueryPage(index);
    });
    $("#aNextPage").click(function () {
        var pageIndex = $("#spanPageIndex").text();
        var toatlPage = $("#spanTotalPageSize").text();
        var index = parseInt(pageIndex) + 1;
        if (index > parseInt(toatlPage)) {
            index = parseInt(toatlPage);
            $("#spanPageIndex").text(toatlPage);
            return false;
        }
        QueryPage(index);
    });
    $("#btnSubmitTollRelease").linkbutton("disable");
});
function AddHtmlRecordData(RecordID, ImageUrl, EntranceTime, PlateNumber, ParkingId) {
    var html = "<li onclick=\"SelectEntranceRecord('" + RecordID + "','" + ParkingId + "')\"><img src='" + ImageUrl + "' /><div>进场时间：" + EntranceTime + "</div><div>车牌号：" + PlateNumber + "</div> <input type=\"hidden\" class=\"hiddParkingId\" value='" + ParkingId + "' /></li>";
    $("#flow-box").append(html);
}
var selectIORecordId = "";
function SelectEntranceRecord(recordId, parkingId) {
    selectIORecordId = recordId;
    $("#cmbCarModel").combobox({
        url: '/p/CentralFee/GetParkCarModelData?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onLoadSuccess: function () {
            LoadRecordData();
        },
        onSelect: function (record) {
            if (selectIORecordId == "") {
                $.messager.alert('系统提示', '请选择进出记录!', 'error');
                return;
            }
            LoadRecordData();
        }
    });
}
function LoadRecordData() {
    ResetIORecordDetial();

    var carModelId = $("#cmbCarModel").combobox("getValue");
    if ($.trim(carModelId) == "") {
        $.messager.alert('系统提示', '获取车型失败!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/CentralFee/QueryWaitPayDetail',
        data: "carModelId=" + carModelId + "&recordId=" + selectIORecordId + "&carModelId=" + carModelId + "",
        error: function () {
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                if (data.data.length > 0) {
                    $(".spanUnit").show();
                    $("#btnSubmitTollRelease").linkbutton("enable");
                    var d = data.data[0];
                    $("#hiddDetailRecordId").val(d.RecordID);
                    $("#spanEntranceTime").text(d.EntranceTime);
                    $("#spanPlateNumber").text(d.PlateNumber);
                    $("#spanOutTime").text(d.OutTime);
                    $("#spanTotalDuration").text(d.TotalDuration);
                    $("#spanTotalFee").text(d.TotalFee);
                    $("#spanPaySuccess").text(d.PaySuccess);
                    $("#spanWaitPay").text(d.WaitPay);
                    $("#spanDiscountAmount").text(d.DiscountAmount);
                    if (d.EntranceImageUrl != "") {
                        $("#bigImgEntrance").attr("src", d.EntranceImageUrl);
                    } else {
                        $("#bigImgEntrance").attr("src", "/Content/images/iorecord_default_not_image_big.jpg");
                    }
                    if (d.EntrancePlateImageUrl != "") {
                       // $("#divSmallImgEntrance").show();
                        //$("#smallImgEntrance").attr("src", d.EntrancePlateImageUrl);
                        $("#divSmallImgEntrance").hide();
                    } else {
                        $("#divSmallImgEntrance").hide();
                    }
                }
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });
}
function ClearIORecordDetial() {
    selectIORecordId = "";
    ResetIORecordDetial();
}
function ResetIORecordDetial() {
    $(".spanUnit").hide();
    $("#btnSubmitTollRelease").linkbutton("disable");
    $("#hiddDetailRecordId").val("");
    $("#spanEntranceTime").text("");
    $("#spanPlateNumber").text("");
    $("#spanOutTime").text("");
    $("#spanTotalDuration").text("");
    $("#spanTotalFee").text("");
    $("#spanPaySuccess").text("");
    $("#spanWaitPay").text("");
    $("#spanEntranceTime").text("");
    $("#spanDiscountAmount").text("");
    $("#bigImgEntrance").attr("src", "/Content/images/iorecord_not_select.jpg");
    $("#divSmallImgEntrance").hide();
    $("#smallImgEntrance").attr("src", "");
}
function btnTollRelease() {
    var recordId = $("#hiddDetailRecordId").val();
    if ($.trim(recordId) == "") {
        $.messager.alert('系统提示', "请选择入场记录", 'error');
        return;
    }
    var waitPay = $("#spanWaitPay").text().replace("元", "");
    if ($.trim(waitPay) == "") {
        $.messager.alert('系统提示', "获取待缴金额失败", 'error');
        return;
    }
    var mount = parseFloat(waitPay);
    if (mount < 0) {
        $.messager.alert('系统提示', "获取待缴金额失败", 'error');
        return;
    }
    if (mount == 0) {
        $.messager.alert('系统提示', "您本次还不需要缴费【如需重新计费请选择入场记录】", 'error');
        return;
    }
    var carModelId = $("#cmbCarModel").combobox("getValue");
    if ($.trim(carModelId) == "") {
        $.messager.alert('系统提示', '获取车型失败!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/CentralFee/TollRelease',
        data: "waitPay=" + waitPay + "&recordId=" + recordId + "&carModelId=" + carModelId + "",
        error: function () {
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                var success = "缴费成功，本次缴费金额 <span style=\"color: Red;font-weight: bold; font-size: 16px; \">" + waitPay + "<span> 元";
                $.messager.alert('系统提示', success, 'success');
                ResetIORecordDetial();
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });
}