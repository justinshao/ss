
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
    var cardtype = $("#selectCardType").val();   
    var platenumber = $("#txtPlateNumber").val();
    var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
    var entranceendtime = $('#txtEntranceEndTime').datetimebox('getValue');
    var entrancegateid = $("#selectGateIn").val();
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_Presence',
        data: "parkingid=" + parkingid + "&cardtype=" + cardtype + "&platenumber=" + platenumber + "&starttime=" + entrancetime + "&endtime=" + entranceendtime + "&entrancegateid="+entrancegateid+"&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
function ExportTest() {
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var cardtype = $("#selectCardType").val();
    var platenumber = $("#txtPlateNumber").val();
    var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
    var entranceendtime = $('#txtEntranceEndTime').datetimebox('getValue');
    var entrancegateid = $("#selectGateIn").val();

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_Presence',
        data: "parkingid=" + parkingid + "&cardtype=" + cardtype + "&platenumber=" + platenumber + "&starttime=" + entrancetime + "&endtime=" + entranceendtime + "&entrancegateid=" + entrancegateid,
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
        ShowImage(row["EntranceImage"], null);
        $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 430, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
    }
}
function DisplayImageDoubleClick(EntranceImage) {
    ShowImage(EntranceImage, null);
    $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 430, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
}
function SetExit() {
    var row = $('#tableListBox').datagrid('getSelected');
    if (row == null) {
        $.messager.alert("系统提示", "请选择需要手动出场的车辆!");
        return;
    }
    $.messager.confirm('系统提示', '确定要手动出场选中的车辆吗?',
    function (r) {
        if (r) {
            $.post('/S/Statistics/SetExit?Id=' + row.ID,
            function (data) {
                if (data.result) {
                    $.messager.show({
                        width: 200,
                        height: 100,
                        msg: '车辆出场成功',
                        title: "手动出场车辆"
                    });
                    $("#btnQueryData").click();
                } else {
                    $.messager.alert('系统提示', data.msg, 'error');
                }

            });
        }
    });
}
$(function () {
    $('#txtEntranceTime').datetimebox('setValue', currentdate00());
    $('#txtEntranceEndTime').datetimebox('setValue', currentdate23());
    //GetParks();
    //GetEntranceCardType();
    //GetEntranceGates();
    //    $("#selectParks").change(function () {
    //        GetEntranceCardType();
    //        GetEntranceGates();
    //    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        if ($.trim(parkingid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        var cardtype = $("#selectCardType").val();
        var platenumber = $("#txtPlateNumber").val();
        var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
        var endtime = $('#txtEntranceEndTime').datetimebox('getValue');
        var entrancegateid = $("#selectGateIn").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/GetMessageData";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, cardtype: cardtype, platenumber: platenumber, starttime: entrancetime, endtime: endtime, entrancegateid: entrancegateid });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        idField: 'ID',
        onDblClickRow: function (rowIndex, rowData) {
            var entranceimage = rowData['EntranceImage'];
            if (entranceimage === null) {
                entranceimage = "";
            }
            DisplayImageDoubleClick(entranceimage);
        },
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
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
                    { field: 'CarTypeName', title: '卡类型', width: 80 },
                    { field: 'InGateName', title: '进场通道', width: 120 },
                    { field: 'AreaName', title: '停车区域', width: 100 },
                    { field: 'EntranceTimeToString', title: '进场时间', width: 120 },
                    { field: 'EmployeeName', title: '车主', width: 80 },
                    { field: 'MobilePhone', title: '联系电话', width: 90 },
                    { field: 'InOperatorName', title: '操作员', width: 110 },
                    { field: 'LongTime', title: '停车时长', width: 100 },
                    { field: 'EntranceCertificateNo', title: '进证件号', width: 140 },
                    { field: 'ExitCertificateNo', title: '出证件号', width: 140 },
				]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '图片', iconCls: 'icon-large-picture', handler: function () { DisplayImage() } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }, '-', { text: '手动出场', iconCls: 'icon-man', handler: function () { SetExit() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof(parkingId) != 'undefined') {
        GetEntranceCardTypeByParkingId(parkingId);
        GetEntranceGatesByParkingId(parkingId);
    }
}