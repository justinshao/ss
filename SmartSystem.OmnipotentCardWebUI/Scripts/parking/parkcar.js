$(function () {
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                $('#tableCar').datagrid('load', { parkingId: node.id });

                BindCarType(node.id);
                BindCarModel(node.id);
                BindAreaComboboxByParkingId(node.id);
                BindGateComboboxByParkingId(node.id);
                BindImportAreaComboboxByParkingId(node.id);
                BindImportGateComboboxByParkingId(node.id);
                BindParkCarBitGroupByParkingId(node.id);


            }
        }
    });

    BindCarListData();
    BindState();
    BindPlateColor();

    $("#txtRenewalRechargeMoney").keyup(function () {
        ComputeRechargeTotalMoney();
    });
    $("#txtRenewalRechargeMoney").change(function () {
        ComputeRechargeTotalMoney();
    });
    $("#sltRenewalMonth").change(function () {
        SelectRenewalMonth($(this).val());
    });
    $("#sltRenewalSeason").change(function () {
        SelectRenewalSeason($(this).val());
    });
    $("#sltRenewalYear").change(function () {
        SelectRenewalYear($(this).val());
    });
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
function BindCarType(parkingId) {
    $.ajax({
        url: '/p/ParkCar/GetParkCarTypeData',
        data: "parkingId=" + parkingId + "",
        type: 'post',
        success: function (data) {
            var comdata = [{ 'text': '所有', 'id': '' }];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].CarTypeName, "id": data[i].CarTypeID });
            }
            $("#cmbQueryCarType").combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text'
            });
            $("#cmbCarTypeID").combobox({
                data: data,
                valueField: 'CarTypeID',
                textField: 'CarTypeName'
            });
            $("#sltImportCarTypeID").combobox({
                data: data,
                valueField: 'CarTypeID',
                textField: 'CarTypeName'
            });

        }
    });
}
function BindState() {
    $.ajax({
        url: '/p/ParkCar/GetParkCarStateData',
        type: 'post',
        success: function (data) {
            var comdata = [{ 'text': '所有', 'id': '-2' }];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].Description, "id": data[i].EnumValue });
            }
            comdata.push({ "text": "过期", "id": "-1" });
            $('#cmbQueryState').combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text'
            });
        }
    });
}
function BindPlateColor() {
    $('#cmbColor').combobox({
        url: '/p/ParkCar/GetPlateColorData',
        valueField: 'EnumValue',
        textField: 'Description'
    });
}
function BindCarModel(parkingId) {
    $.ajax({
        url: '/p/ParkCar/GetParkCarModelData',
        data: "parkingId=" + parkingId + "",
        type: 'post',
        success: function (data) {
            var comdata = [{ 'text': '所有', 'id': '' }];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].CarModelName, "id": data[i].CarModelID });
            }
            $('#cmbQueryCarModel').combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text'
            });
            $('#cmbCarModelID').combobox({
                data: data,
                valueField: 'CarModelID',
                textField: 'CarModelName'
            });
            $('#sltImportCarModelID').combobox({
                data: data,
                valueField: 'CarModelID',
                textField: 'CarModelName'
            });
        }
    });
}
var IsCheckFlag = true;
function BindCarListData() {
    $('#tableCar').datagrid({
        url: '/p/ParkCar/GetParkCarData',
        singleSelect: false,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        checkOnSelect: false,
        onSelect: function (rowIndex, rowData) {
            var basetypeid = rowData.CarBaseTypeID;
            if (basetypeid == "1") {
                //储值卡
                $("#btnrenewal").find(".l-btn-text").text("车辆续费");
            } else {

                $("#btnrenewal").find(".l-btn-text").text("车辆续期");
            }
            ShowOperatOption(basetypeid, rowData.State);
        },
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, checkbox: true },
                    { field: 'GID', title: 'GID', width: 80, hidden: true },
                    { field: 'DefaultSelectStartDate', title: 'DefaultSelectStartDate', width: 80, hidden: true },
                    { field: 'MaxCanSelectStartDate', title: 'MaxCanSelectStartDate', width: 80, hidden: true },
                    { field: 'CardID', title: 'CardID', width: 80, hidden: true },
                    { field: 'PKID', title: 'PKID', width: 80, hidden: true },
                    { field: 'SuspendPlanDate', title: 'SuspendPlanDate', width: 80, hidden: true },
                    { field: 'EmployeeID', title: 'EmployeeID', width: 80, hidden: true },
                    { field: 'PlateID', title: 'PlateID', width: 80, hidden: true },
                    { field: 'CarTypeID', title: 'CarTypeID', width: 80, hidden: true },
                    { field: 'CarBaseTypeID', title: 'CarBaseTypeID', width: 80, hidden: true },
                    { field: 'CarModelID', title: 'CarModelID', width: 80, hidden: true },
                    { field: 'CarMonthlyRentAmount', title: 'CarMonthlyRentAmount', width: 80, hidden: true },
                    { field: 'CardNo', title: '凭证编码', width: 100, hidden: true },
                    { field: 'CardNumber', title: '凭证物理编号', width: 100, hidden: true },
                    { field: 'PlateNo', title: '车牌号', width: 70 },
                    { field: 'Color', title: 'Color', width: 80, hidden: true },
                    { field: 'ColorDes', title: '车牌颜色', width: 60 },
                    { field: 'State', title: 'State', width: 80, hidden: true },
                    { field: 'PKLot', title: 'PKLot', width: 100, hidden: true },
                    { field: 'PKLotNum', title: 'PKLotNum', width: 100, hidden: true },
        //                    { field: 'BitGroupRecordID', title: 'BitGroupRecordID', width: 100, hidden: true },
                    {
                    field: 'StateDes', title: '状态', width: 50, formatter: function (value, row, index) {
                        if (row.CarBaseTypeID == 3) {
                            return "正常";
                        }
                        if (value == "过期") {
                            return " <span style='color:Red;font-weight:600'>过期</span>";
                        } else if (value == "停止") {
                            return " <span style='color:#0000CD;font-weight:600'>停止</span>";
                        }
                        else if (value == "暂停") {
                            return " <span style='color:#D02090;font-weight:600'>暂停</span>";
                        }
                        else {
                            return value;
                        }
                    }
                },
                    { field: 'CarModelName', title: '车型', width: 60 },
                    { field: 'CarTypeName', title: '车类', width: 70 },
                    { field: 'EmployeeName', title: '车主名称', width: 60, formatter: function (value, row, index) {
                        var a = row.EmployeeName;
                        var b = a.substr(0, 1) + '**';
                        return b;
                    }
                    },
                    { field: 'Phone', title: '车主电话', width: 80, formatter: function (value, row, index) {
                        var a = row.Phone;
                        var b = a.substr(0, 3) + '****' + a.substr(7);
                        return b;
                    }
                    },
                    {
                        field: 'Balance', title: '余额', width: 50, formatter: function (value, row, index) {
                            if (row.CarBaseTypeID == 1) {
                                return value;
                            } else {
                                return "";
                            }
                        }
                    },
                    { field: 'PKLotDes', title: '车位组', width: 60 },
                    {
                        field: 'BeginDate', title: 'EndDate', hidden: true, width: 80, formatter: function (value, row, index) {
                            if (row.CarBaseTypeID == 1 || row.CarBaseTypeID == 3) {
                                return "";
                            } else {
                                return value;
                            }
                        }
                    },
                    {
                        field: 'EndDate', title: 'EndDate', hidden: true, width: 80, formatter: function (value, row, index) {
                            if (row.CarBaseTypeID == 1 || row.CarBaseTypeID == 3) {
                                return "";
                            } else {
                                return value;
                            }
                        }
                    },
                     {
                         field: 'DateDes', title: '有效期', width: 150, formatter: function (value, row, index) {
                             if (row.CarBaseTypeID == 3) {
                                 return "";
                             }
                             else if (row.CarBaseTypeID == 1 && $.trim(row.EndDate) != "") {
                                 return " 至 " + row.EndDate;
                             }
                             else if (row.BeginDate != "" && row.EndDate != "") {
                                 return row.BeginDate + " 至 " + row.EndDate;
                             }
                             return "";
                         }
                     },
                    {
                        field: 'FamilyAddr', title: '家庭地址', width: 100, formatter: function (value, row, index) {
                            if (value == null) {
                                return "";
                            }
                            if (value.length <= 7) {
                                return value;
                            } else {
                                var v = value.substr(0, 7);
                                return " <span title='" + value + "'>" + v + "</span>";
                            }
                        }
                    },
                    {
                        field: 'Remark', title: '备注', width: 100, formatter: function (value, row, index) {
                            if (value == null) {
                                return "";
                            }
                            if (value.length <= 7) {
                                return value;
                            } else {
                                var v = value.substr(0, 7);
                                return " <span title='" + value + "'>" + v + "</span>";
                            }
                        }
            },
            { field: 'OnlineUnit', title: '线上续期按月数', width: 60, hidden: true },
            { field: 'OnlineUnitDes', title: '线上续期按月数', width: 60, hidden: true },
        ]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableCar').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkCar/GetParkCarOperatePurview', function (result) {
                    $('#tableCar').datagrid("addToolbarItem", result);
                    $("#btnsuspend").hide();
                    $("#btnrestore").hide();
                    $("#btnstop").hide();
                    $("#btnenabled").hide();
                });
            }
        },
        onLoadSuccess: function (data) {
            var car = $('#tableCar').datagrid('getSelected');
            if (car == null) {
                return;
            }
            ShowOperatOption(car.CarBaseTypeID, car.State);
        }
    });
}
function ShowOperatOption(basetypeid, state) {
    $("#btnsuspend").hide();
    $("#btnrestore").hide();
    $("#btnstop").hide();
    $("#btnenabled").hide();

    if (basetypeid == "0" || basetypeid == "2" || basetypeid == "4" || basetypeid == "5" || basetypeid == "6" || basetypeid == "7") {
        if (state == "0") {
            $("#btnsuspend").show();
            $("#btnstop").show();
        } else if (state == "1") {
            $("#btnenabled").show();
        } else if (state == "2") {
            $("#btnrestore").show();
        }
    }

    //    if (basetypeid == "1" || basetypeid == "3") {
    //        if (state == "0") {
    //            $("#btnstop").show();
    //        } else {
    //            $("#btnenabled").show();
    //        }
    //    }
    if (basetypeid == "3") {
        $("#btnrenewal").hide();
    }
    else {
        $("#btnrenewal").show();
    }
}
function btnSubmitQuery() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        return;
    }
    var employeeName = $("#txtQueryEmployeeName").val();
    var plateNo = $("#txtQueryPlateNo").val();
    var pkLot = $("#txtQueryPKLot").val();
    var familyAddr = $("#txtQueryFamilyAddr").val();
    var carType = $("#cmbQueryCarType").combobox("getValue");
    var carModel = $("#cmbQueryCarModel").combobox("getValue");
    var state = $("#cmbQueryState").combobox("getValue");
    var due = $("#cbxDue").prop("checked");
    $('#tableCar').datagrid('load', { parkingId: park.id, EmployeeNameOrMoblie: employeeName, PlateNo: plateNo, CarTypeId: carType, CarModelId: carModel, HomeAddress: familyAddr, State: state, PKLot: pkLot, Due: due });
}
function Add() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#divCarBoxForm').form('clear');
    AddOrUpdateCar("添加车辆信息");
    $('#divCarBoxForm').form('load', {
        PKID: park.id,
        Color: "0",
        CardID: "",
        CarBaseTypeID: "",
        GID: "",
        PKLotNum: 0,
        PKLot: ""
    });
    var arrAreadId = [];
    arrAreadId.push("-1");
    $("#cmbAreaIDS").combobox('setValues', arrAreadId);

    var arrGateId = [];
    arrGateId.push("-1");
    $("#cmbGateID").combobox('setValues', arrGateId);
    //$("#cmbCarBitGroup").combobox("setValue","-1");
}

function Update() {
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择需要修改的车辆信息!");
        return;
    }
    $('#divCarBoxForm').form('clear');
    AddOrUpdateCar("修改车辆信息");
    $('#divCarBoxForm').form('load', {
        GID: car.GID,
        CardID: car.CardID,
        PKID: car.PKID,
        CarBaseTypeID: car.CarBaseTypeID,
        PlateNo: car.PlateNo,
        EmployeeName: car.EmployeeName,
        MobilePhone: car.Phone,
        PKLot: car.PKLot,
        PKLotNum: car.PKLotNum,
        FamilyAddr: car.FamilyAddr,
        Remark: car.Remark
    });
    var pkLot = car.PKLot == null ? "" : car.PKLot;
    $("#cmbCarBitGroup").combobox('setValue', pkLot);

    $("#cmbCarModelID").combobox('setValue', car.CarModelID);
    $("#cmbCarTypeID").combobox('setValue', car.CarTypeID);
    $("#cmbColor").combobox('setValue', car.Color);

    if (car.PlateNo.length >= 5) {
        $("#defaultProvinceId").find(".l-btn-text").text(car.PlateNo.substr(0, 2));
        $("#defaultNumberId").val(car.PlateNo.substr(2));
    }

    var arrAreadId = [];
    if (car.AreaID == "") {
        arrAreadId.push("-1");
    } else {
        var strAreadIds = car.AreaID.split(',');
        for (var i = 0; i < strAreadIds.length; i++) {
            arrAreadId.push(strAreadIds[i]);
        }
    }
    $("#cmbAreaIDS").combobox('setValues', arrAreadId);

    var arrGateId = [];
    if (car.GateID == "") {
        arrGateId.push("-1");
    } else {
        var strGateIds = car.GateID.split(',');
        for (var i = 0; i < strGateIds.length; i++) {
            arrGateId.push(strGateIds[i]);
        }
    }
    $("#cmbGateID").combobox('setValues', arrGateId);


}



function Refresh() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        return;
    }
    var employeeName = $("#txtQueryEmployeeName").val();
    var plateNo = $("#txtQueryPlateNo").val();
    var pkLot = $("#txtQueryPKLot").val();
    var familyAddr = $("#txtQueryFamilyAddr").val();
    var carType = $("#cmbQueryCarType").combobox("getValue");
    var carModel = $("#cmbQueryCarModel").combobox("getValue");
    var state = $("#cmbQueryState").combobox("getValue");

    $('#tableCar').datagrid('options').queryParams = { parkingId: park.id, EmployeeNameOrMoblie: employeeName, PlateNo: plateNo, CarTypeId: carType, CarModelId: carModel, HomeAddress: familyAddr, State: state, PKLot: pkLot };
    $('#tableCar').datagrid('reload');
}
function AddOrUpdateCar(title) {
    $('#divCarBox').show().dialog({
        title: title,
        width: 550,
        height: 360,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divCarBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divCarBoxForm').form('validate')) {

                    if (!CheckSubmitData()) {
                        return;
                    }
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    var park = $('#parkingTree').tree("getSelected");
                    if (park == null || park.attributes.type != 2) {
                        $.messager.alert('系统提示', '请选择停车场!', 'error');
                        $.messager.progress("close");
                        return;
                    }
                    var grantId = $("#hiddEditGID").val();
                    var carTypeId = $("#cmbCarTypeID").combobox('getValue');
                    var parkingLot = $("#txtPKLot").val();
                    var parkingResult = true;
                    var confirmMsg = "";
                    if ($.trim(parkingLot) != "") {
                        $.ajax({
                            type: "post",
                            url: '/p/ParkCar/CheckSubmitPkLot',
                            data: "grantId=" + grantId + "&parkingId=" + park.id + "&pkLots=" + parkingLot + "&carTypeId=" + carTypeId + "",
                            async: false,
                            error: function () {
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                                $.messager.progress("close");
                                parkingResult = false;
                            },
                            success: function (data) {
                                if (data.result) {
                                    parkingResult = true;
                                    confirmMsg = data.msg;
                                } else {
                                    $.messager.alert('系统提示', data.msg, 'error');
                                    $.messager.progress("close");
                                    parkingResult = false;
                                }
                            }
                        });
                    }
                    if (!parkingResult) {
                        return;
                    }
                    if (confirmMsg != null && confirmMsg != "") {
                        $.messager.confirm('是否保存？', confirmMsg,
                        function (r) {
                            if (r) {
                                ConfirmSubmitSaveCar();
                            } else {
                                $.messager.progress("close");
                            }
                        });
                    } else {
                        ConfirmSubmitSaveCar();
                    }
                }
            }
        }]

    });
}
function ConfirmSubmitSaveCar() {
    $.ajax({
        type: "post",
        url: '/p/ParkCar/SaveParkCar',
        data: $("#divCarBoxForm").serialize(),
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
                $('#divCarBox').dialog('close');
                Refresh();
                var park = $('#parkingTree').tree("getSelected");
                if (park != null && park.attributes.type == 2) {
                    BindParkCarBitGroupByParkingId(park.id);
                }


            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function CheckSubmitData() {
    var carModelId = $("#cmbCarModelID").combobox('getValue');
    if ($.trim(carModelId) == "") {
        $.messager.alert('系统提示', '请选择车型!', 'error');
        return false;
    }
    var carTypeID = $("#cmbCarTypeID").combobox('getValue');
    if ($.trim(carTypeID) == "") {
        $.messager.alert('系统提示', '请选择车类!', 'error');
        return false;
    }
    var platebefore = $("#defaultProvinceId").find(".l-btn-text").text();
    var plateafter = $("#defaultNumberId").val();
    if ($.trim(platebefore).length != 2 || $.trim(plateafter) == "") {
        $.messager.alert('系统提示', '车牌号码格式不正确!', 'error');
        return false;
    }
    $("#hiddPlateNo").val(platebefore + plateafter);

    var color = $("#cmbColor").combobox('getValue');
    if ($.trim(color) == "") {
        $.messager.alert('系统提示', '请选择车牌颜色!', 'error');
        return false;
    }

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

    var employeeName = $("#txtEmployeeName").val();
    if ($.trim(employeeName) == "") {
        $.messager.alert('系统提示', '请填写车主姓名!', 'error');
        return false;
    }
    var mobilePhone = $("#txtMobilePhone").val();
    if ($.trim(mobilePhone) == "") {
        $.messager.alert('系统提示', '请填写车主电话!', 'error');
        return false;
    }
    return true;
}
function BindAreaComboboxByParkingId(parkingId) {
    $("#cmbAreaIDS").combobox({
        url: '/p/ParkCar/GetAreaDataByParkingId?parkingId=' + parkingId,
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
function BindGateComboboxByAreaId(selectareaids) {
    $("#cmbGateID").combobox({
        url: '/p/ParkCar/GetGateDataByAreaIds?areaIds=' + selectareaids,
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
function BindParkCarBitGroupByParkingId(parkingId) {
    $("#cmbCarBitGroup").combobox({
        url: '/p/ParkCar/GetParkCarBitGroupData?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            InitPkLotNum();
        }
    });

}
function InitPkLotNum() {
    var carBitNum = 0;
    var bitGroup = $("#cmbCarBitGroup").combobox("getText");
    if ($.trim(bitGroup) != "" && $.trim(bitGroup) != "无") {
        if (bitGroup.indexOf('/') > -1) {
            var bits = bitGroup.split('/');
            carBitNum = bits[1].replace("个车位", "");
        } else {
            carBitNum = "0";
        }
    }
    $("#txtPKLotNum").numberspinner("setValue", carBitNum);
}
var parserDate = function (date) {
    date = date.replace('-', '/').replace('-', '/');
    var t = Date.parse(date);
    if (!isNaN(t)) {
        return new Date(Date.parse(date.replace(/-/g, "/")));
    } else {
        return new Date();
    }
};

function Renewal() {
    $("#divManyCarManyLot").html("");
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择车辆信息!");
        return;
    }
    if (car.CarBaseTypeID == "3") {
        $.messager.alert("系统提示", "临时车不能续期或续费");
        return;
    }
    $('#divCarRenewalBoxForm').form('clear');

    var title = car.CarBaseTypeID == "1" ? "车辆续费" : "车辆续期";
    CarRenewalBox(title);

    RenewalShowOption(car.CarBaseTypeID);
    $('#divCarRenewalBoxForm').form('load', {
        GID: car.GID,
        CarBaseTypeID: car.CarBaseTypeID,
        EmployeeName: car.EmployeeName,
        PlateNo: car.PlateNo,
        PKLot: car.PKLot,
        BeginDate: car.BeginDate,
        EndDate: car.EndDate,
        CarMonthlyRentAmount: car.CarMonthlyRentAmount,
        CarSeasonRentAmount: car.CarSeasonRentAmount,
        CarYearRentAmount: car.CarYearRentAmount,
        MonthlyRentToTempNoPayAmount: "0",
        RechargeMoney: 0,
        RenewalMonth: "1",
        MonthToTempAmount: "0",
        PayTotalMoney: "0",
        OriginalMoney: car.Balance
    });
    $('#txtRenewalBeginDate').datebox().datebox('calendar').calendar({
        validator: function (date) {
            var max = parserDate(car.MaxCanSelectStartDate + " 00:00:00");
            return date <= max;
        }
    });
    $('#txtRenewalBeginDate').datebox({
        disabled: false,
        onSelect: function (date) {
            var selectCar = $('#tableCar').datagrid('getSelected');
            var strDate = String.GetDateToyyyyMMdd(date);
            CalculateOriginalEndDate(selectCar.GID, strDate);
            CalculateMonthlyRentExpiredWaitPayAmount(selectCar.GID, selectCar.CarBaseTypeID);
            if (selectCar.CarBaseTypeID == "5") {
                var season = $("#sltRenewalSeason").val();
                SelectRenewalSeason(season);
            }
            if (selectCar.CarBaseTypeID == "6") {
                var year = $("#sltRenewalYear").val();
                SelectRenewalYear(year);
            }
            if (selectCar.CarBaseTypeID == "7") {
                var month = car.OnlineUnit;
                SelectRenewalMonth(month);
            }
            if (selectCar.CarBaseTypeID == "0" || selectCar.CarBaseTypeID == "1" || selectCar.CarBaseTypeID == "2" || selectCar.CarBaseTypeID == "3" || selectCar.CarBaseTypeID == "4") {
                var month = $("#sltRenewalMonth").val();
                SelectRenewalMonth(month);
            }
            $("#hiddRenewalBeginDate").val(strDate);
        }
    });
    $('#txtRenewalNewEndDate').datebox({
        onSelect: function (date) {
            var strDate = String.GetDateToyyyyMMdd(date);
            $("#hiddRenewalNewEndDate").val(strDate);
        }
    });
    //var dateDisabled = car.CarBaseTypeID!=1
    $('#txtRenewalEndDate').datebox({
        disabled: car.CarBaseTypeID != 1
    });
    $("#sltRenewalSeason option:first").prop("selected", 'selected');
    $("#sltRenewalYear option:first").prop("selected", 'selected');
    $("#hiddRenewalNewEndDate").val(car.EndDate);
    $('#txtRenewalEndDate').datebox("setValue", car.EndDate);
    $('#txtRenewalBeginDate').datebox("setValue", car.DefaultSelectStartDate);
    $("#hiddRenewalBeginDate").val(car.DefaultSelectStartDate);
    CalculateOriginalEndDate(car.GID, car.DefaultSelectStartDate);
    if (car.CarBaseTypeID == "5") {
        SelectRenewalSeason("1");
    }
    if (car.CarBaseTypeID == "6") {
        SelectRenewalYear("1");
    }
    if (car.CarBaseTypeID == "0" || car.CarBaseTypeID == "1" || car.CarBaseTypeID == "2" || car.CarBaseTypeID == "3" || car.CarBaseTypeID == "4") {
        SelectRenewalMonth("1");
    }
    if (car.CarBaseTypeID == "7") {
        var month = car.OnlineUnit;
        SelectRenewalMonth(month);
    }
    DynamicSetCarRenewalBoxHeight(car.CarBaseTypeID);
    SetMonthlyRentCarManyLot(car.CarBaseTypeID, car.GID);

    CalculateMonthlyRentExpiredWaitPayAmount(car.GID, car.CarBaseTypeID);
}
function CalculateMonthlyRentExpiredWaitPayAmount(grantId, baseTypeId) {
    $("#MonthCardToTemp").hide();
    if (baseTypeId != 2 && baseTypeId != 5 && baseTypeId != 6 && baseTypeId != 7) {
        return false;
    }
    var startDate = $("#txtRenewalBeginDate").datebox("getValue");
    $.ajax({
        type: "post",
        url: '/p/ParkCar/CalculateMonthlyRentExpiredWaitPayAmount',
        data: "grantId=" + grantId + "&start=" + startDate + "",
        async: false,
        error: function () {
            $.messager.alert('系统提示', '获取月租车转临停未支付金额失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                $(".dialog-button").eq(0).find(".l-btn-small").show();

                if (parseFloat(data.data) > 0) {
                    $(".MonthCardToTemp").show()
                } else {
                    $(".MonthCardToTemp").hide();
                }
                var totalmoney = $("#txtMonthlyRentTotalMoney").val()
                $("#txtMonthlyRentToTempNoPayAmount").val(data.data);
                var paytotalmoney = parseFloat(totalmoney) + parseFloat(data.data);
                $("#txtNeedPayTotalMoney").val(paytotalmoney);
            } else {
                if ($(".dialog-button").eq(0).find(".l-btn-small").length > 1) {
                    $(".dialog-button").eq(0).find(".l-btn-small").eq(1).hide();
                } else {
                    $(".dialog-button").eq(0).find(".l-btn-small").hide();
                }
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function SetMonthlyRentCarManyLot(baseTypeId, grantId) {
    if (baseTypeId == 2 || baseTypeId == 5 || baseTypeId == 6 || baseTypeId == 7) {
        $.ajax({
            type: "post",
            url: '/p/ParkCar/GetMonthlyRentCarManyLot',
            data: "grantId=" + grantId + "&carType=" + baseTypeId + "",
            async: false,
            error: function () {
                $.messager.alert('系统提示', '提交获取多车多位到服务器失败!', 'error');
            },
            success: function (data) {
                if (data.result) {
                    if (data.data.length > 0) {
                        $(".trManyCarManyLot").show();
                        $('#divCarRenewalBox').dialog({ height: 500 });
                        for (var i = 0; i < data.data.length; i++) {
                            $tr = $("<div><input type=\"hidden\" class=\"manyCarManyLotGrantId\" vlaue=\"" + data.data[i].GID + "\"/>车牌号：" + data.data[i].PlateNumber + ",有效期至：" + data.data[i].EndDate + "</div>");
                            $("#divManyCarManyLot").append($tr);
                        }
                    }
                } else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }
            }
        });
    }
}
function DynamicSetCarRenewalBoxHeight(baseTypeId) {
    baseTypeId = parseInt(baseTypeId);
    $('#divCarRenewalBox').dialog({ height: 450 });
    if (baseTypeId == 2 || baseTypeId == 5 || baseTypeId == 6 || baseTypeId == 7) {
        $('#divCarRenewalBox').dialog({ height: 450 });
    }
    if (baseTypeId == 4 || baseTypeId == 0) {
        $('#divCarRenewalBox').dialog({ height: 350 });
    }
    if (baseTypeId == 1) {
        $('#divCarRenewalBox').dialog({ height: 350 });
    }
}
function RenewalShowOption(basetypeid) {
    $(".trRenewal").hide();
    $("#spanEndDateDes").text("原有效期结束日期");
    switch (parseInt(basetypeid)) {
        case 0:
        case 4:
            {
                $(".VipCard").show();
                break;
            }
        case 2:
            {
                $(".MonthCard").show();
                break;
            }
        case 3:
            {
                $(".TempCard").show();
                $(".MonthNumber").hide();
                $(".NewLimitEnd").show();
                break;
            }
        case 5:
            {
                $(".SeasonCard").show();
                break;
            }
        case 6:
            {
                $(".YearCard").show();
                break;
            }
        case 7:
            {
                $(".CustomCard").show();
                break;
            }
        default:
            {
                $("#spanEndDateDes").text("有效结束期");
                $(".Recharge").show();
            }
    }
}
function CalculateOriginalEndDate(grantId, startDate) {
    $.ajax({
        type: "post",
        url: '/p/ParkCar/CalculateOriginalEndDate',
        data: "grantId=" + grantId + "&start=" + startDate + "",
        async: false,
        error: function () {
            $.messager.alert('系统提示', '获取原始有效期失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                if (data.data != null) {
                    $("#txtRenewalEndDate").val(data.data);
                }
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });
}
function SelectRenewalMonth(month) {
    var monthlyRentAmount = parseInt($("#txtRenewalCarMonthlyRentAmount").val());
    var pklots = $("#txtRenewalPKLot").val();
    var lotQuantity = 1;
    if ($.trim(lotQuantity) != "") {
        lotQuantity = pklots.split(',').length;
    }
    var intMonth = parseInt(month);
    var monthlyRentTotalMoney = monthlyRentAmount * intMonth * lotQuantity;
    $("#txtMonthlyRentTotalMoney").val(monthlyRentTotalMoney);

    var monthlyRentToTempNoPayAmount = parseFloat($("#txtMonthlyRentToTempNoPayAmount").val());
    var needPayTotalMoney = parseFloat(monthlyRentToTempNoPayAmount) + monthlyRentTotalMoney;
    $("#txtNeedPayTotalMoney").val(needPayTotalMoney);

    var startDate = $("#txtRenewalBeginDate").datebox("getValue");
    var endDate = $("#txtRenewalEndDate").val();
    $.ajax({
        type: "post",
        url: '/p/ParkCar/CalculateNewEndDate',
        data: "startDate=" + startDate + "&endDate=" + endDate + "&month=" + intMonth,
        success: function (data) {
            if (data.result) {
                $("#hiddRenewalNewEndDate").val(data.data);
                $("#txtRenewalNewEndDate").datebox("setValue", data.data);
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });

}
function SelectRenewalSeason(season) {
    var seasonRentAmount = parseInt($("#txtRenewalCarSeasonRentAmount").val());
    var pklots = $("#txtRenewalPKLot").val();
    var lotQuantity = 1;
    if ($.trim(lotQuantity) != "") {
        lotQuantity = pklots.split(',').length;
    }
    var intSeason = parseInt(season);
    var seasonRentTotalMoney = seasonRentAmount * intSeason * lotQuantity;
    $("#txtMonthlyRentTotalMoney").val(seasonRentTotalMoney);

    var seasonRentToTempNoPayAmount = parseFloat($("#txtMonthlyRentToTempNoPayAmount").val());
    var needPayTotalMoney = parseFloat(seasonRentToTempNoPayAmount) + seasonRentTotalMoney;
    $("#txtNeedPayTotalMoney").val(needPayTotalMoney);

    var startDate = $("#txtRenewalBeginDate").datebox("getValue");
    var endDate = $("#txtRenewalEndDate").val();
    $.ajax({
        type: "post",
        url: '/p/ParkCar/CalculateNewEndDate',
        data: "startDate=" + startDate + "&endDate=" + endDate + "&month=" + intSeason * 3,
        success: function (data) {
            if (data.result) {
                $("#hiddRenewalNewEndDate").val(data.data);
                $("#txtRenewalNewEndDate").datebox("setValue", data.data);
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });
}
function SelectRenewalYear(year) {
    var yearRentAmount = parseInt($("#txtRenewalCarYearRentAmount").val());
    var pklots = $("#txtRenewalPKLot").val();
    var lotQuantity = 1;
    if ($.trim(lotQuantity) != "") {
        lotQuantity = pklots.split(',').length;
    }
    var intYear = parseInt(year);
    var yearRentTotalMoney = yearRentAmount * intYear * lotQuantity;
    $("#txtMonthlyRentTotalMoney").val(yearRentTotalMoney);

    var yearRentToTempNoPayAmount = parseFloat($("#txtMonthlyRentToTempNoPayAmount").val());
    var needPayTotalMoney = parseFloat(yearRentToTempNoPayAmount) + yearRentTotalMoney;
    $("#txtNeedPayTotalMoney").val(needPayTotalMoney);

    var startDate = $("#txtRenewalBeginDate").datebox("getValue");
    var endDate = $("#txtRenewalEndDate").val();
    $.ajax({
        type: "post",
        url: '/p/ParkCar/CalculateNewEndDate',
        data: "startDate=" + startDate + "&endDate=" + endDate + "&month=" + intYear * 12,
        success: function (data) {
            if (data.result) {
                $("#hiddRenewalNewEndDate").val(data.data);
                $("#txtRenewalNewEndDate").datebox("setValue", data.data);
            } else {
                $.messager.alert('系统提示', data.msg, 'error');
            }
        }
    });
}
function ComputeRechargeTotalMoney() {
    var omoney = $("#txtRenewalOriginalMoney").val();
    var nmoney = $("#txtRenewalRechargeMoney").val();
    if ($.isNumeric(omoney) && $.isNumeric(nmoney)) {
        if (parseFloat(nmoney) < 0) {
            $("#txtRenewalRechargeMoney").val("0")
            nmoney = 0;
        }
        var total = parseFloat(omoney) + parseFloat(nmoney);
        $("#txtRechargeAfterMoney").val(total);
    }
}

function CarRenewalBox(title) {
    $('#divCarRenewalBox').show().dialog({
        title: title,
        width: 400,
        top: 40,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divCarRenewalBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {

                if ($('#divCarRenewalBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    if (!CheckCarRenewalSumbitData()) {
                        $.messager.progress("close");
                        return;
                    }
                    $.ajax({
                        type: "post",
                        url: '/p/ParkCar/CarRenewal',
                        data: $("#divCarRenewalBoxForm").serialize(),
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
                                    title: "续期"
                                });
                                $.messager.progress("close");
                                $('#divCarRenewalBox').dialog('close');
                                Refresh();
                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '保存数据失败,' + data.msg + '!', 'error');

                            }
                        }
                    });
                }
            }
        }]

    });
}
var intReg = /^[0-9]*$/
function CheckCarRenewalSumbitData() {
    var basetypeid = $("#hiddRenewalCarBaseTypeID").val();
    if (basetypeid == "0" || basetypeid == "4" || basetypeid == "2" || basetypeid == "5" || basetypeid == "6" || basetypeid == "7") {
        var startDate = $("#txtRenewalBeginDate").datebox("getValue");
        if ($.trim(startDate) == "") {
            $.messager.alert('系统提示', '请选择开始生效日期!', 'error');
            return false;
        }
        if ($("#MonthCard").css("display") != "none") {
            var month = $("#sltRenewalMonth").val();
            if ($.trim(month) == "") {
                $.messager.alert('系统提示', '请选择您要续期月数!', 'error');
                return false;
            }
        }
        if ($("#SeasonCard").css("display") != "none") {
        var season = $("#sltRenewalSeason").val();
        if ($.trim(season) == "") {
            $.messager.alert('系统提示', '请选择您要续期季数!', 'error');
            return false;
            }
        }
        if ($("#YearCard").css("display") != "none") {
            var year = $("#sltRenewalYear").val();
            if ($.trim(year) == "") {
                $.messager.alert('系统提示', '请选择您要续期年数!', 'error');
                return false;
            }
        }
        var newend = $("#txtRenewalNewEndDate").datebox("getValue");
        if ($.trim(newend) == "") {
            $.messager.alert('系统提示', '请选择新有效结束日期!', 'error');
            return false;
        }
        $("#hiddRenewalNewEndDate").val(newend);
    }
    else if (basetypeid == "3") {
        //临时卡
        var newend = $('#txtRenewalNewEndDate').val();
        if (newend == null || $.trim(newend) == "") {
            $.messager.alert('系统提示', '请选择新有效结束日期!', 'error');
            return false;
        }
    } else {
        //充值类型卡
        var money = $("#txtRenewalRechargeMoney").val();
        if ($.trim(money) == "" || parseInt(money) < 0) {
            $.messager.alert('系统提示', '充值金额格式不正确!', 'error');
            return false;
        }
    }
    return true;
}
//function Delete() {
//    var car = $('#tableCar').datagrid('getSelected');
//    if (car == null) {
//        $.messager.alert("系统提示", "请选择需要删除的车辆!");
//        return;
//    }
//    $.messager.confirm('系统提示', '确定要删除选中的车辆吗?',
//    function (r) {
//        if (r) {
//            $.post('/p/ParkCar/CancelParkGrant?grantId=' + car.GID,
//            function (data) {
//                if (data.result) {
//                    $.messager.show({
//                        width: 200,
//                        height: 100,
//                        msg: '删除车辆成功',
//                        title: "删除车辆"
//                    });
//                    Refresh();
//                } else {
//                    $.messager.alert('系统提示', data.msg, 'error');
//                }

//            });
//        }
//    });
//}
function Delete() {

    var selectrow = $("#tableCar").datagrid("getChecked");//获取的是数组，多行数据 
    var ids = "";
    for (var i = 0; i < selectrow.length; i++) {
        if (ids == "") ids = ids + selectrow[i].GID;
        else
            ids = ids + "," + selectrow[i].GID;
    }
    if (ids == "") {
        $.messager.alert("系统提示", "请选择需要删除的车辆!");
        return;
    }
    $.messager.confirm('系统提示', '确定要删除选中的车辆吗?',
      function (r) {
          if (r) {
              $.post('/p/ParkCar/CancelAllParkGrant?grantId=' + ids,
               function (data) {
                   if (data.result) {
                       $.messager.show({
                           width: 200,
                           height: 100,
                           msg: '批量删除车辆成功',
                           title: "批量删除车辆"
                       });
                       var park = $('#parkingTree').tree("getSelected");
                       if (park == null || park.attributes.type != 2) {
                           return;
                       }
                       var employeeName = $("#txtQueryEmployeeName").val();
                       var plateNo = $("#txtQueryPlateNo").val();
                       var pkLot = $("#txtQueryPKLot").val();
                       var familyAddr = $("#txtQueryFamilyAddr").val();
                       var carType = $("#cmbQueryCarType").combobox("getValue");
                       var carModel = $("#cmbQueryCarModel").combobox("getValue");
                       var state = $("#cmbQueryState").combobox("getValue");

                       $('#tableCar').datagrid('options').queryParams = { parkingId: park.id, EmployeeNameOrMoblie: employeeName, PlateNo: plateNo, CarTypeId: carType, CarModelId: carModel, HomeAddress: familyAddr, State: state, PKLot: pkLot };
                       $('#tableCar').datagrid('load');
                       $('#tableCar').datagrid('clearSelections');
                       ids = "";
                   } else {
                       $.messager.alert('系统提示', data.msg, 'error');
                   }
               });
          }
      });
}
function Suspend() {
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择待暂停的车辆!");
        return;
    }
    AddSuspendDate("暂停使用");
    $('#divCarSuspendBoxForm').form('load', {
        grantId: car.GID,
        CarBaseTypeID: car.CarBaseTypeID
    });
    if ($.trim(car.SuspendPlanDate) != "") {
        var dates = car.SuspendPlanDate.split('|');
        $("#txtstart").datebox("setValue", dates[0]);
        $("#txtend").datebox("setValue", dates[1]);
    } else {
        $("#txtstart").datebox("setValue", "");
        $("#txtend").datebox("setValue", "");
    }
}
function Restore() {
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择车辆信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定要恢复使用吗?',
    function (r) {
        if (r) {
            $.post('/p/ParkCar/CarRestoreUse?grantId=' + car.GID,
            function (data) {
                if (data.result) {
                    $.messager.show({ width: 200, height: 100, msg: '恢复成功', title: "系统提示" });
                    Refresh();
                } else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }

            });
        }
    });
}
function Stop() {
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择车辆信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定要停用吗?',
    function (r) {
        if (r) {
            $.post('/p/ParkCar/CarStopUse?grantId=' + car.GID,
            function (data) {
                if (data.result) {
                    $.messager.show({ width: 200, height: 100, msg: '停用成功', title: "系统提示" });
                    Refresh();
                } else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }

            });
        }
    });
}
function Enabled() {
    var car = $('#tableCar').datagrid('getSelected');
    if (car == null) {
        $.messager.alert("系统提示", "请选择车辆信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定要重新启用吗?',
    function (r) {
        if (r) {
            $.post('/p/ParkCar/CarAgainEnabledUse?grantId=' + car.GID,
            function (data) {
                if (data.result) {
                    $.messager.show({ width: 200, height: 100, msg: '启用成功', title: "系统提示" });
                    Refresh();
                } else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }

            });
        }
    });
}
AddSuspendDate = function (title) {
    $('#divCarSuspendBox').show().dialog({
        title: title,
        width: 350,
        height: 230,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divCarSuspendBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {

                if ($('#divCarSuspendBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkCar/CarSuspendUse',
                        data: $("#divCarSuspendBoxForm").serialize(),
                        error: function () {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                        },
                        success: function (data) {
                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '设置成功!',
                                    title: "暂停设置"
                                });
                                $.messager.progress("close");
                                $('#divCarSuspendBox').dialog('close');
                                Refresh();
                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '保存数据失败,' + data.msg + '!', 'error');

                            }
                        }
                    });
                }
            }
        }]

    });
}
function Import() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $('#divCarImportBoxForm').form('clear');

    UploadDivBox("导入车辆信息");
    $('#divCarImportBoxForm').form('load', {
        PKID: park.id,
        CarFilePath: ""
    });
    $("#spanUploadFileResult").html("");
}
function UploadDivBox(title) {
    $('#divCarImportBox').show().dialog({
        title: title,
        width: 400,
        height: 300,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divCarImportBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {

                if (!CheckImportCarData()) {
                    return false;
                }
                $.messager.progress({
                    text: '正在保存....',
                    interval: 100
                });
                $.ajax({
                    type: "post",
                    url: '/p/ParkCar/SaveImportCar',
                    data: $("#divCarImportBoxForm").serialize(),
                    error: function () {
                        $.messager.progress("close");
                        $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                    },
                    success: function (data) {
                        if (data.result) {
                            if ($.trim(data.msg) == "") {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '批量导入车辆信息成功!',
                                    title: "批量导入"
                                });
                            } else {
                                $.messager.alert('系统提示', data.msg, 'error');
                            }
                            $("#hiddCarFilePath").val("");
                            $("#spanUploadFileResult").html("");
                            $('#divCarImportBox').dialog('close');
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }
                        $.messager.progress("close");
                    }
                });
            }
        }]

    });
}
function CheckImportCarData() {
    var carTypeId = $("#sltImportCarTypeID").combobox("getValue");
    if ($.trim(carTypeId) == "") {
        $.messager.alert('系统提示', '请选择车类!', 'error');
        return false;
    }
    var carModelId = $("#sltImportCarModelID").combobox("getValue");
    if ($.trim(carModelId) == "") {
        $.messager.alert('系统提示', '请选择车型!', 'error');
        return false;
    }
    var selectareas = $("#sltImportAreaIDS").combobox('getValues').toString();
    if ($.trim(selectareas) == "") {
        $.messager.alert('系统提示', '请选择车场区域!', 'error');
        return false;
    }
    $("#hiddImportAreaIDS").val(selectareas);
    var selectgates = $("#sltImportGateID").combobox('getValues').toString();
    if ($.trim(selectgates) == "") {
        $.messager.alert('系统提示', '请选择车场车场通道!', 'error');
        return false;
    }
    $("#hiddImportGateID").val(selectgates);
    var filepath = $("#hiddCarFilePath").val();
    if ($.trim(filepath) == "") {
        $.messager.alert('系统提示', '请选择文件!', 'error');
        return false;
    }
    return true;
}
function BindImportAreaComboboxByParkingId(parkingId) {
    $("#sltImportAreaIDS").combobox({
        url: '/p/ParkCar/GetAreaDataByParkingId?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#sltImportAreaIDS").combobox('setValues', arr);
                BindImportGateComboboxByParkingId(parkingId);
            } else {
                var selectvalues = $("#sltImportAreaIDS").combobox('getValues').toString();
                BindImportGateComboboxByAreaId(selectvalues);
                var arealist = selectvalues.split(',');
                for (var g = 0; g < arealist.length; g++) {
                    if (arealist[g] == "-1") {
                        $("#sltImportAreaIDS").combobox('unselect', "-1");
                    }
                }
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#sltImportAreaIDS").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                var arr = [];
                arr.push("-1");
                $("#sltImportAreaIDS").combobox('setValues', arr);
                BindImportGateComboboxByParkingId(parkingId);
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
                BindImportGateComboboxByAreaId(values);
            }

        }
    });

}
function BindImportGateComboboxByAreaId(selectareaids) {
    $("#sltImportGateID").combobox({
        url: '/p/ParkCar/GetGateDataByAreaIds?areaIds=' + selectareaids,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#sltImportGateID").combobox('setValues', arr);
            } else {
                $("#sltImportGateID").combobox('unselect', "-1");
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#sltImportGateID").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                $("#sltImportGateID").combobox('setValue', "-1");
            }
        }
    });
}
function BindImportGateComboboxByParkingId(parkingId) {
    $("#sltImportGateID").combobox({
        url: '/p/ParkCar/GetGateDataByParkingId?parkingId=' + parkingId,
        valueField: 'id',
        textField: 'text',
        onSelect: function (node) {
            if (node.id == "-1") {
                var arr = [];
                arr.push("-1");
                $("#sltImportGateID").combobox('setValues', arr);
            } else {
                $("#sltImportGateID").combobox('unselect', "-1");
            }
        },
        onUnselect: function (node) {
            var selectvalues = $("#sltImportGateID").combobox('getValues').toString();
            if ($.trim(selectvalues) == "") {//区域没有选中项就默认选择所有选项
                $("#sltImportGateID").combobox('setValue', "-1");
            }
        }
    });

}
function btnSelectImportFile() {
    $("#carImportFile").click();
}
$(function () {
    $("#carImportFile").fileupload({
        url: '/p/ParkCar/SaveExeclFile',
        progress: function (e, data) {
            $.messager.progress({
                text: '获取图片中....',
                interval: 100
            });
        },
        done: function (e, result) {
            if (result.result.result) {
                $("#spanUploadFileResult").text("获取文件成功").css("color", "#000000");
                $("#hiddCarFilePath").val(result.result.data);
            } else {
                $("#hiddCarFilePath").val("");
                $("#spanUploadFileResult").text(result.result.msg).css("color", "Red");
            }
            $.messager.progress("close");
        }
    })
});
function btnEditCarBitGroup() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    $("#hiddParkCarBitGroupPKID").val(park.id);
    var bitGroup = $("#cmbCarBitGroup").combobox("getText");
    var carBitName = "", carBitNum = "0";
    if ($.trim(bitGroup) != "" && $.trim(bitGroup) != "无") {
        if (bitGroup.indexOf('/') > -1) {
            var bits = bitGroup.split('/');
            carBitName = bits[0];
            carBitNum = bits[1].replace("个车位", "");
        } else {
            carBitName = bitGroup;
            carBitNum = "0";
        }
    }
    $("#txtCarBitName").val(carBitName);
    $("#txtCarBitNum").numberspinner("setValue", carBitNum);
    $('#divParkCarBitGroupBox').show().dialog({
        title: "编辑车位组",
        width: 400,
        height: 200,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkCarBitGroupBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                var carBitName = $("#txtCarBitName").val();
                if ($.trim(txtCarBitName) == "") {
                    $.messager.alert('系统提示', '车位组不能为空!', 'error');
                    return false;
                }
                $.messager.progress({
                    text: '正在保存....',
                    interval: 100
                });
                $.ajax({
                    type: "post",
                    url: '/p/ParkCar/SaveParkCarBitGroup',
                    data: $("#divParkCarBitGroupBoxForm").serialize(),
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
                                title: "系统提示"
                            });
                            Refresh();
                            if ($.trim(data.data) != "" && $.trim(data.msg) != "") {
                                //添加
                                var carBitGroupData = $("#cmbCarBitGroup").combobox("getData");
                                carBitGroupData.push({ text: data.data.toString(), id: data.msg, selected: true });
                                $("#cmbCarBitGroup").combobox("loadData", carBitGroupData);
                                InitPkLotNum();
                            } else if ($.trim(data.msg) != "") {
                                //修改
                                var parkingId = $("#hiddParkCarBitGroupPKID").val();
                                $("#cmbCarBitGroup").combobox({
                                    url: '/p/ParkCar/GetParkCarBitGroupData?parkingId=' + parkingId,
                                    valueField: 'id',
                                    textField: 'text',
                                    onLoadSuccess: function () {
                                        $("#cmbCarBitGroup").combobox("setValue", data.msg);
                                    }
                                });

                            }

                            $('#divParkCarBitGroupBox').dialog('close');
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }
                        $.messager.progress("close");
                    }
                });
            }
        }]

    });
}
function Export() {
    var park = $('#parkingTree').tree("getSelected");
    if (park == null || park.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择停车场!', 'error');
        return;
    }
    var employeeName = $("#txtQueryEmployeeName").val();
    var plateNo = $("#txtQueryPlateNo").val();
    var pkLot = $("#txtQueryPKLot").val();
    var familyAddr = $("#txtQueryFamilyAddr").val();
    var carType = $("#cmbQueryCarType").combobox("getValue");
    var carModel = $("#cmbQueryCarModel").combobox("getValue");
    var state = $("#cmbQueryState").combobox("getValue");
    var due = $("#cbxDue").prop("checked");
    $.ajax({
        type: 'post',
        url: '/p/ParkCar/Export_AllCar',
        data: "parkingId=" + park.id + "&EmployeeNameOrMoblie=" + employeeName + "&PlateNo=" + plateNo + "&CarTypeId=" + carType + "&CarModelId=" + carModel + "&HomeAddress=" + familyAddr + "&State=" + state + "&PKLot=" + pkLot + "&Due=" + due,
        async: true,
        success: function (data) {
            window.open(data);
        },
        error: function (data) {
            alert("导出失败");
        }
    });
}