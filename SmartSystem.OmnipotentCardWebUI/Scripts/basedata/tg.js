function Add() {
    
    $('#divPersontgBoxForm').form('load', {
        id: '',
        name: '',
        phone: '',
        bz: '',
        PKID: '', 
        PKName: '' 
    })
    AddOrUpdateBox("增加"); 
};
function Update() {
    var selectRow = $('#tablePerson').treegrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要修改信息!");
        return;
    }
    $('#divPersontgBoxForm').form('load', {
        id: selectRow.id,
        name: selectRow.name,
        phone: selectRow.phone,
        bz: selectRow.bz,
        PKID: selectRow.PKID ,
        PKName: selectRow.PKName, 
        selectParks: selectRow.PKName, 
    }); 

    AddOrUpdateBox("修改");
    //$('#divCompanyBox').show().dialog({ height: 250 });
}
function Delete() {
    var selectRow = $('#tablePerson').treegrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要删除的人员信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的人员信息吗?',
        function (r) {
            if (r) {
                $.post('/TgPerson/Delete?personId=' + selectRow.id,
                    function (data) {
                        if (data.result) {
                            //$('#tablePerson').treegrid('remove', selectRow.id);
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '数据删除成功!',
                                title: "保存数据"
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
    $('#tablePerson').datagrid({
        singleSelect: true,
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/TgPerson/tgPersonInfo',
        idField: 'id',
        columns: [[
            { field: 'id', title: '人员编号ID', width: 100 },
            { field: 'name', title: '姓名', width: 100 },
            { field: 'phone', title: '联系电话', width: 100 },
            { field: 'bz', title: '备注', width: 100 },
            { field: 'count', title: '推广数', width: 100, hidden: true },
            { field: 'PKID', title: '车场ID', width: 100, hidden: true },
            { field: 'PKName', title: '车场名称', width: 100 },

        ]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tablePerson').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/TgPerson/GETtgOperatePurview', function (result) {
                    $('#tablePerson').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}

function BindGetParkTrees() {
    $.ajax({
        url: '/S/Statistics/GetParks',
        type: 'post',
        success: function (data) {
            var comdata = [];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].PKName, "id": data[i].PKID });
            }

            $('#selectParks').combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text',
                onSelect: function (data) { 
                    $('#PKID').val(data.id);
                    $('#PKName').val(data.text);
                }
            }); 
        }
    });
    
}
$(function () {
    BindGetParkTrees();
    
      
    $('#tablePerson').datagrid({ 
        singleSelect: true,
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/TgPerson/tgPersonInfo',
        idField: 'id',
        columns: [[
            { field: 'id', title: '人员编号ID', width: 100 },
            { field: 'name', title: '姓名', width: 100 },
            { field: 'phone', title: '联系电话', width: 100 },
            { field: 'bz', title: '备注', width: 100 },
            { field: 'count', title: '推广数', width: 100, hidden: true },
            { field: 'PKID', title: '车场ID', width: 100, hidden: true },
            { field: 'PKName', title: '车场名称', width: 100 },

        ]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tablePerson').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/p/TgPerson/GETtgOperatePurview', function (result) {
                    $('#tablePerson').datagrid("addToolbarItem", result);
                });
            }
        }
    });
});

AddOrUpdateBox = function (title) {
    $('#divPersontgBox').show().dialog({
        title: title,
        width: 350,
        height: 250,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divPersontgBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                //var activeName = $("#txtActiveName").val();
                if ($('#divPersontgBox').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: 'post',
                        url: '/TgPerson/EdittgPerson?title=' + title,
                        data: $("#divPersontgBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!' + data, 'error'); },
                        success: function (data) {
                            console.log(data.result);
                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '数据保存成功!',
                                    title: "保存数据"
                                });
                                $.messager.progress("close");
                                $('#divPersontgBox').dialog('close');
                                Refresh();
                                $('#selectParks').html("");
                                $('#PKID').val("");
                                $('#PKName').val("");
                                BindGetParkTrees();
                                $.messager.alert('系统提示', data.msg, 'success');
                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', data.msg, 'error');
                            }
                        }
                    })

                }
            }
        }]

    });
}
 

function DownloadQRCode() {
    var selectPerson = $('#tablePerson').datagrid('getSelected');
    if (selectPerson == null) {
        $.messager.alert("系统提示", "请选择人员!");
        return;
    }
    $("#hiddDownloadPersonId").val(selectPerson.id);
    $("#hiddDownloadParkingId").val(selectPerson.PKID);
    GetQrCode();
}


function GetQrCode() {
    var personId = $("#hiddDownloadPersonId").val();
    var PKID = $("#hiddDownloadParkingId").val();
    $.ajax({
        type: "POST",
        url: "/TgPerson/DownloadQRCode",
        data: "size=0&personId=" + personId + "&parkingId=" + PKID+" ",
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

function btnDownloadQRCode(obj, size) {
    odownLoad = $(obj).get(0);
    var url = $(obj).attr("href");
    StartDownLoad(url, obj);
}

 



//            handler: function () {
//                tj(document.getElementById("txtCPName").value, document.getElementById("lxdh").value, document.getElementById("bz").value);
//            }

//function tj(a, b, c) {
//    var parkingId = "";
//    $.ajax({
//        type: "POST",
//        url: "/tg/addNum",
//        data: "a=" + a + "&b=" + b + "&c=" + c + "",
//        async: true,
//        success: function (data) {
//            if (data) {
//                $.messager.show({
//                    width: 200,
//                    height: 100,
//                    msg: '数据保存成功!',
//                    title: "保存人员"
//                });
//                $.messager.progress("close");
//                Refresh();
//                $('#divCompanyBox').dialog('close');
//            } else {
//                $.messager.progress("close");
//                $.messager.alert('系统提示', data.msg, 'error');

//            }
//        }
//    });
//}

//AddEdit = function (data) {
//    $('#Directional').show().dialog({
//        title: data,
//        width: 393,
//        height: 267,
//        modal: true,
//        collapsible: false,
//        minimizable: false,
//        maximizable: false,
//        buttons: [{
//            text: '取消',
//            iconCls: 'icon-cancel',
//            handler: function () {
//                $.messager.confirm('提示', '确认取消创建或修改活动?', function (b) {
//                    if (b) {
//                        $('#Directional').dialog('close');
//                    } else {
//                        //alert('取消');
//                    }
//                });

//            }
//        }, {
//            text: '确定',
//            iconCls: 'icon-ok',
//            handler: function () {
//                var activeName = $("#txtActiveName").val();
//                if ($.trim(activeName) == "") {
//                    $.messager.alert('系统提示', "活动名称不能为空", 'error');
//                    return false;
//                }
//                if ($('#FullPromotionBox').form('validate')) {
//                    $.messager.progress({
//                        text: '正在保存....',
//                        interval: 100
//                    });
//                    $.ajax({
//                        type: 'post',
//                        url: '/DirectionalSecurities/EditDirectional',
//                        data: $("#DirectionalBox").serialize(),
//                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
//                        success: function (data) {
//                            if (data.result) {
//                                $.messager.show({
//                                    width: 200,
//                                    height: 100,
//                                    msg: '数据保存成功!',
//                                    title: "保存数据"
//                                });
//                                $.messager.progress("close");
//                                $('#Directional').dialog('close');
//                                Refresh();
//                                $.messager.alert('系统提示', data.msg, 'success');
//                            } else {
//                                $.messager.progress("close");
//                                $.messager.alert('系统提示', data.msg, 'error');
//                            }
//                        }
//                    })

//                }
//            }
//        }]
//    });

//}


//function DownLoad() {
//    var selectRow = $('#tablePerson').treegrid('getSelected');
//    if (selectRow == null) {
//        $.messager.alert("系统提示", "请选择要下载的人员信息!");
//        return;
//    }
//    //$.messager.confirm('系统提示', '确定下载选中的人员信息吗?',
//    //function (r) {
//    //    if (r) {
//    //        $.post('/TgPerson/getEWM?parkingId=' + selectRow.id,
//    //            function (data) {
//    //                if (data.result) {
//    //                    for (var i = 0; i < data.data.length; i++) {
//    //                        var value = data.data[i].split('|');
//    //                        $("#btn" + value[0]).attr("href", value[1]);
//    //                    }
//    //                    ShowDownloadBox();
//    //                } else {
//    //                    $.messager.alert("系统提示", data.msg);
//    //                    return;
//    //                }
//    //                //window.open(data, "_black");
//    //            });

//    //    }
//    //});
//    $.ajax({
//        type: "POST",
//        url: "/TgPerson/getqr",
//        data: "size=0&parkingId=" + selectRow.id + "",
//        success: function (data) {
//            if (data.result) {
//                for (var i = 0; i < data.data.length; i++) {
//                    var value = data.data[i].split('|');
//                    $("#btn" + value[0]).attr("href", value[1]);
//                }
//                ShowDownloadBox();
//            } else {
//                $.messager.alert("系统提示", data.msg);
//                return;
//            }
//        }
//    });

//}


//function ShowDownloadBox() {
//    $('#divDownloadBox').show().dialog({
//        title: "下载二维码",
//        width: 450,
//        height: 300,
//        modal: true,
//        collapsible: false,
//        minimizable: false,
//        maximizable: false,
//        buttons: [{
//            text: '关闭',
//            iconCls: 'icon-cancel',
//            handler: function () {
//                $('#divDownloadBox').dialog('close');
//            }
//        }]
//    });
//}


//function DownLoad2() {
//    var parkingId = $("#hiddDownloadParkingId").val();
//    $.ajax({
//        type: "POST",
//        url: "/tg/DownloadQRCode",
//        data: "size=0&parkingId=" + parkingId + "",
//        success: function (data) {
//            if (data.result) {
//                for (var i = 0; i < data.data.length; i++) {
//                    var value = data.data[i].split('|');
//                    $("#btn" + value[0]).attr("href", value[1]);
//                }
//                ShowDownloadBox();
//            } else {
//                $.messager.alert("系统提示", data.msg);
//                return;
//            }
//        }
//    });
//}


//function btnDownloadQRCode(obj, size) {
//    odownLoad = $(obj).get(0);
//    var url = $(obj).attr("href");
//    StartDownLoad(url, obj);
//}

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


