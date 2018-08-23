$(function () {
    $('#parkSellerTree').tree({
        url: '/ParkSellerData/GetSellerTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                BindFeeRule(node.id);
                $('#tableSellerDerate').datagrid('load', { sellerId: node.id });
            }
        }
    });
    $("[name=DerateType]").change(function () {
        ChangeDerateType();
    });

    $('#tableSellerDerate').datagrid({
        url: '/p/ParkDerate/GetSellerDerateData/',
        singleSelect: true,
        columns: [[
                    { field: 'DerateID', title: 'DerateID', width: 100, hidden: true },
                    { field: 'SellerID', title: 'SellerID', width: 100, hidden: true },
                    { field: 'DerateSwparate', title: 'DerateSwparate', width: 100, hidden: true },
                    { field: 'DerateMoney', title: 'DerateMoney', width: 100, hidden: true },
                    { field: 'FreeTime', title: 'FreeTime', width: 100, hidden: true },
                    { field: 'StartTime', title: 'StartTime', width: 100, hidden: true },
                    { field: 'EndTime', title: 'EndTime', width: 100, hidden: true },
                    { field: 'FeeRuleID', title: 'FeeRuleID', width: 100, hidden: true },
                    { field: 'DerateType', title: 'DerateType', width: 100, hidden: true },
                    { field: 'DerateIntervar', title: 'DerateIntervar', width: 100, hidden: true },
                    { field: 'Name', title: '优免名称', width: 200 },
                    { field: 'DerateTypeDes', title: '优免类型', width: 150 },
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableSellerDerate').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkDerate/GetSellerDerateOperatePurview', function (result) {
                    $('#tableSellerDerate').datagrid("addToolbarItem", result);
                });
            }
        }

    });
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkSellerTree").tree("search", content);
}
function btnAddPeriodFreeRule() {
    var money = $("#txtPeriodFreemoney").val();
    if ($.trim(money) == "") {
        $.messager.alert('系统提示', '请输入收取商家金额!', 'error');
        return;
    }
    if (parseInt(money) <= 0) {
        $.messager.alert('系统提示', '收取商家金额必须大于0!', 'error');
        return;
    }
    var hour = $("#txtPeriodFreeHour").val();
    if ($.trim(hour) == "") {
        $.messager.alert('系统提示', '请输入车主免费停车时长!', 'error');
        return;
    }
    AddTableDerateIntervarTr(parseFloat(hour) * 60, money);
}

function btnAddConsumptionRule() {
    var money = $("#txtConsumptionMoney").val();
    if ($.trim(money) == "") {
        $.messager.alert('系统提示', '请输入消费金额!', 'error');
        return;
    }
    if (parseInt(money) <= 0) {
        $.messager.alert('系统提示', '消费金额必须大于0!', 'error');
        return;
    }
    var hour = $("#txtConsumptionFreeHour").val();
    if ($.trim(hour) == "") {
        $.messager.alert('系统提示', '请输入车主免费停车时长!', 'error');
        return;
    }
    AddTableDerateIntervarTr(parseFloat(hour) * 60, money);
}

function ChangeDerateType() {
    var obj = $('input:radio[name="DerateType"]:checked');
    var value = $(obj).val();
    var txt = $(obj).attr("data-desciption");
    $("#txtName").val(txt);
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller != null && seller.attributes.type == 1) {
        $("#txtSellerName").val(seller.text);
    }
    $(".trDetail").hide();
    switch (parseInt(value)) {
        case 0:
        case 1:
        case 8:
            {
                $('#divSellerDerateBox').dialog({ height: 250 });
                $(".trDetail").hide();
                break;
            }
        case 2:
            {
                $('#divSellerDerateBox').dialog({ height: 350 });
                $(".tr2").show();
                break;
            }
        case 3:
        case 9:
            {
                $('#divSellerDerateBox').dialog({ height: 300 });
                $(".tr3").show();
                break;
            }
        case 4:
            {
                RemoveDerateIntervarAllTr();
                $('#divSellerDerateBox').dialog({ height: 430 });
                $(".tr4").show();
                break;
            }
        case 5:
            {
                $('#divSellerDerateBox').dialog({ height: 300 });
                $(".tr5").show();
                break;
            }
        case 6:
            {
                $('#divSellerDerateBox').dialog({ height: 350 });
                $(".tr6").show();
                break;
            }
        case 7:
            {
                RemoveDerateIntervarAllTr();
                $('#divSellerDerateBox').dialog({ height: 430 });
                $(".tr7").show();
                break;
            }
        case 10:
            {
                RemoveDerateIntervarAllTr();
                $('#divSellerDerateBox').dialog({ height: 300 });
                $(".tr8").show();
                break;
            }
    }
}
function BindFeeRule(sellerId) {
    $('#cmbFeeRuleID').combobox({
        url: '/p/ParkDerate/GetParkFeeRuleTreeData?sellerId=' + sellerId,
        valueField: 'FeeRuleID',
        textField: 'RuleName'
    });
}

function Add() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    AddOrUpdateBox("添加优免");
    $('#divSellerDerateBoxForm').form('load', {
        DerateID: '',
        SellerID: seller.id,
        DerateSwparate: 0,
        DerateMoney: 0,
        FreeTimeHour: 0,
        DerateSwparate: 0,
        StartTime: "00:00",
        EndTime: "00:00",
        FeeRuleID: '',
        MaxFreeHour: 0,
        PeriodFreeHour: 0,
        PeriodFreemoney: 0,
        ConsumptionMoney: 0,
        ConsumptionFreeHour: 0
    });
    $("input[name='DerateType'][value=0]").prop("checked", true);
    ChangeDerateType();
}
function Update() {
    var derate = $('#tableSellerDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要修改的优免信息!");
        return;
    }
    $("input[name='DerateType'][value=" + derate.DerateType + "]").prop("checked", true);

    AddOrUpdateBox("修改优免");

    $('#divSellerDerateBoxForm').form('load', {
        DerateID: derate.DerateID,
        SellerID: derate.SellerID,
        DerateSwparate: derate.DerateSwparate,
        DerateMoney: derate.DerateMoney,
        FreeTimeHour: derate.FreeTime / 60,
        Name: derate.Name,
        DerateSwparate: derate.DerateSwparate,
        StartTime: derate.StartTime,
        EndTime: derate.EndTime,
        MaxFreeHour: derate.FreeTime / 60,
        DayMoney:derate.DerateMoney,
        PeriodFreeHour: 0,
        PeriodFreemoney: 0,
        ConsumptionMoney: 0,
        ConsumptionFreeHour: 0
    });

    ChangeDerateType();
    if (derate.FeeRuleID != "") {
        $("#cmbFeeRuleID").combobox("setValue", derate.FeeRuleID);
    }

    if (derate.DerateIntervar != "") {
        var trs = derate.DerateIntervar.split('|');
        for (var i = 0; i < trs.length; i++) {
            var tds = trs[i].split(',');
            AddTableDerateIntervarTr(tds[0], tds[1]);
        }
    }
}

function Delete() {
    var derate = $('#tableSellerDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要删除的优免信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的优免信息吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkDerate/Delete?derateId=' + derate.DerateID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除优免信息成功',
                                title: "删除优免信息"
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
    $('#tableSellerDerate').datagrid('load', { sellerId: seller.id });
}
AddOrUpdateBox = function (title) {
    $('#divSellerDerateBox').show().dialog({
        title: title,
        width: 600,
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
                    $('#divSellerDerateBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divSellerDerateBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        if (!CheckSubmitData()) {
                            $.messager.progress("close");
                            return;
                        }
                        $.ajax({
                            type: "post",
                            url: '/p/ParkDerate/SaveParkDerate',
                            data: $("#divSellerDerateBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {
                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '保存成功!',
                                        title: "保存商家优免"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divSellerDerateBox').dialog('close');
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
    var value = $("input[name=DerateType]:checked").val();
    var strRule = "";
    if (value == "4" || value == "7") {
        var trlen = $("#tableDerateIntervar tr").length;
        if (trlen < 2) {
            $.messager.alert('系统提示', "请添加规则", 'error');
            return false;
        }
        $("#tableDerateIntervar tr:gt(0)").each(function () {
            strRule += $(this).find("td").eq(0).html() + "," + $(this).find("td").eq(1).html() + "|";
        });
        if ($.trim(strRule) == "") {
            $.messager.alert('系统提示', "请添加规则", 'error');
            return false;
        }
    }
    $("#hiddDerateIntervar").val(strRule);
    return true;
}
function RemoveDerateIntervarAllTr() {
    $("#tableDerateIntervar tr:gt(0)").remove();
}
function btnRomoveTimeMoneyRule(obj) {
    $(obj).parents("tr").eq(0).remove();
}
function AddTableDerateIntervarTr(time, money) {
    var tr = $("<tr><td>" + time + "</td><td>" + money + "</td><td>  <a href=\"#\" class=\"easyui-linkbutton\"  onclick=\"return btnRomoveTimeMoneyRule(this)\">删除</a></td></tr>");
    tr.appendTo($("#tableDerateIntervar"));
}