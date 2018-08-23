$(function () {
    $("#btnQueryData").click(function () {
        var parkingName = $("#txtQueryParkName").val();
        var gateName = $("#txtQueryGateName").val();
        var dataSource = $("#sltDataSource").val();
        $('#tableData').datagrid('load', { ParkName: parkingName, GateName: gateName, DataSource: dataSource });
    });

    $('#tableData').datagrid({
        url: '/p/BWYParkGate/GetBWYParkGateData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        onSelect: function (rowIndex, rowData) {
            var datasource = rowData.DataSource;
            if (datasource == 0) {
                $("#btnadd").show();
                $("#btnupdate").show();
                $("#btndelete").show();
                $("#btnupdateparkno").hide();
            } else {
                $("#btnadd").hide();
                $("#btnupdate").hide();
                $("#btndelete").hide();
                $("#btnupdateparkno").show();
            }
        },
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 120, hidden: true },
                    { field: 'ParkingID', title: '车场编号', width: 200 },
                    { field: 'ParkNo', title: '二维码车场编号', width: 100 },
                     { field: 'ParkingName', title: '车场名称', width: 100 },
                     { field: 'ParkBoxID', title: '岗亭编号', width: 150 },
                     { field: 'ParkBoxName', title: '岗亭名称', width: 150 },
                     { field: 'GateID', title: '通道编号', width: 100 },
                     { field: 'GateName', title: '通道名称', width: 100 },
                      { field: 'DataSource', title: '来源', width: 80, formatter: function (value, row, index) {
                          if (value == 1) {
                              return "赛菲姆";
                          } else {
                              return "泊物云";
                          }
                      }
                      },
                     { field: 'CreateTime', title: '添加时间', width: 140, formatter: function (value, row, index) {
                         if (value != null && value != "") {
                             return String.GetDateyyyyMMddHHmmss(value);
                         } else {
                             return "";
                         }
                     }
                     }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableData').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/BWYParkGate/GetParkGateOperatePurview', function (result) {
                    $('#tableData').datagrid("addToolbarItem", result);
                });
            }
        },
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45]
    });
});
var ipReg = /^\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/;
//弹出编辑对话框
AddOrUpdate = function (data) {
    $('#divParkGateBox').show().dialog({
        title: data,
        width: 300,
        height: 250,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkGateBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divParkGateForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/BWYParkGate/SaveEdit',
                        data: $("#divParkGateForm").serialize(),
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
                                $('#divParkGateBox').dialog('close');
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



//增加岗亭信息
function Add() {
    AddOrUpdate("添加通道");
    $('#divParkGateForm').form('clear');
};

//修改车辆类型
function Update() {
    var selectGate = $('#tableData').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择需要修改的通道信息!");
        return;
    }

    AddOrUpdate('修改通道');
    $('#divParkGateForm').form('load', {
        RecordID: selectGate.RecordID,
        ParkingID: selectGate.ParkingID,
        ParkingName: selectGate.ParkingName,
        GateID: selectGate.GateID,
        GateName: selectGate.GateName
    });
};
function UpdateParkNo() {
    var selectGate = $('#tableData').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择需要修改的通道信息!");
        return;
    }

    UpdateQRCodeParkNo();
    $('#divQRCodeParkNoBoxForm').form('load', {
        RecordID: selectGate.RecordID,
        ParkNo: selectGate.ParkNo
    });
}
function UpdateQRCodeParkNo() {
    $('#divQRCodeParkNoBox').show().dialog({
        title: "二维码车场编号",
        width: 350,
        height: 150,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divQRCodeParkNoBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divQRCodeParkNoBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/BWYParkGate/SaveQRCodeParkNo',
                        data: $("#divQRCodeParkNoBoxForm").serialize(),
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
                                $('#divQRCodeParkNoBox').dialog('close');
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
//删除车辆类型
function Delete() {
    var selectGate = $('#tableData').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择要删除的通道信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的通道信息吗?',
            function (r) {
                if (r) {
                    $.post('/p/BWYParkGate/Delete?recordId=' + selectGate.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除成功',
                                title: "删除通道"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
};

//刷新车辆类型
Refresh = function () {
    var parkingName = $("#txtQueryParkName").val();
    var gateName = $("#txtQueryGateName").val();
    $('#tableData').datagrid('load', { ParkName: parkingName, GateName: gateName });
};
function DownloadQRCode() {
    var selectBox = $('#tableData').datagrid('getSelected');
    if (selectBox == null) {
        $.messager.alert("系统提示", "请选择通道!");
        return;
    }
    $("#hiddDownloadRecordID").val(selectBox.RecordID);
    GetQrCode();
}
function ShowDownloadBox() {
    $('#divDownloadGate').show().dialog({
        title: "下载二维码",
        width: 450,
        height: 300,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divDownloadGate').dialog('close');
            }
        }]
    });
}
function myBrowser() {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    }; //判断是否Opera浏览器
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    } //判断是否Firefox浏览器
    if (userAgent.indexOf("Chrome") > -1) {
        return "Chrome";
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    } //判断是否Safari浏览器
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    }; //判断是否IE浏览器
    if (userAgent.indexOf("Trident") > -1) {
        return "Edge";
    } //判断是否Edge浏览器
}

function SaveAs5(imgURL) {
    var oPop = window.open(imgURL, "", "width=1, height=1, top=5000, left=5000");
    for (; oPop.document.readyState != "complete"; ) {
        if (oPop.document.readyState == "complete") break;
    }
    oPop.document.execCommand("SaveAs");
    oPop.close();
}
function StartDownLoad(url, obj) {
    myBrowser();
    if (myBrowser() === "IE" || myBrowser() === "Edge") {
        //IE
        odownLoad.href = "#";
        var oImg = document.createElement("img");
        oImg.src = url;
        oImg.id = "downImg";
        var odown = document.getElementById("down");
        odown.appendChild(oImg);
        SaveAs5(document.getElementById('downImg').src)
    } else {
        //!IE
        odownLoad.href = url;
        odownLoad.download = "";
    }
}
var odownLoad = null;
function btnDownloadQRCode(obj, size) {
    odownLoad = $(obj).get(0);
    var url = $(obj).attr("href");
    StartDownLoad(url, obj);
}
function GetQrCode() {
    var gateId = $("#hiddDownloadRecordID").val();
    $.ajax({
        type: "POST",
        url: "/p/BWYParkGate/DownloadQRCode",
        data: "size=0&gateId=" + gateId + "",
        success: function (data) {
            if (data.result) {
                for (var i = 0; i < data.data.length; i++) {
                    var value = data.data[i].split('|');
                    $("#btn" + value[0]).attr("href", value[1]);
                }
                ShowDownloadBox();
            } else {
                $.messager.alert("系统提示", data.msg);
                return;
            }
        }
    });
}
function DownloadSFMParking() {
    $.ajax({
        type: "POST",
        url: "/p/BWYParkGate/ImportSFMParking",
        success: function (data) {
            if (data.result) {
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '导入成功',
                    title: "系统提示"
                });
                Refresh();
            } else {
                $.messager.alert("系统提示", data.msg);
                return;
            }
        }
    });
}