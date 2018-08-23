$(function () {
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            //$('#tableVillage').datagrid('loadData', { total: 0, rows: [] }); 
            $('#tableVillage').datagrid('load', { companyId: record.id });
        }
    });
});
function btnSearchTreeContent() {
    var content = $("#txtSearchTreeContent").val();
    $("#treeCompany").tree("search", content);
}
function Add() {
    var companyId;
    var selectRow = $('#treeCompany').tree('getSelected');
    if (selectRow != null) {
        companyId = selectRow.id;
    }
    EditVillageBox('增加小区');
    $('#divVillageBoxForm').form('load', {
        VID: '',
        VNo: '',
        VName: '',
        CPID: '',
        ProxyNo: '',
        LinkMan: '',
        Mobile: '',
        Address: ''
    });
    $("#CPID").combotree('setValue', companyId);
    $(".trAgentID").hide();
}
function Update() {
    var selectRow = $('#tableVillage').datagrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要修改的小区信息!");
        return;
    }
    EditVillageBox('修改小区');
    $('#divVillageBoxForm').form('load', {
        VID: selectRow.VID,
        VNo: selectRow.VNo,
        VName: selectRow.VName,
        CPID: selectRow.CPID,
        ProxyNo: selectRow.ProxyNo,
        LinkMan: selectRow.LinkMan,
        Mobile: selectRow.Mobile,
        Address: selectRow.Address
    });
    $("#CPID").combotree('setValue', selectRow.CPID);
    $(".trAgentID").show();
}
function Delete(){
      var selectRow = $('#tableVillage').datagrid('getSelected');

    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要删除的小区信息!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的小区信息吗?', function (r) {
        if (r) {
            $.post('/b/Village/Delete?villageId=' + selectRow.VID,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除小区成功',
                                title: "删除小区"
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
    $('#tableVillage').datagrid("reload");
}

$(function () {
    $('#CPID').combotree({
        url: '/CompanyData/GetCompanyTree'
    });
    $('#tableVillage').datagrid({
        url: '/b/Village/GetVillageData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'VID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'VID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 100, hidden: true },
                    { field: 'VID', title: 'VID', width: 100, hidden: true },
                    { field: 'CPID', title: 'CPID', width: 100, hidden: true },
                    { field: 'VNo', title: '编号', width: 80 },
                    { field: 'VName', title: '名称', width: 130 },
                    { field: 'LinkMan', title: '联系人', width: 60 },
                    { field: 'Mobile', title: '联系电话', width: 80 },
                    { field: 'ProxyNo', title: '代理编码', width: 230 },
                     { field: 'Address', title: '地址', width: 300 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableVillage').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/b/Village/GetVillageOperatePurview', function (result) {
                    $('#tableVillage').datagrid("addToolbarItem", result);
                });
            }
        }
    });
});

EditVillageBox = function (data) {
    $('#divVillageBox').show().dialog({
        title: data,
        width: 360,
        height: 360,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#Savevillage').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if ($('#divVillageBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/b/Village/EditVillage',
                        data: $("#divVillageBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                        success: function (data) {
                            if (data.result) {
                                $.messager.progress("close");
                                $('#divVillageBox').dialog('close');
                                Refresh();
                                $.messager.alert('系统提示', data.msg, 'success');
                            }else {
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