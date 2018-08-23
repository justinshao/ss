function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    if (printtype == 2) {
        pageIndex = -1;
    }
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var platenumber = $("#txtPlateNumber").val();
    var owner = $("#txtOwner").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    var onlineoffline = $("#selectOnLineOffLine").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_MonthCardExtend',
        data: "parkingid=" + parkingid + "&onlineoffline=" + onlineoffline + "&platenumber=" + platenumber + "&owner=" + owner + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
        async: true,
        success: function (data) {
            var iWidth = 880;
            var iHeight = 450;
            var iTop = (window.screen.availHeight - iHeight) / 2;
            var iLeft = (window.screen.availWidth - iWidth) / 2;
            var openUrl = "../../Report/StatisticsReport.aspx";
            var params = "width=880,height=450,scrollbars=yes,menubar=yes,location=yes,top=" + iTop + ",left=" + iLeft + "";
            window.open(openUrl, 'mywindow', params);
        }
    });
}
function ExportTest() {
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var platenumber = $("#txtPlateNumber").val();
    var owner = $("#txtOwner").val();
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    var onlineoffline = $("#selectOnLineOffLine").val();

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_CardExtend',
        data: "parkingid=" + parkingid + "&onlineoffline=" + onlineoffline + "&platenumber=" + platenumber + "&owner=" + owner + "&starttime=" + starttime+ "&endtime=" + endtime,
        async: true,
        success: function (data) {
            var el = document.createElement("a");
            document.body.appendChild(el);
            url = "../../Report/ReportFile/" + data;
            el.href = url; //url 是你得到的连接
            el.target = '_blank';
            el.download = data;
            el.click();
            document.body.removeChild(el);
        }
    });

}
$(function () {
    //本月第一天
    $('#txtStartTime').datetimebox('setValue', currentdate00());
    //当前时间
    $('#txtEndTime').datetimebox('setValue', currentdate23());
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var platenumber = $("#txtPlateNumber").val();
        var owner = $("#txtOwner").val();
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var onlineoffline = $("#selectOnLineOffLine").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_CardExtension";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, onlineoffline: onlineoffline, platenumber: platenumber, owner: owner, starttime: starttime, endtime: endtime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'CardNo', title: '卡片编号', width: 80 },
                    { field: 'EmployeeName', title: '卡片持有人', width: 80 },
                    { field: 'PayWayName', title: '支付方式', width: 80 },
                    { field: 'Amount', title: '充值金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OldUserulDateToString', title: '原有效期', width: 120 },
                    { field: 'NewUsefulDateToString', title: '现有效期', width: 120},
                    { field: 'OrderTimeToString', title: '充值时间', width: 120 },
                    { field: 'MonthLongTime', title: '续期时长', width: 140 },
                    { field: 'Remark', title: '备注', width: 140 },
                    { field: 'Operator', title: '操作员', width: 110 }
                    
				]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
}