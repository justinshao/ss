$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                var plateNo = $("#txtQueryPlateNo").val();
                $('#tableBlackList').datagrid('load', { parkingId: node.id, queryPlateNo: plateNo });
            }
        }
    });

    $("#btnQueryBackListData").click(function () {
        Refresh();
    });

    BindBlackListData();
    BindBlackListStatus();
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
function BindBlackListData() {
    $('#tableBlackList').datagrid({
        url: '/p/ParkBackList/GetBlackListData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 80, hidden: true },
                    { field: 'Status', title: 'Status', width: 80, hidden: true },
                    { field: 'PKID', title: 'PKID', width: 80, hidden: true },
                    { field: 'PlateNumber', title: '车牌号', width: 80 },
                    { field: 'StatusDes', title: '类型', width: 200 },
                    { field: 'Remark', title: '原因', width: 200 },
                    { field: 'LastUpdateTime', title: '最后修改时间', width: 120 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableBlackList').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkBackList/GetBlackListOperatePurview', function (result) {
                    $('#tableBlackList').datagrid("addToolbarItem", result);
                });
            }
        }

    });
}
function BindBlackListStatus() {
    $('#cmbStatus').combobox({
        url: '/p/ParkBackList/GetBlackListStatusData',
        valueField: 'EnumValue',
        textField: 'Description',
        onLoadSuccess: function () {
            $('#cmbStatus').combobox("setValue", "0");
        }
    });
}
function Add() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
  
    $("#defaultNumberId").val("");

    AddOrUpdate("添加黑名单");
    
    $('#divBlackListBoxForm').form('load', {
        PKID: park.id,
        RecordID: "",
        PlateNo: "",
        Remark: ""
    })
    $('#cmbStatus').combobox("setValue", "1");

}
function Update() {
    var model = $('#tableBlackList').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要修改的黑名单!");
        return;
    }
    $('#divBlackListBoxForm').form('clear');
    $("#defaultNumberId").val("");
    AddOrUpdate("修改黑名单");
    $('#divBlackListBoxForm').form('load', {
        PKID: model.PKID,
        RecordID: model.RecordID,
        Remark: model.Remark
    })
    $('#cmbStatus').combobox("setValue", model.Status);

    var province = "";
    var number = "";
    if (model.PlateNumber.length > 2) {
        province = model.PlateNumber.substr(0, 2);
        number = model.PlateNumber.substr(2);
    }
    $("#defaultProvinceId").find(".l-btn-text").text(province);
    $("#defaultNumberId").val(number);
}
function Delete() {
    var model = $('#tableBlackList').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要删除的黑名单!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的黑名单吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkBackList/Delete?recordId=' + model.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除黑名单成功',
                                title: "删除黑名单"
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
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    var plateNo = $("#txtQueryPlateNo").val();
    $('#tableBlackList').datagrid('load', { parkingId: park.id, queryPlateNo: plateNo });
}

//弹出编辑对话框
function AddOrUpdate(title) {
    $('#divBlackListBox').show().dialog({
        title: title,
        width: 350,
        height: 250,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divBlackListBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {

                    var platebefore = $("#defaultProvinceId").find(".l-btn-text").text();
                    var plateafter = $("#defaultNumberId").val();
                    if ($.trim(platebefore).length != 2 || $.trim(plateafter)=="") {
                        $.messager.alert('系统提示', '车牌号码格式不正确!', 'error');
                        return false;
                    }
                    $("#hiddPlateNo").val(platebefore + plateafter);

                    if ($('#divBlackListBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkBackList/SaveBlackList',
                            data: $("#divBlackListBoxForm").serialize(),
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
                                    $('#divBlackListBox').dialog('close');
                                    Refresh();
                                   
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