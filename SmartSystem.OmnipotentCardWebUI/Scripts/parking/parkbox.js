$(function () {
    $('#ParkAreaTree').tree({
        url: '/PrakAreaData/GetPrakAreaTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                $('#tableParkBox').datagrid('load', { areaId: node.id, parkName: node.attributes.parkname, areaName: node.text });
            }
        }
    });


    $('#tableParkBox').datagrid({
        url: '/p/ParkBox/GetParkBoxData/',
        singleSelect: true,
        columns: [[
                    { field: 'BoxID', title: 'BoxID', width: 100, hidden: true },
                    { field: 'AreaID', title: 'AreaID', width: 100, hidden: true },
                    { field: 'BoxNo', title: '岗亭编号', width: 80 },
                    { field: 'BoxName', title: '岗亭名称', width: 100 },
                    { field: 'ComputerIP', title: '岗亭IP', width: 100 },
                    { field: 'ParkingName', title: '所属停车场', width: 100 },
                    { field: 'AreaName', title: '所属区域', width: 100 },
                    { field: 'IsCenterPayment', title: '是中心缴费岗亭', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'Remark', title: '备注', width: 150 }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableParkBox').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkBox/GetParkBoxOperatePurview', function (result) {
                    $('#tableParkBox').datagrid("addToolbarItem", result);
                });
            }
        }
    });
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#ParkAreaTree").tree("search", content);
}
var ipReg = /^\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/;
//弹出编辑对话框
AddOrUpdate = function (data) {
    $('#divParkBoxForm').form('clear');

    $('#divParkBox').show().dialog({
        title: data,
        width: 380,
        height: 350,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                var ip = $("#txtComputerIP").val();
                if (!$.trim(ip).match(ipReg)) {
                    $.messager.alert('系统提示', '岗亭主机IP格式不正确!', 'error');
                    return;
                }
                if ($('#divParkBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkBox/SaveEdit',
                        data: $("#divParkBoxForm").serialize(),
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
                                $('#divParkBox').dialog('close');
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
    var parkArea = $('#ParkAreaTree').tree("getSelected");
    if (parkArea == null || parkArea.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择停车区域!', 'error');
        return;
    }
    AddOrUpdate("添加岗亭");

    $('#divParkBoxForm').form('load', {
        BoxID: '',
        ParkingID: parkArea.attributes.parkingid,
        AreaID: parkArea.id,
        AreaName: parkArea.text
    })
    $("#chkIsCenterPayment").prop("checked", false);
};

//修改车辆类型
function Update() {
    var selectParkBox = $('#tableParkBox').datagrid('getSelected');
    if (selectParkBox == null) {
        $.messager.alert("系统提示", "请选择需要修改的岗亭!");
        return;
    }

    AddOrUpdate('修改岗亭');
    $('#divParkBoxForm').form('load', {
        BoxID: selectParkBox.BoxID,
        AreaID: selectParkBox.AreaID,
        BoxNo: selectParkBox.BoxNo,
        AreaName: selectParkBox.AreaName,
        ComputerIP: selectParkBox.ComputerIP,
        BoxName: selectParkBox.BoxName,
        Remark: selectParkBox.Remark
    });
    if (selectParkBox.IsCenterPayment==1) {
        $("#chkIsCenterPayment").prop("checked", true);
    } else {
        $("#chkIsCenterPayment").prop("checked", false);
    }
};

//删除车辆类型
function Delete() {
    var selectParkBox = $('#tableParkBox').datagrid('getSelected');
    if (selectParkBox == null) {
        $.messager.alert("系统提示", "请选择要删除的岗亭信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除删除选中的岗亭吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkBox/Delete?recordId=' + selectParkBox.BoxID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除岗亭成功',
                                title: "删除岗亭"
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
    var selectArea = $('#ParkAreaTree').tree("getSelected");
    if (selectArea == null || selectArea.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择停车区域!', 'error');
        return;
    }
    $('#tableParkBox').datagrid('load', { areaId: selectArea.id, parkName: selectArea.attributes.parkname, areaName: selectArea.text });
};
function DownloadQRCode() {
    var selectBox = $('#tableParkBox').datagrid('getSelected');
    if (selectBox == null) {
        $.messager.alert("系统提示", "请选择岗亭!");
        return;
    }
    $("#hiddDownloadBoxId").val(selectBox.BoxID);
    GetQrCode();
}
function ShowDownloadBox() {
    $('#divDownloadBox').show().dialog({
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
                $('#divDownloadBox').dialog('close');
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
    var boxId = $("#hiddDownloadBoxId").val();
    $.ajax({
        type: "POST",
        url: "/p/ParkBox/DownloadQRCode",
        data: "size=0&boxId=" + boxId + "",
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