$(function () {
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            $('#tableArticle').datagrid('load', { companyId: record.id });
        }
    });
    BindArticle();
});
function BindArticle() {
    $('#tableArticle').datagrid({
        url: '/nwx/WXArticle/GetWXArticleData/',
        singleSelect: true,
        columns: [[
                    { field: 'GroupId', title: 'GroupId', width: 100, hidden: true },
                    { field: 'Title', title: '标题', width: 400 },
                    { field: 'Count', title: '图文总数量', width: 100 }
				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableArticle').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/nwx/WXArticle/GetArticleOperatePurview', function (result) {
                    $('#tableArticle').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}
function Add() {
    var selectRow = $('#treeCompany').tree('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请先选择单位!");
        return;
    }
    location.href = "/nwx/WXArticle/Edit?companyId=" + selectRow.id;
}
function Update() {
    var model = $('#tableArticle').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要修改的图文!");
        return;
    }
    location.href = "/nwx/WXArticle/Edit?groupId=" + model.GroupId;
}
function Delete() {
    var model = $('#tableArticle').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要删除的图文!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的图文吗?',
            function (r) {
                if (r) {
                    $.post('/nwx/WXArticle/Delete?groupId=' + model.GroupId,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除图文成功',
                                title: "删除图文"
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
    $('#tableArticle').datagrid('load');
}