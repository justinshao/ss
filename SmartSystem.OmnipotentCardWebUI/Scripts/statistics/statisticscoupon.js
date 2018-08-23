//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    if (printtype == 2) {
        pageIndex = -1;
    }
    var parkingid = $("#selectParks").combobox("getValue");
    var status = $("#selectStatus").val();
    var couponno = $("#txtCouponNo").val();
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    var sellerid = $("#selectSeller").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_Coupon',
        data: "parkingid=" + parkingid + "&status=" + status + "&couponno=" + couponno + "&starttime=" + starttime + "&endtime=" + endtime + "&sellerid=" + sellerid + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
        async: true,
        success: function (data) {
            var iWidth = 1180;
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
    var status = $("#selectStatus").val();
    var couponno = $("#txtCouponNo").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    var sellerid = $("#selectSeller").val();

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_Coupon',
        data: "parkingid=" + parkingid + "&status=" + status + "&couponno=" + couponno + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime+ "&sellerid=" + sellerid,
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
    $('#txtStartTime').datetimebox('setValue', currentdate00())
    $('#txtEndTime').datetimebox('setValue', currentdate23())
//    GetParks();
    GetSellers();
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var status = $("#selectStatus").val();
        var couponno = $("#txtCouponNo").val();
        var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
        var entranceendtime = $('#txtEndTime').datetimebox('getValue');
        var sellerid = $("#selectSeller").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_Coupons";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, status: status, couponno: couponno, starttime: entrancestarttime, endtime: entranceendtime, sellerid: sellerid });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'SellerName', title: '商家名称', width: 180 },
                    { field: 'RuleName', title: '优免规则', width: 80 },
                    { field: 'PlateNumber', title: '车牌号码', width: 80,
                        formatter: function (value, row, index) {
                            if (value.substring(0, 3) == "无车牌") {
                                return "无车牌";
                            }
                            else
                                return value;
                        }
                    },
                    { field: 'FreeMoney', title: '优免金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'FreeTime', title: '优免时长', width: 80 },
                    { field: 'StatusName', title: '状态', width: 80 },
                    { field: 'CreateTimeToString', title: '打折时间', width: 120 }
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
}