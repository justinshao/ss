
function GetNoPlateGate() {
    var parkingid = $("#selectParks").combobox("getValue");
    $.ajax({
        type: "post",
        url: '/Statistics/GetEntranceGate',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectInGate").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectInGate").append("<option value='" + item.GateID + "'>" + item.GateName + "</option>");
                })
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
    if ($.trim(parkingid) == "-1") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var ingateid = $("#selectGateIn").val();
    var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
    var endtime = $('#txtEntranceEndTime').datetimebox('getValue');
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_NoPlateNumber',
        data: "parkingid=" + parkingid + "&ingateid=" + ingateid + "&starttime=" + entrancetime + "&endtime=" + endtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var ingateid = $("#selectGateIn").val();
    var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
    var endtime = $('#txtEntranceEndTime').datetimebox('getValue');

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_Exception',
        data: "parkingid=" + parkingid + "&starttime=" + entrancetime + "&endtime=" + endtime + "&ingateid=" + ingateid,
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
         ShowImage(row["EntranceImage"], row["ExitImage"]);
        $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
    }
}
function DisplayImageDoubleClick(EntranceImage,ExitImage) {
     ShowImage(EntranceImage, ExitImage);
    $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 480, modal: true, collapsible: false, minimizable: false, maximizable: false });
}
$(function () {
    $("#txtEntranceTime").datetimebox('setValue', currentdate00());
    $("#txtEntranceEndTime").datetimebox('setValue', currentdate23());
//    GetParks();
//    GetEntranceGates();
//    $("#selectParks").change(function () {
//        GetEntranceGates();
//    })
     BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var ingateid = $("#selectGateIn").val();
        var entrancetime = $('#txtEntranceTime').datetimebox('getValue');
        var endtime = $('#txtEntranceEndTime').datetimebox('getValue');
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_NoPlateNumber";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, ingateid: ingateid, starttime: entrancetime, endtime: endtime });
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
                    { field: 'IsExit', title: '是否出场', width: 80,
                        formatter: function (value, row, index) {
                            if (value == "1")
                                return "已出场";
                            else
                                return "在场";
                        },
                        styler: function (value, row, index) {
                            if (value == "1") {
                                return 'background-color:pink';
                            }
                            else {
                                return 'background-color:lightblue';
                            }
                        }
                    },
                    { field: 'CarModelName', title: '车类型', width: 80 },
                    { field: 'CarTypeName', title: '卡类型', width: 80 },
                    { field: 'ReleaseTypeName', title: '放行类型', width: 80,
                        styler: function (value, row, index) {
                            if (value == "异常放行") {
                                return 'background-color:lightcoral';
                            }
                        },
                        formatter: function (value, row, index) {
                            if (!row["IsExit"]) {
                                return "";
                            }
                            else
                                return value;
                        },
                    },
                    { field: 'EntranceTimeToString', title: '进场时间', width: 120},
                    { field: 'InGateName', title: '进场通道', width: 120 },
                    { field: 'ExitTimeToString', title: '出场时间', width: 120},
                    { field: 'OutGateName', title: '出场通道', width: 120 },
                    { field: 'LongTime', title: '停车时长', width: 100 },
                    { field: 'AreaName', title: '停车区域', width: 100 },
                    { field: 'EmployeeName', title: '车主', width: 80 },
                    { field: 'MobilePhone', title: '联系电话', width: 90 },
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
        pageSize: 50,
        pageList: [50],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '图片', iconCls: 'icon-large-picture', handler: function () { DisplayImage() } }, '-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof(parkingId) != 'undefined') {
        GetEntranceGatesByParkingId(parkingId);
    }
}