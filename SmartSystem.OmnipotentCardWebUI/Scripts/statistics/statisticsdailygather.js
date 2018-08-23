//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
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
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_GatherDaily',
        data: "parkingid=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
        async: true,
        success: function (data) {
            var iWidth = 1180;
            var iHeight = 450;
            var iTop = (window.screen.availHeight - iHeight) / 2;
            var iLeft = (window.screen.availWidth - iWidth) / 2;
            var openUrl = "../../Report/StatisticsReport.aspx";
            var params = "width=1180,height=450,scrollbars=yes,menubar=yes,location=yes,top=" + iTop + ",left=" + iLeft + "";
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
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_DailyGather',
        data: "ParkingID=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime,
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
    $('#txtStartTime').datebox('setValue', CurrentMonthFirstDay())
    $('#txtEndTime').datebox('setValue', currentdateDay())
    BindGetParkTree();
//    $("#selectParks").change(function () {
//        GetCardType();
//    })
    $("#btnQueryData").click(function () {
        var ParkingID = $("#selectParks").combobox("getValue");
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        if (ParkingID == "-1") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_StatisticsDailyGather";
        $('#tableListBox').datagrid('load', { ParkingID: ParkingID, starttime: starttime, endtime: endtime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'KeyName', title: '统计时间', width: 80 },
                    { field: 'Receivable_Amount', title: '应收金额', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Real_Amount', title: '实收金额', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Diff_Amount', title: '差异金额', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        },
                        styler: function (value, row, index) {
                            if (value > 0) {
                                return 'background-color:lightcoral';
                            }
                        }
                    },
                    { field: 'Entrance_Count', title: '进场数', width: 100 },
                    { field: 'Exit_Count', title: '出场数', width: 100 },
                    { field: 'ReleaseType_Normal', title: '正常放行次数', width: 100 },
                    { field: 'ReleaseType_Charge', title: '收费放行次数', width: 100 },
                    { field: 'ReleaseType_Free', title: '免费放行次数', width: 100 },
                    { field: 'ReleaseType_Catch', title: '异常放行次数', width: 100 },
                    { field: 'VIPExtend_Count', title: 'VIP卡续期次数', width: 100 },
                    { field: 'OnLineMonthCardExtend_Count', title: '线上月卡续期次数', width: 100 },
                    { field: 'MonthCardExtend_Count', title: '线下月卡续期次数', width: 100 },
                    { field: 'OnLineStordCard_Count', title: '线上储值卡充值次数', width: 100 },
                    { field: 'StordCardRecharge_Count', title: '线下储值卡充值次数', width: 100 }
				]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 50,
        pageList: [50],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof (parkingId) != 'undefined') {
        GetCardTypeByParkingId(parkingId);
    }
}