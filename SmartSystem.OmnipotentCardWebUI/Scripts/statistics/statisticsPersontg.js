 
//导出
function ExportTest() {
    var parkingid = $("#selectParks").combobox("getValue"); 
    var starttime = $('#txtStartTime').datetimebox('getValue');
    var endtime = $('#txtEndTime').datetimebox('getValue');
    if ($("#tableListBox").datagrid("getData").total == 0) {
        $.messager.alert('系统提示', '当前时间段无数据!', 'error');
        return;
    }
    $.ajax({
        type: "post",
        url: '/S/Statistics/Export_TgOrderInfo',
        data: "ParkingID=" + parkingid + "&starttime=" + starttime + "&endtime=" + endtime,
        async: true,
        success: function (data) {
            window.open(data);
        }
    });

}

$(function () {
    $('#txtStartTime').datetimebox('setValue', currentdate00())
    $('#txtEndTime').datetimebox('setValue', currentdate23())
    //$('#txtStartTime').datebox('setValue', CurrentMonthFirstDay());
    //$('#txtEndTime').datebox('setValue', currentdateDay())
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
        options.url = "/S/Statistics/CountTgPersonOrder";
        $('#tableListBox').datagrid('load', { ParkingID: ParkingID, starttime: starttime, endtime: endtime });
    });
    
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
            { field: 'ID', title: ' ID', width: 100, hidden: true },
            { field: 'OrderID', title: 'OrderID', width: 100, hidden: true },
            { field: 'PKID', title: '车场id', width: 100, hidden: true }, 
            { field: 'RealPayTime', title: '实际支付时间', width: 100, hidden: true  },
            { field: 'RealPayTimeToString', title: '实际支付时间串', width: 100, hidden: true },
            { field: 'PKName', title: '车场名称', width: 100 }, 
            { field: 'PersonId', title: '人员编号ID', width: 100 },
            { field: 'PersonName', title: '人员姓名', width: 100 },
            { field: 'Count', title: '推广数', width: 100 },
            { field: 'Amount', title: '推广订单总额', width: 100},             
        ]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [ { text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
}); 