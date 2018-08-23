//打印报表  暂时打印成文件  当前页:printtype=1  所有页:printtype=2
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize
    if (printtype == 2) {
        pageIndex = -1;
    }
    var parkingid = $("#selectParks").combobox("getValue");
    var exitgate = $("#selectGateOut").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_ExceptionReleaseInOut',
        data: "parkingid=" + parkingid + "&platenumber=" + platenumber + "&exitgate=" + exitgate + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var exitgate = $("#selectGateOut").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancestarttime = $('#txtStartTime').datetimebox('getValue');
    var entranceendtime = $('#txtEndTime').datetimebox('getValue');

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_Exception',
        data: "parkingid=" + parkingid + "&platenumber=" + platenumber + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&exitgateid=" + exitgate,
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
//    GetExitGates();
//    $("#selectParks").change(function () {
//        GetExitGates();
    //    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var exitgate = $("#selectGateOut").val();
        var platenumber = $("#txtPlateNumber").val();
        var startdate = $('#txtStartTime').datetimebox('getValue');
        var enddate = $('#txtEndTime').datetimebox('getValue');
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_ExceptionRelease";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, platenumber: platenumber, exitgateid: exitgate, starttime: startdate, endtime: enddate });
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
                    { field: 'PlateNumber', title: '车牌号码', width: 80,
                        formatter: function (value, row, index) {
                            if (value.substring(0, 3) == "无车牌") {
                                return "无车牌";
                            }
                            else
                                return value;
                        }
                    },
                    { field: 'CarModelName', title: '车类型', width: 80 },
                    { field: 'CarTypeName', title: '卡类型', width: 80 },
                    { field: 'EntranceTimeToString', title: '进场时间', width: 120 },
                    { field: 'InGateName', title: '进场通道', width: 100 },
                    { field: 'ExitTimeToString', title: '出场时间', width: 120},
                    { field: 'OutGateName', title: '出场通道', width: 135 },
                    { field: 'ReleaseTypeName', title: '放行类型', width: 80 },
                    { field: 'Remark', title: '备注', width: 120 },
                    { field: 'AreaName', title: '停车区域', width: 80 },
                    { field: 'InOperatorName', title: '操作员', width: 120 },
                    { field: 'EntranceCertificateNo', title: '进证件号', width: 140 },
                    { field: 'ExitCertificateNo', title: '出证件号', width: 140 }
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
        GetExitGatesByParkingId(parkingId);
    }
}