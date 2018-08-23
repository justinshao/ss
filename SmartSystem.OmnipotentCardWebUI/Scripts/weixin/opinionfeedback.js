$(function () {
    var defaultStart = $("#hiddDefaultStartDate").val();
    var defaultEnd = $("#hiddDefaultEndDate").val();
    $("#txtStartTime").datetimebox("setValue", defaultStart);
    $("#txtEndTime").datetimebox("setValue", defaultEnd);

    $("#btnQueryData").click(function () {
        var userAccount = $("#txtUserAccount").val();
        var start = $("#txtStartTime").datetimebox("getValue");
        var end = $("#txtEndTime").datetimebox("getValue");
        $('#tableOpinionFeedback').datagrid('load', {StartTime: start, EndTime: end });
    });
    $('#tableOpinionFeedback').datagrid({
        url: '/w/WXOpinionFeedback/GetOpinionFeedbackData',
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
                    { field: 'OpenId', title: '反馈人昵称', width: "15%" },
                    { field: 'FeedbackContent', title: '反馈内容', width: "60%" },
                    { field: 'CreateTime', title: '反馈时间', width: "20%", formatter: function (value, row, index) {
                         if (value != null && value != "") {
                             return String.GetDateyyyyMMddHHmmss(value);
                         } else {
                             return "";
                         }
                     }
                     }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45]
    });
});