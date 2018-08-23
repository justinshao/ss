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
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    var onlineoffline = $("#selectOnLineOffLine").val();
    var ordersource = $("#selectOrderSource").val();
    var platenumber = $("#txtPlateNumber").val();
    var isshowzero = $("#cbxzero").prop("checked");
    var isnoconfirm = $("#cbxnoconfirm").prop("checked");
    var isdiffamount = $("#cbxdiffamount").prop("checked");
    var adminid = $("#selectOnDutys").val();
    var boxid = $("#selectBox").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_TempPay',
        data: "parkingid=" + parkingid + "&onlineoffline=" + onlineoffline + "&ordersource=" + ordersource + "&platenumber=" + platenumber + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&isshowzero=" + isshowzero + "&boxid=" + boxid + "&isnoconfirm=" + isnoconfirm + "&isdiffamount=" + isdiffamount + "&adminid=" + adminid + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "", 
        async: true,
        success: function (data) {
            var iWidth = 880;
            var iHeight = 450;
            var iTop = (window.screen.availHeight - iHeight) / 2;
            var iLeft = (window.screen.availWidth - iWidth) / 2;
            var openUrl = "../../Report/StatisticsReport.aspx";
            var params = "width=1180,height=450,scrollbars=yes,menubar=yes,location=yes,top=" + iTop + ",left=" + iLeft + "";
            //            window.open(openUrl, 'mywindow', 'fullscreen=yes,width=880,height=450, scrollbars=auto,top="+iTop+",left="+iLeft+"');
            window.open(openUrl, 'mywindow', params);
        }
    });
}
function DisplayImage() {
    var row = $('#tableListBox').datagrid('getSelected');
    if (row != null) {
        ShowImage(row["EntranceImage"], row["ExitImage"]);
        $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
    }
}
function DisplayImageDoubleClick(EntranceImage, ExitImage) {
    ShowImage(EntranceImage, ExitImage);
    $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
}
function ExportTest() {
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var onlineoffline = $("#selectOnLineOffLine").val();
    var adminid = $("#selectOnDutys").val();
    var ordersource = $("#selectOrderSource").val();
    var platenumber = $("#txtPlateNumber").val();
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    var zero = $("#cbxzero").prop("checked");
    var isnoconfirm = $("#cbxnoconfirm").prop("checked");
    var isdiffamount = $("#cbxdiffamount").prop("checked");
    var boxid = $("#selectBox").val();

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_TempPay',
        data: "parkingid=" + parkingid + "&isshowzero=" + zero + "&onlineoffline=" + onlineoffline + "&ordersource=" + ordersource + "&platenumber=" + platenumber + "&starttime=" + starttime + "&endtime=" + endtime + "&boxid=" + boxid + "&isnoconfirm=" + isnoconfirm + "&isdiffamount=" + isdiffamount + "&exitoperatorid=" + adminid, 
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
//    GetParks();
//    GetBoxes();
//    GetOnDutys();
//    $("#selectParks").change(function () {
//        GetBoxes();
//        GetOnDutys();
//    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {

        var parkingid = $("#selectParks").combobox("getValue");
        var onlineoffline = $("#selectOnLineOffLine").val();
        var adminid = $("#selectOnDutys").val();
        var ordersource = $("#selectOrderSource").val();
        var platenumber = $("#txtPlateNumber").val();
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var isdiffamount = $("#cbxdiffamount").prop("checked");
        var zero = $("#cbxzero").prop("checked");
        var isnoconfirm = $("#cbxnoconfirm").prop("checked");
        var boxid = $("#selectBox").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_TempPay";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, isshowzero: zero, onlineoffline: onlineoffline, ordersource: ordersource, platenumber: platenumber, starttime: starttime, endtime: endtime, boxid: boxid, isnoconfirm: isnoconfirm, isdiffamount: isdiffamount, exitoperatorid: adminid });
    }); 
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        onDblClickRow: function (rowIndex, rowData) {
            var entranceimage = rowData['EntranceImage'];
            var exitimage = rowData['ExitImage'];
            DisplayImageDoubleClick(entranceimage, exitimage);
        },
        columns: [[
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'OrderNo', title: '订单编号', width: 160 },
                    { field: 'PlateNumber', title: '缴费车牌', width: 80,
                        formatter: function (value, row, index) {
                            if (value!= null && value.substring(0, 3) == "无车牌") {
                                return "无车牌";
                            }
                            else
                                return value;
                        }
                    },
                    { field: 'Amount', title: '缴费金额', width: 80,
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
                        },
                        styler: function (value, row, index) {
                            if (value > 0) {
                                return 'background-color:pink';
                            }
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
                    { field: 'OrderTimeToString', title: '缴费时间', width: 120},
                    { field: 'PayWayName', title: '付款方式', width: 80 },
                    { field: 'LongTime', title: '停车时长', width: 120 },
                    { field: 'OrderSourceName', title: '订单来源', width: 80 },
                    { field: 'EntranceTimeToString', title: '进场时间', width: 120},
                    { field: 'ExitTimeToString', title: '出场时间', width: 120},
                    { field: 'Operator', title: '收费员', width: 120 },
		        ]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '图片', iconCls: 'icon-large-picture', handler: function () { DisplayImage() } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof (parkingId) != 'undefined') {
        GetOnDutysByParkingId(parkingId);
        GetBoxesByParkingId(parkingId);
    }
}