
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    if (printtype == 2) {
        pageIndex = -1;
    }
    var PlateNumber = $("#txtPlateNumber").val();
    var ParkingId = $("#selectParks").val();
    var MoblieOrName = $("#txtMoblieOrName").val();
    var VisitorSource = $("#sltVisitorSource").val();
    var VisitorState = $("#sltVisitorState").val();
    var BeginTime = $("#txtBeginTime").datetimebox("getValue");
    var EndTime = $("#txtEndTime").datetimebox("getValue");

    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_ParkVisitorInfo',
        data: "PlateNumber=" + PlateNumber + "&ParkingId=" + ParkingId + "&MoblieOrName=" + MoblieOrName + "&VisitorSource=" + VisitorSource + "&VisitorState=" + VisitorState + "&BeginTime=" + BeginTime + "&EndTime=" + EndTime + "&page=" + pageIndex + "&rows=" + pageSize + "",
        async: true,
        success: function (data) {
            var iWidth = 880;
            var iHeight = 450;
            var iTop = (window.screen.availHeight - iHeight) / 2;
            var iLeft = (window.screen.availWidth - iWidth) / 2;
            var openUrl = "../../Report/StatisticsReport.aspx";
            var params = "width=1180,height=450,scrollbars=yes,menubar=yes,location=yes,top=" + iTop + ",left=" + iLeft + "";
            window.open(openUrl, 'mywindow', params);
        }
    });
}
$(function () {
    $("#btnQueryData").click(function () {       
        var PlateNumber = $("#txtPlateNumber").val();
        var ParkingId = $("#selectParks").val();
        var MoblieOrName = $("#txtMoblieOrName").val();
        var VisitorSource = $("#sltVisitorSource").val();
        var VisitorState = $("#sltVisitorState").val();
        var BeginTime = $("#txtBeginTime").datetimebox("getValue");
        var EndTime = $("#txtEndTime").datetimebox("getValue");
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_ParkVisitorInfo";
        $('#tableListBox').datagrid('load', { PlateNumber: PlateNumber, ParkingId: ParkingId, MoblieOrName: MoblieOrName, VisitorSource: VisitorSource, VisitorState: VisitorState, BeginTime: BeginTime, EndTime: EndTime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'VName', title: '小区名称', width: 150 },
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'PlateNumber', title: '车牌号码', width: 80,
                        formatter: function (value, row, index) {
                            if (value.substring(0, 3) == "无车牌") {
                                return "无车牌";
                            }
                            else
                                return value;
                        }
                    },
                    { field: 'BeginTimeDes', title: '有效开始时间', width: 130 },
                    { field: 'EndTimeDes', title: '有效结束时间', width: 130 },
                    { field: 'VisitorCountDes', title: '访问次数', width: 80 },
                    { field: 'VisitorSourceDes', title: '访客来源', width: 80 },
                    { field: 'VisitorStateDes', title: '访客状态', width: 80 },
                    { field: 'VisitorName', title: '访客姓名', width: 60 },
                    { field: 'VisitorMobilePhone', title: '访客电话', width: 80 },
                    { field: 'CreateTimeDes', title: '添加时间', width: 130 },
                    { field: 'OperatorName', title: '添加人', width: 80 },
				]],
        onLoadSuccess: function (data) {
             $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }]
    });
});
function GetVisitorParks() {
    $("#selectParks option").remove();
    $("#selectParks").append("<option value='-1'>全部</option>");
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetParks',
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectParks").append("<option value='" + item.PKID + "'>" + item.PKName + "</option>");
                })
            }
        }
    });
}
$(function () {
    //本月第一天
    $('#txtBeginTime').datetimebox('setValue', currentdate00());
    //当前时间
    $('#txtEndTime').datetimebox('setValue', currentdate23());
    GetVisitorParks();
});