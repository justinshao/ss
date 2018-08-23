$(function () {
    $('#villageTree').tree({
        url: '/VillageData/CreateLoginUserSubVillageTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                CurrentSelectVillage(node.id);
            }
        }
    });

});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#villageTree").tree("search", content);
}
function CurrentSelectVillage(villageId) {
    $('#tableParkSettleConfig').datagrid('load', { villageId: villageId });
}
$(function () {
    $('#tableParkSettleConfig').datagrid({
        nowrap: false,
        striped: true,
        collapsible: false,
        singleSelect: true,
        url: '/p/ParkSettleConfig/GetParkingData',
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { title: 'ID', field: 'ID', width: 80, hidden: true },
                    { title: 'PKID', field: 'PKID', width: 80, hidden: true },
                    { field: 'PKNo', title: '车场编号', width: 80 },
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'HandlingFee', title: '手续费率(元)', width: 80, formatter: function (value) {
                        return value + "‰";
                    }
                    },
                    { field: 'MaxAmountOfCash', title: '最大提现金额(元)', width: 100 },
                    { field: 'MinAmountOfCash', title: '最小提现金额(元)', width: 100 }
				]],
        pagination: true,
        rownumbers: true,
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableParkSettleConfig').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkSettleConfig/GetSettleOperatePurview', function (result) {
                    $('#tableParkSettleConfig').datagrid("addToolbarItem", result);
                });
            }
        }
    });
});
function Update() {
    var selectParking = $('#tableParkSettleConfig').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择需要配置结算的车场!");
        return;
    }
    ShowParkSettleBox("修改结算车场");
    $('#divParkSettleBoxForm').form('load', {
        PKID: selectParking.PKID,
        HandlingFee: selectParking.HandlingFee,
        MaxAmountOfCash: selectParking.MaxAmountOfCash,
        MinAmountOfCash: selectParking.MinAmountOfCash
    });
}

function Refresh() {
    var selectVillage = $('#villageTree').tree('getSelected');
    if (selectVillage == null || selectVillage.attributes.type != 1) {
        return;
    }
    CurrentSelectVillage(selectVillage.id);
}
//弹出编辑对话框
ShowParkSettleBox = function (data) {
    $('#divParkSettleBoxForm').form('clear');
    $('#divParkSettleBox').show().dialog({
        title: data,
        width: 350,
        height: 200,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkSettleBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divParkSettleBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });

                    $.ajax({
                        type: "post",
                        url: '/p/ParkSettleConfig/SaveConfig',
                        data: $("#divParkSettleBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                        success: function (data) {
                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '数据保存成功!',
                                    title: "保存提示"
                                });
                                $.messager.progress("close");
                                $('#divParkSettleBox').dialog('close');
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
