$(function () {
    $("#ckbTwoCameraWait").change(function () {
        ShowCameraIsWaitTime("1");
    });
    $('#parkingTree').tree({
        url: '/ParkingData/GetParkingTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 2) {
                CurrentSelectParking(node.id);
            }
        }
    });
    $('#tableArea').treegrid({
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/p/ParkArea/GetParkAreaData',
        idField: 'AreaID',
        treeField: 'AreaName',
        columns: [[
                    { title: 'ID', field: 'id', width: 80, hidden: true },
                    { title: 'AreaID', field: 'AreaID', width: 80, hidden: true },
                    { title: 'PKID', field: 'PKID', width: 80, hidden: true },
                    { title: 'MasterID', field: 'MasterID', width: 80, hidden: true },
                    { field: 'AreaName', title: '名称', width: 80 },
                    { field: 'CarbitNum', title: '车位数', width: 80 },
                    { field: 'NeedToll', title: '是否收费', width: 40, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'TwoCameraWait', title: '双摄像头是否等待', width: 80, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                      { field: 'CameraWaitTime', title: '等待时间（秒）', width: 60, formatter: function (value, row, index) {
                          if (row.TwoCameraWait == 1) {
                              return value;
                          } else {
                              return '';
                          }
                      }
                      },
                    { field: 'Remark', title: '备注', width: 80 }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableArea').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkArea/GetParkKAreaOperatePurview', function (result) {
                    $('#tableArea').datagrid("addToolbarItem", result);
                });
            }
        }
    });

});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkingTree").tree("search", content);
}
function CurrentSelectParking(parkingId) {
    $.post('/p/ParkArea/GetParkAreaData?parkingId=' + parkingId, function (data) {
        $('#tableArea').treegrid('loadData', data);
    }, 'json');
}
function ShowCameraIsWaitTime(time) {
    if ($("#ckbTwoCameraWait").is(":checked")) {
        $("#spanWaitTime").show();
        $("#txtCameraWaitTime").val(time);
    } else {
        $("#spanWaitTime").hide();
    }
}
//增加区域
function Add() {
    var parking = $('#parkingTree').tree('getSelected');
    if (parking == null || parking.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择车场信息', 'error');
        return;
    }
    AddOrUpdate('增加车场区域');

    $.post('/p/ParkArea/GetAreaTreeData?parkingId=' + parking.id, function (data) {
        $('#MasterID').combotree({
            data: data,
            onLoadSuccess: function () {
                var selectArea = $('#tableArea').treegrid('getSelected');
                if (selectArea != null && selectArea.PKID == parking.id) {
                    if ($.trim(selectArea.MasterID) == "") {
                        $('#MasterID').combotree("setValue", selectArea.AreaID);
                    } else {
                        $('#MasterID').combotree("setValue", selectArea.MasterID);
                    }

                } else {
                    $('#MasterID').combotree("setValue", "");
                }
            }
        });
    }, 'json');


    $('#divParkAreaBoxForm').form('load', {
        AreaID: '',
        PKID: parking.id,
        AreaName: '',
        ParkingName: parking.text,
        CarbitNum: 100,
        Remark: '',
        CameraWaitTime: "1"
    });
    $('#ckbNeedToll').prop("checked", true);
    $('#ckbTwoCameraWait').prop("checked", true);
    ShowCameraIsWaitTime("1");
};

//修改区域
function Update() {
    var selectArea = $('#tableArea').treegrid('getSelected');
    if (selectArea == null) {
        $.messager.alert("系统提示", "请选择要修改的区域信息!");
        return;
    }
    var parking = $('#parkingTree').tree('getSelected');
    if (parking == null || parking.attributes.type != 2) {
        $.messager.alert('系统提示', '请选择车场信息', 'error');
        return;
    }
    AddOrUpdate('修改车场区域');
    $('#divParkAreaBoxForm').form('load', {
        AreaID: selectArea.AreaID,
        PKID: selectArea.PKID,
        AreaName: selectArea.AreaName,
        ParkingName: parking.text,
        CarbitNum: selectArea.CarbitNum,
        Remark: selectArea.Remark,
        CameraWaitTime: selectArea.CameraWaitTime
    });
    if (selectArea.TwoCameraWait == 1) {
        $('#ckbTwoCameraWait').prop("checked", true);
    } else {
        $('#ckbTwoCameraWait').prop("checked", false);
    }
    if (selectArea.NeedToll == 1) {
        $('#ckbNeedToll').prop("checked", true);
    } else {
        $('#ckbNeedToll').prop("checked", false);
    }
    ShowCameraIsWaitTime(selectArea.CameraWaitTime);
    $.post('/p/ParkArea/GetAreaTreeData?parkingId=' + parking.id + "&excludeAreaID=" + selectArea.AreaID, function (data) {
        $('#MasterID').combotree({
            data: data,
            onLoadSuccess: function () {
                $('#MasterID').combotree("setValue", selectArea.MasterID);
                var currArea = $('#MasterID').combotree("getValue", selectArea.AreaID);
                if (currArea != null) {
                    $('#MasterID').combotree("remove", currArea.target);
                }


            }
        });
    }, 'json');
};

//删除区域
function Delete() {
    var selectArea = $('#tableArea').treegrid('getSelected');
    if (selectArea == null) {
        $.messager.alert("系统提示", "请选择要删除的区域信息!");
        return;
    }

    $.messager.confirm('系统提示', '确定删除选中的区域信息吗?',
        function (r) {
            if (r) {
                $.post('/p/ParkArea/Delete?areaId=' + selectArea.AreaID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({ width: 200, height: 100, msg: '删除区域成功', title: "删除区域" });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
            }
        });
};

//刷新区域
function Refresh() {
    var selectparking = $('#parkingTree').tree('getSelected');
    if (selectparking == null || selectparking.attributes.type != 2) {
        return;
    }
    CurrentSelectParking(selectparking.id);
};
var intReg = /^[0-9]*$/;
//弹出编辑对话框
function AddOrUpdate(data) {
    $('#divParkAreaBoxForm').form('clear');
    $('#divParkAreaBox').show().dialog({
        title: data,
        width: 400,
        height: 350,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divParkAreaBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divParkAreaBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        if ($("#ckbTwoCameraWait").is(":checked")) {
                            var waitTime = $("#txtCameraWaitTime").val();
                            if (!$.trim(waitTime).match(intReg)) {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '双摄像头等待时间格式不正确!', 'error');
                                return false;
                            }
                        }
                        $.ajax({
                            type: "post",
                            url: '/p/ParkArea/Edit',
                            data: $("#divParkAreaBoxForm").serialize(),
                            error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                            success: function (data) {

                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '数据保存成功!',
                                        title: "保存车场"
                                    });
                                    $.messager.progress("close");
                                    $('#divParkAreaBox').dialog('close');
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