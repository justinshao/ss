//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
/*function PrintReport(printtype) {
var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
var pageIndex = pageopt.pageNumber;
var pageSize = pageopt.pageSize
if (printtype == 2) {
pageIndex = -1;
}
var parkingid = $("#selectParks").combobox("getValue");
var payway = $("#selectPaySource").val();
var status = $("#selectPayStatus").val();
var starttime = $('#txtStartTime').datetimebox('getValue');
var endtime = $('#txtEndTime').datetimebox('getValue');
var platenumber = $("#txtPlateNumber").val();
$.ajax({
type: "post",
url: '/S/ReportParams/Params_OnlinePay',
data: "parkingid=" + parkingid + "&payway=" + payway + "&status=" + status + "&starttime=" + starttime + "&endtime=" + endtime + "&platenumber=" + platenumber + "",
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
var payway = $("#selectPaySource").val();
var status = $("#selectPayStatus").val();
var starttime = $('#txtStartTime').datetimebox('getValue');
var endtime = $('#txtEndTime').datetimebox('getValue');
var platenumber = $("#txtPlateNumber").val();
$.ajax({
type: "post",
url: '/S/Statistics/Export_OnlinePay',
data: "parkingid=" + parkingid + "&payway=" + payway + "&status=" + status + "&starttime=" + starttime + "&endtime=" + endtime + "&platenumber=" + platenumber,
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

*/
$(function () {
    $('#txtStartTime').datetimebox('setValue', currentdate00())
    $('#txtEndTime').datetimebox('setValue', currentdate23())
    //    GetParks();
    //    GetBoxes();
    //    GetOnDutys();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");

        var payway = $("#selectPaySource").val();
        var status = $("#selectPayStatus").val();
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var platenumber = $("#txtPlateNumber").val();


        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_OnlinePay";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, payway: payway, status: status, platenumber: platenumber, starttime: starttime, endtime: endtime });
    });
    //    $("#selectParks").change(function () {
    //        GetBoxes();
    //        GetOnDutys();
    //    })
    BindGetParkTree();
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'OrderNo', title: '订单编号', width: 150 },
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'PlateNumber', title: '车牌号', width: 100 },
                    { field: 'PayAmount', title: '支付金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },

                    { field: 'MonthNum', title: '续期月数', width: 80 },
                    { field: 'NickName', title: '支付人', width: 200 },
                    { field: 'SyncResultTimes', title: '同步支付次数', width: 100 },
                    { field: 'LastSyncResultTimeToString', title: '最后同步时间', width: 180 },
                    { field: 'RefundOrderId', title: '退款订单号', width: 120 },
                    { field: 'lx', title: '订单类型', width: 80 },
                    { field: 'zt', title: '订单状态', width: 80 },
                    { field: 'OrderTimeToString', title: '订单时间', width: 150 },
                    { field: 'PayTimeToString', title: '支付时间', width: 150 }
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
    if (typeof (parkingId) != 'undefined') {
        GetOnDutysByParkingId(parkingId);
        GetBoxesByParkingId(parkingId);
    }
}