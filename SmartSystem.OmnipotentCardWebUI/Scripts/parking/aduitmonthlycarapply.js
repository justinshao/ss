$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                BindAreaComboboxByParkingId(node.id);
                BindGateComboboxByParkingId(node.id);
                BindCarType(node.id);
                BindCarModel(node.id);
                var applyInfo = $("#txtQueryApplyInfo").val();
                var status = $("#sltStatus").val();
                var starttime = $("#txtStartTime").datetimebox("getValue");
                var endtime = $("#txtEndTime").datetimebox("getValue");
                $('#tableData').datagrid('load', { parkingId: node.id, applyInfo: applyInfo, status: status, starttime: starttime, endtime: endtime });
            }
        }
    });

    $("#btnQueryData").click(function () {
        LoadApplyData();
    });
    BindMonthlyCarApplyData();
});
function BindCarType(parkingId) {
    $.ajax({
        url: '/p/ParkCar/GetParkCarTypeData',
        data: "parkingId=" + parkingId + "",
        type: 'post',
        success: function (data) {
            $("#cmbCarTypeID").combobox({
                data: data,
                valueField: 'CarTypeID',
                textField: 'CarTypeName'
            });
        }
    });
}
function BindCarModel(parkingId) {
    $.ajax({
        url: '/p/ParkCar/GetParkCarModelData',
        data: "parkingId=" + parkingId + "",
        type: 'post',
        success: function (data) {
            $('#cmbCarModelID').combobox({
                data: data,
                valueField: 'CarModelID',
                textField: 'CarModelName'
            });
        }
    });
}
function LoadApplyData() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    var applyInfo = $("#txtQueryApplyInfo").val();
    var status = $("#sltStatus").val();
    var starttime = $("#txtStartTime").datetimebox("getValue");
    var endtime = $("#txtEndTime").datetimebox("getValue");
    $('#tableData').datagrid('load', { parkingId: park.id, applyInfo: applyInfo, status: status, starttime: starttime, endtime: endtime });
}
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
function BindMonthlyCarApplyData() {
    $('#tableData').datagrid({
        url: '/p/AduitMonthlyCarApply/GetAduitMonthlyCarApplyData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        onSelect: function (rowIndex, rowData) {
            $("#btnUpdate").hide();
            $("#btnDelete").hide();
            var status = rowData.ApplyStatus;
            if (status == 0) {
                $("#btnUpdate").show();
                $("#btnDelete").show();
            }
        },
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 80, hidden: true },
                    { field: 'ApplyStatus', title: 'ApplyStatus', width: 80, hidden: true },
                    { field: 'CarTypeID', title: 'CarTypeID', width: 80, hidden: true },
                    { field: 'CarModelID', title: 'CarModelID', width: 80, hidden: true },
                    { field: 'ApplyName', title: '申请人', width: 60 },
                    { field: 'ApplyMoblie', title: '申请人电话', width: 80 },
                    { field: 'PlateNo', title: '车牌号', width: 80 },
                    { field: 'PKLot', title: '车位号', width: 60 },
                    { field: 'CarTypeName', title: '车类', width: 100 },
                    { field: 'CarModelName', title: '车型', width: 100 },
                    { field: 'FamilyAddress', title: '家庭地址', width: 120 },
                    { field: 'ApplyStatusDes', title: '申请状态', width: 60 },
                    { field: 'ApplyDateTime', title: '申请时间', width: 140 },
                    { field: 'ApplyRemark', title: '申请备注', width: 150 },
                    { field: 'AuditRemark', title: '审核备注', width: 150 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableData').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/AduitMonthlyCarApply/GetAduitMonthlyCarApplyOperatePurview', function (result) {
                    $('#tableData').datagrid("addToolbarItem", result);
                });
            }
        }

    });
}
function BindAreaComboboxByParkingId(parkingId) {
    $("#cmbAreaIDS").combobox({
        url: '/p/AduitMonthlyCarApply/GetAreaDataByParkingId?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#cmbAreaIDS").combobox('setValues', arr);
                BindGateComboboxByParkingId(parkingId);
            } else {
                var selectvalues = $("#cmbAreaIDS").combobox('getValues').toString();
                BindGateComboboxByAreaId(selectvalues);
                var arealist = selectvalues.split(',');
                for (var g = 0; g < arealist.length; g++) {
                    if (arealist[g] == "-1") {
                        $("#cmbAreaIDS").combobox('unselect', "-1");
                    }
                }
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#cmbAreaIDS").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                var arr = [];
                arr.push("-1");
                $("#cmbAreaIDS").combobox('setValues', arr);
                BindGateComboboxByParkingId(parkingId);
            } else {
                var values = "";
                var arealist = selectvalues.split(',');
                for (var g = 0; g < arealist.length; g++) {
                    if (arealist[g] == "-1") {
                        continue;
                    }
                    values += arealist[g];
                    if ((g + 1) != arealist.length) {
                        values += ",";
                    }
                }
                BindGateComboboxByAreaId(values);
            }
        }
    });
}
function BindGateComboboxByParkingId(parkingId) {
    $("#cmbGateID").combobox({
        url: '/p/ParkCar/GetGateDataByParkingId?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#cmbGateID").combobox('setValues', arr);
            } else {
                $("#cmbGateID").combobox('unselect', "-1");
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#cmbGateID").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                $("#cmbGateID").combobox('setValue', "-1");
            }
        }
    });

}
function BindGateComboboxByAreaId(selectareaids) {
    $("#cmbGateID").combobox({
        url: '/p/AduitMonthlyCarApply/GetGateDataByAreaIds?areaIds=' + selectareaids,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#cmbGateID").combobox('setValues', arr);
            } else {
                $("#cmbGateID").combobox('unselect', "-1");
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#cmbGateID").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                $("#cmbGateID").combobox('setValue', "-1");
            }
        }
    });
}
function Update() {
    var model = $('#tableData').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要审核的数据!");
        return;
    }
    AddOrUpdate("月租车申请审核");
    $("#hiddPassedRecordID").val(model.RecordID);
    $("#cmbCarTypeID").combobox("setValue", model.CarTypeID);
    $("#cmbCarModelID").combobox("setValue", model.CarModelID);
    $("#spanPlateNo").html(model.PlateNo);
    $("#spanCarTypeID").html(model.CarTypeName);
    $("#spanCarModelID").html(model.CarModelName);
    $("#spanApplyName").html(model.ApplyName);
    $("#spanApplyMoblie").html(model.ApplyMoblie);
    $("#spanPKLot").html(model.PKLot);
    $("#spanFamilyAddress").html(model.FamilyAddress);
    $("#spanApplyRemark").html(model.ApplyRemark);
    $("#spanApplyDateTime").html(model.ApplyDateTime);
    $("#txtAuditRemark").textbox("getValue", model.ApplyRemark);
    $("#txtRefusedAuditRemark").textbox("getValue","");
}
//弹出编辑对话框
function AddOrUpdate(title) {
    $('#divMonthlyCarApplyBox').show().dialog({
        title: title,
        width: 350,
        height: 420,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divMonthlyCarApplyBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var selectareas = $("#cmbAreaIDS").combobox('getValues').toString();
                    if ($.trim(selectareas) == "") {
                        $.messager.alert('系统提示', '请选择车场区域!', 'error');
                        return false;
                    }
                    $("#hiddAreaIDS").val(selectareas);

                    var selectgates = $("#cmbGateID").combobox('getValues').toString();
                    if ($.trim(selectgates) == "") {
                        $.messager.alert('系统提示', '请选择车场通道!', 'error');
                        return false;
                    }
                    $("#hiddGateID").val(selectgates);
                    $.messager.progress({ text: '正在保存....', interval: 100 });
                    $.ajax({
                        type: "post",
                        url: '/p/AduitMonthlyCarApply/Passed',
                        data: $("#divMonthlyCarApplyBoxForm").serialize(),
                        error: function () {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                        },
                        success: function (data) {
                            $.messager.progress("close");
                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: "审核成功",
                                    title: "系统提示"
                                });
                                $.messager.progress("close");
                                $('#divMonthlyCarApplyBox').dialog('close');
                                Refresh();

                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                }
            }]

    });
}
function Delete() {
    var model = $('#tableData').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要审核的数据!");
        return;
    }
    $("#hiddRefusedRecordID").val(model.RecordID);
    $("#txtRefusedAuditRemark").textbox("getValue", model.ApplyRemark);

    $('#divRefusedApplyBox').show().dialog({
        title: "拒绝申请",
        width: 350,
        height: 200,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divRefusedApplyBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $.messager.progress({ text: '正在保存....',interval: 100});
                    $.ajax({
                        type: "post",
                        url: '/p/AduitMonthlyCarApply/Refused',
                        data: $("#divRefusedApplyBoxForm").serialize(),
                        error: function () {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                        },
                        success: function (data) {
                            $.messager.progress("close");
                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: "拒绝成功",
                                    title: "系统提示"
                                });
                                $.messager.progress("close");
                                $('#divRefusedApplyBox').dialog('close');
                                Refresh();

                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                   
                }
            }]

    });
}
function Refresh() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    var applyInfo = $("#txtQueryApplyInfo").val();
    var status = $("#sltStatus").val();
    var starttime = $("#txtStartTime").datetimebox("getValue");
    var endtime = $("#txtEndTime").datetimebox("getValue");
  
    $('#tableData').datagrid('options').queryParams = { parkingId: park.id, applyInfo: applyInfo, status: status, starttime: starttime, endtime: endtime };
    $('#tableData').datagrid('reload');
}