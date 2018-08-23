function StatisticsCardIODate00() {
    try {
        var date = new Date();
        return date.getFullYear() + "-02";
    } catch (ex) {
        return "";
    }
}
function StatisticsCardIODate() {  
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 2) : date.getMonth() + 2;
        
        return date.getFullYear() + "-" + month;
    } catch (ex) {
        return "";
    }
}
//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_FeeMonth',
        data: "parkingid=" + parkingid + "&strstarttime=" + starttime + "&strendtime=" + endtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize,
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
        url: '/S/Statistics/Export_Month',
        data: "parkingid=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime,
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
    $('#txtStartTime').val(StatisticsCardIODate00);
    $('#txtEndTime').val(StatisticsCardIODate);
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var ParkingID = $("#selectParks").combobox("getValue");
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        if (ParkingID == "-1") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_MonthStatisticsData";
        $('#tableListBox').datagrid('load', { parkingid: ParkingID, starttime: starttime, endtime: endtime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'KeyName', title: '统计月份', width: 80 },
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
                    { field: 'OnLine_Amount', title: '线上缴费', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Cash_Amount', title: '现金收费', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Discount_Amount', title: '消费打折', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'StordCard_Amount', title: '储值扣款', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                     { field: 'OnLineTemp_Amount', title: '临停缴费(线上)', width: 100,
                         formatter: function (value, row, index) {
                             if (value != null)
                                 return value.toFixed(2);
                         }
                     },
                    { field: 'Temp_Amount', title: '临停缴费', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLineMonthCardExtend_Amount', title: '月卡续期(线上)', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'MonthCardExtend_Amount', title: '月卡续期(线下)', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLineStordCard_Amount', title: '储值卡充值(线上)', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'StordCardRecharge_Amount', title: '储值卡充值(线下)', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    }
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