function GetPriod() {
    $.ajax({
        type: "post",
        url: '/p/ParkSettle/GetPriod',
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectPriod").append("<option value='" + item + "'>" + item + "</option>");
                })
            }
        }
    });
}
function DGReflush() {
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var status = $("#selectStatus").val();
    var priod = $("#selectPriod").val();
    var options = $('#tableListBox').datagrid('options');
    options.url = "/p/ParkSettle/Search_Settlements";
    $('#tableListBox').datagrid('load', { parkingid: parkingid, status: status, priod: priod });
}
function GetMaxSettlement() {
    var pkid = $("#selectParks").combobox("getValue");
    if ($.trim(pkid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    $("#txtAuditingRemark").html("");
    $.ajax({
        type: "post",
        url: '/p/ParkSettle/GetMaxSettlement',
        data: "pkid=" + pkid + "",
        async: false,
        success: function (data) {
            if (data != null) {
                if (data.EndTimeName == '') {
                    $('#txtStartTime').datetimebox('setValue', currentdate00());
                    $("#txtAuditingRemark").html("最后结算时间: 无");
                }
                else {
                    $("#txtAuditingRemark").html("最后结算时间:" + data.EndTimeName);
                    $('#txtStartTime').datetimebox('setValue', data.EndTimeName1);
                }
            }
            else {
                $('#txtStartTime').datetimebox('setValue', currentdate00());
            }
        }
    });
}

function GetMaxAmount() {
    var pkid = $("#selectParks").combobox("getValue");
    if ($.trim(pkid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/ParkSettle/GetMaxAmount',
        data: "pkid=" + pkid + "",
        async: false,
        success: function (data) {
            if (data != null) {
                $("#spanMaxAmount").html("每次最大提现额度" + data.MaxAmountOfCash);
                $("#txtMaxAmount").val(data.MaxAmountOfCash);
            }
            else {
                $("#spanMaxAmount").html("每次最大提现额度500");
                $("#txtMaxAmount").val(500);
            }
        }
    });
}
function GetMinAmount() {
    var pkid = $("#selectParks").combobox("getValue");
    if ($.trim(pkid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/p/ParkSettle/GetMinAmount',
        data: "pkid=" + pkid + "",
        async: false,
        success: function (data) {
            if (data != null) {
                $("#spanMinAmount").html("每次最小提现额度" + data.MinAmountOfCash);
                $("#txtMinAmount").val(data.MinAmountOfCash);
            }
            else {
                $("#spanMinAmount").html("每次最小提现额度500");
                $("#txtMinAmount").val(500);
            }

        }
    });
}

function BindGetParkTrees() {
    $.ajax({
        url: '/S/Statistics/GetParks',
        type: 'post',
        success: function (data) {
            var comdata = [{ 'text': '全部', 'id': '-1'}];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].PKName, "id": data[i].PKID });
            }

            $('#selectParks').combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text'
            });
        }
    });
}
$(function () {
    $('#txtStartTime').datetimebox('setValue', currentdate00())
    $('#txtEndTime').datetimebox('setValue', currentdate23());

    BindGetParkTrees();
    $("#btnApply").click(function () {
        var flag = true;
        var pkid = $("#selectParks").combobox("getValue");
        if ($.trim(pkid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        $.ajax({
            type: "post",
            url: '/p/ParkSettle/IsApprovalSettlement',
            data: "pkid=" + pkid + "",
            async: false,
            success: function (data) {
                if (data != null) {
                    if (!data.result) {
                        flag = false;
                        $.messager.alert('系统提示', data.msg, 'error');
                    }
                }
            }
        });
        if (!flag)
            return;
        GetMaxSettlement();
        GetMaxAmount();
        GetMinAmount();
        $("#IsCal").val("0");
        $('#divAuditing').show().dialog({ title: "结算", width: 450, height: 380, modal: true, collapsible: false, minimizable: false, maximizable: false });
    })
    $("#btnCalAmount").click(function () {
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var parkingid = $("#selectParks").combobox("getValue");
        if ($.trim(parkingid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        if (starttime >= endtime) {
            $.messager.alert('系统提示', '结束时间不能大于开始时间', 'error');
            return;
        }

        $.ajax({
            type: "post",
            url: '/p/ParkSettle/CalSettleAmount',
            data: "pkid=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime,
            success: function (data) {
                if (data.IsSuccess) {
                    $("#txtTotalAmount").val(data.TotalAmount);
                    $("#txtFeeAmount").val(data.RateFeeAmount);
                    $("#txtReceiveAmount").val(data.ReceiveAmount);
                    $("#IsCal").val("1");
                }
                else {
                    $.messager.alert('系统提示', data.Message, 'error');
                }
            }
        });
    })
    $("#btnQueryData").click(function () {
        DGReflush();
    });
    //生成结算单
    $("#btnBuildSettlement").click(function () {
        var remark = $("#txtRemark").val();
        var iscal = $("#IsCal").val();
        var maxamount = $("#txtMaxAmount").val();
        var minamount = $("#txtMinAmount").val();
        var receiveamount = $("#txtReceiveAmount").val();
        if (iscal != "1") {
            $.messager.alert('系统提示', '请先计算费用 再生成结算单', 'error');
            return;
        }
        if (parseFloat(receiveamount) > parseFloat(maxamount)) {
            $.messager.alert('系统提示', '结算金额大于可结算金额 不能提现', 'error');
            return;
        }
                if (parseFloat(receiveamount) < parseFloat(minamount)) {
                    $.messager.alert('系统提示', '结算金额小于可结算金额 不能提现', 'error');
                    return;
                }

                if (receiveamount <= 0) {
                    $.messager.alert('系统提示', '可提现金额为0', 'error');
                    return;
                }



        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var parkingid = $("#selectParks").combobox("getValue");
        var newdate = new Date();
        var enddate = new Date(endtime);
        
        if ($.trim(parkingid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        if (starttime >= endtime) {
            $.messager.alert('系统提示', '结束时间不能大于开始时间', 'error');
            return;
        }
//        if (enddate.getYear() > newdate.getYear() || enddate.getMonth() > newdate.getMonth() || enddate.getDay() >= newdate.getDay()) {
//            $.messager.alert('系统提示', '结束时间最迟为当前日期的前一天', 'error');
//            return;
//        }
        if (enddate.getYear() > newdate.getYear() ) {
            $.messager.alert('系统提示', '结束时间最迟为当前日期的前一天', 'error'); return;
        }
            else if(enddate.getYear() ==newdate.getYear() && enddate.getMonth() > newdate.getMonth() ){
                $.messager.alert('系统提示', '结束时间最迟为当前日期的前一天', 'error'); return;
            }
            else if(enddate.getYear() ==newdate.getYear() && enddate.getMonth() == newdate.getMonth()&&enddate.getDay() >= newdate.getDay()){
                $.messager.alert('系统提示', '结束时间最迟为当前日期的前一天', 'error'); return;
            }
            
        
        $.ajax({
            type: "post",
            url: '/p/ParkSettle/BuildSettlement',
            data: "pkid=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime + "&remark=" + remark,
            success: function (data) {
                $('#divAuditing').dialog('close');
                if (data.result) {
                    DGReflush();
                }
                else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }
            }
        });
    })
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'RecordID', title: '记录编号', width: 150, hidden: true },
                    { field: 'IsHide', title: '是否隐藏', width: 150, hidden: true },
                    { field: 'PKID', title: '车场编号', width: 150, hidden: true },
                    { field: 'ParkName', title: '车场名称', width: 150 },
                    { field: 'Priod', title: '帐期编号', width: 80 },
                    { field: 'TotalAmount', title: '结算总额', width: 80 },
                    { field: 'HandlingFeeAmount', title: '手续费', width: 80 },
                    { field: 'ReceivableAmount', title: '应结金额', width: 80 },
                    { field: 'SettleStatus', title: '状态', width: 100,
                        formatter: function (value, row, index) {
                            var str = "";
                            switch (value) {
                                case -1:
                                    str = "撤销";
                                    break;
                                case 0:
                                    str = "待转款";
                                    break;
                                case 1:
                                    str = "待收款";
                                    break;
                                case 2:
                                    str = "完成";
                                    break;
                                default:
                                    break;
                            }
                            return str;
                        }
                    },
                    { field: 'StartTimeName', title: '开始时间', width: 135 },
                    { field: 'EndTimeName', title: '结束时间', width: 135 },
                    { field: 'PayTimeName', title: '付款时间', width: 135 },
                    { field: 'CreateTimeName', title: '创建时间', width: 135 },
                    { field: 'UserName', title: '创建人', width: 80 },
                    { field: 'Remark', title: '备注', width: 150 },
        //                    { field: 'Receip', title: '转款回执', width: 100 }
                    {field: 'opt', title: '', width: 160, align: 'center',
                    formatter: function (value, rec) {
                        var btn = "";

                        if (rec.IsHide) {
                            switch (rec.SettleStatus) {
                                //已申请  状态为0  订单处理待审批状态                                          
                                case 0:
                                    btn = '<a name=\'opera1\' disabled onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">待转款</a><a name=\'opera2\' disabled onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">撤销</a>';
                                    break;
                                //已审批  状态为1  订单处理已审批状态                                          
                                case 1:
                                    btn = '<a name=\'opera1\' disabled onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">待收款</a><a name=\'opera2\' disabled onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">撤销</a>';
                                    break;
                                //处理已转款状态                                          
                                case 2:
                                    btn = '<a name=\'opera1\' disabled onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">已收款</a><a name=\'opera2\' disabled onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">未收</a>';
                                    break;
                                default:
                                    break;
                            }
                        }
                        else {
                            switch (rec.SettleStatus) {
                                //已申请  状态为0  订单处理待审批状态                                         
                                case 0:
                                    btn = '<a name=\'opera1\' onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">待转款</a><a name=\'opera2\' onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">撤销</a>';
                                    break;
                                //已审批  状态为1  订单处理已审批状态                                         
                                case 1:
                                    btn = '<a name=\'opera1\' onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">待收款</a><a name=\'opera2\' onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">撤销</a>';
                                    break;
                                //平台已审批  状态为2  订单处理平台已审批状态                                         
                                case 2:
                                    btn = '<a name=\'opera1\' onclick="editRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">已收款</a><a name=\'opera2\' onclick="CancelRow(\'' + rec.RecordID + '\',\'' + rec.SettleStatus + '\')" href="javascript:void(0)">未收</a>';
                                    break;

                                default:
                                    break;
                            }
                        }


                        return btn;
                    }
                }
	    ]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
            $("a[name='opera1']").linkbutton({ plain: true, iconCls: 'icon-edit' });
            $("a[name='opera2']").linkbutton({ plain: true, iconCls: 'icon-cancel' });

        },
        pagination: false,
        rownumbers: true,
        pageSize: 35,
        pageList: [35]
    });
});
function OpeConfirm(settlestatus) {
    var str = "";
    switch (settlestatus) {
        case "-1":
            str = "撤销";
            break;
        case "0":
            str = "转款金额 并且已转款处理";
            break;
        case "1":
            str = "收款金额 并已到帐";
            break;
        case "2":
            str = "完成";
            break;
        default:
            break;
    }
    return str;
}
function editRow(recordid, settlestatus) {
    $.messager.confirm('确认', "请确认 "+OpeConfirm(settlestatus)+" 操作", function (r) {
        if (r) {
            $.ajax({
                type: "post",
                url: '/p/ParkSettle/ApplySettleDocument',
                data: "recordid=" + recordid + "&settlestatus=" + settlestatus,
                success: function (data) {
                    if (data) {
                        $.messager.alert('系统提示', "操作成功", '成功');
                        DGReflush();
                    }
                    else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }
                }
            });
        }
    });
}
function CancelRow(recordid, settlestatus) {
    $.messager.confirm('确认', "请确认此操作", function (r) {
        if (r) {
            $.ajax({
                type: "post",
                url: '/p/ParkSettle/CancelSettleDocument',
                data: "recordid=" + recordid + "&settlestatus=" + settlestatus,
                success: function (data) {
                    if (data) {
                        $.messager.alert('系统提示', "操作成功", '成功');
                        DGReflush();
                    }
                    else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }
                }
            });
        }
    });
}
function StatisticsChangeParking(parkingId) {
    if (typeof(parkingId) != 'undefined') {
        GetPriod();
    }
}