
$(function () {
    $('#tableGateDevice').datagrid({
        url: '/p/ParkGate/GetParkGateDevicesData/',
        singleSelect: true,
        onSelect: function (rowIndex, rowData) {
            if (rowData.DeviceType == 12) {
                $("#btnupdatedeviceparam").show();
            } else {
                $("#btnupdatedeviceparam").hide();
            }
        },
        columns: [[
                    { field: 'DeviceID', title: 'DeviceID', width: 80, hidden: true },
                    { field: 'GateID', title: 'GateID', width: 80, hidden: true },
                    { field: 'PortType', title: 'PortType', width: 80, hidden: true },
                    { field: 'DeviceType', title: 'DeviceType', width: 80, hidden: true },
                    { field: 'DeviceTypeBK', title: 'DeviceTypeBK', width: 80, hidden: true },
                    { field: 'LedNum', title: 'LedNum', width: 80, hidden: true },
                    { field: 'DeviceNo', title: '设备编号', width: 80 },
                     { field: 'DeviceTypeDes', title: '设备类型', width: 80 },
                    { field: 'PortTypeDes', title: '通讯类型', width: 80 },
                    { field: 'Baudrate', title: '波特率', width: 50, formatter: function (value, row, index) {
                        if (row.PortType == 0) {
                            return value;
                        } else {
                            return '';
                        }
                    }
                    },
                    { field: 'SerialPort', title: '串口号', width: 50, formatter: function (value, row, index) {
                        if (row.PortType == 0) {
                            return value;
                        } else {
                            return '';
                        }
                    }
                    },
                    { field: 'IpAddr', title: 'IP地址', width: 90, formatter: function (value, row, index) {
                        if (row.PortType == 1) {
                            return value;
                        } else {
                            return '';
                        }
                    }
                    },
                    { field: 'IpPort', title: '端口号', width: 40, formatter: function (value, row, index) {
                        if (row.PortType == 1) {
                            return value;
                        } else {
                            return '';
                        }
                    }
                    }

                    ,
                     { field: 'OfflinePort', title: '脱机端口号', width: 80 },
                   
                    { field: 'NetID', title: '网络编号', width: 60, formatter: function (value, row, index) {
                        if (row.PortType == 0) {
                            return value;
                        } else {
                            return '';
                        }
                    }
                }, 
                    { field: 'IsCapture', title: '是否相机抓拍', width: 80, formatter: function (value, row, index) {
                        if (value) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    }
                    ,
                    { field: 'IsSVoice', title: '是否智能语音', width: 80, formatter: function (value, row, index) {
                        if (value) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                },
                { field: 'IsCarBit', title: '是否车位显示屏', width: 80, formatter: function (value, row, index) {
                    if (value) {
                        return '<img src="/Content/images/yes.png"/>';
                    } else {
                        return '<img src="/Content/images/no.png?v=1"/>';
                    }
                }
            }, { field: 'IsContestDev', title: '是否为控制器', width: 80, formatter: function (value, row, index) {
                if (value) {
                    return '<img src="/Content/images/yes.png"/>';
                } else {
                    return '<img src="/Content/images/no.png?v=1"/>';
                }
            }
        }, { field: 'IsMonitor', title: '是否为监控相机', width: 80, formatter: function (value, row, index) {
            if (value) {
                return '<img src="/Content/images/yes.png"/>';
            } else {
                return '<img src="/Content/images/no.png?v=1"/>';
            }
        }
        },
                 { field: 'UserName', title: '用户名', width: 70 },
                    { field: 'UserPwd', title: '密码', width: 70 }

				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableGateDevice').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkGate/GetParkDevicesOperatePurview', function (result) {
                    $('#tableGateDevice').datagrid("addToolbarItem", result);
                });
            }
        }

    });
    BindCombobox();
});

function BindCombobox() {
    $('#cmbDeviceType').combobox({
        url: '/p/ParkGate/GetParkGateDeviceType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            SetEditOption();
            SetDefaultParam();
        }
    });
    $('#cmbPortType').combobox({
        url: '/p/ParkGate/GetParkGateDevicePortType',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            SetEditOption();
            SetDefaultParam();
        }
    });
    $('#cmbDeviceTypeBK').combobox({
        url: '/p/ParkGate/GetParkDeviceTypeBK',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
        }
    });
}

function AddDevice() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请先选择通道!");
        return;
    }
    $('#divDeviceBoxForm').form('clear');
    SaveOrUpdateDevices("添加设备");
    $('#divDeviceBoxForm').form('load', {
        DeviceID: "",
        GateID: selectGate.GateID,
        NetID: "0",
        SerialPort: "COM1",
        Baudrate: "9600",
        ChannelID: "0",
        LedNum: "0",
        DisplayMode:"0",
        ControllerType: "0"
    });
    $('#cmbDeviceType').combobox("setValue", "0");
    $('#cmbPortType').combobox("setValue", "0");
    $("#chkIsCapture").prop("checked", false);
    $("#chkIsSVoice").prop("checked", false);
    $("#chkIsCarBit").prop("checked", false);
    $("#chkIsContestDev").prop("checked", false);
    $("#chkIsMonitor").prop("checked", false);
    
    SetEditOption();
    SetDefaultParam();
}

function UpdateDevice() {
    var selectDevice = $('#tableGateDevice').datagrid('getSelected');
    if (selectDevice == null) {
        $.messager.alert("系统提示", "请选择需要修改的设备!");
        return;
    }
    $('#divDeviceBoxForm').form('clear');
    SaveOrUpdateDevices("修改设备");

    $('#divDeviceBoxForm').form('load', {
        DeviceID: selectDevice.DeviceID,
        GateID: selectDevice.GateID,
        NetID: selectDevice.NetID,
        SerialPort: selectDevice.SerialPort,
        Baudrate: selectDevice.Baudrate,
        IpAddr: selectDevice.IpAddr,
        IpPort: selectDevice.IpPort,
        UserName: selectDevice.UserName,
        UserPwd: selectDevice.UserPwd,
        ChannelID: selectDevice.ChannelID,
        LedNum: selectDevice.LedNum,
        DeviceNo: selectDevice.DeviceNo,
        OfflinePort: selectDevice.OfflinePort,
        DisplayMode: selectDevice.DisplayMode,
        ControllerType:selectDevice.DeviceTypeBK
    });
    $('#cmbDeviceType').combobox("setValue", selectDevice.DeviceType);
    $('#cmbPortType').combobox("setValue", selectDevice.PortType);
    if (selectDevice.IsCapture) {
        $("#chkIsCapture").prop("checked", true);
    } else {
        $("#chkIsSVoice").prop("checked", false);
    }
    if (selectDevice.IsSVoice) {
        $("#chkIsSVoice").prop("checked", true);
    } else {
        $("#chkIsSVoice").prop("checked", false);
    }
    if (selectDevice.IsCarBit) {
        $("#chkIsCarBit").prop("checked", true);
    } else {
        $("#chkIsCarBit").prop("checked", false);
    }
    $("#chkIsMonitor").prop("checked", selectDevice.IsMonitor);
    SetEditOption();
    //SetDefaultParam();
    $("#chkIsContestDev").prop("checked", selectDevice.IsContestDev);
}
function DeleteDevice() {
    var selectDevice = $('#tableGateDevice').datagrid('getSelected');
    if (selectDevice == null) {
        $.messager.alert("系统提示", "请选择要删除的设备信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的设备吗?',
        function (r) {
            if (r) {
                $.post('/p/ParkGate/DeleteDevice?recordId=' + selectDevice.DeviceID,
                function (data) {
                    if (data.result) {
                        $.messager.show({
                            width: 200,
                            height: 100,
                            msg: '删除设备成功',
                            title: "删除设备"
                        });
                        RefreshDevice();
                    } else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }

                });
            }
        });
}
function RefreshDevice() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate != null) {
        $('#tableGateDevice').datagrid('load', { gateId: selectGate.GateID });
    }
}
//弹出编辑对话框
SaveOrUpdateDevices = function (data) {
    $('#divDeviceBox').show().dialog({
        title: data,
        width: 400,
        height: 430,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divDeviceBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!CheckSubmitData()) {
                    return;
                }
                if ($('#divDeviceBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkGate/SaveEditDevice',
                        data: $("#divDeviceBoxForm").serialize(),
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
                                $('#divDeviceBox').dialog('close');
                                RefreshDevice();

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
var ipReg = /^\d{3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/;
var ipInt = /^\d*$/;
function CheckSubmitData() {
    var deNo = $("#txtDeviceNo").val();
    if ($.trim(deNo) == "" || !$.trim(deNo).match(ipInt)) {
        $.messager.alert('系统提示', '设备编号必须为数字!', 'error');
        return false;
    }
    var type = $('#cmbPortType').combobox("getValue");
    if (type == "0") {
        var netid = $("#txtNetID").val();
        if ($.trim(netid) == "") {
            $.messager.alert('系统提示', '网络编号不能为空!', 'error');
            return false;
        }
    } else {
        var ipAddr = $("#txtIpAddr").val();
        if ($.trim(ipAddr) == "" || !$.trim(ipAddr).match(ipReg)) {
            $.messager.alert('系统提示', 'IP地址不能为空或格式不正确!', 'error');
            return false;
        }
        var ipPort = $("#txtIpPort").val();
        if ($.trim(ipPort) == "" || !$.trim(ipPort).match(ipInt)) {
            $.messager.alert('系统提示', '端口号不能为空!', 'error');
            return false;
        }
    }
    return true;
}
function SetEditOption() {
    $(".trall").hide();
    var type = $('#cmbPortType').combobox("getValue");
    if (type == "0") {
        $(".cklx").show();
    } else {
        $(".tcpiplx").show();
    }
}
function UpdateDeviceParam() {
   var selectDevice = $('#tableGateDevice').datagrid('getSelected');
    if (selectDevice == null) {
        $.messager.alert("系统提示", "请选择需要修改的设备!");
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/ParkGate/GetParkDeviceParam',
        data: 'deviceId=' + selectDevice.DeviceID,
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                if (data.data != null) {
                    $("#hiddParamDeviceID").val(data.data.DeviceID);
                    $("#hiddParamRecordID").val(data.data.RecordID);
                    $("#sltVipMode").val(data.data.VipMode);
                    $("#sltTempMode").val(data.data.TempMode);
                    $("#sltNetOffMode").val(data.data.NetOffMode);
                    $("#sltVipDevMultIn").val(data.data.VipDevMultIn);
                    $("#sltPloicFree").val(data.data.PloicFree);
                    $("#txtVipDutyDay").numberspinner("setValue",data.data.VipDutyDay);
                    $("#sltOverDutyYorN").val(data.data.OverDutyYorN);
                    $("#txtOverDutyDay").numberspinner("setValue", data.data.OverDutyDay);
                    $("#txtSysID").numberspinner("setValue",data.data.SysID);
                    $("#txtDevID").numberspinner("setValue",data.data.DevID);
                    $("#txtSysInDev").numberspinner("setValue", data.data.SysInDev);
                    $("#txtSysOutDev").numberspinner("setValue", data.data.SysOutDev);
                    $("#txtSysParkNumber").numberspinner("setValue", data.data.SysParkNumber);
                    $("#sltDevInorOut").val(data.data.DevInorOut);
                    $("#txtSwipeInterval").numberspinner("setValue", data.data.SwipeInterval);
                    $("#sltUnKonwCardType").val(data.data.UnKonwCardType);
                    $("#txtLEDNumber").numberspinner("setValue", data.data.LEDNumber);
                }
                SaveOrUpdateDeviceParam("修改设备参数");

            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
   
}
//弹出编辑对话框
SaveOrUpdateDeviceParam = function (data) {
    $('#divParkDeviceParamBox').show().dialog({
        title: data,
        width: 500,
        height: 430,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkDeviceParamBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divParkDeviceParamBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkGate/SaveParkDeviceParam',
                        data: $("#divParkDeviceParamBoxForm").serialize(),
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
                                $('#divParkDeviceParamBox').dialog('close');
                                RefreshDevice();

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
function SetDefaultParam() {
   // $("#spanIsContestDev").hide();
    var deviceType = $('#cmbDeviceType').combobox("getValue");
    switch (deviceType) {
        case "0":
            {
                $("#txtIpPort").val("37777");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "1":
            {
                $("#txtIpPort").val("8000");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "2":
            {
                $("#spanIsContestDev").show();
                $("#txtIpPort").val("80");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                break;
            }
        case "3":
            {
                $("#txtIpPort").val("0");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "4":
            {
                $("#txtIpPort").val("5000");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "6":
            {
                $("#txtIpPort").val("0");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "7":
            {
                $("#txtIpPort").val("50000");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "8":
            {
                $("#txtIpPort").val("7");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "9":
            {
                $("#txtIpPort").val("37777");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "10":
            {
                $("#txtIpPort").val("12345");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "11":
            {
                $("#txtIpPort").val("5000");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "12":
            {
                $("#txtIpPort").val("8888");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        case "14":
            {
                $("#txtIpPort").val("5000");
                $("#txtUserName").val("admin");
                $("#txtUserPwd").val("admin");
                $("#spanIsContestDev").show();
                break;
            }
        case "16":
            {
                $("#txtIpPort").val("50001");
                $("#txtUserName").val("");
                $("#txtUserPwd").val("");
                $("#chkIsContestDev").prop("checked", false);
                break;
            }
        default: { }
    }
}