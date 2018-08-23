$(function () {
    $('#parkSellerTree').tree({
        url: '/ParkSellerData/GetSellerTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                LoadParkQRCodeDerate(node.id);
                BindParkDerate(node.id);
                BindParking(node.id);
            }
        }
    });

    $('#tableQRCodeDerate').datagrid({
        url: '/p/ParkCarDerateQRCode/GetParkDerateQRcodeData/',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 100, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 100, hidden: true },
                    { field: 'VID', title: 'VID', width: 100, hidden: true },
                    { field: 'DerateID', title: 'DerateID', width: 100, hidden: true },
                    { field: 'PKID', title: 'PKID', width: 100, hidden: true },
                    { field: 'StartTime', title: 'StartTime', width: 100, hidden: true },
                    { field: 'EndTime', title: 'EndTime', width: 100, hidden: true },
                    { field: 'DerateSwparate', title: 'DerateSwparate', width: 100, hidden: true },
                    { field: 'DerateMoney', title: 'DerateMoney', width: 100, hidden: true },
                    { field: 'CanUseTimes', title: 'CanUseTimes', width: 100, hidden: true },
                    { field: 'DerateQRcodeZipFilePath', title: 'DerateQRcodeZipFilePath', width: 100, hidden: true },
                    { field: 'AlreadyUseTimes', title: 'AlreadyUseTimes', width: 100, hidden: true },
                    { field: 'SellerName', title: '商家名称', width: 100 },
                    { field: 'DerateName', title: '优免规则', width: 100 },
                    { field: 'DerateValue', title: '优免值', width: 60 },
                    { field: 'UseTimesDes', title: '发放张数/已结算张数', width: 150 },
                    { field: 'PKName', title: '所属车场', width: 100 },
                    { field: 'EndTimeToString', title: '券有效期至', width: 140 },
                    { field: 'DataSource', title: '来源', width: 80,
                        formatter: function (value) {
                            if (value == 0) {
                                return '管理处';
                            } else if (value == 1) {
                                return '消费打折端';
                            } else {
                                return "";
                            }
                        }
                    },
                    { field: 'OperatorAccount', title: '操作人账号', width: 100 },
                    { field: 'Remark', title: '备注', width: 150 },
                    { field: 'CreateTimeToString', title: '添加时间', width: 150 },
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableQRCodeDerate').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkCarDerateQRCode/GetParkDerateQRCodeOperatePurview', function (result) {
                    $('#tableQRCodeDerate').datagrid("addToolbarItem", result);
                });
            }
        }

    });
});

function LoadParkQRCodeDerate() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    var derateId = $("#cmbQueryParkDerate").combobox("getValue");
    var status = $("#sltStatus").val();
    var derateQRCodeSource = $("#sltDerateQRCodeSource").val();
    $('#tableQRCodeDerate').datagrid('load', { sellerId: seller.id, derateId: derateId, status: status, derateQRCodeSource: derateQRCodeSource });
}
function BindParkDerate(sellerId) {
    $("#cmbQueryParkDerate").combobox({
        url: '/p/ParkCarDerateQRCode/GetSellerDerateTree?needDefaultValue=false&sellerId=' + sellerId,
        valueField: 'id',
        textField: 'text'
    });
    $("#cmbEtidParkDerate").combobox({
        url: '/p/ParkCarDerateQRCode/GetSellerDerateData?sellerId=' + sellerId,
        valueField: 'DerateID',
        textField: 'Name',
        onSelect: function (e) {
            SetDerateDescription(e.DerateType);
        },
        onLoadSuccess: function () {
            var derateData = $('#cmbEtidParkDerate').combobox('getData');
            if (derateData.length > 0) {
                $('#cmbEtidParkDerate').combobox('setValue', derateData[0].DerateID);
                SetDerateDescription(derateData[0].DerateType);
            }
        }
    });
}
function BindParking(sellerId) {
    $("#cmbPKID").combobox({
        url: '/p/ParkCarDerateQRCode/GetParkingData?sellerId=' + sellerId,
        valueField: 'PKID',
        textField: 'PKName'
    });
}
function SetDerateDescription(derateType) {
    if (derateType == 8) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免金额');
        $('#spanUnitDes').html('元');

    } else if (derateType == 9 || derateType == 3) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免时间');
        $('#spanUnitDes').html('分钟');
    }
    else if (derateType == 10) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免');
        $('#spanUnitDes').html('天');
    } 
    else {
        $('#txtDerateValue').numberspinner('setValue', 0);
        $('#txtDerateValue').numberspinner('disable');
    }
}
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkSellerTree").tree("search", content);
}

function Add() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    $("#cmbPKID").combobox("enable");
    $("#cmbEtidParkDerate").combobox("enable");
    AddOrUpdateBox("添加优免券规则");
    var start = $("#hiddDefaultStartTime").val();
    var end = $("#hiddDefaultEndTime").val();
    $('#divDerateQRCodeBoxForm').form('load', {
        RecordID: '',
        DerateValue: 0,
        CanUseTimes: 0,
        Remark: "",
        EndTime: end.toString()
    });
    var derateData = $('#cmbEtidParkDerate').combobox('getData');
    if (derateData.length > 0) {
        $('#cmbEtidParkDerate').combobox('setValue', derateData[0].DerateID);
        SetDerateDescription(derateData[0].DerateType);
    }
    var parkData = $('#cmbPKID').combobox('getData');
    if (parkData.length > 0) {
        $('#cmbPKID').combobox('setValue', parkData[0].PKID);
    }
}
function Update() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要修改的优免券规则!");
        return;
    }

    AddOrUpdateBox("修改优免券规则");

    $('#divDerateQRCodeBoxForm').form('load', {
        RecordID: derate.RecordID,
        DerateValue: derate.DerateValue,
        CanUseTimes: derate.CanUseTimes,
        Remark: derate.Remark,
        EndTime: derate.EndTime.replace("T", " "),
        DerateID: derate.DerateID,
        PKID: derate.PKID
    });
    $("#cmbPKID").combobox("disable");

    if (derate.CanUseTimes > 0) {
        $("#cmbEtidParkDerate").combobox("disable");
    }
    else {
        $("#cmbEtidParkDerate").combobox("enable");
    }
    var derateData = $('#cmbEtidParkDerate').combobox('getData');
    if (derateData.length > 0) {
        for (var i = 0; i < derateData.length; i++) {
            if (derate.DerateID == derateData[i].DerateID) {
                $('#cmbEtidParkDerate').combobox('setValue', derateData[i].DerateID);
                SetDerateDescription(derateData[i].DerateType);
            }
        }
    }
}

function Delete() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要删除的优免券规则!");
        return;
    }
    $.messager.confirm('系统提示', '删除该优免规则将同时删除之前发放并未使用的优免券,你确定要删除吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkCarDerateQRCode/Delete?recordId=' + derate.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除优免券规则成功',
                                title: "系统提示"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function Refresh() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    var derateId = $("#cmbQueryParkDerate").combobox("getValue");
    var status = $("#sltStatus").val();
    var derateQRCodeSource = $("#sltDerateQRCodeSource").val();

    $('#tableQRCodeDerate').datagrid('options').queryParams = { sellerId: seller.id, derateId: derateId, status: status, derateQRCodeSource: derateQRCodeSource };
    $('#tableQRCodeDerate').datagrid('reload');
}
AddOrUpdateBox = function (title) {
    $('#divDerateQRCodeBox').show().dialog({
        title: title,
        width: 350,
        height: 300,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divDerateQRCodeBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divDerateQRCodeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkCarDerateQRCode/AddOrUpdate',
                            data: $("#divDerateQRCodeBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {
                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '保存优免券规则成功!',
                                        title: "系统提示"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divDerateQRCodeBox').dialog('close');
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
function GrantCarDerate() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要优免券规则!");
        return;
    }
    $('#divGrantDerateQRCodeBoxForm').form('load', {
        qid: derate.RecordID,
        vid: derate.VID,
        sellerName: derate.SellerName,
        derateName: derate.DerateName
    });
    $('#divGrantDerateQRCodeBox').show().dialog({
        title: "优免发券",
        width: 300,
        height: 150,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divGrantDerateQRCodeBox').dialog('close');
                }
            },
            {
                text: '确定',
                iconCls: 'icon-add',
                id: 'btnGarntDerate',
                handler: function () {

                    var selectRow = $('#tableQRCodeDerate').datagrid('getSelected');
                    if (selectRow == null) {
                        $.messager.alert("系统提示", "请选择优免券规则!");
                        return;
                    }
                    $.messager.progress({ text: '下发中....', interval: 100 });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkCarDerateQRCode/GrantCarDerate',
                        data: $("#divGrantDerateQRCodeBoxForm").serialize(),
                        error: function () {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                        },
                        success: function (data) {
                            $.messager.progress("close");
                            if (data.result) {
                                $('#divGrantDerateQRCodeBox').dialog('close');
                                Refresh();
                                location.href = data.data;
                            } else {
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                }
            }]

    });
}
function DownloadQRCode() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要优免券规则!");
        return;
    }
    if (derate.DerateQRcodeZipFilePath == "") {
        $.messager.alert("系统提示", "该优免券规则无二维码文件!");
        return;
    }
    location.href = derate.DerateQRcodeZipFilePath;
}
