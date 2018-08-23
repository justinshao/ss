//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    if (printtype == 2) {
        pageIndex = -1;
    }
    var parkingid = $("#selectParks").combobox("getValue");
    var onlineoffline = $("#selectOnLineOffLine").val();
    var ordersource = $("#selectOrderSource").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    var isshowdiffamount = $("#cbxDiff").prop("checked");
    var isshowzero = $("#cbxzero").prop("checked");
    var isnoconfirm = $("#cbxnoconfirm").prop("checked");
    var boxid = $("#selectBox").val();
    var adminid = $("#selectOnDutys").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_Order',
        data: "parkingid=" + parkingid + "&onlineoffline=" + onlineoffline + "&ordersource=" + ordersource + "&platenumber=" + platenumber + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&isshowdiffamount=" + isshowdiffamount + "&isshowzero=" + isshowzero + "&boxid=" + boxid + "&isnoconfirm=" + isnoconfirm + "&adminid=" + adminid + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var adminid = $("#selectOnDutys").val();
    var onlineoffline = $("#selectOnLineOffLine").val();
    var ordersource = $("#selectOrderSource").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    var chxdiff = $("#cbxDiff").prop("checked");
    var zero = $("#cbxzero").prop("checked");
    var boxid = $("#selectBox").val();
    var isnoconfirm = $("#cbxnoconfirm").prop("checked");
    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_Order',
        data: "parkingid=" + parkingid + "&onlineoffline=" + onlineoffline + "&ordersource=" + ordersource + "&platenumber=" + platenumber + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&isshowzero=" + zero + "&isshowdiffamount=" + chxdiff + "&boxid=" + boxid + "&isnoconfirm=" + isnoconfirm+ "&exitoperatorid=" + adminid,
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
//    GetBoxes();
//    GetOnDutys();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var adminid = $("#selectOnDutys").val();
        var onlineoffline = $("#selectOnLineOffLine").val();
        var ordersource = $("#selectOrderSource").val();
        var platenumber = $("#txtPlateNumber").val();
        var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
        var entranceendtime = $('#txtEndTime').datetimebox('getValue');
        var chxdiff = $("#cbxDiff").prop("checked");
        var zero = $("#cbxzero").prop("checked");
        var boxid = $("#selectBox").val();
        var isnoconfirm = $("#cbxnoconfirm").prop("checked");
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_Orders";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, onlineoffline: onlineoffline, ordersource: ordersource, platenumber: platenumber, starttime: entrancestarttime, endtime: entranceendtime, isshowzero: zero, isshowdiffamount: chxdiff, boxid: boxid, isnoconfirm: isnoconfirm, exitoperatorid: adminid });
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
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'PlateNumber', title: '车牌号码', width: 80,
                        formatter: function (value, row, index) {
                            if (row.OrderTypeName == "月卡续期" || row.OrderTypeName == "VIP卡续期")
                                return row.CardNo;
                            else if (row.OrderTypeName == "临时卡缴费")
                                return value;
                        }
                    },
                    { field: 'OrderNo', title: '订单编号', width: 150 },
                    { field: 'Amount', title: '金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'PayAmount', title: '已付金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'UnPayAmount', title: '未付金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'DiscountAmount', title: '折扣金额', width: 80,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        },
                        styler: function (value, row, index) {
                            if (value > 0) {
                                return 'background-color:pink';
                            }
                        }
                    },
                    { field: 'OrderTypeName', title: '订单类型', width: 120 },
                    { field: 'PayWayName', title: '支付方式', width: 80 },
                    { field: 'OrderTimeToString', title: '订单时间', width: 120},
                    { field: 'OrderSourceName', title: '订单来源', width: 80 },
                    { field: 'Operator', title: '操作人', width: 120 },
                    { field: 'Remark', title: '备注', width: 150 }
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