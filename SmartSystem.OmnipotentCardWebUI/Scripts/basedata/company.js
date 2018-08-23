
function Add() {
    $('#MasterID').combotree({
        url: '/CompanyData/GetCompanyTree',
        onLoadSuccess: function (node, data) {
            var companyId = $("#hiddCurrUserCompayId").val();
            var selectRow = $('#tableCompany').treegrid('getSelected');
            if (selectRow != null) {
                companyId = selectRow.CPID;
            }
            $('#MasterID').combotree('setValue', companyId);
        }
    });
    BindCity();
    $(".trUserInfo").show();
    AddOrUpdateBox('增加单位信息');
    var masterId = $("#hiddCurrUserCompayId").val();
    $('#divCompanyBoxForm').form('load', {
        CPID: '',
        CPName: '',
        LinkMan: '',
        Mobile: '',
        ProvinceID: '',
        CityID: '',
        Address: '',
        UserAccount: '',
        UserPassword: '',
        MasterID: masterId
    })
    $('#divCompanyBox').show().dialog({ height: 430 });
}
function Update() {
    var selectRow = $('#tableCompany').treegrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要修改的单位信息!");
        return;
    }
    $('#MasterID').combotree({
        url: '/CompanyData/GetCompanyTree?excludeCompanyId=' + selectRow.CPID,
        onLoadSuccess: function (node, data) {
            $('#MasterID').combotree('setValue', selectRow.MasterID);

        }
    });
    $(".trUserInfo").hide();
    AddOrUpdateBox('修改单位信息');
    $('#divCompanyBoxForm').form('load', {
        CPID: selectRow.CPID,
        CPName: selectRow.CPName,
        LinkMan: selectRow.LinkMan,
        Mobile: selectRow.Mobile,
        Address: selectRow.Address,
        UserAccount: '',
        UserPassword: ''
    });
    $('#divCompanyBox').show().dialog({ height: 380 });
 
    if (selectRow.CityID != "0") {
        $.post('/City/GetSelectProvincesByCityId?cityId=' + selectRow.CityID,
            function (data) {
                if (data.result) {
                    $('#ProvinceID').combobox('setValue', data.data);
                        $('#CityID').combobox({
                            url: '/City/GetCitys?ProvinceID=' + data.data,
                            valueField: 'CityID',
                            textField: 'CityName',
                            onLoadSuccess: function () {
                                $('#CityID').combobox('setValue', selectRow.CityID.toString());
                            }
                        });
                }
            });
    }
}
function Delete() {
    var selectRow = $('#tableCompany').treegrid('getSelected');
    if (selectRow == null) {
        $.messager.alert("系统提示", "请选择要删除的单位信息!");
        return;
    }

    $.messager.confirm('系统提示', '确定删除选中的单位信息吗?',
    function (r) {
        if (r) {
            $.post('/b/Company/Delete?companyId=' + selectRow.CPID,
                function (data) {
                    if (data.result) {
                        $('#tableCompany').treegrid('remove', selectRow.CPID);
                        $.messager.show({
                            width: 200,
                            height: 100,
                            msg: '删除单位成功',
                            title: "删除单位"
                        });
                    } else {
                        $.messager.alert('系统提示', data.msg, 'error');
                    }

                });
        }
    });
}
function Refresh() {
    $('#tableCompany').treegrid('reload');
}
AddOrUpdateBox = function (title) {
    $('#divCompanyBox').show().dialog({
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
                $('#divCompanyBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                if (!CheckSubmitData()) {
                    return;
                }
                if ($('#divCompanyBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });

                    $.ajax({
                        type: "post",
                        url: '/b/Company/EditCompany',
                        data: $("#divCompanyBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                        success: function (data) {

                            if (data.result) {
                                $.messager.show({
                                    width: 200,
                                    height: 100,
                                    msg: '数据保存成功!',
                                    title: "保存公司"
                                });
                                $.messager.progress("close");
                                Refresh();
                                $('#divCompanyBox').dialog('close');
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

    var companyId = $("#hiddCPID").val();
    if ($.trim(companyId) == "") {
        var userAccount = $("#txtUserAccount").val();
        if ($.trim(userAccount) == "") {
            $.messager.alert('系统提示', "登录名不能为空", 'error');
            return false;
        }
        var password = $("#txtUserPassword").val();
        if ($.trim(password) == "") {
            $.messager.alert('系统提示', "密码不能为空", 'error');
            return false;
        }
    }
    return true;
}
$(function () {
    $("#btnQueryData").click(function () {
        var str = $("#txtcompany").val();
        $('#tableCompany').treegrid({
            rownumbers: false,
            animate: true,
            collapsible: false,
            fitColumns: true,
            url: '/b/Company/Search_Company?str=' + str,
            idField: 'CPID',
            treeField: 'CPName',
            columns: [[
                        { field: 'id', title: 'ID', width: 100, hidden: true },
                        { field: 'CPID', title: 'CPID', width: 100, hidden: true },
                        { field: 'MasterID', title: 'MasterID', width: 100, hidden: true },
                        { field: 'CityID', title: 'CityID', width: 100, hidden: true },
                        { field: 'ProvinceID', title: 'ProvinceID', width: 100, hidden: true },
                        { field: 'CPName', title: '单位名称', width: 150 },
                        { field: 'LinkMan', title: '联系人', width: 150 },
                        { field: 'Mobile', title: '联系电话', width: 150 },
                        { field: 'Address', title: '地址', width: 150 }
            ]]
               , onBeforeLoad: function (row, data) {
                   var dpanel = $('#tableCompany').datagrid('getPanel');
                   var toolbar = dpanel.children("div.datagrid-toolbar");
                   if (toolbar.length == 0) {
                       $.post('/b/Company/GetCompanyOperatePurview', function (result) {
                           $('#tableCompany').datagrid("addToolbarItem", result);
                       });
                   }
               },
            onLoadError: function (row, data) {               
                $.messager.alert("系统提示", "未找到相关单位，请确保关键字正确");
            }
        });
    });
    $('#tableCompany').treegrid({
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/b/Company/GetCompanyData/',
        idField: 'CPID',
        treeField: 'CPName',
        columns: [[
                    { field: 'id', title: 'ID', width: 100, hidden: true },
                    { field: 'CPID', title: 'CPID', width: 100, hidden: true },
                    { field: 'MasterID', title: 'MasterID', width: 100, hidden: true },
                    { field: 'CityID', title: 'CityID', width: 100, hidden: true },
                    { field: 'ProvinceID', title: 'ProvinceID', width: 100, hidden: true },
                    { field: 'CPName', title: '单位名称', width: 150 },
                    { field: 'LinkMan', title: '联系人', width: 150 },
                    { field: 'Mobile', title: '联系电话', width: 150 },
                    { field: 'Address', title: '地址', width: 150 }
				]]
                , onBeforeLoad: function (row, data) {
                    var dpanel = $('#tableCompany').datagrid('getPanel');
                    var toolbar = dpanel.children("div.datagrid-toolbar");
                    if (toolbar.length == 0) {
                        $.post('/b/Company/GetCompanyOperatePurview', function (result) {
                            $('#tableCompany').datagrid("addToolbarItem", result);
                        });
                    }
                }
    });
    BindCity();
});
function BindCity() {
    $('#ProvinceID').combobox({
        url: '/City/GetAllProvinces/',
        valueField: 'ProvinceID',
        textField: 'ProvinceName',
        onSelect: function (record) {
            $('#CityID').combobox({
                url: '/City/GetCitys?provinceID=' + record.ProvinceID,
                valueField: 'CityID',
                textField: 'CityName',
                onLoadSuccess: function () {
                               
                }
            });
        }
    });
}