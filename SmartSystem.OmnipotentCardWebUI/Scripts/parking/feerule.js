$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                InitDataCombobox(node.id);
                $('#tableFeeRule').datagrid('load', { parkingId: node.id });

            }
        }
    });

    BindDataTable();
    InitEditShowOption();
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
function BindDataTable() {
    $('#tableFeeRule').datagrid({
        url: '/p/ParkFeeRule/GetParkFeeRuleData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        columns: [[
                    { field: 'AreaID', title: 'AreaID', width: 80, hidden: true },
                    { field: 'ParkingID', title: 'ParkingID', width: 80, hidden: true },
                    { field: 'FeeRuleID', title: 'FeeRuleID', width: 80, hidden: true },
                    { field: 'FeeType', title: 'FeeType', width: 80, hidden: true },
                    { field: 'RuleName', title: '规则名称', width: 100 },
                    { field: 'FeeTypeName', title: '计费方式', width: 60 },
                    { field: 'AreaName', title: '应用区域', width: 100 },
                    { field: 'CarModelID', title: 'CarModelID', width: 80, hidden: true },
                    { field: 'CarModelName', title: '车型', width: 60 },
                    { field: 'CarTypeID', title: 'CarTypeID', width: 80, hidden: true },
                    { field: 'CarTypeName', title: '车类', width: 50 },
                    { field: 'LastUpdateTime', title: '最后修改时间', width: 120, formatter: function (value, row, index) {
                        return String.getjosntoDate(value) + " " + String.getJsonHourAndMinutes(value);
                    }
                },
                     { field: 'IsOffline', title: '是否脱机', width: 100, formatter: function (value, row, index) {
                         if (value) {
                             return '<img src="/Content/images/yes.png"/>';
                         } else {
                             return '<img src="/Content/images/no.png?v=1"/>';
                         }
                     }
                     },
                    { field: 'FirstRuleDetailDes', title: '规则描述', width: 300, formatter: function (value, row, index) {
                        if (row.FeeType == 5) {
                            return toTXT(value);
                        }
                        if (row.FeeType == 3) {
                            return value + "<br><br>" + row.LastRuleDetailDes;
                        }
                        return value;
                    }
                    },

                    { field: 'FirstRuleDetailID', title: 'FirstRuleDetailID', width: 80, hidden: true },
                    { field: 'FirstStartTime', title: 'FirstStartTime', width: 80, hidden: true },
                    { field: 'FirstEndTime', title: 'FirstEndTime', width: 80, hidden: true },
                    { field: 'FirstFirstFee', title: 'FirstFirstFee', width: 80, hidden: true },
                    { field: 'FirstFirstTime', title: 'FirstFirstTime', width: 80, hidden: true },
                    { field: 'FirstFreeTime', title: 'FirstFreeTime', width: 80, hidden: true },
                    { field: 'FirstLimit', title: 'FirstLimit', width: 80, hidden: true },
                    { field: 'FirstLoop1PerFee', title: 'FirstLoop1PerFee', width: 80, hidden: true },
                    { field: 'FirstLoop1PerTime', title: 'FirstLoop1PerTime', width: 80, hidden: true },
                    { field: 'FirstLoop2PerFee', title: 'FirstLoop2PerFee', width: 80, hidden: true },
                    { field: 'FirstLoop2PerTime', title: 'FirstLoop2PerTime', width: 80, hidden: true },
                    { field: 'FirstLoop2Start', title: 'FirstLoop2Start', width: 80, hidden: true },
                    { field: 'FirstLoopType', title: 'FirstLoopType', width: 80, hidden: true },
                    { field: 'FirstSupplement', title: 'FirstSupplement', width: 80, hidden: true },
                    { field: 'FirstFeeRuleTypes', title: 'FirstFeeRuleTypes', width: 80, hidden: true },

                    { field: 'LastRuleDetailID', title: 'LastRuleDetailID', width: 80, hidden: true },
                    { field: 'LastStartTime', title: 'LastStartTime', width: 80, hidden: true },
                    { field: 'LastEndTime', title: 'LastEndTime', width: 80, hidden: true },
                    { field: 'LastFirstFee', title: 'LastFirstFee', width: 80, hidden: true },
                    { field: 'LastFirstTime', title: 'LastFirstTime', width: 80, hidden: true },
                    { field: 'LastFreeTime', title: 'LastFreeTime', width: 80, hidden: true },
                    { field: 'LastLimit', title: 'LastLimit', width: 80, hidden: true },
                    { field: 'LastLoop1PerFee', title: 'LastLoop1PerFee', width: 80, hidden: true },
                    { field: 'LastLoop1PerTime', title: 'LastLoop1PerTime', width: 80, hidden: true },
                    { field: 'LastLoop2PerFee', title: 'LastLoop2PerFee', width: 80, hidden: true },
                    { field: 'LastLoop2PerTime', title: 'LastLoop2PerTime', width: 80, hidden: true },
                    { field: 'LastLoop2Start', title: 'LastLoop2Start', width: 80, hidden: true },
                    { field: 'LastLoopType', title: 'LastLoopType', width: 80, hidden: true },
                    { field: 'LastSupplement', title: 'LastSupplement', width: 80, hidden: true },
                    { field: 'LastFeeRuleTypes', title: 'LastFeeRuleTypes', width: 80, hidden: true }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableFeeRule').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkFeeRule/GetParkFeeRuleOperatePurview', function (result) {
                    $('#tableFeeRule').datagrid("addToolbarItem", result);
                });
            }
        }

    });
}
function InitEditShowOption() {
    $("#sltDayStartHour").change(function () {
        var v = $(this).val();
        SetLastEndHour(v);
    });

    $("#sltDayStartMinute").change(function () {
        var v = $(this).val();
        SetLastEndMinute(v);
    });

    $("#sltDayEndHour").change(function () {
        var v = $(this).val();
        SetLastStartHour(v);
    });

    $("#sltDayEndMinute").change(function () {
        var v = $(this).val();
        SetLastStartMinute(v);
    });

    $("#sltFirstSupplement").change(function () {
        var v = $(this).val();
        SetLastSupplement(v);
    });

    var starthour = $("#sltDayStartHour").val();
    SetLastEndHour(starthour);

    var startminute = $("#sltDayStartMinute").val();
    SetLastEndMinute(startminute);

    var endhour = $("#sltDayEndHour").val();
    SetLastStartHour(endhour);

    var endminute = $("#sltDayEndMinute").val();
    SetLastStartMinute(endminute);

    var supplement = $("#sltFirstSupplement").val();
    SetLastSupplement(supplement);

    ShowFirstContent();
}
function ShowFirstContent() {
    $("#myTab0").find(".active").removeClass("active").addClass("normal");
    $("#myTab0 li:eq(0)").removeClass("normal").addClass("active");
    $("[id^=mytab0_content]").hide();
    $("#mytab0_content0").show();
}
function InitDataCombobox(parkingId) {
    $('#cmbCarModelID').combobox({
        url: '/p/ParkFeeRule/GetParkCarModel',
        valueField: 'CarModelID',
        textField: 'CarModelName',
        queryParams: { parkingId: parkingId },
        onLoadSuccess: function () {
            var data = $(this).combobox('getData');
            if (data.length > 0) {
                $("#cmbCarModelID").combobox('setValue', data[0].CarModelID);
            }
        }
    });
    $('#cmbFeeType').combobox({
        url: '/p/ParkFeeRule/GetFeeType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            DefatulEditShowOption(record.EnumValue);
        },
        onLoadSuccess: function () {
            $("#cmbFeeType").combobox('setValue', "1");
            DefatulEditShowOption("1");
        }
    });
    $('#cmbCarTypeID').combobox({
        url: '/p/ParkFeeRule/GetParkCarType',
        valueField: 'CarTypeID',
        textField: 'CarTypeName',
        queryParams: { parkingId: parkingId },
        onLoadSuccess: function () {
            var data = $(this).combobox('getData');
            if (data.length > 0) {
                $("#cmbCarTypeID").combobox('setValue', data[0].CarTypeID);
            }
        },
        onSelect: function (record) {
        }
    });
    $('#cmbAreaID').combobox({
        url: '/p/ParkFeeRule/GetParkArea',
        valueField: 'AreaID',
        textField: 'AreaName',
        queryParams: { parkingId: parkingId },
        onLoadSuccess: function () {
            var data = $(this).combobox('getData');
            if (data.length > 0) {
                $("#cmbAreaID").combobox('setValue', data[0].AreaID);
            }
        }
    });
    $('#cmbFirstLoopType').combobox({
        url: '/p/ParkFeeRule/GetLoopType',
        valueField: 'EnumValue',
        textField: 'Description',
        onLoadSuccess: function () {
            $("#cmbFirstLoopType").combobox('setValue', "1");
        }
    });
    $('#cmbLastLoopType').combobox({
        url: '/p/ParkFeeRule/GetLoopType',
        valueField: 'EnumValue',
        textField: 'Description',
        onLoadSuccess: function () {
            $("#cmbLastLoopType").combobox('setValue', "1");
        }
    });
    $('#cmbFirstFeeRuleTypes').combobox({
        url: '/p/ParkFeeRule/GetFeeRuleType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            FirstFeeRuleTypesShow(record.EnumValue);
        },
        onLoadSuccess: function () {
            $("#cmbFirstFeeRuleTypes").combobox('setValue', "0");
            FirstFeeRuleTypesShow("0");
        }
    });
    $('#cmbLastFeeRuleTypes').combobox({
        url: '/p/ParkFeeRule/GetFeeRuleType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            LastFeeRuleTypesShow(record.EnumValue);
        },
        onLoadSuccess: function () {
            $("#cmbLastFeeRuleTypes").combobox('setValue', "0");
            LastFeeRuleTypesShow("0");
        }
    });
}
function btnSelectTabs(thisObj, Num) {
    if ($(thisObj).hasClass("active")) return;

    $("#myTab0").find(".active").removeClass("active").addClass("normal");
    $(thisObj).removeClass("normal").addClass("active");
    $("[id^=mytab0_content]").hide();
    $("#mytab0_content" + Num).show();
    if (Num == 1) {
        var lastfeeruletype = $("#cmbLastFeeRuleTypes").combobox('getValue');
        LastFeeRuleTypesShow(lastfeeruletype);
    }
}
function SetLastSupplement(value) {
    $("#sltLastSupplement").val(value);
}

function SetLastStartHour(value) {
    $("#sltLastStartHour").val(value);
}
function SetLastStartMinute(value) {
    $("#sltLastStartMinute").val(value);
}
function SetLastEndHour(value) {
    $("#sltLastEndHour").val(value);
}
function SetLastEndMinute(value) {
    $("#sltLastEndMinute").val(value);
}
function FirstFeeRuleTypesShow(value) {
    if (value == "0") {
        $(".FirstNumberOfTimes").show();
    } else {
        $(".FirstNumberOfTimes").hide();
    }
}
function LastFeeRuleTypesShow(value) {
    if (value == "0") {
        $(".LastNumberOfTimes").show();
    } else {
        $(".LastNumberOfTimes").hide();
    }
}
function DefatulEditShowOption(value) {
    value = value.toString();
    $(".tr_content0_other").show();
    $(".trDayAndLastTime").hide();
    $(".tr_content0_feerulefile").hide();
    switch (value) {
        case "1":
            {
                $("#myTab0 li").eq(0).show().text("24小时");
                $("#myTab0 li").eq(1).hide();
                break;
            }
        case "2":
            {
                $("#myTab0 li").eq(0).show().text("12小时");
                $("#myTab0 li").eq(1).hide();
                break;
            }
        case "3":
            {
                $(".trDayAndLastTime").show();
                $("#myTab0 li").eq(0).show().text("白天");
                $("#myTab0 li").eq(1).show().text("夜间");
                break;
            }
        case "4":
            {
                $("#myTab0 li").eq(0).show().text("自然天");
                $("#myTab0 li").eq(1).hide();
                break;
            }
        case "5":
            {
                $("#myTab0 li").eq(0).show().text("自定义");
                $("#myTab0 li").eq(1).hide();
                $(".tr_content0_other").hide();
                $(".tr_content0_feerulefile").show();
                break;
            }
    }
}
function Add() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $("#spanfeeruleresult").text("");
    AddOrUpdateBox("添加收费规则");
    $('#divFeeRuleBoxForm').form('load', {
        ParkingID: park.id,
        FeeRuleID: "",
        RuleName: "",
        RuleText: "",
        FirstLimit: "100",
        FirstFirstTime: "30",
        FirstFirstFee: "5",
        FirstLoop1PerTime: "60",
        FirstLoop1PerFee: "0",
        FirstLoop2Start: "0",
        FirstLoop2PerTime: "0",
        FirstLoop2PerFee: "0",
        FirstFreeTime: "30",
        LastLimit: "100",
        LastFirstTime: "30",
        LastFirstFee: "5",
        LastLoop1PerTime: "60",
        LastLoop1PerFee: "0",
        LastLoop2Start: "0",
        LastLoop2PerTime: "0",
        LastLoop2PerFee: "0",
        LastFreeTime: "30"
    })


    $('#sltFirstSupplement option:eq(0)').prop("selected", 'selected');
    $('#sltLastSupplement option:eq(0)').prop("selected", 'selected');

    $("#sltDayStartHour").val("07");
    $("#sltDayStartMinute").val("00");
    $("#sltDayEndHour").val("19");
    $("#sltDayEndMinute").val("00");

    $("#sltLastStartHour").val("19");
    $("#sltLastStartMinute").val("00");
    $("#sltLastEndHour").val("07");
    $("#sltLastEndMinute").val("00");
    $("#chkIsOffline").prop("checked", false);
    ShowFirstContent();
}
function Update() {
    var feeRule = $('#tableFeeRule').datagrid('getSelected');
    if (feeRule == null) {
        $.messager.alert("系统提示", "请选择需要修改的收费规则!");
        return;
    }
    AddOrUpdateBox("修改收费规则");

    $('#divFeeRuleBoxForm').form('load', {
        ParkingID: feeRule.ParkingID,
        feeRule: feeRule.feeRule,
        RuleText: feeRule.RuleText,
        FeeRuleID: feeRule.FeeRuleID,
        RuleName: feeRule.RuleName,
        FirstLimit: feeRule.FirstLimit,
        FirstFirstTime: feeRule.FirstFirstTime,
        FirstFirstFee: feeRule.FirstFirstFee,
        FirstLoop1PerTime: feeRule.FirstLoop1PerTime,
        FirstFirstTime: feeRule.FirstFirstTime,
        FirstLoop1PerFee: feeRule.FirstLoop1PerFee,
        FirstLoop2Start: feeRule.FirstLoop2Start,
        FirstLoop2PerTime: feeRule.FirstLoop2PerTime,
        FirstLoop2PerFee: feeRule.FirstLoop2PerFee,
        FirstFreeTime: feeRule.FirstFreeTime,
        LastLimit: feeRule.LastLimit,
        LastFirstTime: feeRule.LastFirstTime,
        LastFirstFee: feeRule.LastFirstFee,
        LastLoop1PerTime: feeRule.LastLoop1PerTime,
        LastFirstTime: feeRule.LastFirstTime,
        LastLoop1PerFee: feeRule.LastLoop1PerFee,
        LastLoop2Start: feeRule.LastLoop2Start,
        LastLoop2PerTime: feeRule.LastLoop2PerTime,
        LastLoop2PerFee: feeRule.LastLoop2PerFee,
        LastFreeTime: feeRule.LastFreeTime
    })
    $("#chkIsOffline").prop("checked", feeRule.IsOffline);
    $("#spanfeeruleresult").text("");
    $("#cmbFirstFeeRuleTypes").combobox('setValue', feeRule.FirstFeeRuleTypes);
    $("#cmbLastFeeRuleTypes").combobox('setValue', feeRule.LastFeeRuleTypes);
    FirstFeeRuleTypesShow(feeRule.FirstFeeRuleTypes);
    LastFeeRuleTypesShow(feeRule.LastFeeRuleTypes);

    $("#cmbCarModelID").combobox('setValue', feeRule.CarModelID);
    $("#cmbFeeType").combobox('setValue', feeRule.FeeType);
    $("#cmbCarTypeID").combobox('setValue', feeRule.CarTypeID);
    $("#cmbAreaID").combobox('setValue', feeRule.AreaID);

    $('#cmbFirstLoopType').val(feeRule.FirstLoopType);
    $('#sltFirstSupplement').val(feeRule.FirstSupplement);

    $("#sltDayStartHour").val("07");
    $("#sltDayStartMinute").val("00");
    $("#sltDayEndHour").val("19");
    $("#sltDayEndMinute").val("00");

    $("#sltNightStartHour").val("19");
    $("#sltNightStartMinute").val("00");
    $("#sltNightEndHour").val("07");
    $("#sltNightEndMinute").val("00");

    $('#cmbLastLoopType').val(feeRule.LastLoopType);
    $('#sltLastSupplement').val(feeRule.LastSupplement);

    if (feeRule.FeeType == 3) {

        if ($.trim(feeRule.FirstStartTime) != "" && feeRule.FirstStartTime.indexOf(':') > 0) {
            var start = feeRule.FirstStartTime.split(':');
            $("#sltDayStartHour").val(start[0]);
            $("#sltDayStartMinute").val(start[1]);
        }
        if ($.trim(feeRule.FirstEndTime) != "" && feeRule.FirstEndTime.indexOf(':') > 0) {
            var end = feeRule.FirstEndTime.split(':');
            $("#sltDayEndHour").val(end[0]);
            $("#sltDayEndMinute").val(end[1]);
        }

        if ($.trim(feeRule.LastStartTime) != "" && feeRule.LastStartTime.indexOf(':') > 0) {
            var start = feeRule.LastStartTime.split(':');
            $("#sltLastStartHour").val(start[0]);
            $("#sltLastStartMinute").val(start[1]);
        }
        if ($.trim(feeRule.LastEndTime) != "" && feeRule.LastEndTime.indexOf(':') > 0) {
            var end = feeRule.LastEndTime.split(':');
            $("#sltLastEndHour").val(end[0]);
            $("#sltLastEndMinute").val(end[1]);
        }
    }


    DefatulEditShowOption(feeRule.FeeType);
    ShowFirstContent();

}
function Delete() {
    var feeRule = $('#tableFeeRule').datagrid('getSelected');
    if (feeRule == null) {
        $.messager.alert("系统提示", "请选择需要删除的收费规则!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的收费规则吗?',
        function (r) {
            if (r) {
                $.post('/p/ParkFeeRule/Delete?feeRuleId=' + feeRule.FeeRuleID,
                function (data) {
                    if (data.result) {
                        $.messager.show({
                            width: 200,
                            height: 100,
                            msg: '删除收费规则成功',
                            title: "删除收费规则"
                        });
                        Refresh();
                    } else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }

                });
            }
        });
}
function TestFeeRule() {
    var feeRule = $('#tableFeeRule').datagrid('getSelected');
    if (feeRule == null) {
        $.messager.alert("系统提示", "请选择收费规则!");
        return;
    }
    $('#divCalculateFeeBoxForm').form('clear');
    TestFeeRuleBox("收费测试");
    $('#divCalculateFeeBoxForm').form('load', {
        FeeRuleID: feeRule.FeeRuleID
    });
}
function Refresh() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#tableFeeRule').datagrid('load', { parkingId: park.id });
}
function TestFeeRuleBox(data) {
    $('#divCalculateFeeBox').show().dialog({
        title: data,
        width: 350,
        height: 250,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divCalculateFeeBox').dialog('close');
                }
            }, {
                text: '收费测试',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divCalculateFeeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在计算....',
                            interval: 100
                        });
                        $.ajax({
                            type: "post",
                            url: '/p/ParkFeeRule/TestCalculateFee',
                            data: $("#divCalculateFeeBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {
                                if (data.result) {
                                    $.messager.progress("close");
                                    $("#txtCalculateFeeResult").val(data.data);

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
function AddOrUpdateBox(data) {
    $('#divFeeRuleBox').show().dialog({
        title: data,
        width: 600,
        height: 500,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divFeeRuleBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if (!CheckSubmitData()) {
                        return;
                    }
                    if ($('#divFeeRuleBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });
                        $.ajax({
                            type: "post",
                            url: '/p/ParkFeeRule/SaveEdit',
                            data: $("#divFeeRuleBoxForm").serialize(),
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
                                    $('#divFeeRuleBox').dialog('close');
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

    var carModelId = $("#cmbCarModelID").combobox('getValue');
    if ($.trim(carModelId) == "") {
        $.messager.alert('系统提示', '请选择车型!', 'error');
        return false;
    }
    var areaId = $("#cmbAreaID").combobox('getValue');
    if ($.trim(areaId) == "") {
        $.messager.alert('系统提示', '请选择停车区域!', 'error');
        return false;
    }
    var cartypeid = $("#cmbCarTypeID").combobox('getValue');
    if ($.trim(cartypeid) == "") {
        $.messager.alert('系统提示', '请选择车类!', 'error');
        return false;
    }
    var feeType = $("#cmbFeeType").combobox('getValue');
    if ($.trim(feeType) == "") {
        $.messager.alert('系统提示', '请选择车计费方式!', 'error');
        return false;
    }
    var firstfeeRuleType = $("#cmbFirstFeeRuleTypes").combobox('getValue');
    if (firstfeeRuleType == "0") {
        var firstloop1 = $("#txtFirstLoop1PerTime").val();
        if ($.trim(firstloop1) == "") {
            $.messager.alert('系统提示', '超过后多少分钟不能为空!', 'error');
            return false;
        }
        if (parseInt(firstloop1) <= 0) {
            $.messager.alert('系统提示', '超过后多少分钟必须大于0分钟!', 'error');
            return false;
        }
    }
    if (feeType == "3") {
        var FirstStartTime = $("#sltDayStartHour").val() + ":" + $("#sltDayStartMinute").val();
        $("#hiddFirstStartTime").val(FirstStartTime);
        var FirstEndTime = $("#sltDayEndHour").val() + ":" + $("#sltDayEndMinute").val();
        $("#hiddFirstEndTime").val(FirstEndTime);
        var LastStartTime = $("#sltLastStartHour").val() + ":" + $("#sltLastStartMinute").val();
        $("#hiddLastStartTime").val(LastStartTime);
        var LastEndTime = $("#sltLastEndHour").val() + ":" + $("#sltLastEndMinute").val();
        $("#hiddLastEndTime").val(LastEndTime);
        var lastfeeRuleType = $("#cmbLastFeeRuleTypes").combobox('getValue');
        if (lastfeeRuleType == "0") {
            var lastloop1 = $("#txtLastLoop1PerTime").val();
            if ($.trim(lastloop1) == "") {
                $.messager.alert('系统提示', '夜间超过后多少分钟不能为空!', 'error');
                return false;
            }
            if (parseInt(lastloop1) <= 0) {
                $.messager.alert('系统提示', '夜间超过后多少分钟必须大于0分钟!', 'error');
                return false;
            }
        }
    } else {
        $("#hiddFirstStartTime").val("");
        $("#hiddFirstEndTime").val("");
        $("#hiddLastStartTime").val("");
        $("#hiddLastEndTime").val("");
    }
    if (feeType == "5") {
        var ruleText = $("#hiddFeeRuleText").val();
        if ($.trim(ruleText) == "") {
            $.messager.alert('系统提示', '请上传收费规则文件!', 'error');
            return false;
        }
    }
    return true;
}
$(function () {
    $("#fileupload_feerule").fileupload({
        url: '/p/ParkFeeRule/PostFeeRuleFile',
        done: function (e, result) {
            if (result.result.result) {
                $("#hiddFeeRuleText").val(result.result.data);
                $("#spanfeeruleresult").text("上传成功");
            } else {
                $("#spanfeeruleresult").text(result.result.msg);
            }
        }
    })
});
function btnSelectFeeRuleFile() {
    $("#fileupload_feerule").click();
}
//Html结构转字符串形式显示 支持<br>换行
function ToHtmlString(htmlStr) {
    return toTXT(htmlStr).replace(/\&lt\;br[\&ensp\;|\&emsp\;]*[\/]?\&gt\;|\r\n|\n/g, "<br/>");
}
//Html结构转字符串形式显示
function toTXT(str) {
    var RexStr = /\<|\>|\"|\'|\&|　| /g
    str = str.replace(RexStr,
    function (MatchStr) {
        switch (MatchStr) {
            case "<":
                return "&lt;";
                break;
            case ">":
                return "&gt;";
                break;
            case "\"":
                return "&quot;";
                break;
            case "'":
                return "&#39;";
                break;
            case "&":
                return "&amp;";
                break;
            case " ":
                return "&ensp;";
                break;
            case "　":
                return "&emsp;";
                break;
            default:
                break;
        }
    }
    )
    return str;
}