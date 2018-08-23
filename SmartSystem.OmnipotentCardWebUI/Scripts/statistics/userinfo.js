$(function () {
    $('#txtStartTime').datebox('setValue', CurrentMonthFirstDay());
    $('#txtEndTime').datebox('setValue', currentdateDay())
    GetTgPerson();
    $("#btnQueryData").click(function () {
        var ParkingID = $("#selectParks").val();
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');
        if (ParkingID == "-1") {
            $.messager.alert('系统提示', '请选推广人员!', 'error');
            return;
        }
        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Searchrwm"
        $('#tableListBox').datagrid('load', { ParkingID: ParkingID, starttime: starttime, endtime: endtime });
    });
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        fitColumns: true, //自动适应宽度
        autoRowHeight: true, //自动行高度
        columns: [[
                    { field: 'times', title: '时间', width: 200 },
                    { field: 'username', title: '新增关注人', width: 100 },
                    { field: 'name', title: '推广人员', width: 100 },
                    {
                        field: 'phone', title: '手机', width: 100
                    },
                    {
                        field: 'time', title: '时间戳', width: 100
                    },
				]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ text: '当前页', iconCls: 'icon-print', handler: function () { PrintReport(1) } }, '-', { text: '所有页', iconCls: 'icon-print', handler: function () { PrintReport(2) } }]
    });
});
