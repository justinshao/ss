function LoadUserRoleTreeData(userId, isdefault) {
    $('#treeRole').tree({
        checkbox: true,
        url: '/u/User/GetRoleTreeData?userId=' + userId,
        onLoadSuccess: function () {
//            if (isdefault == 1) {
//                $(this).find('span.tree-checkbox').unbind().click(function () {
//                    return false;
//                });
//            }
        }
    });
}
function LoadScopeTreeData(userId, isdefault) {
    $('#treeScope').tree({
        checkbox: true,
        url: '/u/User/GetScopeTreeData?userId=' + userId,
        onLoadSuccess: function () {
//            if (isdefault == 1) {
//                $(this).find('span.tree-checkbox').unbind().click(function () {
//                    return false;
//                });
//            }
        }
    });
}
$(function () {
    $('#tableUser').datagrid({
        url: '/u/User/GetUserTreeData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
                    { field: 'IsDefaultUser', title: 'IsDefaultUser', width: 80, hidden: true },
                    { field: 'RecordID', title: 'RecordID', width: 80, hidden: true },
                    { field: 'UserName', title: '用户姓名', width: 100 },
                    { field: 'UserAccount', title: '登录名', width: 100 },
                    { field: 'RoleDescription', title: '用户组', width: 150 },
                    { field: 'ScopeDescription', title: '作用域', width: 300 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableUser').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/u/User/GetSysUserOperatePurview', function (result) {
                    $('#tableUser').datagrid("addToolbarItem", result);
                });
            }
        }
    });


    $("#btnQueryUserInfo").click(function () {
        var userAccount = $("#txtqueryUserAccount").val();
        $('#tableUser').datagrid('load', { queryUserAccount: userAccount });
    });
});
//弹出编辑对话框
function SaveOrEditSysUser(data) {
    $('#editSysUserBox').show().dialog({
        title: data,
        width: 650,
        height: 480,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [
            {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#editSysUserBox').dialog('close');
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    if (!CheckSubmitData()) {
                        return;
                    }
                  
                    if ($('#editSysUserBoxForm').form('validate')) {
                        $.messager.progress({
                            text: '正在保存....',
                            interval: 100
                        });

                        $.ajax({
                            type: "post",
                            url: '/u/User/SaveUser',
                            data: $("#editSysUserBoxForm").serialize(),
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
                                    Refresh();
                                    $('#editSysUserBox').dialog('close');
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
    var userName = $("#txtUserName").val();
    if ($.trim(userName) == "") {
        $.messager.alert('系统提示', '姓名不能为空!', 'error');
        return false;
    }

    var account = $("#txtUserAccount").val();
    if ($.trim(account) == "") {
        $.messager.alert('系统提示', '登录名不能为空!', 'error');
        return false;
    }

    var recordId = $("#hiddRecordID").val();
    var password = $("#txtPassword").val();
    if ($.trim(recordId) == "" && $.trim(password) == "") {
        $.messager.alert('系统提示', '密码不能空!', 'error');
        return false;
    }
    var nodes = $('#treeRole').tree('getChecked');
    var srole = '';
    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].attributes.type != 1) {
            continue;
        }

        if (srole != '') srole += ',';
        srole += nodes[i].id;
    }
    if ($.trim(srole) == "") {
        $.messager.alert('系统提示', '请选择角色!', 'error');
        return false;
    }
    $("#hiddUserRoles").val(srole);

    var scopeNodes = $('#treeScope').tree('getChecked');
    var scopes = '';
    for (var i = 0; i < scopeNodes.length; i++) {
        if (scopeNodes[i].attributes.type != 1) {
            continue;
        }
        if (scopes != '') scopes += ',';
        scopes += scopeNodes[i].id;
    }
    if ($.trim(scopes) == "") {
        $.messager.alert('系统提示', '请选择用户组!', 'error');
        return false;
    }
    $("#hiddUserScopes").val(scopes);
    return true;
}
function Add()
 {
    SaveOrEditSysUser("添加用户");
    $('#editSysUserBoxForm').form('load', {
        RecordID: "",
        UserName: "",
        UserAccount: "",
        Password: "",
        IsDefaultUser: 0
    })
    $("#txtPassword").prop("required", true);
    $("#txtUserID").removeAttr("disabled");
    LoadUserRoleTreeData("", 0);
    LoadScopeTreeData("", 0);
}
function Update() {
    var selectRow = $('#tableUser').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择需要修改的用户信息!");
        return;
    }
    $('#editSysUserBoxForm').form('clear');
    SaveOrEditSysUser("修改用户");
    $('#editSysUserBoxForm').form('load', {
        RecordID: selectRow.RecordID,
        UserName: selectRow.UserName,
        UserAccount: selectRow.UserAccount,
        IsDefaultUser: selectRow.IsDefaultUser,
        Password: ""
    })
    $("#txtUserID").prop("disabled", "disabled");
    LoadUserRoleTreeData(selectRow.RecordID, selectRow.IsDefaultUser);
    LoadScopeTreeData(selectRow.RecordID, selectRow.IsDefaultUser);
}
function Delete() {
    var selectRow = $('#tableUser').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择需要删除的用户!");
        return;
    } else {
        if (selectRow.IsDefaultUser == 1) {
            $.messager.alert("系统提示", "系统默认用户不能删除!");
            return;
        }
        $.messager.confirm('系统提示', '确定删除选中的用户吗?',
            function (r) {
                if (r) {
                    $.post('/u/User/Delete?recordId=' + selectRow.RecordID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除用户成功',
                                title: "删除用户"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
    }
}
function Refresh() {
    $('#tableUser').datagrid('load');
}