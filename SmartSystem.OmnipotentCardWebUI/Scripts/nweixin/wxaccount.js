$(function () {
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            var accountname = $("#txtAccountName").val();
            var mobile = $("#txtMobile").val();
            var startdate = $('#txtStartTime').datetimebox('getValue');
            var enddate = $('#txtEndTime').datetimebox('getValue');
            var options = $('#tableListBox').datagrid('options');
            options.url = "/nwx/WXAccount/Search_WXAccount";
            $('#tableListBox').datagrid('load', { accountname: accountname, mobile: mobile, starttime: startdate, endtime: enddate,CompanyID:record.id });
        }
    });
});
$(function () {
    $('#txtStartTime').datetimebox('setValue', currentdate00());
    $('#txtEndTime').datetimebox('setValue', currentdate23());
    $("#btnQueryData").click(function () {
        var selectRow = $('#treeCompany').tree('getSelected');
        if (selectRow == null) {
            $.messager.alert("系统提示", "请先选择单位!");
            return;
        }
        var accountname = $("#txtAccountName").val();
        var mobile = $("#txtMobile").val();
        var startdate = $('#txtStartTime').datetimebox('getValue');
        var enddate = $('#txtEndTime').datetimebox('getValue');
        var options = $('#tableListBox').datagrid('options');
        options.url = "/nwx/WXAccount/Search_WXAccount";
        $('#tableListBox').datagrid('load', { accountname: accountname, mobile: mobile, starttime: startdate, endtime: enddate,CompanyID:selectRow.id });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'AccountName', title: '帐户名称', width: 160 },
                    { field: 'Sex', title: '性别', width: 80 },
                    { field: 'MobilePhone', title: '移动电话', width: 120 },
                    { field: 'Email', title: '电子邮件', width: 180 },
                    { field: 'IsAutoLock', title: '是否支持锁车', width: 100 ,
                        formatter: function (value) {
                          if (value) {
                              return '<img src="/Content/images/yes.png"/>';
                          } else {
                              return '<img src="/Content/images/no.png?v=1"/>';
                          }
                      }
                    },
                    { field: 'OpenAnswerPhone', title: '是否开启挪车手机接听', width: 140 ,
                        formatter: function (value) {
                            if (value) {
                                return '<img src="/Content/images/yes.png"/>';
                            } else {
                                return '<img src="/Content/images/no.png?v=1"/>';
                            }
                        }
                    },
                    { field: 'RegTime', title: '注册时间', width: 180 ,
                        formatter: function (value, row, index) {
                            var date = new Date(value);
                            var year = date.getFullYear().toString();
                            var month = (date.getMonth() + 1);
                            var day = date.getDate().toString();
                            var hour = date.getHours().toString();
                            var minutes = date.getMinutes().toString();
                            var seconds = date.getSeconds().toString();
                            if (month < 10) {
                                month = "0" + month;
                            }
                            if (day < 10) {
                                day = "0" + day;
                            }
                            if (hour < 10) {
                                hour = "0" + hour;
                            }
                            if (minutes < 10) {
                                minutes = "0" + minutes;
                            }
                            if (seconds < 10) {
                                seconds = "0" + seconds;
                            }
                            if (year == "1")
                                return "";
                            else
                                return year + "-" + month + "-" + day + " " + hour + ":" + minutes + ":" + seconds;
                        }
                    }
				]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35]
    });
});
