
function PrintReport(printtype) {
    var pageopt = $('#tableListBox').datagrid('getPager').data("pagination").options;
    var pageIndex = pageopt.pageNumber;
    var pageSize = pageopt.pageSize;

    if (pageSize == 0) {
        pageSize = 1; 
    }
    if (printtype == 2) {
        pageIndex = -1;
    }
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var cardtypeid = $("#selectCardType").val();
    var cartypeid= $("#selectCarType").val();
    var exitgateid= $("#selectGateOut").val();
    var entrancegateid= $("#selectGateIn").val();
    var exitoperatorid= $("#selectOnDutys").val();
    var releasetype= $("#selectReleaseType").val();
    var areaid= $("#selectArea").val();
    var entrancetime = $('#txtEntranceTime').datetimebox('getValue');  
    var endtime = $('#txtExitTime').datetimebox('getValue'); 
    var owner= $("#txtOwner").val();
    var platenumber= $("#txtPlateNumber").val();
    var isexist = $("#selectIsExit").val();  
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_InOutRecord',
        data: "parkingid=" + parkingid + "&cardtypeid=" + cardtypeid + "&cartypeid=" + cartypeid + "&exitgateid=" + exitgateid + "&entrancegateid=" + entrancegateid + "&exitoperatorid=" + entrancegateid + "&releasetype=" + releasetype + "&areaid=" + areaid + "&isexit=" + isexist+"&platenumber=" + platenumber+"&owner=" + owner + "&starttime=" + entrancetime + "&endtime=" + endtime + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "",
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
    var cardtypeid = $("#selectCardType").val();
    var cartypeid = $("#selectCarType").val();
    var exitgateid = $("#selectGateOut").val();
    var entrancegateid = $("#selectGateIn").val();
    var exitoperatorid = $("#selectOnDutys").val();
    var releasetype = $("#selectReleaseType").val();
    var areaid = $("#selectArea").val();
    var entrancedate = $('#txtEntranceTime').datetimebox('getValue');
    var endtime = $('#txtExitTime').datetimebox('getValue');
    var platenumber = $("#txtPlateNumber").val();
    var owner = $("#txtOwner").val();
    var isexit = $("#selectIsExit").val();
    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_InOut',
        data: "parkingid=" + parkingid + "&cardtypeid=" + cardtypeid + "&cartypeid=" + cartypeid + "&exitgateid=" + exitgateid + "&entrancegateid=" + entrancegateid+ "&exitoperatorid=" + exitoperatorid + "&releasetype=" + releasetype + "&areaid=" + areaid+ "&starttime=" + entrancedate + "&platenumber=" + platenumber + "&owner=" + owner+ "&endtime=" + endtime + "&isexit=" + isexit,
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
        ShowImage(row["EntranceImage"],row["ExitImage"]);
        $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 490, modal: true, collapsible: false, minimizable: false, maximizable: false });
    }
}
function DisplayImageDoubleClick(EntranceImage,ExitImage) {
    ShowImage(EntranceImage,ExitImage);
    $('#DivShowImage').show().dialog({ title: "车辆进出图片", width: 820, height: 490, modal: true, collapsible: false, minimizable: false, maximizable: false });
}
$(function () {
    $('#txtEntranceTime').datetimebox('setValue', currentdate00())
    $('#txtExitTime').datetimebox('setValue', currentdate23())
//    GetParks();
//    GetEntranceCardType();
//    GetEntranceGates();
//    GetExitGates();
//    GetCarTypes();
//    GetAreas();
//    GetOnDutys();
//    $("#selectParks").change(function () {
//        GetEntranceCardType();
//        GetEntranceGates();
//        GetExitGates();
//        GetCarTypes();
//        GetAreas();
//        GetOnDutys();
//    })
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var cardtypeid = $("#selectCardType").val();
        var cartypeid = $("#selectCarType").val();
        var exitgateid = $("#selectGateOut").val();
        var entrancegateid = $("#selectGateIn").val();
        var exitoperatorid = $("#selectOnDutys").val();
        var releasetype = $("#selectReleaseType").val();
        var areaid = $("#selectArea").val();
        var entrancedate = $('#txtEntranceTime').datetimebox('getValue');
        var endtime = $('#txtExitTime').datetimebox('getValue');
        var platenumber = $("#txtPlateNumber").val();
        var owner = $("#txtOwner").val();
        var isexit = $("#selectIsExit").val();
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_InOut";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, cardtypeid: cardtypeid, cartypeid: cartypeid, exitgateid: exitgateid, entrancegateid: entrancegateid, exitoperatorid: exitoperatorid, releasetype: releasetype, areaid: areaid, starttime: entrancedate, platenumber: platenumber, owner: owner, endtime: endtime, isexit: isexit });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        fixRowHeight: true,
        remoteSort: false,
        onDblClickRow: function (rowIndex, rowData) {
            var entranceimage = rowData['EntranceImage'];
            var exitimage = rowData['ExitImage'];
            DisplayImageDoubleClick(entranceimage,exitimage);
        },
        columns: [[
                    { field: 'EntranceImage', title: '进场图片', width: 120, hidden: true },
                    { field: 'ExitImage', title: '出场图片', width: 120, hidden: true },
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
                    { field: 'ExitCertificateNo', title: '出证件号', width: 140 },
                    { field: 'Remark', title: '备注', width: 200 }
				]],
        onLoadSuccess:function(data){  
            $('.editcls').linkbutton({plain:true,iconCls:'icon-large-picture'});  
            $('#tableListBox').datagrid('fixRowHeight');  
        },  
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }, '-', { text: '图片', iconCls: 'icon-large-picture', handler: function () { DisplayImage() } },'-', { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof(parkingId) != 'undefined') {
        GetEntranceCardTypeByParkingId(parkingId);
        GetEntranceGatesByParkingId(parkingId);
        GetExitGatesByParkingId(parkingId);
        GetCarTypesByParkingId(parkingId);
        GetAreasByParkingId(parkingId);
        GetOnDutysByParkingId(parkingId);
    }
}