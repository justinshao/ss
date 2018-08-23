function send() {
    var selectRow = $('#tablemessage').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要发送的通知!");
        return;
    }

    $.messager.confirm('系统提示', '确定发送选中的信息吗?',
        function (r) {
            if (r) {
                $.post('/b/Message/Send?title=' + selectRow.MessageTitle + "&text=" + selectRow.MessageTxt,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '发送成功',
                                title: "发送通知"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
            }
        });

}
function Add() { 
    AddOrUpdateBox('增加'); 
    $('#divMessageBoxForm').form('load', { 
        RecordID: "",
        MessageTitle: "",
        MessageTxt: ""
    })
    $('#divMessageBox').show().dialog({ height: 430 });
}
function Update() {
    var model = $('#tablemessage').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要修改的信息!");
        return;
    }
    $('#divMessageBoxForm').form('clear'); 
    AddOrUpdateBox("修改");
    $('#divMessageBoxForm').form('load', { 
        RecordID: model.RecordID,
        MessageTitle: model.MessageTitle,
        MessageTxt: model.MessageTxt,
    })
    $('#divMessageBox').show().dialog({ height: 430 });
}
function Delete() {
    var selectRow = $('#tablemessage').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要删除的通知!");
        return;
    }

    $.messager.confirm('系统提示', '确定删除选中的信息吗?',
        function (r) {
            if (r) {
                $.post('/b/Message/Delete?RecordID=' + selectRow.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除成功',
                                title: "删除通知记录"
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
    $('#tablemessage').datagrid('reload');
}
AddOrUpdateBox = function (title) {
    $('#divMessageBox').show().dialog({
        title: title,
        width: 350,
        height: 500,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divMessageBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divMessageBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });

                    $.ajax({
                        type: "post",
                        url: '/b/Message/EditMessage',
                        data: $("#divMessageBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                        success: function (data) {

                            if (data.result) {
                                $('#divMessageBox').dialog('close');
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '发送通知成功!',
                                    title: "发送通知"
                                });
                                $.messager.progress("close");
                                Refresh();
                               
                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                }
            }
        }
        ]

    });
}


$(function () {

    $('#txtStartTime').datetimebox('setValue', currentdate00());
    $('#txtEndTime').datetimebox('setValue', currentdate23());

    $("#btnQueryData").click(function () {
        var text = $("#txtMessage").val();
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var options = $('#tablemessage').datagrid('options');
        options.url = "/b/Message/GetMessageData";
        $('#tablemessage').datagrid('load', { text: text, starttime: starttime, endtime: endtime });
    });
    $('#tablemessage').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        url: '/b/Message/GetMessageData/',
        idField: 'RecordID',
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        columns: [[
            { field: 'ID', title: 'ID', width: 100, hidden: true },
            { field: 'RecordID', title: 'RecordID', width: 100, hidden: true },
            { field: 'MessageTitle', title: '标题', width: 100},
            { field: 'MessageTxt', title: '内容', width: 400 },
            { field: 'LastUpdateTimeToString', title: '时间', width: 200  },
            { field: 'MessageStates', title: '通知状态', width: 100 },
            { field: 'PostStates', title: '发送状态', width: 100 },
            { field: 'DataStatus', title: '是否有效', width: 50, hidden: true },
            { field: 'UserID', title: '操作员id', width: 150, hidden: true},
            { field: 'UserName', title: '操作员', width: 100 }
        ]]
        , onBeforeLoad: function (row, data) {
            var dpanel = $('#tablemessage').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/b/Message/GetMessageOperatePurview', function (result) {
                    $('#tablemessage').datagrid("addToolbarItem", result);
                });
            }
        }
    }); 
});

function currentdate00() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " 00:00:00"
    } catch (ex) {
        return "";
    }
}
function currentdate23() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " 23:59:59"
    } catch (ex) {
        return "";
    }
}
 