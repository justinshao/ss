$(function () {
    $('#villageTree').tree({
        url: '/VillageData/CreateVillageTreeData',
        onSelect: function (node) {
            if (node.attributes.type == 1) {
                CurrentSelectVillage(node.id);
            }
        }
    });
    BindCity();
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#villageTree").tree("search", content);	 
}
function CurrentSelectVillage(villageId) {
    $('#tableParking').datagrid('load', { villageId: villageId });
}
$(function () {
    $('#tableParking').datagrid({
        nowrap: false,
        striped: true,
        collapsible: false,
        singleSelect: true,
        url: '/p/parking/GetParkingData',
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { title: 'ID', field: 'ID', width: 80, hidden: true },
                    { title: 'PKID', field: 'PKID', width: 80, hidden: true },
                    { title: 'VID', field: 'VID', width: 80, hidden: true },
                    { title: 'CityID', field: 'CityID', width: 80, hidden: true },
                    { title: 'OnlineDiscount', field: 'OnlineDiscount', width: 80, hidden: true },
                    { title: 'IsOnlineDiscount', field: 'IsOnlineDiscount', width: 80, hidden: true },
                    { title: 'UnconfirmedCalculation', field: 'UnconfirmedCalculation', width: 80, hidden: true },
                    { title: 'OuterringCharge', field: 'OuterringCharge', width: 80, hidden: true },
                    { field: 'PKNo', title: '车场编号', width: 80 },
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'CarBitNum', title: '车位数', width: 60 },
                    { field: 'SpaceBitNum', title: '剩余车位', width: 60 },
//                    { field: 'NeedFee', title: '是否收费', width: 60, formatter: function (value) {
//                        if (value == 1) {
//                            return '<img src="/Content/images/yes.png"/>';
//                        } else {
//                            return '<img src="/Content/images/no.png?v=1"/>';
//                        }
//                    }
//                    },
                    { field: 'ExpiredAdvanceRemindDay', title: '到期前多少天提醒', width: 100 },
                    { field: 'PictureSaveDays', title: '图片保存天数', width: 80 },
                    { field: 'DataSaveDays', title: '数据保存天数', width: 80 },

                    { field: 'LinkMan', title: '联系人', width: 100 },
                    { field: 'Mobile', title: '联系电话', width: 100 },
                     { field: 'PoliceFree', title: '军警是否收费', width: 100, formatter: function (value) {
                         if (value) {
                             return '<img src="/Content/images/yes.png"/>';
                         } else {
                             return '<img src="/Content/images/no.png?v=1"/>';
                         }
                     }
                     },
                    { field: 'MobilePay', title: '手机缴费', width: 80, formatter: function (value) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'IsParkingSpace', title: '车位预定', width: 80, formatter: function (value) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'IsReverseSeekingVehicle', title: '反向寻车', width: 80, formatter: function (value) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'MobileLock', title: '手机锁车', width: 80, formatter: function (value) {
                        if (value == 1) {
                            return '<img src="/Content/images/yes.png"/>';
                        } else {
                            return '<img src="/Content/images/no.png?v=1"/>';
                        }
                    }
                    },
                    { field: 'OnLine', title: '是否在线', width: 80,
                        formatter: function (value) {
                            if (value == 1) {
                                return '<img src="/Content/images/yes.png"/>';
                            } else {
                                return '<img src="/Content/images/no.png?v=1"/>';
                            }
                        }
                    },
                    { field: 'IsLine', title: '进出是否排队', width: 80,
                        formatter: function (value) {
                            if (value == 1) {
                                return '<img src="/Content/images/yes.png"/>';
                            } else {
                                return '<img src="/Content/images/no.png?v=1"/>';
                            }
                        }
                    },
                    { field: 'SupportAutoRefund', title: '线上支付支持自动退款', width: 80,
                        formatter: function (value) {
                            if (value) {
                                return '<img src="/Content/images/yes.png"/>';
                            } else {
                                return '<img src="/Content/images/no.png?v=1"/>';
                            }
                        }
                    },
                    { field: 'IsNoPlateConfirm', title: '无牌车是否确认入场', width: 80,
                        formatter: function (value) {
                            if (value) {
                                return '<img src="/Content/images/yes.png"/>';
                            } else {
                                return '<img src="/Content/images/no.png?v=1"/>';
                            }
                        }
                    }
				]],
        pagination: true,
        rownumbers: true,
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableParking').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/parking/GetParkingOperatePurview', function (result) {
                    $('#tableParking').datagrid("addToolbarItem", result);
                });
            }
        }
    });

    $('#VID').combotree({ url: '/VillageData/CreateSelectVillageTreeData' });

    LoadPassRemark();
});
function LoadPassRemark() {
    $('#tablePassRemark').datagrid({
        singleSelect: true,
        url: '/p/parking/GetPassRemarkData',
        columns: [[
                    { title: 'ID', field: 'ID', width: 80, hidden: true },
                    { title: 'PKID', field: 'PKID', width: 80, hidden: true },
                    { title: 'RecordID', field: 'RecordID', width: 80, hidden: true },
                    { field: 'PassType', title: 'PassType', width: 80, hidden: true },
                    { field: 'PassTypeDes', title: '放行类型', width: 120 },
                    { field: 'Remark', title: '放行备注', width: 310 },
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tablePassRemark').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/parking/GetParkingPassRemarkOperatePurview', function (result) {
                    $('#tablePassRemark').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}
function btnSavePassRemark() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "获取车场失败，请关闭窗口重新打开!");
        return;
    }

    var passType = $("#sltPassType").combobox("getValue");
    if (passType == "") {
        $.messager.alert("系统提示", "请选择备注类型!");
        return;
    }
    var remark = $("#txtPassRemark").val();
    if (remark == "") {
        $.messager.alert("系统提示", "请填写备注信息!");
        return;
    }
    $.ajax({
        type: "POST",
        url: "/p/parking/SavePassRemark",
        data: "ParkingID=" + selectParking.PKID + "&PassType=" + passType + "&Remark=" + remark + "",
        success: function (data) {
            if (data.result) {
                RefreshPassRemark();
                $("#txtPassRemark").val("");
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '添加成功',
                    title: "系统提示"
                });
            } else {
                $.messager.alert("系统提示", data.msg);
                return;
            }
        }
    });
}
function UpdatePassRemark() {
    var selectRemark = $('#tablePassRemark').datagrid('getSelected');
    if (selectRemark == null) {
        $.messager.alert("系统提示", "请选择备注!");
        return;
    }
    $("#hiddPassRemarkRecordID").val(selectRemark.RecordID);
    $("#hiddPassRemarkPKID").val(selectRemark.PKID);
    $("#sltPassType").combobox("setValue", selectRemark.PassType);
    $("#txtPassRemark").val(selectRemark.Remark);
}
function DeletePassRemark() {
    var selectRemark = $('#tablePassRemark').datagrid('getSelected');
    if (selectRemark == null) {
        $.messager.alert("系统提示", "请选择备注!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的备注吗?',
    function (r) {
        if (r) {
            $.post('/p/parking/DeletePassRemark?recordId=' + selectRemark.RecordID,
                function (data) {
                    if (data.result) {
                        $.messager.show({
                            width: 200,
                            height: 100,
                            msg: '删除备注成功',
                            title: "删除备注"
                        });
                        RefreshPassRemark();
                    } else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }

                });
        }
    });
}
function RefreshPassRemark() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        return;
    }
    $('#tablePassRemark').datagrid('load', { parkingId: selectParking.PKID });
}
function Add() {
    var selectVillage = $('#villageTree').tree('getSelected');
    if (selectVillage == null || selectVillage.attributes.type != 1) {
        $.messager.alert("系统提示", "请先选择小区!");
        return;
    }
    BindCity();
    ShowParkingBox("添加车场");
    $('#divParkingBoxForm').form('load', {
        ID: '',
        PKID: '',
        PKNo: '',
        PKName: '',
        CenterTime: 10,
        LinkMan: '',
        Mobile: '',
        Coordinate: '',
        Address: '',
        FeeRemark: '',
        Remark: '',
        DataSaveDays: '90',
        PictureSaveDays: '90',
        ExpiredAdvanceRemindDay: '7',
        OnlineDiscount:'10'
    });
    $('#VID').combotree('setValue', selectVillage.id);

    $('#NeedFee').prop("checked", true);
    $('#MobilePay').prop("checked", true);
    $('#IsParkingSpace').prop("checked", true);
    $('#IsReverseSeekingVehicle').prop("checked", true);
    $('#MobileLock').prop("checked", true);
    $('#IsLine').prop("checked", false);
    $('#PoliceFree').prop("checked", true);
    $('#IsOnlineDiscount').prop("checked", false);
    $('#UnconfirmedCalculation').prop("checked", false);
    $('#SupportAutoRefund').prop("checked", false);
    $('#IsNoPlateConfirm').prop("checked", false);
    $('#OuterringCharge').prop("checked", false);
    
}
function Update() {

    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择需要修改的车场!");
        return;
    }
    ShowParkingBox("修改车场");
    $('#divParkingBoxForm').form('load', {
        ID: selectParking.ID,
        PKID: selectParking.PKID,
        PKNo: selectParking.PKNo,
        PKName: selectParking.PKName,
        CenterTime: selectParking.CenterTime,
        LinkMan: selectParking.LinkMan,
        Mobile: selectParking.Mobile,
        Coordinate: selectParking.Coordinate,
        Address: selectParking.Address,
        FeeRemark: selectParking.FeeRemark,
        Remark: selectParking.Remark,
        DataSaveDays: selectParking.DataSaveDays,
        PictureSaveDays: selectParking.PictureSaveDays,
        ExpiredAdvanceRemindDay: selectParking.ExpiredAdvanceRemindDay,
        OnlineDiscount: selectParking.OnlineDiscount
    });
    $('#VID').combotree('setValue', selectParking.VID);

    if (selectParking.NeedFee) {
        $('#NeedFee').prop("checked", true);
    } else {
        $('#NeedFee').prop("checked", false);
    }

    if (selectParking.MobilePay) {
        $('#MobilePay').prop("checked", true);
    } else {
        $('#MobilePay').prop("checked", false);
    }
    if (selectParking.IsParkingSpace) {
        $('#IsParkingSpace').prop("checked", true);
    } else {
        $('#IsParkingSpace').prop("checked", false);
    }
    if (selectParking.IsReverseSeekingVehicle) {
        $('#IsReverseSeekingVehicle').prop("checked", true);
    } else {
        $('#IsReverseSeekingVehicle').prop("checked", false);
    }
    if (selectParking.MobileLock) {
        $('#MobileLock').prop("checked", true);
    } else {
        $('#MobileLock').prop("checked", false);
    }
    if (selectParking.IsLine) {
        $('#IsLine').prop("checked", true);
    } else {
        $('#IsLine').prop("checked", false);
    }
    if (selectParking.PoliceFree) {
        $('#PoliceFree').prop("checked", true);
    } else {
        $('#PoliceFree').prop("checked", false);
    }
    $('#IsOnlineDiscount').prop("checked", selectParking.IsOnlineDiscount);
    $('#UnconfirmedCalculation').prop("checked", selectParking.UnconfirmedCalculation);
    $('#SupportAutoRefund').prop("checked", selectParking.SupportAutoRefund);
    $('#IsNoPlateConfirm').prop("checked", selectParking.IsNoPlateConfirm);
    $('#OuterringCharge').prop("checked", selectParking.OuterringCharge);
    if (selectParking.CityID != "0") {
        $.post('/City/GetSelectProvincesByCityId?cityId=' + selectParking.CityID,
            function (data) {
                if (data.result) {
                    $('#ProvinceID').combobox('setValue', data.data);
                        $('#CityID').combobox({
                            url: '/City/GetCitys?ProvinceID=' + data.data,
                            valueField: 'CityID',
                            textField: 'CityName',
                            onLoadSuccess: function () {
                                $('#CityID').combobox('setValue', selectParking.CityID.toString());
                            }
                        });
                }
            });
    }
   
}
function Delete() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择需要删除的车场!");
        return;
    }

    $.messager.confirm('系统提示', '确定删除选中的车场吗?',
    function (r) {
        if (r) {
            $.post('/p/parking/Delete?parkingId=' + selectParking.PKID,
                function (data) {
                    if (data.result) {
                        $.messager.show({
                            width: 200,
                            height: 100,
                            msg: '删除车场成功',
                            title: "删除车场"
                        });
                        Refresh();
                    } else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }

                });
        }
    });
}
function PassRmark() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择车场!");
        return;
    }
    $('#divRemarkBox').show().dialog({
        title: "放行备注",
        width: 510,
        height: 350,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false
    });
    $("#hiddPassRemarkRecordID").val("");
    $("#hiddPassRemarkPKID").val(selectParking.PKID);
    $("#sltPassType").combobox("setValue", 1);
    $("#txtPassRemark").val("");
    RefreshPassRemark();
}
function Refresh() {
    var selectVillage = $('#villageTree').tree('getSelected');
    if (selectVillage == null || selectVillage.attributes.type != 1) {
        return;
    }
    CurrentSelectVillage(selectVillage.id);
}
//弹出编辑对话框
ShowParkingBox = function (data) {
    $('#divParkingBoxForm').form('clear');
    $('#divParkingBox').show().dialog({
        title: data,
        width: 650,
        height: 480,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkingBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!CheckSubmitData()) {
                    return false;
                }
                if ($('#divParkingBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });

                    $.ajax({
                        type: "post",
                        url: '/p/parking/SaveParking',
                        data: $("#divParkingBoxForm").serialize(),
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
                                $('#divParkingBox').dialog('close');
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
function CheckSubmitData() {
    var province = $('#ProvinceID').combobox("getValue");
    if ($.trim(province) == "") {
        $.messager.alert('系统提示', "请选择所在地省份", 'error');
        return false;
    }
    var city = $("#CityID").combobox("getValue");
    if ($.trim(city) == "") {
        $.messager.alert('系统提示', "请选择所在地城市", 'error');
        return false;
    }
    var platebefore = $("#defaultProvinceId").find(".l-btn-text").text();
    if ($.trim(platebefore).length != 2) {
        $.messager.alert('系统提示', '请选择默认车牌!', 'error');
        return false;
    }
    $("#hiddDefaultPlate").val(platebefore);
    return true;
}
function BindCity() {
    $('#ProvinceID').combobox({
        url: '/City/GetAllProvinces/',
        valueField: 'ProvinceID',
        textField: 'ProvinceName',
        onSelect: function (record) {
            $('#CityID').combobox({
                url: '/City/GetCitys?ProvinceID=' + record.ProvinceID,
                valueField: 'CityID',
                textField: 'CityName',
                onLoadSuccess: function () {
                    
                }

            });
        }
    });
}
function DownloadQRCode() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择车场!");
        return;
    }
    $("#hiddDownloadParkingId").val(selectParking.PKID);
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
    var parkingId = $("#hiddDownloadParkingId").val();
    $.ajax({
        type: "POST",
        url: "/p/parking/DownloadQRCode",
        data: "size=0&parkingId=" + parkingId + "",
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
function ParkDerate() {
    var selectParking = $('#tableParking').datagrid('getSelected');
    if (selectParking == null) {
        $.messager.alert("系统提示", "请选择车场!");
        return;
    }
    $("#hiddDerateParkingId").val(selectParking.PKID);
    ShowParkDerateBox();
    GetDerateConfigData()
    SetDerateTypeDefault("0");
    CancelUpdateConfig();
}
function btnSaveParkDerate() {
    $("#hiddDerateRecordID").val("");
    if (!CheckSubmitDerateData()) {
        return;
    }
    $.messager.progress({
        text: '正在保存....',
        interval: 100
    });
    $.ajax({
        type: "post",
        url: '/p/parking/SaveDerateConfig',
        data: $("#divParkDerateBoxForm").serialize(),
        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
        success: function (data) {
            if (data.result) {
                $.messager.progress("close");
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '数据保存成功!',
                    title: "系统提示"
                });
                GetDerateConfigData();
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function GetDerateConfigData() {
    var parkingId = $("#hiddDerateParkingId").val();
    $.messager.progress({
        text: '正在获取....',
        interval: 100
    });
    $.ajax({
        type: "post",
        url: '/p/parking/GetDerateConfigData',
        data: "parkingId=" + parkingId,
        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
        success: function (data) {
            if (data.result) {
                $.messager.progress("close");
                $("#tableDerateConfig tbody tr").remove();
                for (var i = 0; i < data.data.length; i++) {
                    FillParkDerateTr(data.data[i].RecordID, data.data[i].ConsumeStartAmount, data.data[i].ConsumeEndAmount, data.data[i].DerateType, data.data[i].DerateValue);
                }
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function FillParkDerateTr(recrodId, startAmount, endAmount, derateType, derateValue) {
    var derateTypeDes = "";
    var derateUnit = "";
    if (derateType == 0) {
        derateTypeDes = "折扣";
        derateUnit = "折";
    }
    if (derateType ==1) {
        derateTypeDes = "减免金额";
        derateUnit = "元";
    }
    if (derateType == 2) {
        derateTypeDes = "减免时间";
        derateUnit = "分钟";
    }
    var trHtml = $('<tr><td>' + startAmount + '元 至 ' + endAmount + '元</td><td>' + derateTypeDes + '</td><td>' + derateValue + ' ' + derateUnit + '</td><td><a href="#" onclick="return btnUpdateDerateConfig(' + "'" + recrodId + "'" + ',' + "'" + startAmount + "'" + ',' + "'" + endAmount + "'" + ',' + "'" + derateType + "'" + ',' + "'" + derateValue + "'" + ')">修改</a>&nbsp;&nbsp;<a href="#" onclick="return btnDeleteDerateConfig(' + "'" + recrodId + "'" + ')">删除</a></td></tr>');
    $("#tableDerateConfig tbody").append(trHtml);
}
function btnUpdateDerateConfig(recrodId, startAmount, endAmount, derateType, derateValue) {
    SetDerateTypeDefault(derateType);
    $("#hiddDerateRecordID").val(recrodId);
    $("#txtConsumeStartAmount").numberspinner("setValue", startAmount);
    $("#txtConsumeEndAmount").numberspinner("setValue", endAmount);
    $("#sltDerateType").val(derateType);
    $("#txtDerateValue").numberspinner("setValue", derateValue);
    $("#btnAddDerate").hide();
    $("#btnUpdateDerate").show();
    $("#btnCancelUpdate").show();
}
function ShowParkDerateBox() {
    $('#divParkDerateBox').show().dialog({
        title: "消费减免",
        width: 700,
        height: 300,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divParkDerateBox').dialog('close');
            }
        }]
    });
}
function btnDeleteDerateConfig(recordId) {
    $.ajax({
        type: "post",
        url: '/p/parking/DeleteDerateConfig',
        data: "recordId=" + recordId,
        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
        success: function (data) {
            if (data.result) {
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '删除成功!',
                    title: "系统提示"
                });
                GetDerateConfigData();
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function UpdateDerateConfig() {
    var recordId = $("#hiddDerateRecordID").val();
    if ($.trim(recordId) == "") {
        $.messager.alert('系统提示', "获取优免配置编号失败", 'error');
        return;
    }
    if (!CheckSubmitDerateData()) {
        return;
    }
    $.messager.progress({
        text: '正在保存....',
        interval: 100
    });
    $.ajax({
        type: "post",
        url: '/p/parking/SaveDerateConfig',
        data: $("#divParkDerateBoxForm").serialize(),
        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
        success: function (data) {
            if (data.result) {
                $.messager.progress("close");
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '数据保存成功!',
                    title: "系统提示"
                });
                CancelUpdateConfig();
                GetDerateConfigData();
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}
function CancelUpdateConfig() {
    $("#hiddDerateRecordID").val("");
    $("#txtConsumeStartAmount").numberspinner("setValue", "0.0");
    $("#txtConsumeEndAmount").numberspinner("setValue", "0.0");
    $("#sltDerateType").val("0");
    $("#txtDerateValue").numberspinner("setValue", "0.0");
    $("#btnAddDerate").show();
    $("#btnUpdateDerate").hide();
    $("#btnCancelUpdate").hide();
}
function SetDerateTypeDefault(derateType) {
    if (derateType == "0") {
        $("#spanUnit").text("折");
        $("#txtDerateValue").numberspinner({
            min: 0,
            max: 1,
            increment:0.1,
            precision: 1
        });
    }
    if (derateType == "1") {
        $("#spanUnit").text("元");
        $("#txtDerateValue").numberspinner({
            min: 0,
            max: 10000,
            increment: 1,
            precision: 1
        });
    }
    if (derateType == "2") {
        $("#spanUnit").text("分钟");
        $("#txtDerateValue").numberspinner({
            min: 0,
            max: 10000,
            increment: 1,
            precision: 1
        });
    }
}
function CheckSubmitDerateData() {
    var startAmount = $("#txtConsumeStartAmount").numberspinner("getValue");
    if ($.trim(startAmount) == "" || parseFloat(startAmount) < 0) {
        $.messager.alert('系统提示', "消费开始金额格式不正确", 'error');
        return false;
    }
    var endAmount = $("#txtConsumeEndAmount").numberspinner("getValue");
    if ($.trim(endAmount) == "" || parseFloat(endAmount) < 0) {
        $.messager.alert('系统提示', "消费结束金额格式不正确", 'error');
        return false;
    }
    if (parseFloat(endAmount) <= parseFloat(startAmount)) {
        $.messager.alert('系统提示', "消费开始金额不能大于消费结束金额", 'error');
        return false;
    }
    var derateValue = $("#txtDerateValue").numberspinner("getValue");
    if ($.trim(derateValue) == "" || parseFloat(derateValue) < 0) {
        $.messager.alert('系统提示', "减免值格式不正确", 'error');
        return false;
    }
    return true;
}
$(function () {
    $("#sltDerateType").change(function () {
        SetDerateTypeDefault($(this).val());
    });
});