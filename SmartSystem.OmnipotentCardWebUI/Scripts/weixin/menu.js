﻿$(function () {
    BindMenus();
    BindComboboxData();
    $("#divUrl").show();
});
function BindComboboxData() {
    $('#cmdModule').combobox({
        url: '/w/WXMenu/GetWeiXinModuleData',
        valueField: 'EnumValue',
        textField: 'Description'
    });
    $('#cmbMenuType').combobox({
        url: '/w/WXMenu/GetMenuTypeData',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            ShowEidtItem(record.EnumValue);
        }
    });
    $('#cmbKeywordId').combobox({
        url: '/w/WXMenu/GetKeywordData',
        valueField: 'id',
        textField: 'text'
    });
    BindMasterMenu();
}
function ShowEidtItem(type) {
    if (type == "0") {
        $("#divUrl").show();
        $("#divKeyword").hide();
        $("#divModule").hide();
    }
    if (type == "1") {
        $("#divUrl").hide();
        $("#divKeyword").show();
        $("#divModule").hide();
    }
    if (type == "2") {
        $("#divUrl").hide();
        $("#divKeyword").hide();
        $("#divModule").show();
    }
}
function BindMenus() {
    $('#tableMenu').treegrid({
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/w/WXMenu/GetMenuData',
        idField: 'id',
        treeField: 'MenuName',
        columns: [[
                    { title: 'id', field: 'id', width: 80, hidden: true },
                    { title: 'Url', field: 'Url', width: 80, hidden: true },
                    { title: 'HasChildMenu', field: 'HasChildMenu', width: 80, hidden: true },
                    { title: 'KeywordId', field: 'KeywordId', width: 80, hidden: true },
                    { title: 'ModuleId', field: 'ModuleId', width: 80, hidden: true },
                    { title: 'MenuType', field: 'MenuType', width: 80, hidden: true },
                    { title: 'Sort', field: 'Sort', width: 80, hidden: true },
                    { title: 'MasterID', field: 'MasterID', width: 80, hidden: true },
                    { field: 'MenuName', title: '菜单名称', width: 80 },
                    { field: 'MenuTypeDes', title: '菜单类型', width: 80 },
                    { field: 'MenuTypeValue', title: '对应值', width: 200 },
				]],
        pagination: false,
        rownumbers: false,
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableMenu').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/w/WXMenu/GetMenuOperatePurview', function (result) {
                    $('#tableMenu').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}
function Add() {
    var masterId = '-1';
    var model = $('#tableMenu').datagrid('getSelected');
    if (model != null) {
        if (model.MasterID == '') {
            masterId = model.id;
        } else {
            masterId = model.MasterID;
        }

    }
    AddOrUpdate("添加菜单");
    $('#divMenuBoxForm').form('load', {
        ID: '',
        MenuName: '',
        Sort: '0',
        Url: ''
    })

    $("#cmbMenuType").combobox("setValue", "0");
    $("#cmbKeywordId").combobox("setValue", "");
    $("#cmdModule").combobox("setValue", "0");
    $("#cmbMasterID").combobox("setValue", masterId);
    ShowEidtItem("0");
    $("#divMenu").show();
    $("#divMenuType").show();
}
function PublishMenu() {
    $.messager.confirm('系统提示', '确定要发布菜单吗?',
            function (r) {
                if (r) {
                    $.post('/w/WXMenu/PublishMenu',
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '发布成功',
                                title: "发布菜单"
                            });
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}

function Update() {
    var model = $('#tableMenu').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要修改的菜单!");
        return;
    }
    AddOrUpdate("修改菜单");
    $('#divMenuBoxForm').form('load', {
        ID: model.id,
        MenuName: model.MenuName,
        Sort: model.Sort,
        Url: model.Url
    })
    var masterId = '-1';
    if (model.MasterID != '') {
        masterId = model.MasterID;
    }
    $("#cmbMenuType").combobox("setValue", model.MenuType);
    $("#cmbKeywordId").combobox("setValue", model.KeywordId);
    $("#cmdModule").combobox("setValue", model.ModuleId);
    $("#cmbMasterID").combobox("setValue", masterId);

    ShowEidtItem(model.MenuType);
    if (model.HasChildMenu == "1") {
        $("#divUrl").hide();
        $("#divKeyword").hide();
        $("#divModule").hide();
        $("#divMenu").hide();
        $("#divMenuType").hide();
        SetMenuBoxHeight("1");
    } else {
        $("#divMenu").show();
        $("#divMenuType").show();
        SetMenuBoxHeight("0");
    }
}
function Delete() {
    var model = $('#tableMenu').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要删除的菜单!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的菜单吗?',
            function (r) {
                if (r) {
                    $.post('/w/WXMenu/Delete?id=' + model.id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除菜单成功',
                                title: "删除菜单"
                            });
                            Refresh();
                            BindMasterMenu();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function BindMasterMenu() {
    $('#cmbMasterID').combobox({
        url: '/w/WXMenu/GetMasterMenu',
        valueField: 'id',
        textField: 'text'
    });
}
function Refresh() {
    $('#tableMenu').treegrid('load');
}
function SetMenuBoxHeight(type) {
    if (type == "0") {
        $('#divMenuBox').show().dialog({ height: 380 });
    } else {
        $('#divMenuBox').show().dialog({ height: 250 });
    }

}
function AddOrUpdate(title) {
    $('#divMenuBox').show().dialog({
        title: title,
        width: 380,
        height: 380,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divMenuBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!CheckSubmitData()) {
                    return;
                }
                if ($('#divMenuBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/w/WXMenu/AddOrUpdate',
                        data: $("#divMenuBoxForm").serialize(),
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
                                $('#divMenuBox').dialog('close');
                                Refresh();
                                BindMasterMenu();
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
    var menuName = $("#txtMenuName").val();
    if (menuName.length == 0 || menuName.length > 8) {
        $.messager.alert('系统提示', "菜单名称必须在8个字之内", 'error');
        return false;
    }
    var type = $("#cmbMenuType").combobox("getValue");
    if (type == "0") {
        var urlReg = /^(http|https)\:///;
        var url = $("#txtUrl").textbox("getValue");
        if ($.trim(url) == "" || !$.trim(url).match(urlReg)) {
            $.messager.alert('系统提示', "请输入链接URL", 'error');
            return false;
        }
    }
    if (type == "1") {
        var key = $("#cmbKeywordId").combobox("getValue");
        if ($.trim(key) == "") {
            $.messager.alert('系统提示', "请选择关键字", 'error');
            return false;
        }
    }
    if (type == "2") {
        var key = $("#cmdModule").combobox("getValue");
        if ($.trim(key) == "") {
            $.messager.alert('系统提示', "请选择模块", 'error');
            return false;
        }
    }
    return true;
}