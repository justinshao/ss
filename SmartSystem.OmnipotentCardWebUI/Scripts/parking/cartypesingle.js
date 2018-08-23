function AddCarTypeSingle() {
    var SelectRow = $('#tableCarType').datagrid('getSelected');
    if (SelectRow == null) {
        $.messager.alert("系统提示", "请选择车类!");
        return;
    }
    $('#divParkCarTypeSingleBox').show().dialog({
        title: "单双车牌配置",
        width: 450,
        height: 350,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false
    });
    GetCarTypeSingleList(SelectRow.CarTypeID);
}
function btnSubmitCarTypeSingle() {
    var carTypeId = $("#hiddCarTypeIdSingle").val();
    if ($.trim(carTypeId) == "") {
        $.messager.alert('系统提示', '获取车类失败!', 'error');
        return;
    }
    var singleId = $("#hiddSingleID").val();
    if ($.trim(singleId) == "") {
        $.messager.alert('系统提示', '获取编号失败!', 'error');
        return;
    }
    var singleType = $("#sltSingleType").val();
    if ($.trim(singleType) == "") {
        $.messager.alert('系统提示', '请选择配置类型!', 'error');
        return;
    }
    var week = $("#hiddWeek").val();
    if ($.trim(week) == "") {
        $.messager.alert('系统提示', '请选择需要修改的配置!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/ParkCarType/SaveCarTypeSingle',
        data: "SingleID=" + singleId + "&CarTypeID=" + carTypeId + "&SingleType=" + singleType + "&Week=" + week + "",
        error: function () { $.messager.alert('系统提示', '提交数据失败!', 'error'); },
        success: function (data) {
            if (data != null && data.result) {
                $("#hiddSingleID").val("");
                $("#hiddWeek").val("");
                $("#spanWeek").text("");
                GetCarTypeSingleList(carTypeId);
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '数据保存成功!',
                    title: "系统提示"
                });
            } else {
                $.messager.alert('系统提示', data, 'error');
            }
        }
    });
}
function GetCarTypeSingleList(carTypeId) {
    $("#tableCarTypeSingle tbody tr").remove();
    $.ajax({
        type: "post",
        url: '/p/ParkCarType/GetCarTypeSingle',
        data: "carTypeId=" + carTypeId,
        error: function () { $.messager.alert('系统提示', '查询数据信息失败!', 'error'); },
        success: function (data) {
            if (data != null && data.result) {
                for (var i = 0; i < data.data.length; i++) {
                    AddCarTypeSingleTr(data.data[i].SingleID, data.data[i].CarTypeID, data.data[i].Week.toString(), data.data[i].SingleType.toString());
                }

            }
        }
    });
}

function AddCarTypeSingleTr(SingleID, CarTypeID, Week, SingleType) {
    var strSingleTypeDes = "";
    if (SingleType == "0") {
        strSingleTypeDes = "单双可进出";
    }
    if (SingleType == "1") {
        strSingleTypeDes = "单可进出";
    }
    if (SingleType == "2") {
        strSingleTypeDes = "双可进出";
    }
    var strWeekDes = "";
    if (Week == "1") {
        strWeekDes = "星期一";
    }
    if (Week == "2") {
        strWeekDes = "星期二";
    }
    if (Week == "3") {
        strWeekDes = "星期三";
    }
    if (Week == "4") {
        strWeekDes = "星期四";
    }
    if (Week == "5") {
        strWeekDes = "星期五";
    }
    if (Week == "6") {
        strWeekDes = "星期六";
    }
    if (Week == "7") {
        strWeekDes = "星期七";
    }
    var trhtml = $('<tr><td style="width:40%"><span id="spanWeek">' + strWeekDes + '</span></td><td style="width:20%"><span>' + strSingleTypeDes + '</span></td><td style="width:20%"><a href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:\'icon-cancel\'" id="btnUpdateCarTypeSingle" onclick="return UpdateCarTypeSingle(this,' + "'" + strWeekDes + "'" + ',' + "'" + SingleType + "'" + ',' + "'" + SingleID + "'" + ',' + "'" + CarTypeID + "'" + ',' + "'" + Week + "'" + ')">修改</a></td></tr>');
    $("#tableCarTypeSingle tbody").append(trhtml);
}
function UpdateCarTypeSingle(obj, strWeekDes, SingleType, SingleID, CarTypeID, Week) {
    $("#hiddCarTypeIdSingle").val(CarTypeID);
    $("#hiddSingleID").val(SingleID);
    $("#sltSingleType").val(SingleType);
    $("#spanWeek").text(strWeekDes);
    $("#hiddWeek").val(Week);
}