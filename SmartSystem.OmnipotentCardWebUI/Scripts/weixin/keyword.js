$(function () {
    BindKeyword();
    BindComboboxData();
    $(".trText").show();
});
function BindComboboxData() {
    $('#cmbMatchType').combobox({
        url: '/w/WXKeyword/GetMatchTypeData',
        valueField: 'EnumValue',
        textField: 'Description'
    });
    $('#cmbKeywordType').combobox({
        url: '/w/WXKeyword/GetKeywordTypeData',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            if (record.EnumValue == "0") {
                $(".trArticle").hide();
                $(".trText").show();
            } else {
                $(".trArticle").show();
                $(".trText").hide();
            }
            SetKeywordBoxHeight();
        }
    });
    $('#cmbArticleGroupID').combobox({
        url: '/w/WXKeyword/GetArticleData',
        valueField: 'id',
        textField: 'text'
    });
}
function BindKeyword() {
    $('#tableKeyword').datagrid({
        url: '/w/WXKeyword/GetKeywordData/',
        singleSelect: true,
        columns: [[
                    { field: 'Id', title: 'Id', width: 100, hidden: true },
                     { field: 'KeywordType', title: 'KeywordType', width: 100, hidden: true },
                     { field: 'MatchType', title: 'MatchType', width: 100, hidden: true },
                     { field: 'ReplyType', title: 'ReplyType', width: 100, hidden: true },
                     { field: 'Text', title: 'Text', width: 100, hidden: true },
                     { field: 'ArticleGroupID', title: 'ArticleGroupID', width: 100, hidden: true },
                    { field: 'Keyword', title: '关键字', width: 150 },
                    { field: 'KeywordTypeDes', title: '关键字类型', width: 150 },
                    { field: 'ReplyTypeDes', title: '回复类型', width: 150 },
                     { field: 'MatchTypeDes', title: '匹配类型', width: 150 },
                    

				]],
        onBeforeLoad: function (row, data) {
            var dpanel = $('#tableKeyword').datagrid('getPanel');
            var toolbar = dpanel.children("div.datagrid-toolbar");
            if (toolbar.length == 0) {
                $.post('/w/WXKeyword/GetKeyWordOperatePurview', function (result) {
                    $('#tableKeyword').datagrid("addToolbarItem", result);
                });
            }
        }
    });
}
function Add() {
    AddOrUpdate("添加关键字");
    $('#divKeywordBoxForm').form('load', {
        Id: '',
        Keyword: '',
        Text: ''
    })
    $("#cmbMatchType").combobox("setValue", "0");
    $("#cmbKeywordType").combobox("setValue", "0");
    $(".trArticle").hide();
    $(".trText").show();
    SetKeywordBoxHeight();
}
function UpdateForDefault() {
    var model = $('#tableKeyword').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要设为默认回复的关键字!");
        return;
    }
    $.messager.confirm('系统提示', '确定要设置为默认回复吗?',
            function (r) {
                if (r) {
                    $.post('/w/WXKeyword/UpdateForDefault?id=' + model.Id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '设置默认回复成功',
                                title: "设置默认回复"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function UpdateForSubscribe() {
    var model = $('#tableKeyword').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要设为关注回复的关键字!");
        return;
    }
    $.messager.confirm('系统提示', '确定要设置为关注回复吗?',
            function (r) {
                if (r) {
                    $.post('/w/WXKeyword/UpdateForSubscribe?id=' + model.Id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '设置关注回复成功',
                                title: "设置关注回复"
                            });
                            Refresh();
                        } else {
                            $.messager.alert('系统提示', data.msg, 'error');
                        }

                    });
                }
            });
}
function Update() {
    var model = $('#tableKeyword').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要修改的关键字!");
        return;
    }
    AddOrUpdate("修改关键字");
    $('#divKeywordBoxForm').form('load', {
        ID: model.Id,
        Keyword: model.Keyword,
        Text: model.Text
    })
    $("#cmbMatchType").combobox("setValue", model.MatchType);
    $("#cmbKeywordType").combobox("setValue", model.KeywordType);

    if (model.KeywordType == "0") {
        $(".trArticle").hide();
        $(".trText").show();
    } else {
        $("#cmbArticleGroupID").combobox("setValue", model.ArticleGroupID);
        $(".trArticle").show();
        $(".trText").hide();
    }
    SetKeywordBoxHeight();
}
function Delete() {
    var model = $('#tableKeyword').datagrid('getSelected');
    if (model == null) {
        $.messager.alert("系统提示", "请选择需要删除的关键字!");
        return;
    }
    $.messager.confirm('系统提示', '确定删除选中的关键字吗?',
            function (r) {
                if (r) {
                    $.post('/w/WXKeyword/Delete?id=' + model.Id,
                    function (data) {
                        if (data.result) {
                            $.messager.show({
                                width: 200,
                                height: 100,
                                msg: '删除关键字成功',
                                title: "删除关键字"
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
    $('#tableKeyword').datagrid('load');
}
function SetKeywordBoxHeight() { 
    var type = $("#cmbKeywordType").combobox("getValue");
    if(type=="0"){
     $('#divKeywordBox').show().dialog({ height: 380});
    }else{
     $('#divKeywordBox').show().dialog({ height: 280 });
    }
    
}
function AddOrUpdate(title) {
    $('#divKeywordBox').show().dialog({
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
                $('#divKeywordBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                var type = $("#cmbKeywordType").combobox("getValue");
                if (type == "0") {
                    var txt = $("#txtText").textbox("getValue");
                    if ($.trim(txt) == "") {
                        $.messager.alert('系统提示', "请输入回复内容", 'error');
                        return;
                    }
                } else {
                    var gruopId = $("#cmbArticleGroupID").combobox("getValue");
                    if ($.trim(gruopId) == "") {
                        $.messager.alert('系统提示', "请选择图文", 'error');
                        return;
                    }
                }
                if ($('#divKeywordBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/w/WXKeyword/AddOrUpdate',
                        data: $("#divKeywordBoxForm").serialize(),
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
                                $('#divKeywordBox').dialog('close');
                                Refresh();

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