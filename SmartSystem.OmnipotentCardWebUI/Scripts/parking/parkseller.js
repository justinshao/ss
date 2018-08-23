
function QueryParkSellerData() {
    var village = $('#villageTree').tree('getSelected');
    if (village == null || village.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择小区!', 'error');
        return;
    }
    var sellerName = $("#txtParkSellerName").val();
    $('#tableParkSeller').datagrid('load', { villageId: village.id, sellerName: sellerName });
}
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#villageTree").tree("search", content);
}
$(function () {
    $('#villageTree').tree({
        url: '/VillageData/CreateVillageTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                var sellerName = $("#txtParkSellerName").val();
                $('#tableParkSeller').datagrid('load', { villageId: node.id, sellerName: sellerName });
            }
        }
    });
    $('#tableParkSeller').datagrid({
        url: '/p/ParkSeller/GetParkSellerData',
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
                    { field: 'SellerID', title: 'SellerID', width: 80, hidden: true },
                    { field: 'VID', title: 'VID', width: 80, hidden: true },
                    { field: 'SellerNo', title: '商户号', width: 80 },
                    { field: 'SellerName', title: '商户名', width: 100 },
                     { field: 'Addr', title: '商户地址', width: 250 },
                     { field: 'Balance', title: '余额', width: 80},
                    { field: 'Creditline', title: '可透支金额', width: 80, formatter: function (value, row, index) {
                        if (value <= 0) {
                            return "不可透支";
                        } else {
                            return value;
                        }
                    }
                    }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableParkSeller').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkSeller/GetSellerOperatePurview', function (result) {
                    $('#tableParkSeller').datagrid("addToolbarItem", result);
                });
            }
        }

    });
});
function Add() {
    $('#SellerNo').validatebox('enable');
    var village = $('#villageTree').tree('getSelected');
    if (village == null || village.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择小区!', 'error');
        return;
    }
    $('#divParkSellerBoxForm').form('clear');

    SaveOrUpdate("添加商家");
    $('#divParkSellerBoxForm').form('load', {
        VID: village.id,
        Creditline: 0
    })
    $(".trPassword").show();
}
function Update() {
    $('#SellerNo').validatebox('disable');
    var seller = $('#tableParkSeller').datagrid('getSelected');
    if (seller == null) {
        $.messager.alert("系统提示", "请选择需要修改的商家!");
        return;
    }
    $('#divParkSellerBoxForm').form('clear');

    SaveOrUpdate("修改商家");

    $('#divParkSellerBoxForm').form('load', {
        SellerNo: seller.SellerNo,
        VID: seller.VID,
        SellerID: seller.SellerID,
        Creditline: seller.Creditline,
        SellerName: seller.SellerName,
        Addr: seller.Addr,
        Creditline: seller.Creditline
    })
    $(".trPassword").hide();
}
function ResetPassword() {
    var seller = $('#tableParkSeller').datagrid('getSelected');
    if (seller == null) {
        $.messager.alert("系统提示", "请选择需要重置密码的商家!");
        return;
    }
    $.messager.confirm('系统提示', '您确定要重置密码吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkSeller/ResetPassword?sellerId=' + seller.SellerID,
                    function (data) {
                        if (data.result) {
                            $.messager.alert("系统提示", data.msg);
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function Delete() {
    var seller = $('#tableParkSeller').datagrid('getSelected');
    if (seller == null) {
        $.messager.alert("系统提示", "请选择需要删除的商家!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的商家吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkSeller/Delete?sellerId=' + seller.SellerID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除商家成功',
                                title: "删除商家"
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
    var village = $('#villageTree').tree('getSelected');
    if (village == null || village.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择小区!', 'error');
        return;
    }
    var sellerName = $("#txtParkSellerName").val();
    $('#tableParkSeller').datagrid('load', { villageId: village.id, sellerName: sellerName });
}
//弹出编辑对话框
SaveOrUpdate = function (title) {
    $('#divParkSellerBox').show().dialog({
        title: title,
        width: 400,
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
                    $('#divParkSellerBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var sellerId = $("#hiddEditSellerID").val();
                    if ($.trim(sellerId)=="") {
                        var password = $("#PWD").val();
                        if ($.trim(password) == "" || $.trim(password).length < 6) {
                            $.messager.alert('系统提示', '密码不能为空或长度不能小于6位!', 'error');
                            return;
                        }
                    }

                    if ($('#divParkSellerBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkSeller/AddOrUpdate',
                            data: $("#divParkSellerBoxForm").serialize(),
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
                                    $('#divParkSellerBox').dialog('close');
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
    function Charge() {
    var seller = $('#tableParkSeller').datagrid('getSelected');
    if (seller == null) {
        $.messager.alert("系统提示", "请选择需要充值的商家!");
        return;
    }
    $('#divParkSellerChargeBoxForm').form('clear');

    SellerChargeBox("商家充值");

    $('#divParkSellerChargeBoxForm').form('load', {
        SellerID: seller.SellerID,
        SellerName: seller.SellerName,
        Balance: seller.Balance,
        chargeBalance: 0
    })

}
//弹出编辑对话框
SellerChargeBox = function (title) {
    $('#divParkSellerChargeBox').show().dialog({
        title: title,
        width: 400,
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
                    $('#divParkSellerChargeBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {

                    if ($('#divParkSellerChargeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkSeller/SellerCharge',
                            data: $("#divParkSellerChargeBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {
                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '充值成功!',
                                        title: "商户充值"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divParkSellerChargeBox').dialog('close');
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