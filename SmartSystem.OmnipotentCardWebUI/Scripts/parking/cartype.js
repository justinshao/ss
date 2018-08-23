
$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                $('#tableCarType').datagrid('load', { parkingId: node.id });
            }
        }
    });

    $("#ckbAffirmOut,#ckbAffirmIn").change(function () {
        SetAffirmInOutTime();

    });
//    $('#txtMaxMonth').numberspinner({
//        onChange: function (newValue, oldValue) {
//            BindOnlineUnitOption();
//        }
//    });
    BindDataTable();
    BindCombobox();
//    BindOnlineUnitOption();
});
//function BindOnlineUnitOption() {
//    $("#sltOnlineUnit option").remove();
//    var maxMonth = parseInt($('#txtMaxMonth').numberspinner("getValue"));
//    for (var i = 1; i <= maxMonth; i++) {
//        $("#sltOnlineUnit").append("<option value='"+i+"'>按"+i+"个月</option>");
//    }
//}
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
//增加
function Add() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    IsUpdateCarType = false;
    $('#divCarTypeBoxForm').form('clear');
    SaveOrEdit("增加车类");
    $('#divCarTypeBoxForm').form('load', {
        PKID: park.id,
        CarTypeID: "",
        CarTypeName: "",
        InOutTime: "30",
        BaseTypeID: 0,
        Deposit: 0,
        Amount: 300,
        MaxMonth: 12,
        LpDistinguish:3,
        MaxValue: 0,
        MaxUseMoney: 0,
        OverdueToTemp: "0",
        LotOccupy: "0",
        MonthCardExpiredEnterDay: "0",
        OnlineUnit:"1"
    })
    $("#cmbBaseTypeID").combobox('setValue',"2");
    CarTypeSelectItem("2");
    $("#ckbIsDispatch").prop("checked", false);

    $("#ckbIsIgnoreHZ").prop("checked", true);
    $("#hiddIsIgnoreHZ").val("true");
};

//修改
function Update() {
    var carType = $('#tableCarType').datagrid('getSelected');
    if (carType == null) {
        $.messager.alert("系统提示", "请选择需要修改的车辆类型!");
        return;
    }

    $('#divCarTypeBoxForm').form('clear');
    IsUpdateCarType = true;
    SaveOrEdit('修改车类');
    CarTypeSelectItem(carType.BaseTypeID);

    var inouttime = carType.InOutTime;
    if (inouttime == "-1") {
        inouttime = "";
    }
    var amount = carType.Amount;
    if (amount == "-1") {
        amount = "";
    }
    var maxmonth = carType.MaxMonth;
    if (maxmonth == "-1") {
        maxmonth = "";
    }
    var maxvalue = carType.MaxValue;
    if (maxvalue == "-1") {
        maxvalue = "";
    }
    var maxusemoney = carType.MaxUseMoney;
    if (maxusemoney == "-1") {
        maxusemoney = "";
    }
    var seasonamount = carType.Amount * 3;
    if (seasonamount == "-1") {
        seasonamount = "";
    }
    var maxseason = carType.MaxMonth / 3;
    if (maxseason == "-1") {
        maxseason = "";
    }
    var yearamount = carType.Amount * 12;
    if (yearamount == "-1") {
        yearamount = "";
    }
    var maxyear = carType.MaxMonth / 12;
    if (maxyear == "-1") {
        maxyear = "";
    }

    $('#divCarTypeBoxForm').form('load', {
        PKID: carType.PKID,
        CarTypeID: carType.CarTypeID,
        CarTypeName: carType.CarTypeName,
        LpDistinguish: carType.LpDistinguish,
        InOutTime: inouttime,
        BaseTypeID: carType.BaseTypeID,
        OverdueToTemp: carType.OverdueToTemp,
        LotOccupy: carType.LotOccupy,
        Deposit: carType.Deposit,
        Amount: amount,
        SeasonAmount: amount * 3,
        YearAmount: amount * 12,
        MaxMonth: maxmonth,
        MaxSeason: maxmonth / 3,
        MaxYear: maxmonth / 12,
        AveMonth: carType.OnlineUnit,
        MaxValue: maxvalue,
        MaxUseMoney: maxusemoney,
        MonthCardExpiredEnterDay: carType.MonthCardExpiredEnterDay,
        OnlineUnit: carType.OnlineUnit
    });
   
    if (carType.RepeatIn) {
        $("#ckbRepeatIn").prop("checked", true);
    } else {
        $("#ckbRepeatIn").prop("checked", false);
    }

    if (carType.RepeatOut) {
        $("#ckbRepeatOut").prop("checked", true);
    } else {
        $("#ckbRepeatOut").prop("checked", false);
    }

    if (carType.AffirmIn) {
        $("#ckbAffirmIn").prop("checked", true);
    } else {
        $("#ckbAffirmIn").prop("checked", false);
    }

    if (carType.AffirmOut) {
        $("#ckbAffirmOut").prop("checked", true);
    } else {
        $("#ckbAffirmOut").prop("checked", false);
    }
    if (carType.AllowLose) {
        $("#ckbAllowLose").prop("checked", true);
    } else {
        $("#ckbAllowLose").prop("checked", false);
    }

    if (carType.InOutEditCar) {
        $("#ckbInOutEditCar").prop("checked", true);
    } else {
        $("#ckbInOutEditCar").prop("checked", false);
    }

    if (carType.CarNoLike) {
        $("#ckbCarNoLike").prop("checked", true);
    } else {
        $("#ckbCarNoLike").prop("checked", false);
    }
    if (carType.IsAllowOnlIne) {
        $("#ckbIsAllowOnlIne").prop("checked", true);
    } else {
        $("#ckbIsAllowOnlIne").prop("checked", false);
    }
    $("#ckbIsDispatch").prop('checked', carType.IsDispatch);

    if (carType.IsIgnoreHZ) {
        $("#ckbIsIgnoreHZ").prop('checked',true);
        $("#hiddIsIgnoreHZ").val("true");
    } else {
        $("#ckbIsIgnoreHZ").prop("checked", false);
        $("#hiddIsIgnoreHZ").val("false");
    }
   
    SetAffirmInOutTime();
    OverdueDaysSet();
};


//刷新车辆类型
function Refresh() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#tableCarType').datagrid('load', { parkingId: park.id });
};

function Deletetype() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    } 
    var carType = $('#tableCarType').datagrid('getSelected');
    if (carType == null) {
        $.messager.alert("系统提示", "请选择需要修改的车辆类型!");
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/ParkCarType/Deletetype?recordId='+ carType.CarTypeID,
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) { 
            if (data.result) {
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '数据删除成功!',
                    title: "删除车类型"
                });
                $.messager.progress("close");
                Refresh();
                //$('#divCarTypeBox').dialog('close');
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}

var IsUpdateCarType = false;
function CarTypeSelectItem(carType) {
    $(".trItem").hide();
    switch (parseInt(carType)) {
        case 0:
            {
                //贵宾卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", false);

                $(".overduetotemp").show();
                $(".lotoccupy").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 390 });
                break;
            }
        case 1:
            {
                //储值卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $(".maxmalue").show();
//                $(".maxusemoney").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 390 });

                $("#txtMaxValue").numberspinner("setValue", "2000");
                $("#txtMaxUseMoney").numberspinner("setValue", "0");
                break;
            }
        case 2:
            {
                //月卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $(".amount").show();
                $(".maxmonth").show();
                $(".overduetotemp").show();
                $(".lotoccupy").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 480 });

                var overdueToTemp = $("#cmbOverdueToTemp").combobox('getValue');
                if (overdueToTemp == "0") {
                    $("#trOverdueDays").show();
                } else {
                    $("#trOverdueDays").hide();
                }

                $("#txttbAmount").numberspinner("setValue", "300");
                //$("#txtMaxMonth").numberspinner("setValue", "12");

                break;
            }
        case 3:
            {
                //临时卡
                $("#ckbRepeatIn").prop("checked", false);
                $("#ckbRepeatOut").prop("checked", false);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", true);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked",true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $("#trOverdueDays").hide();
//                $(".maxusemoney").show();
                $('#divCarTypeBox').dialog({ height: 360 });

                $("#txtMaxUseMoney").numberspinner("setValue", "0");
                break;
            }
        case 4:
            {
                //工作卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", true);
                $("#ckbAffirmOut").prop("checked", true);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", false);

                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 330 });
                break;
            }
        case 5:
            {
                //季卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $(".seasonamount").show();
                $(".maxseason").show();
                $(".overduetotemp").show();
                $(".lotoccupy").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 480 });

                var overdueToTemp = $("#cmbOverdueToTemp").combobox('getValue');
                if (overdueToTemp == "0") {
                    $("#trOverdueDays").show();
                } else {
                    $("#trOverdueDays").hide();
                }

                $("#txtSeason").numberspinner("setValue", "900");
                $("#txtMaxSeason").numberspinner("setValue", "4");

                break;
            }
        case 6:
            {
                //年卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $(".yearamount").show();
                $(".maxyear").show();
                $(".overduetotemp").show();
                $(".lotoccupy").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 480 });

                var overdueToTemp = $("#cmbOverdueToTemp").combobox('getValue');
                if (overdueToTemp == "0") {
                    $("#trOverdueDays").show();
                } else {
                    $("#trOverdueDays").hide();
                }

                $("#txtYear").numberspinner("setValue", "3600");
                $("#txtMaxYear").numberspinner("setValue", "1");

                break;
            }
        case 7:
            {
                //自定义月卡
                $("#ckbRepeatIn").prop("checked", true);
                $("#ckbRepeatOut").prop("checked", true);
                $("#ckbAffirmIn").prop("checked", false);
                $("#ckbAffirmOut").prop("checked", false);
                $("#ckbAllowLose").prop("checked", false);
                $("#ckbInOutEditCar").prop("checked", true);
                $("#ckbCarNoLike").prop("checked", false);
                $("#ckbIsAllowOnlIne").prop("checked", true);

                $(".custom").show();
                $(".avecustom").show();
                $(".maxcustom").show();
                $(".overduetotemp").show();
                $(".lotoccupy").show();
                $("#trOverdueDays").hide();
                $('#divCarTypeBox').dialog({ height: 480 });

                var overdueToTemp = $("#cmbOverdueToTemp").combobox('getValue');
                if (overdueToTemp == "0") {
                    $("#trOverdueDays").show();
                } else {
                    $("#trOverdueDays").hide();
                }

                $("#txtAmount").numberspinner("setValue", "300");
                $("#txtMaxMonth").numberspinner("setValue", "");

                break;
            }
    }
    SetAffirmInOutTime();
}

//弹出编辑对话框
function SaveOrEdit(data) {
    $('#divCarTypeBox').show().dialog({
        title: data,
        width: 460,
        height: 520,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divCarTypeBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if (!CheckSubmitData()) {
                        return;
                    }
                    if ($('#divCarTypeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });
                        $.ajax({
                            type: "post",
                            url: '/p/ParkCarType/SaveUpdate',
                            data: $("#divCarTypeBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {

                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '数据保存成功!',
                                        title: "保存车类型"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divCarTypeBox').dialog('close');
                                } else {
                                    $.messager.progress("close");
                                    $.messager.alert('系统提示', data.msg, 'error');

                                }
                            }
                        });
                    }
                }
            }]

    });
}
function CheckSubmitData() {
    var carTypeName = $("#txtCarTypeName").val();
    if ($.trim(carTypeName) == "") {
        $.messager.alert('系统提示', '车类名称不能为空！', 'error');
        return false;
    }
    var inOutTime = $("#txtInOutTime").val();
    if ($.trim(inOutTime) == "") {
        $.messager.alert('系统提示', '进出间隔不能为空或格式不正确！', 'error');
        return false;
    }

    var defaultCarType = $("#cmbBaseTypeID").combobox('getValue');
    if (defaultCarType == "1") {
        var maxvalue = $("#txtMaxValue").val();
        if ($.trim(maxvalue) == "" ) {
            $.messager.alert('系统提示', '线上充值最大金额不能为空或格式不正确！', 'error');
            return false;
        }

        var maxusemoney = $("#txtMaxUseMoney").val();
        if ($.trim(maxusemoney) == "" ) {
            $.messager.alert('系统提示', '每天最大额不能为空或格式不正确！', 'error');
            return false;
        }
    }

    if (defaultCarType == "2") {
        var amount = $("#txtAmount").val();
        if ($.trim(amount) == "" ) {
            $.messager.alert('系统提示', '续费月金额不能为空或格式不正确！', 'error');
            return false;
        }
//        var maxmonth = $("#txtMaxMonth").val();
//        if ($.trim(maxmonth) == "" ) {
//            $.messager.alert('系统提示', '线上续期最大月数不能为空或格式不正确！', 'error');
//            return false;
//        }
//        if (parseInt(maxmonth) < 1 || parseInt(maxmonth) > 12) {
//            $.messager.alert('系统提示', '线上续期最大月数必须在1-12之间！', 'error');
//            return false;
//        }
    }
    if (defaultCarType == "3") {
        var maxusemoney = $("#txtMaxUseMoney").val();
        if ($.trim(maxusemoney) == "" ) {
            $.messager.alert('系统提示', '每天最大额不能为空或格式不正确！', 'error');
            return false;
        }
    }
    if (defaultCarType == "5") {
        var amount = $("#txtSeason").val();
        if ($.trim(amount) == "") {
            $.messager.alert('系统提示', '续费季金额不能为空或格式不正确！', 'error');
            return false;
        }
    }
    if (defaultCarType == "6") {
        var amount = $("#txtYear").val();
        if ($.trim(amount) == "") {
            $.messager.alert('系统提示', '续费年金额不能为空或格式不正确！', 'error');
            return false;
        }
    }
    if (defaultCarType == "7") {
        var amount = $("#txtAmount").val();
        if ($.trim(amount) == "") {
            $.messager.alert('系统提示', '续费月金额不能为空或格式不正确！', 'error');
            return false;
        }
        var avemonth = $("#txtAveMonth").val();
        if ($.trim(avemonth) == "") {
            $.messager.alert('系统提示', '线上续期按月数不能为空或格式不正确！', 'error');
            return false;
        }
        var maxmonth = $("#txtMaxMonth").val();
        if ($.trim(maxmonth) == "") {
            $.messager.alert('系统提示', '线上续期最大月数不能为空或格式不正确！', 'error');
            return false;
        }
    }
    if ($("#ckbAffirmIn,#ckbAffirmOut").is(":checked")) {
        var start = $("#sltStartHH").val() + ":" + $("#sltStartMM").val();
        var end = $("#sltEndHH").val() + ":" + $("#sltEndMM").val();
        $("#hiddUpdateAffirmIn").val(start);
        $("#hiddUpdateAffirmOut").val(end);
    } else {
        $("#hiddUpdateAffirmIn").val("00:00");
        $("#hiddUpdateAffirmOut").val("23:59");
    }
    if ($("#ckbIsIgnoreHZ").is(":checked")) {
        $("#hiddIsIgnoreHZ").val("true");
    } else {
        $("#hiddIsIgnoreHZ").val("false");
    }
    return true;

}
function BindCombobox() {
    $('#cmbBaseTypeID').combobox({
        url: '/p/ParkCarType/GetCarTypeBaseCarType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            if (IsUpdateCarType) {
                var selectRow = $('#tableCarType').datagrid('getSelected');
                if (selectRow != null && record.EnumValue.toString() == selectRow.BaseTypeID.toString()) {
                    Update();
                } else {
                    CarTypeSelectItem(record.EnumValue);
                }

            } else {
                CarTypeSelectItem(record.EnumValue);
            }
        },
        onLoadSuccess: function () {
            $("#cmbBaseTypeID").combobox('setValue', "2");
            //CarTypeSelectItem("2");
        }
    });
    $('#cmbLpDistinguish').combobox({
        url: '/p/ParkCarType/GetLpDistinguish',
        valueField: 'EnumValue',
        textField: 'Description',
        onLoadSuccess: function () {
            $("#cmbLpDistinguish").combobox('setValue', "3");
        }
    });
    $('#cmbOverdueToTemp').combobox({
        url: '/p/ParkCarType/GetOverdueToTemp',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            var carType = $("#cmbBaseTypeID").combobox("getValue");
            CarTypeSelectItem(carType);
        },
        onLoadSuccess: function () {
            $("#cmbOverdueToTemp").combobox('setValue', "0");
        }
    });
    $('#cmbLotOccupy').combobox({
        url: '/p/ParkCarType/GetLotOccupy',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
        }
    });
}
function OverdueDaysSet() {
    var type = $("#cmbBaseTypeID").combobox('getValue');
    var overdueToTemp = $("#cmbOverdueToTemp").combobox('getValue');
    if (type == "2" && overdueToTemp == "0") {
        $("#trOverdueDays").show();
    } else {
        $("#trOverdueDays").hide();
    }
}
function SetAffirmInOutTime() {
    var start = "00:00";
    var end = "23:59";
    var SelectRow = $('#tableCarType').datagrid('getSelected');
    if (SelectRow != null) {
        start = SelectRow.AffirmBegin; end = SelectRow.AffirmEnd
    }
    if ($("#ckbAffirmIn,#ckbAffirmOut").is(":checked")) {
        $("#trAffirmInOutTime").show();
    } else {
        $("#trAffirmInOutTime").hide();
    }
    if ($.trim(start) != "") {
        var times = start.split(':');
        if (times.length == 2) {
            $("#sltStartHH").val(times[0]);
            $("#sltStartMM").val(times[1]);
        }
    }
    if ($.trim(end) != "") {
        var times = end.split(':');
        if (times.length == 2) {
            $("#sltEndHH").val(times[0]);
            $("#sltEndMM").val(times[1]);
        }
    }
}
function BindDataTable() {
    $('#tableCarType').datagrid({
        url: '/p/ParkCarType/GetParkCarTypeData/',
        singleSelect: true,
        columns: [[
                    { field: 'BaseTypeID', title: 'BaseTypeID', width: 100, hidden: true },
                    { field: 'CarTypeID', title: 'CarTypeID', width: 100, hidden: true },
                    { field: 'InBeginTime', title: 'InBeginTime', width: 100, hidden: true },
                    { field: 'InEdnTime', title: 'InEdnTime', width: 100, hidden: true },
                    { field: 'AffirmBegin', title: 'AffirmBegin', width: 100, hidden: true },
                    { field: 'AffirmEnd', title: 'AffirmEnd', width: 100, hidden: true },
                    { field: 'PKID', title: 'PKID', width: 100, hidden: true },
                    { field: 'IsDispatch', title: 'IsDispatch', width: 100, hidden: true },
                    { field: 'IsIgnoreHZ', title: 'IsIgnoreHZ', width: 100, hidden: true },
                    { field: 'CarTypeName', title: '车类名称', width: 80 },
                    { field: 'ParkName', title: '停车场', width: 100 },
                    { field: 'BaseTypeDes', title: '基础类型', width: 80 },
                    { field: 'LpDistinguish', title: 'LpDistinguish', width: 100, hidden: true },
                    { field: 'LpDistinguishDes', title: '识别类型', width: 80, width: 80 },
                    { field: 'InOutTime', title: '进出最小间隔', width: 80 },
                    { field: 'RepeatIn', title: '重复入场', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'RepeatOut', title: '重复出场', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'AffirmIn', title: '进场确认', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'AffirmOut', title: '出场确认', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },

                    { field: 'AllowLose', title: '满位可进', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'InOutEditCar', title: '进出修改车位', width: 80, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },

                    { field: 'CarNoLike', title: '模糊识别', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                },
               { field: 'IsAllowOnlIne', title: '允许线上缴费', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                },
                    
                     { field: 'MaxUseMoney', title: '每天最大金额', hidden: true, width: 80, formatter: function (value, row, index) {
                         if (value == -1) {
                             return '';
                         } else {
                             return value;
                         }
                     }
                     },
                     { field: 'Amount', title: '续费月金额', width: 80, formatter: function (value, row, index) {
                         if (value == -1) {
                             return '';
                         } else {
                             return value;
                         }
                     }
                     },
                     { field: 'MaxMonth', title: '线上续期最大月数', width: 100, formatter: function (value, row, index) {
                         if (value == -1) {
                             return '';
                         } else {
                             return value;
                         }
                     }
                 },
                       //{ field: 'OnlineUnit', title: '线上续期按月数', width: 80, formatter: function (value, row, index) {
                       //    if (value == -1) {
                       //        return '';
                       //    } else {
                       //        return value+"个月";
                       //    }
                       //}
                       //},
                     { field: 'OverdueToTemp', title: 'OverdueToTemp', width: 100, hidden: true },
                     { field: 'OverdueToTempDes', title: '过期', width: 80, width: 80 },
                        { field: 'MonthCardExpiredEnterDay', title: '可进入天数', width: 80, width: 80, formatter: function (value, row, index) {
                            if (row.BaseTypeID == "2") {
                                if (value == "0") {
                                    return '不延期';
                                } else {
                                    return value + '天';
                                }
                            } else {
                                return "";
                            }

                        }
                        },
                         { field: 'LotOccupy', title: 'LotOccupy', width: 100, hidden: true },
                     { field: 'LotOccupyDes', title: '车位占用', width: 80, width: 80 },

				]],
        onBeforeLoad: function (param) {
            var dpanel = $('#tableCarType').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkCarType/GetParkCarTypeOperatePurview', function (result) {
                    $('#tableCarType').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}
