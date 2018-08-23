$(function () {
    $("#addrole").click(function () {
        AddOrEditUserRole("添加角色");
        $('#editUserRoleBoxForm').form('load', {
            RecordID: "",
            RoleName: ""
        })
    })
    $("#updaterole").click(function () {
        var selectRow = $('#treeUserRole').tree('getSelected');
        if (selectRow == null) {
            $.messager.alert('系统提示', '请选择需要修改的角色!', 'error');
            return;
        }
        AddOrEditUserRole("修改角色");
        $('#editUserRoleBoxForm').form('load', {
            RecordID: selectRow.id,
            RoleName: selectRow.text
        })
    })
    $("#deleterole").click(function () {
        var selectRow = $('#treeUserRole').tree('getSelected');
        if (selectRow == null) {
            $.messager.alert('系统提示', '请选择需要删除的角色!', 'error');
            return;
        }
        if (selectRow.attributes.isdefaultrole == 1) {
            $.messager.alert('系统提示', '系统默认角色不能删除!', 'error');
            return;
        }
        $.messager.confirm('系统提示', '确定删除选中的角色吗?',
            function (r) {
                if (r) {
                    $.post('/u/Role/DeleteRole?recordId=' + selectRow.id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除用户组成功!',
                                title: "删除用户组"
                            });
                            RefreshRole();
                        } else {
                            $.messager.progress("close");
                            $.messager.alert('系统提示', data.msg, 'error');

                        }
                    });
                }
            });
    })
    $("#reloadrole").click(function () {
        RefreshRole();
    })
    $('#treeUserRole').tree({
        url: '/u/Role/GetSystemRoles',
        onSelect: function (node) {
            GetSystemModule(node.id);
        }
    });
})
function GetSystemModule(roleId) {
    $('#treeRoleModule').tree({
        url: '/u/Role/GetSystemModule?roleId=' + roleId,
        checkbox: true,
        onLoadSuccess: function (node, data) {
            var checkedTree = $('#treeRoleModule').tree('getChecked');
            for (var i = 0; i < checkedTree.length; i++) {
                if (checkedTree[i].attributes.type == "1" && checkedTree[i].attributes.isdefault == "1") {
                    $('#treeRoleModule').tree('disableCheck', checkedTree[i].id); //禁用  
                }
            }

        }
    });
}
function RefreshRole() {
    $('#treeUserRole').tree("reload");
}
AddOrEditUserRole = function (data) {
    $('#editUserRoleBox').show().dialog({
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
                    $('#editUserRoleBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {

                    if ($('#editUserRoleBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });
                        $.ajax({
                            type: "post",
                            url: '/u/Role/SaveRole',
                            data: $("#editUserRoleBoxForm").serialize(),
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
                                    RefreshRole();
                                    $('#editUserRoleBox').dialog('close');
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
    function btnSaveRoleModule() {
        var selectRole = $('#treeUserRole').tree('getSelected');
        if (selectRole == null) {
            $.messager.alert('系统提示', '请选择角色!', 'error');
            return;
        }
        var nodes = $('#treeRoleModule').tree('getChecked');
        var modules = '';
        for (var i = 0; i < nodes.length; i++) {
            if (modules != '') modules += ',';
            modules += nodes[i].id;
        }
        var indeterminates = $('#treeRoleModule').tree('getChecked', 'indeterminate');    // 获取不确定的节点
        for (var i = 0; i < indeterminates.length; i++) {
            if (modules != '') modules += ',';
            modules += indeterminates[i].id;
        }
//        if ($.trim(modules) == "") {
//            $.messager.alert('系统提示', '请选择授权模块!', 'error');
//            return;
//        }
        $.messager.progress({
            text: '正在保存....',
            interval: 100
        });
        $.ajax({
            type: "post",
            url: '/u/Role/SaveRoleModule',
            data: "selectModuleIds=" + modules + "&roleId=" + selectRole.id,
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
                    $.messager.progress("close");
                } else {
                    $.messager.progress("close");
                    $.messager.alert('系统提示', data.msg, 'error');

                }
            }
        });

    }