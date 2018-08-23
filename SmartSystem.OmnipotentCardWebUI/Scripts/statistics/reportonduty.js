function GetNoPlateParking() {
    $.ajax({
        type: "post",
        url: '/Statistics/GetParking',
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectParks").append("<option value='" + item.ParkingID + "'>" + item.ParkName + "</option>");
                })
                GetOnDutyUser();
                GetBox();
            }
        }
    });
}




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
    var endtime = $("#txtEndTime").datetimebox('getValue');
    var adminid = $("#selectOnDutyUser").val();
    var boxid = $("#selectBox").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_OnDuty',
        data: "parkingid=" + parkingid + "&adminid=" + adminid + "&starttime=" + starttime + "&endtime=" + endtime + "&boxid=" + boxid + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var adminid = $('#selectOnDutys').val();

    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $("#txtEndTime").datetimebox('getValue');
    var boxid = $("#selectBox").val();

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_OnDuty',
        data: "parkingid=" + parkingid + "&adminid=" + adminid + "&starttime=" + starttime + "&endtime=" + endtime + "&boxid=" + boxid,
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
    $('#txtEndTime').datetimebox('setValue', currentdatenow())
    //GetParks();
//    GetBoxes();
//    GetOnDutys();
//    $("#selectParks").change(function () {
//        GetOnDutys();
//        GetBoxes();
    //    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var adminid = $('#selectOnDutys').val();
     
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $("#txtEndTime").datetimebox('getValue');
        var boxid = $("#selectBox").val();
        if (parkingid == "-1") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_OnDuty";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, adminid: adminid, starttime: starttime, endtime: endtime, boxid: boxid });
    });

    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'AdminName', title: '当班人', width: 100 },
                    { field: 'BoxName', title: '岗亭名称', width: 120 },
                    { field: 'StartWorkTimeToString', title: '上班时间', width: 120},
                    { field: 'EndWorkTimeToString', title: '下班时间', width: 120},
                    { field: 'Receivable_Amount', title: '应收金额', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Real_Amount', title: '实收金额', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Diff_Amount', title: '差异金额', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Cash_Amount', title: '现金收费', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Cash_Count', title: '现金次数', width: 70 },
                    { field: 'Temp_Amount', title: '临停缴费', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Temp_Count', title: '临停缴费次数', width: 100 },
                    { field: 'OnLineTemp_Amount', title: '线上临停缴费', width: 105,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLineTemp_Count', title: '线上临停缴费次数', width: 105 },
                    { field: 'StordCard_Amount', title: '储值卡扣费', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'StordCard_Count', title: '储值卡扣款次数', width: 100 },
                    { field: 'OnLine_Amount', title: '线上缴费', width: 70,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLine_Count', title: '线上缴费次数', width: 100 },
                    { field: 'Discount_Amount', title: '优免抵扣金额', width: 100,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'Discount_Count', title: '优免抵扣次数', width: 100 },
                    { field: 'ReleaseType_Normal', title: '正常放行', width: 70 },
                    { field: 'ReleaseType_Charge', title: '收费放行', width: 70 },
                    { field: 'ReleaseType_Free', title: '免费放行', width: 70 },
                    { field: 'ReleaseType_Catch', title: '异常放行', width: 70 },
                    { field: 'VIPExtend_Count', title: 'VIP卡续期', width: 100 },
                    { field: 'Entrance_Count', title: '进场数', width: 70 },
                    { field: 'Exit_Count', title: '出场数', width: 70 },
                    { field: 'VIPCard', title: 'VIP卡进场数', width: 100 },
                    { field: 'StordCard', title: '储值卡进场数', width: 100 },
                    { field: 'MonthCard', title: '月卡进场数', width: 100 },
                    { field: 'JobCard', title: '工作卡进场数', width: 100 },
                    { field: 'TempCard', title: '临时卡进场数', width: 100 },
                    { field: 'OnLineMonthCardExtend_Amount', title: '线上月卡续期金额', width: 120,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLineMonthCardExtend_Count', title: '线上月卡续期次数', width: 120 },
                    { field: 'MonthCardExtend_Amount', title: '线下月卡续期金额', width: 120,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'MonthCardExtend_Count', title: '线下月卡续期次数', width: 120 },
                    { field: 'OnLineStordCard_Amount', title: '线上储值卡充值金额', width: 120,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'OnLineStordCard_Count', title: '线上储值卡充值次数', width: 120 },
                    { field: 'StordCardRecharge_Amount', title: '线下储值卡充值金额', width: 120,
                        formatter: function (value, row, index) {
                            if (value != null)
                                return value.toFixed(2);
                        }
                    },
                    { field: 'StordCardRecharge_Count', title: '线下储值卡充值次数', width: 120 }
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