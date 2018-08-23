
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
    //var cardtype = $("#selectCarType").val();
    var cardtype = "-1";
    var platenumber = $("#txtPlateNumber").val();
    var addr = $("#txtHomeAddr").val();
    var mobile = $("#txtMobile").val();
    var due = $("#cbxDue").prop("checked");
    $.ajax({
        type: "post",
        url: '/S/ReportParams/Params_MonthCardInfo',
        data: "parkingid=" + parkingid + "&cardtype=" + cardtype + "&platenumber=" + platenumber + "&addr=" + addr + "&mobile=" + mobile + "&pageindex=" + pageIndex + "&pagesize=" + pageSize + "&due=" + due + "",
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
    var cardtype = "-1";
    var platenumber = $("#txtPlateNumber").val();
    var addr = $("#txtHomeAddr").val();
    var mobile = $("#txtMobile").val();
    var due = $("#cbxDue").prop("checked");

    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_MonthCardInfo',
        data: "parkingid=" + parkingid + "&cardtype=" + cardtype + "&platenumber=" + platenumber + "&addr=" + addr + "&mobile=" + mobile+ "&due=" + due,
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
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        if ($.trim(parkingid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        //        var cardtype = $("#selectCarType").val();
        var cardtype = "-1";
        var platenumber = $("#txtPlateNumber").val();
        var addr = $("#txtHomeAddr").val();
        var mobile = $("#txtMobile").val();
        var options = $('#tableListBox').datagrid('options');
        var due = $("#cbxDue").prop("checked");
        options.url = "/S/Statistics/Search_MonthCardInfo";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, cardtype: cardtype, platenumber: platenumber, addr: addr, mobile: mobile, due: due });
    });
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
                            if (value.substring(0, 3) == "无车牌") {
                                return "无车牌";
                            }
                            else
                                return value;
                        }
                    },
                    { field: 'EmployeeName', title: '车主名称', width: 100 },
                    { field: 'Mobile', title: '联系电话', width: 120 },
                    { field: 'CarTypeName', title: '卡片类型', width: 100 },
                    { field: 'strStartTime', title: '开始时间', width: 140 },
                    { field: 'strEndTime', title: '结束时间', width: 140 },
                    { field: 'Addr', title: '家庭地址', width: 120 }
				]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
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
        if (parkingId != "") {
            GetCarTypes(parkingId);
        }
    }
}