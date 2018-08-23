
$(function () {
    $('#treeScope').tree({
        url: '/u/Scope/GetScopeTreeData',
        onSelect: function (node) {
            GetVillage(node.id);
        }
    });
    $("#btnAdd").click(function () {
        AddOrEditScope("添加作用域");
        $('#editScopeBoxForm').form('load', {
            ASID: "",
            ASName: ""
        })
    })
    $("#btnUpdate").click(function () {
        var selectRow = $('#treeScope').tree('getSelected');
        if (selectRow == null || selectRow.attributes.type == 0) {
            $.messager.alert('系统提示', '请选择需要修改的作用域!', 'error');
            return;
        }
        AddOrEditScope("修改作用域");
        $('#editScopeBoxForm').form('load', {
            ASID: selectRow.id,
            ASName: selectRow.text
        })
    })
    $("#btnDelete").click(function () {
        var selectRow = $('#treeScope').tree('getSelected');
        if (selectRow == null || selectRow.attributes.type == 0) {
            $.messager.alert('系统提示', '请选择需要删除的作用域!', 'error');
            return;
        }
        if (selectRow.attributes.isdefaultrole == 1) {
            $.messager.alert('系统提示', '系统默认作用域不能删除!', 'error');
            return;
        }
        $.messager.confirm('系统提示', '确定删除选中的作用域吗?',
            function (r) {
                if (r) {
                    $.post('/u/Scope/Delete?recordId=' + selectRow.id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除作用域成功!',
                                title: "删除作用域"
                            });
                            RefreshScope();
                        } else {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', data.msg, 'error');

                        }
                    });
                }
            });
    })
    $("#btnReload").click(function () {
        RefreshScope();
    })
});
function RefreshScope() {
    $('#treeScope').tree("reload");
}
function GetVillage(id) {
    $('#treeScopeVillage').tree({
        url: '/u/Scope/GetSysScopeAuthorize?scopeId=' + id,
        checkbox: true
    });
}
AddOrEditScope = function (data) {
    $('#editScopeBox').show().dialog({
        title: data,
        width: 300,
        height: 150,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#editScopeBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if ($('#editScopeBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });
                        $.ajax({
                            type: "post",
                            url: '/u/Scope/SaveScope',
                            data: $("#editScopeBoxForm").serialize(),
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
                                    RefreshScope();
                                    $('#editScopeBox').dialog('close');
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
function btnScopeAuthorizeVillage() {
    var nodes = $('#treeScopeVillage').tree('getChecked');
    var selectVillageIds = "";
    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].attributes.type == 0) {
            continue;
        }
        if (selectVillageIds != "") selectVillageIds += ",";
        selectVillageIds += nodes[i].id;
    }

    if ($.trim(selectVillageIds) == "") {
        $.messager.alert('系统提示', '请选择小区!', 'error');
        return;
    }
    $.messager.progress({
        text: '正在保存....',
        interval: 100
    });
    $.ajax({
        type: "post",
        url: '/u/Scope/SaveSysScopeAuthorize',
        data: "selectVillageIds=" + selectVillageIds,
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            $.messager.progress("close");
            if (data.result) {
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '数据保存成功!',
                    title: "保存车类型"
                });
            } else {
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}