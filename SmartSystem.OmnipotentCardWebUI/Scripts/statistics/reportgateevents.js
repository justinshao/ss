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
    var gateid = $("#selectGate").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancestarttime = $('#txtEntranceTime').datetimebox('getValue');
    var entranceendtime = $('#txtExitTime').datetimebox('getValue');
    var eventname = $("#selectEvent").val();
    var cartype = $("#selectCarType").val();
    var cardtype = $("#selectCardType").val();
    var inorout = $("#selectInOut").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_GateInOutRecord',
        data: "parkingid=" + parkingid + "&eventid=" + eventname + "&cartype=" + cartype + "&cardtype=" + cardtype + "&inorout=" + inorout + "&platenumber=" + platenumber + "&gatein=" + gateid + "&starttime=" + entrancestarttime + "&endtime=" + entranceendtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var gateid = $("#selectGate").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancedate = $('#txtEntranceTime').datetimebox('getValue');
    var exitdate = $('#txtExitTime').datetimebox('getValue');
    var eventtype = $("#selectEvent").val();
    var cartype = $("#selectCarType").val();
    var cardtype = $("#selectCardType").val();
    var inorout = $("#selectInOut").val();
    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_GateEvent',
        data: "parkingid=" + parkingid + "&inorout=" + inorout + "&eventtype=" + eventtype + "&cartype=" + cartype + "&cardtype=" + cardtype+ "&platenumber=" + platenumber + "&gateid=" + gateid + "&entrancedate=" + entrancedate+ "&exitdate=" + exitdate,
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
function DisplayImage() {
    var row = $('#tableListBox').datagrid('getSelected');
    if (row != null) {
        ShowImage(row["PictureName"], null);
        $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 430, height: 380, modal: true, collapsible: false, minimizable: false, maximizable: false });
    }
}
function DisplayImageDoubleClick(EntranceImage) {
    ShowImage(EntranceImage, null);
    $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 430, height: 380, modal: true, collapsible: false, minimizable: false, maximizable: false });
}
$(function () {
    $('#txtEntranceTime').datetimebox('setValue', currentdate00())
    $('#txtExitTime').datetimebox('setValue', currentdate23())
//    GetParks();
//    GetEventType();
//    GetEntranceCardType();
//    GetEntranceGates();
//    GetExitGates();
//    GetCarTypes();
//    GetAreas();
//    GetGates();
//    $("#selectParks").change(function () {
//        GetEntranceCardType();
//        GetEntranceGates();
//        GetExitGates();
//        GetCarTypes();
//        GetAreas();
//        GetGates();
//    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var gateid = $("#selectGate").val();
        var platenumber = $("#txtPlateNumber").val();
        var entrancedate = $('#txtEntranceTime').datetimebox('getValue');
        var exitdate = $('#txtExitTime').datetimebox('getValue');
        var eventtype = $("#selectEvent").val();
        var cartype = $("#selectCarType").val();
        var cardtype = $("#selectCardType").val();
        var inorout = $("#selectInOut").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_GateEvents";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, inorout: inorout, eventtype: eventtype, cartype: cartype, cardtype: cardtype, platenumber: platenumber, gateid: gateid, entrancedate: entrancedate, exitdate: exitdate });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        onDblClickRow: function (rowIndex, rowData) {
            var entranceimage = rowData['PictureName'];
            DisplayImageDoubleClick(entranceimage);
        },
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'GateName', title: '通道名称', width: 150 },
                    { field: 'PlateNumber', title: '车牌号码', width: 80 },
                    { field: 'EventName', title: '事件类型', width: 150 },
                    { field: 'IOStateName', title: '进出方向', width: 80 },
                    { field: 'CarModelName', title: '车类型', width: 80 },
                    { field: 'CarTypeName', title: '卡类型', width: 80 },
                    { field: 'RecTimeToString', title: '事件时间', width: 120 },
                    { field: 'Operator', title: '操作员', width: 120 }
				]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 50,
        pageList: [50],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '图片', iconCls: 'icon-large-picture', handler: function () { DisplayImage() } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof (parkingId) != 'undefined') {
        GetEventType();
        GetEntranceCardTypeByParkingId(parkingId);
        GetEntranceGatesByParkingId(parkingId);
        GetExitGatesByParkingId(parkingId);
        GetCarTypesByParkingId(parkingId);
        GetAreasByParkingId(parkingId);
        GetGatesByParkingId(parkingId);
    }
}