$(function () {
    var defaultStart = $("#hiddDefaultStartDate").val();
    var defaultEnd = $("#hiddDefaultEndDate").val();
    $("#txtOrderStartTime").datetimebox("setValue", defaultStart);
    $("#txtOrderEndTime").datetimebox("setValue", defaultEnd);

    $("#btnQueryData").click(function () {
        var selectRow = $('#treeCompany').tree('getSelected');
        if (selectRow == null) {
            $.messager.alert("系统提示", "请先选择单位!");
            return;
        }
        LoadQueryData(selectRow.id);
    });
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            LoadQueryData(record.id);
            BindParkingCombobox(record.id);
        }
    });
    BindCombobox();
    BindDataTable();
    var showbwy = $("#hiddNeedShowBWYParking").length;
    if (showbwy > 0) {
        BindExternalParkingCombobox();
    }
});
function LoadQueryData(companyId) {
    var orderId = $("#txtOrderId").textbox("getValue");
    var start = $("#txtOrderStartTime").datetimebox("getValue");
    var end = $("#txtOrderEndTime").datetimebox("getValue");
    var platebefore = $("#defaultProvinceId").find(".l-btn-text").text();
    var plateafter = $("#defaultNumberId").val();
    var plateNo = "";
    if ($.trim(plateafter) != "") {
        plateNo = platebefore + plateafter;
    }
    var parkingId = $("#cmbParking").combobox("getValue");
    var status = $("#cmbOrderStatus").combobox("getValue");
    var type = $("#cmbOrderType").combobox("getValue");
    var externalPKID = "-1"
    var showbwy = $("#hiddNeedShowBWYParking").length;
    if (showbwy > 0) {
        externalPKID = $("#cmExternalPKID").combobox("getValue");
    }

    $('#tableOrder').datagrid('load', { Query: "1", StartTime: start, EndTime: end, OrderId: orderId, PlateNo: plateNo, Status: status, OrderType: type, CompanyId: companyId, ParkingId: parkingId, ExternalPKID: externalPKID });
}
function BindDataTable() {
    $('#tableOrder').datagrid({
        url: '/a/AliPayOnlineOrder/GetOnlineOrderData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        idField: 'OrderID',
        onSelect: function (rowIndex, rowData) {
            $("#btnmanualrefund").hide();
            $("#btnpayresult").hide();
            var status = rowData.Status;
            if (status == 0 || status == 1) {
                $("#btnpayresult").show();
            }
            if (status == 6) {
                $("#btnmanualrefund").show();
            }
        },
        columns: [[
                    { field: 'OrderType', title: 'OrderType', width: 80, hidden: true },
                    { field: 'Status', title: 'Status', width: 80, hidden: true },
                    { field: 'OrderID', title: '订单编号', width: 120 },
                    { field: 'PKName', title: '车场名称', width: 100 },
                    { field: 'PlateNo', title: '车牌号', width: 60 },
                    { field: 'Amount', title: '支付金额', width: 60 },
                    { field: 'MonthNum', title: '续期月数', width: 60 },
                    { field: 'PayerNickName', title: '支付人', width: 100 },
                    { field: 'SyncResultTimes', title: '同步支付次数', width: 60 },
                    { field: 'LastSyncResultTime', title: '最后同步时间', width: 120 },
                    { field: 'RefundOrderId', title: '退款订单号', width: 100 },
                    { field: 'OrderTypeDes', title: '订单类型', width: 60 },
                    { field: 'StatusDes', title: '订单状态', width: 120 },
                    { field: 'OrderTime', title: '订单时间', width: 120 },
                    { field: 'RealPayTime', title: '支付时间', width: 120 },
                    { field: 'BWYParkingName', title: '外部车场名称', width: 100 },
                    { field: 'Remark', title: '备注', width: 100 },
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableOrder').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/a/AliPayOnlineOrder/GetOnlieOrderOperatePurview', function (result) {
                    $('#tableOrder').datagrid("addToolbarItem", result);
                    $("#btnmanualrefund").hide();
                    $("#btnpayresult").hide();
                });
            }
        }, onLoadSuccess: function (data) {
            var showbwy = $("#hiddNeedShowBWYParking").length;
            if (showbwy > 0) {
                $('#tableOrder').datagrid('showColumn', 'BWYParkingName');
            } else {
                $('#tableOrder').datagrid('hideColumn', 'BWYParkingName');
            }

        },
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45]
    });
}
function BindCombobox() {
    $("#cmbOrderStatus").combobox({
        url: '/a/AliPayOnlineOrder/GetOrderStatus',
        valueField: 'id',
        textField: 'text'
    });
    $("#cmbOrderType").combobox({
        url: '/a/AliPayOnlineOrder/GetOrderType',
        valueField: 'id',
        textField: 'text'
    });
}
function BindParkingCombobox(companyId) {
    $("#cmbParking").combobox({
        url: '/a/AliPayOnlineOrder/GetParkingData?companyId=' + companyId,
        valueField: 'id',
        textField: 'text'
    });
}
function BindExternalParkingCombobox() {
    $("#cmExternalPKID").combobox({
        url: '/a/AliPayOnlineOrder/GetExternalParkingData',
        valueField: 'id',
        textField: 'text'
    });
}
function SynchPaymentResult() {
    var order = $('#tableOrder').datagrid('getSelected');
    if (order == null) {
        $.messager.alert("系统提示", "请选择订单!");
        return;
    }
    $.messager.confirm('系统提示', '确定要同步支付结果吗?',
            function (r) {
                if (r) {
                    $.post('/a/AliPayOnlineOrder/SynchAliPayPaymentResult?orderId=' + order.OrderID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '同步支付结果成功',
                                title: "同步支付结果"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function ManualRefund() {
    var order = $('#tableOrder').datagrid('getSelected');
    if (order == null) {
        $.messager.alert("系统提示", "请选择订单!");
        return;
    }
    $.messager.confirm('系统提示', '确定要退款吗?',
            function (r) {
                if (r) {
                    $.post('/a/AliPayOnlineOrder/ManualRefund?orderId=' + order.OrderID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '退款成功',
                                title: "退款结果"
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
    var selectRow = $('#treeCompany').tree('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请先选择单位!");
        return;
    }
    LoadQueryData(selectRow.id);
}
function Export() {
    var selectRow = $('#treeCompany').tree('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请先选择单位!");
        return;
    }
    var orderId = $("#txtOrderId").textbox("getValue");
    var start = $("#txtOrderStartTime").datetimebox("getValue");
    var end = $("#txtOrderEndTime").datetimebox("getValue");
    var platebefore = $("#defaultProvinceId").find(".l-btn-text").text();
    var plateafter = $("#defaultNumberId").val();
    var plateNo = "";
    if ($.trim(plateafter) != "") {
        plateNo = platebefore + plateafter;
    }
    var parkingId = $("#cmbParking").combobox("getValue");
    var status = $("#cmbOrderStatus").combobox("getValue");
    var type = $("#cmbOrderType").combobox("getValue");
    var externalPKID = "-1"
    var showbwy = $("#hiddNeedShowBWYParking").length;
    if (showbwy > 0) {
        externalPKID = $("#cmExternalPKID").combobox("getValue");
    }
    $.ajax({
        type: 'post',
        url: '/a/AliPayOnlineOrder/Export',
        data: "StartTime=" + start + "&EndTime=" + end + "&OrderId=" + orderId + "&PlateNo=" + plateNo + "&Status=" + status + "&OrderType=" + type + "&CompanyId=" + selectRow.id + "&ParkingId=" + parkingId + "&ExternalPKID=" + externalPKID,
        async: true,
        success: function (data) {
            window.open(data);
        },
        error: function (data) {
            alert("导出失败");
        }
    });
}