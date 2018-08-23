$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                $('#tableCarModel').datagrid('load', { parkingId: node.id });
            }
        }
    });
    $('#tableCarModel').datagrid({
        url: '/p/ParkCarModel/GetCarModelData/',
        singleSelect: true,
        columns: [[
                    { field: 'CarModelID', title: 'CarModelID', width: 100, hidden: true },
                    { field: 'DayMaxMoney', title: 'DayMaxMoney', width: 100, hidden: true },
                    { field: 'NightMaxMoney', title: 'NightMaxMoney', width: 100, hidden: true },
                    { field: 'NaturalTime', title: 'NaturalTime', width: 100, hidden: true },
                    { field: 'DayStartTime', title: 'DayStartTime', width: 100, hidden: true },
                    { field: 'DayEndTime', title: 'DayEndTime', width: 100, hidden: true },
                    { field: 'NightStartTime', title: 'NightStartTime', width: 100, hidden: true },
                    { field: 'NightEndTime', title: 'NightEndTime', width: 100, hidden: true },
                    { field: 'CarModelName', title: '车型名称', width: 150 },
                    { field: 'PlateColor', title: '相机车型', width: 100, formatter: function (value, row, index) {
                        if (value == "0") {
                            return '无';
                        }
                        if (value == "1") {
                            return '小车';
                        }
                        if (value == "2") {
                            return '大车';
                        }
                        return "";
                    }
                    },
                    { field: 'IsDefault', title: '是否为默认类型', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    }, { field: 'IsNaturalDay', title: '是否为自然天', width: 100, formatter: function (value, row, index) {
                        if (value) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'MaxUseMoney', title: '每天最大金额', width: 100 },
                    { field: 'DayDes', title: '白天收费描述', width: 160 },
                    { field: 'NightDes', title: '夜晚收费描述', width: 160 },
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableCarModel').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkCarModel/GetParkCarModelOperatePurview', function (result) {
                    $('#tableCarModel').datagrid("addToolbarItem", result);
                });
            }
        }

    });
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
//增加车辆类型
function Add() {
    $('#divCarModelBoxForm').form('clear');

    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#tableCarModel').datagrid('load', { parkingId: park.id });

    SaveOrUpdate("增加车型");
    $('#divCarModelBoxForm').form('load', {
        CarTypeID: "",
        PKID: park.id,
        MaxUseMoney: 0,
        CarModelName: "",
        PlateColor: "0",
        DayMaxMoney: 0,
        NightMaxMoney: 0,
        DayStartTime:"00:00",
        DayEndTime: "00:00",
        NightStartTime: "00:00",
        NightEndTime: "00:00",
        NaturalTime:0
    })
    $("#IsDefaultSelected").attr("checked", false);
    $("#IsNaturalDaySelected").prop("checked", false);

};

//修改车辆类型
function Update() {
    var carModel = $('#tableCarModel').datagrid('getSelected');
    if (carModel == null) {
        $.messager.alert("系统提示", "请选择需要修改的车型!");
        return;
    }

    $('#divCarModelBoxForm').form('clear');
    SaveOrUpdate('修改车型');
    $('#divCarModelBoxForm').form('load', {
        CarModelID: carModel.CarModelID,
        PKID: carModel.PKID,
        MaxUseMoney: carModel.MaxUseMoney,
        CarModelName: carModel.CarModelName,
        PlateColor: carModel.PlateColor,
        DayMaxMoney: carModel.DayMaxMoney,
        NightMaxMoney: carModel.NightMaxMoney,
        DayStartTime: carModel.DayStartTime,
        DayEndTime: carModel.DayEndTime,
        NightStartTime: carModel.NightStartTime,
        NightEndTime: carModel.NightEndTime,
        NaturalTime: carModel.NaturalTime
    });
    if (carModel.PlateColor != "0" && carModel.PlateColor != "1" && carModel.PlateColor != "2") {
        $("#sltPlateColor").val("0");
    }
    if (carModel.IsDefault) {
        $("#IsDefaultSelected").prop("checked", true);
    } else {
        $("#IsDefaultSelected").prop("checked", false);
    }
    if (carModel.IsNaturalDay) {
        $("#IsNaturalDaySelected").prop("checked", true);
    } else {
        $("#IsNaturalDaySelected").prop("checked", false);
    }
};


//刷新车辆类型
function Refresh() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#tableCarModel').datagrid('load', { parkingId: park.id });
};

//弹出编辑对话框
SaveOrUpdate = function (data) {
    $('#divCarModelBox').show().dialog({
        title: data,
        width: 500,
        height: 400,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divCarModelBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divCarModelBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkCarModel/SaveEdit',
                            data: $("#divCarModelBoxForm").serialize(),
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
                                        title: "保存车型"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divCarModelBox').dialog('close');
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