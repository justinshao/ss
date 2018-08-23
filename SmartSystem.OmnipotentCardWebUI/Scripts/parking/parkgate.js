$(function () {
    $('#parkBoxTree').tree({
        url: '/ParkBoxData/GetParkBoxTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                $('#tableParkGate').datagrid('load', { boxId: node.id });
                $('#tableGateDevice').datagrid('load', { gateId: "" });
            }
        }
    });

    $('#tableParkGate').datagrid({
        url: '/p/ParkGate/GetParkGateData/',
        singleSelect: true,
        onSelect: function (rowIndex, rowData) {
            var gateid = rowData.GateID;
            $('#tableGateDevice').datagrid('load', { gateId: gateid });
        },
        columns: [[
                    { field: 'GateID', title: 'GateID', width: 100, hidden: true },
                    { field: 'BoxID', title: 'BoxID', width: 100, hidden: true },
                    { field: 'GateNo', title: '通道编号', width: 80 },
                    { field: 'GateName', title: '通道名称', width: 100 },
                    { field: 'IoState', title: '进出方向', width: 60, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '进';
                        } else {
                            return '出';
                        }
                    }
                    },

                    { field: 'IsTempInOut', title: '临停是否允许进出', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'IsEnterConfirm', title: '是否需要通行确认', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'OpenPlateBlurryMatch', title: '进出开启模糊识别', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'IsNeedCapturePaper', title: '需要身份验证', width: 100, formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                     { field: 'PlateNumberAndCard', title: '是否卡加车牌', width: 100, formatter: function (value, row, index) {
                         if (value == 1) {
                             return '<img src="/Content/images/yes.png"/>';
                         } else {
                             return '<img src="/Content/images/no.png?v=1"/>';
                         }
                     }
                     },
                    { field: 'Remark', title: '备注', width: 150 },
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableParkGate').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/ParkGate/GetParkGateOperatePurview', function (result) {
                    $('#tableParkGate').datagrid("addToolbarItem", result);
                });

            }
        }
    });
});

function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#parkBoxTree").tree("search", content);
}
//增加通道
function Add() {
    var selectbox = $('#parkBoxTree').tree('getSelected');
    if (selectbox == null || selectbox.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择岗亭!', 'error');
        return;
    }
    $('#divGateBoxForm').form('clear');
    SaveOrUpdateGate("增加通道");
    $('#divGateBoxForm').form('load', {
        BoxID: selectbox.id,
        BoxName: selectbox.text,
        ParkingID: selectbox.attributes.parkingId,
        IoState: 1
    });
    $("#ckbIsNeedCapturePaper").prop("checked", false);
    $("#ckbIsTempInOut").prop("checked", true);
    $("#ckbIsEnterConfirm").prop("checked", true);
    $("#ckbOpenPlateBlurryMatch").prop("checked", true);
    $("#ckbOpenPlateBlurryMatch").prop("checked", true);
    $("#ckbPlateNumberAndCard").prop("checked", false);
};

//修改通道
function Update() {
    var selectbox = $('#parkBoxTree').tree('getSelected');
    if (selectbox == null || selectbox.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择岗亭!', 'error');
        return;
    }
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择需要修改的通道!");
        return;
    }
    $('#divGateBoxForm').form('clear');
    SaveOrUpdateGate('修改通道');
    $('#divGateBoxForm').form('load', {
        GateID: selectGate.GateID,
        BoxID: selectGate.BoxID,
        GateNo: selectGate.GateNo,
        GateName: selectGate.GateName,
        IoState: selectGate.IoState,
        BoxName: selectbox.text,
        Remark: selectGate.Remark
    });

    if (selectGate.IsTempInOut) {
        $("#ckbIsTempInOut").prop("checked", true);
    } else {
        $("#ckbIsTempInOut").prop("checked", false);
    }
    if (selectGate.IsEnterConfirm) {
        $("#ckbIsEnterConfirm").prop("checked", true);
    } else {
        $("#ckbIsEnterConfirm").prop("checked", false);
    }
    if (selectGate.OpenPlateBlurryMatch) {
        $("#ckbOpenPlateBlurryMatch").prop("checked", true);
    } else {
        $("#ckbOpenPlateBlurryMatch").prop("checked", false);
    }
    if (selectGate.IsNeedCapturePaper) {
        $("#ckbIsNeedCapturePaper").prop("checked", true);
    } else {
        $("#ckbIsNeedCapturePaper").prop("checked", false);
    }
    $("#ckbPlateNumberAndCard").prop("checked", selectGate.PlateNumberAndCard);
};

//删除通道
function Delete() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择要删除的通道信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的通道吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkGate/Delete?recordId=' + selectGate.GateID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除通道成功',
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

//刷新通道
Refresh = function () {
    var selectbox = $('#parkBoxTree').tree('getSelected');
    if (selectbox == null || selectbox.attributes.type != 1) {
        $.messager.alert('系统提示', '请选择岗亭!', 'error');
        return;
    }
    $('#tableParkGate').datagrid('load', { boxId: selectbox.id });
};
//弹出编辑对话框
function SaveOrUpdateGate(data) {
    $('#divGateBox').show().dialog({
        title: data,
        width: 440,
        height: 400,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divGateBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divGateBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });

                    $.ajax({
                        type: "post",
                        url: '/p/ParkGate/SaveEdit',
                        data: $("#divGateBoxForm").serialize(),
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
                                $('#divGateBox').dialog('close');
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
function DownloadQRCode() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择通道!");
        return;
    }
    $("#hiddDownloadGateId").val(selectGate.GateID);
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
    var gateId = $("#hiddDownloadGateId").val();
    $.ajax({
        type: "POST",
        url: "/p/ParkGate/DownloadQRCode",
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
$(function () {
    $('#divGateIOTime').tabs({
        border: false,
        onSelect: function (title) {
           
            if (title == "按星期规则") {
                $("#btnCancelGateIOTime").show();
                $("#btnAddGateIOTime").show();
            } else {
                $("#btnCancelGateIOTime").hide();
                $("#btnAddGateIOTime").hide();
            }
            window.setTimeout("SetWeekPanelHeight()", 100);
        }
    });
    BindSpecialParkGateIOTime();
});
function SetWeekPanelHeight() {
    $(".easyui-panel").css("height", "60px");
}
function BindWeekParkGateIOTime(gateId) {
    InitWeekParkGateIOTime();
    $.messager.progress({text: '正在查询....',interval: 100 });
    $.ajax({
        type: "post",
        url: '/p/ParkGate/GetWeekParkGateIOTime',
        async: false,
        data: "GateID=" + gateId + "",
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                for (var i = 0; i < data.data.length; i++) {
                    var startTime = data.data[i].StartTime;
                    var endTime = data.data[i].EndTime;
                    var inOutState = data.data[i].InOutState;
                    var weekIndex = data.data[i].WeekIndex;
                    var obj = $("#pweek_" + weekIndex);
                    if ($(obj).length > 0) {
                        $(obj).find(".txtStartTime").timespinner("setValue", startTime);
                        $(obj).find(".txtEndTime").timespinner("setValue", endTime);
                        if (inOutState == 0) {
                            $(obj).find(".chkInOutState").prop("checked", true);
                        } else {
                            $(obj).find(".chkInOutState").prop("checked", false);
                        }
                    }
                }
                $.messager.progress("close");
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function InitWeekParkGateIOTime() {
    $("[id^=pweek_]").each(function () {
        $(this).find(".txtStartTime").timespinner("setValue","00:00");
        $(this).find(".txtEndTime").timespinner("setValue", "23:59");
        $(this).find(".chkInOutState").prop("checked", true);
    });
}
function ParkGateIOTime() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择通道!");
        return;
    }
    $("#hiddParkGateIOTimeId").val(selectGate.GateID);
    ShowParkGateIOTimeBox();
    BindWeekParkGateIOTime(selectGate.GateID);
    $('#tableSpecialParkGateIOTime').datagrid('load', { gateId: selectGate.GateID });
}
function ShowParkGateIOTimeBox() {
    $('#divParkGateIOTimeBox').show().dialog({
        title: "通道进出规则配置",
        width: 600,
        height: 500,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            id: 'btnCancelGateIOTime',
            handler: function () {
                $('#divGateBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            id: 'btnAddGateIOTime',
            handler: function () {
                var weeks = GetWeekSpecialParkGateIOTime();
                if (weeks == "") {
                    $.messager.alert('系统提示', '获取星期规则设置失败!', 'error');
                    return false;
                }
                var gateId = $("#hiddParkGateIOTimeId").val();
                if (gateId == "") {
                    $.messager.alert('系统提示', '获取通道编号失败!', 'error');
                    return false;
                }
                $.messager.progress({
                    text: '正在保存....',
                    interval: 100
                });
                $.ajax({
                    type: "post",
                    url: '/p/ParkGate/SaveWeekSpecialParkGateIOTime',
                    data: "IOTimeDatas=" + weeks + "&GateID=" + gateId + "",
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
    function GetWeekSpecialParkGateIOTime() {
        var strWeeks = "";
        $("[id^=pweek_]").each(function () {
            var startTime = $(this).find(".txtStartTime").timespinner("getValue");
            var endTime = $(this).find(".txtEndTime").timespinner("getValue");
            var inoutstate = $(this).find(".chkInOutState").is(":checked") ? "0" : "1";
            var week = $(this).find(".hiddWeek").val();
            strWeeks += startTime + "," + endTime + "," + inoutstate + ","+week+"|";
        });
        return strWeeks;
    }
function AddSpecialParkGateIOTime() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择通道!");
        return;
    }
    var RuleDate = $("#txtSpecialRuleDate").datebox("getValue");
    if ($.trim(RuleDate) == "") {
        $.messager.alert('系统提示', '请选择日期!', 'error');
        return false;
    }
    var StartTime = $("#txtSpecialParkGateStartTime").timespinner("getValue");
    if ($.trim(StartTime) == "") {
        $.messager.alert('系统提示', '请选择开始时间!', 'error');
        return false;
    } 
    var EndTime = $("#txtSpecialParkGateEndTime").timespinner("getValue");
    if ($.trim(EndTime) == "") {
        $.messager.alert('系统提示', '请选择结束时间!', 'error');
        return false;
    }
    var InOutState = $("#ckbInOutState").is(":checked") ? "0" : "1";
    $.messager.progress({
        text: '正在保存....',
        interval: 100
    });
    $.ajax({
        type: "post",
        url: '/p/ParkGate/SaveSpecialParkGateIOTime',
        data: "RuleDate=" + RuleDate + "&StartTime=" + StartTime + "&EndTime=" + EndTime + "&InOutState=" + InOutState + "&GateID=" + selectGate.GateID + "",
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
                ReloadSpecialParkGateIOTime();
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function BindSpecialParkGateIOTime() {
    $('#tableSpecialParkGateIOTime').datagrid({
        url: '/p/ParkGate/GetSpecialParkGateIOTime/',
        singleSelect: true,
        columns: [[
                    { field: 'GateID', title: 'GateID', width: 100, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 100, hidden: true },
                    { field: 'RuleDate', title: '日期', width: 150 },
                    { field: 'StartTime', title: '开始时间', width: 100 },
                    { field: 'EndTime', title: '结束时间', width: 100 },
                    { field: 'InOutState', title: '临停是否允许进出', width: 150 }
                   
				]],
        toolbar: [{ text: '删除', iconCls: 'icon-remove', handler: function () { DeleteSpecialParkGateIOTime() } }, '-', { text: '刷新', iconCls: 'icon-reload', handler: function () { ReloadSpecialParkGateIOTime() } }]
    });
}
function DeleteSpecialParkGateIOTime() {
    var selectGateIOTime = $('#tableSpecialParkGateIOTime').datagrid('getSelected');
    if (selectGateIOTime == null) {
        $.messager.alert("系统提示", "请选择要删除的规则!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的规则吗?',
            function (r) {
                if (r) {
                    $.post('/p/ParkGate/DeleteSpecialParkGateIOTime?recordId=' + selectGateIOTime.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除成功',
                                title: "系统提示"
                            });
                            ReloadSpecialParkGateIOTime();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function ReloadSpecialParkGateIOTime() {
    var selectGate = $('#tableParkGate').datagrid('getSelected');
    if (selectGate == null) {
        $.messager.alert("系统提示", "请选择通道!");
        return;
    }
    $('#tableSpecialParkGateIOTime').datagrid('load', { gateId: selectGate.GateID });
}