$(function () {
    $('#tableOtherConfigBox').datagrid({
        url: '/w/WXOtherConfig/GetWxOtherConfigData',
        singleSelect: true,
        columns: [[
                    { field: 'DefaultDescription', title: 'DefaultDescription', width: 100, hidden: true },
                    { field: 'ConfigType', title: 'ConfigType', width: 100, hidden: true },
                    { field: 'Description', title: '配置名称', width: 170 },
                    { field: 'ConfigValue', title: '配置值', width: 500 }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableOtherConfigBox').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/w/WXOtherConfig/GetWxOtherConfigOperatePurview', function (result) {
                    $('#tableOtherConfigBox').datagrid("addToolbarItem", result);
                });
            }
        }
    });

});
function Update() {
    var config = $('#tableOtherConfigBox').datagrid('getSelected');
    if (config == null) {
        $.messager.alert("系统提示", "请选择需要修改的配置!");
        return;
    }
    $('#editConfigBoxForm').form('load', {
        ConfigType: config.ConfigType,
        ConfigValue: config.ConfigValue
    })
    $('#editConfigBox').show().dialog({
        title: "修改配置",
        width: 350,
        height: 180,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#editConfigBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $.ajax({
                        type: "post",
                        url: '/w/WXOtherConfig/AddOrUpdate',
                        data: $("#editConfigBoxForm").serialize(),
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
                                    title: "系统提示"
                                });
                                $.messager.progress("close");
                                Refresh();
                                $('#editConfigBox').dialog('close');
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
    $('#tableOtherConfigBox').datagrid('load');
}
function Format() {
    var config = $('#tableOtherConfigBox').datagrid('getSelected');
    if (config == null) {
        $.messager.alert("系统提示", "请选择需要查看的配置!");
        return;
    }
    var title = "对应格式";
    if (config.ConfigType == 0 || config.ConfigType == 1 || config.ConfigType == 2 || config.ConfigType == 3 ||
        config.ConfigType == 4 || config.ConfigType == 5 || config.ConfigType == 6 || config.ConfigType == 7 ||
        config.ConfigType == 8) {
        title = "对应微信模板消息格式";
    }
    $("#divContent").html(config.DefaultDescription);
    $('#editDetailBox').show().dialog({
        title: title,
        width: 350,
        height: 230,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                $.messager.progress("close");
            }
        }]
    });

}
   