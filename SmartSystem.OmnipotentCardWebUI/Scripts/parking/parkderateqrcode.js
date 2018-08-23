$(function () {
    $('#parkSellerTree').tree({
        url: '/ParkSellerData/GetSellerTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                LoadParkQRCodeDerate(node.id);
                BindParkDerate(node.id);
            }
        }
    });

    $('#tableQRCodeDerate').datagrid({
        url: '/p/ParkDerateQRCode/GetParkDerateQRcodeData/',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 100, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 100, hidden: true },
                    { field: 'VID', title: 'VID', width: 100, hidden: true },
                    { field: 'DerateID', title: 'DerateID', width: 100, hidden: true },
                    { field: 'StartTime', title: 'StartTime', width: 100, hidden: true },
                    { field: 'EndTime', title: 'EndTime', width: 100, hidden: true },
                    { field: 'DerateSwparate', title: 'DerateSwparate', width: 100, hidden: true },
                    { field: 'DerateMoney', title: 'DerateMoney', width: 100, hidden: true },
                    { field: 'CanUseTimes', title: 'CanUseTimes', width: 100, hidden: true },
                    { field: 'AlreadyUseTimes', title: 'AlreadyUseTimes', width: 100, hidden: true },
                    { field: 'SellerName', title: '商家名称', width: 100 },
                    { field: 'DerateName', title: '优免规则', width: 100 },
                    { field: 'DerateValue', title: '优免值', width: 60 },
                    { field: 'UseTimesDes', title: '可使用次数/已使用次数', width: 150 },
                    { field: 'StartTimeToString', title: '开始时间', width: 140 },
                    { field: 'EndTimeToString', title: '结束时间', width: 140 },
                    { field: 'DataSource', title: '来源', width: 80,
                        formatter: function (value) {
                            if (value == 0) {
                                return '管理处';
                            } else if (value == 1) {
                                return '消费打折端';
                            }else {
                                return "";
                            }
                        }
                    },
                    { field: 'OperatorAccount', title: '操作人账号', width: 100 },
                    { field: 'Remark', title: '备注', width: 150 },
                    { field: 'CreateTimeToString', title: '添加时间', width: 150 },
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableQRCodeDerate').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkDerateQRCode/GetParkDerateQRCodeOperatePurview', function (result) {
                    $('#tableQRCodeDerate').datagrid("addToolbarItem", result);
                });
            }
        }

    });
});

function LoadParkQRCodeDerate() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    var derateId = $("#cmbQueryParkDerate").combobox("getValue");
    var status = $("#sltStatus").val();
    var derateQRCodeSource = $("#sltDerateQRCodeSource").val();
    $('#tableQRCodeDerate').datagrid('load', { sellerId: seller.id, derateId: derateId, status: status, derateQRCodeSource: derateQRCodeSource });
}
function BindParkDerate(sellerId) {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    $("#cmbQueryParkDerate").combobox({
        url: '/p/ParkDerateQRCode/GetSellerDerateTree?needDefaultValue=false&sellerId=' + seller.id,
        valueField: 'id',
        textField: 'text'
    });
    $("#cmbEtidParkDerate").combobox({
        url: '/p/ParkDerateQRCode/GetSellerDerateData?sellerId=' + seller.id,
        valueField: 'DerateID',
        textField: 'Name',
        onSelect: function (e) {
            SetDerateDescription(e.DerateType);
        },
        onLoadSuccess: function () {
            var derateData = $('#cmbEtidParkDerate').combobox('getData');
            if (derateData.length > 0) {
                $('#cmbEtidParkDerate').combobox('setValue', derateData[0].DerateID);
                SetDerateDescription(derateData[0].DerateType);
            }
        }
    });
}
function SetDerateDescription(derateType) {
    if (derateType == 8) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免金额');
        $('#spanUnitDes').html('元');

    } else if (derateType == 9 || derateType == 3) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免时间');
        $('#spanUnitDes').html('分钟');
    }
    else if (derateType == 10) {
        $('#txtDerateValue').numberspinner('enable');
        $('#spanDerateValueDes').html('优免');
        $('#spanUnitDes').html('天');
    } 
    else {
        $('#txtDerateValue').numberspinner('setValue', 0);
        $('#txtDerateValue').numberspinner('disable');
    }
}
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkSellerTree").tree("search", content);
}

function Add() {
    var seller = $('#parkSellerTree').tree("getSelected");
    if (seller == null || seller.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择商家!', 'error');
        return;
    }
    AddOrUpdateBox("添加优免二维码");
    var start = $("#hiddDefaultStartTime").val();
    var end = $("#hiddDefaultEndTime").val();
    $('#divDerateQRCodeBoxForm').form('load', {
        RecordID: '',
        DerateValue: 0,
        CanUseTimes: 0,
        Remark: "",
        StartTime: start,
        EndTime: end.toString()
    });
    var derateData = $('#cmbEtidParkDerate').combobox('getData');
    if (derateData.length > 0) {
        $('#cmbEtidParkDerate').combobox('setValue', derateData[0].DerateID);
        SetDerateDescription(derateData[0].DerateType);
    }
}
function Update() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要修改的优免二维码!");
        return;
    }

    AddOrUpdateBox("修改优免二维码");

    $('#divDerateQRCodeBoxForm').form('load', {
        RecordID: derate.RecordID,
        DerateValue: derate.DerateValue,
        CanUseTimes: derate.CanUseTimes,
        Remark: derate.Remark,
        StartTime: derate.StartTime.replace("T", " "),
        EndTime: derate.EndTime.replace("T", " "),
        DerateID: derate.DerateID
    });
    var derateData = $('#cmbEtidParkDerate').combobox('getData');
    if (derateData.length > 0) {
        for (var i = 0; i < derateData.length; i++) {
            if (derate.DerateID == derateData[i].DerateID) {
                $('#cmbEtidParkDerate').combobox('setValue', derateData[i].DerateID);
                SetDerateDescription(derateData[i].DerateType);
            }
        }
    }
}

function Delete() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要删除的优免二维码!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的优免二维码吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkDerateQRCode/Delete?recordId=' + derate.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除优免二维码成功',
                                title: "系统提示"
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
    var derateId = $("#cmbQueryParkDerate").combobox("getValue");
    var status = $("#sltStatus").val();
    var derateQRCodeSource = $("#sltDerateQRCodeSource").val();

    $('#tableQRCodeDerate').datagrid('options').queryParams = { sellerId: seller.id, derateId: derateId, status: status, derateQRCodeSource: derateQRCodeSource };
    $('#tableQRCodeDerate').datagrid('reload');
}
AddOrUpdateBox = function (title) {
    $('#divDerateQRCodeBox').show().dialog({
        title: title,
        width: 350,
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
                    $('#divDerateQRCodeBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#divDerateQRCodeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/p/ParkDerateQRCode/AddOrUpdate',
                            data: $("#divDerateQRCodeBoxForm").serialize(),
                            error: function () {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                            },
                            success: function (data) {
                                if (data.result) {
                                    $.messager.show({
                                        width: 200,
                                        height: 100,
                                        msg: '保存优免二维码成功!',
                                        title: "系统提示"
                                    });
                                    $.messager.progress("close");
                                    Refresh();
                                    $('#divDerateQRCodeBox').dialog('close');
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
function ShowDerateQRCode() {
    $("#imgTempQRCode").hide();
    var selectRow = $('#tableQRCodeDerate').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择优免二维码!");
        return;
    }
    $.messager.progress({ text: '获取中....', interval: 100 });
    $.ajax({
        type: "post",
        url: '/p/ParkDerateQRCode/AddIdenticalQRCode',
        data: "recordId=" + selectRow.RecordID + "&vid=" + selectRow.VID + "&isAdd=false",
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            $.messager.progress("close");
            if (data.result) {
                $("#imgTempQRCode").attr("src", "data:image/png;base64," + data.data);
                $("#imgTempQRCode").show();
                LoadDerateQRCdoe();

            } else {
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });

}
function LoadDerateQRCdoe() {
    $('#divShowDerateQRCodeBox').show().dialog({
        title: "优免二维码",
        width: 450,
        height: 430,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#divShowDerateQRCodeBox').dialog('close');
                }
            },
            {
                text: '添加相同规则二维码',
                iconCls: 'icon-add',
                id: 'btnAddIdenticalQRCode',
                handler: function () {
                    $("#imgTempQRCode").hide();
                    var selectRow = $('#tableQRCodeDerate').datagrid('getSelected');
                    if (selectRow == null) {
                        $.messager.alert("系统提示", "请选择优免二维码!");
                        return;
                    }
                    $.messager.progress({ text: '获取中....', interval: 100 });
                    $.ajax({
                        type: "post",
                        url: '/p/ParkDerateQRCode/AddIdenticalQRCode',
                        data: "recordId=" + selectRow.RecordID + "&vid=" + selectRow.VID + "&isAdd=true",
                        error: function () {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
                        },
                        success: function (data) {
                            $.messager.progress("close");
                            if (data.result) {
                                $("#imgTempQRCode").attr("src", "data:image/png;base64," + data.data);
                                $("#imgTempQRCode").show();
                                Refresh();

                            } else {
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                }
            }]

    });
}

function DownloadQRCode() {
    var derate = $('#tableQRCodeDerate').datagrid('getSelected');
    if (derate == null) {
        $.messager.alert("系统提示", "请选择需要删除的优免二维码!");
        return;
    }
    GetQrCode(derate.VID, derate.RecordID, derate.SellerName, derate.DerateName);
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
function GetQrCode(vid, qid, sellerName, derateName) {
    $.ajax({
        type: "POST",
        url: "/p/ParkDerateQRCode/DownloadQRCode",
        data: "vid=" + vid + "&qid=" + qid + "&sellerName=" + sellerName + "&derateName=" + derateName + "",
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