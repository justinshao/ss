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
        url: '/S/Statistics/Export_PlateNumberPrifix',
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
    BindGetParkTree();
    //本月第一天
    $('#txtStartTime').datetimebox('setValue', currentdate00());
    //当前时间
    $('#txtEndTime').datetimebox('setValue', currentdate23());
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/GetPlateNumberAnalyse";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, starttime: starttime, endtime: endtime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'ParkingName', title: '车场名称', width: 150 },
                    { field: 'PlateNumberPrefix', title: '车牌前缀', width: 80 },
                    { field: 'Number', title: '数量', width: 80 },
                    { field: 'Rate', title: '占比', width: 80 }
				]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
