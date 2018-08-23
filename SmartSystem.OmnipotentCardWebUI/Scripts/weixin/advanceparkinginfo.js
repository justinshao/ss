$(function () {
    $("#btnQueryData").click(function () {
        var plateNo = $("#txtPlateNo").val();
        var start = $("#txtStartTime").datetimebox("getValue");
        var end = $("#txtEndTime").datetimebox("getValue");

        $('#tableAdvanceParking').datagrid('load', { PlateNo: plateNo, StartTime: start, EndTime: end });
    });

    $('#tableAdvanceParking').datagrid({
        url: '/w/AdvanceParkingInfo/GetAdvanceParkingInfoData',
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        sortName: 'ID',
        sortOrder: 'desc',
        remoteSort: false,
        idField: 'ID',
        columns: [[
                    { field: 'ID', title: 'ID', width: 80, hidden: true },
                    { field: 'OrderId', title: '订单号', width: 120 },
                    { field: 'PlateNo', title: '车牌号', width: 120 },
                     { field: 'StartTime', title: '开始时间', width: 150 },
                     { field: 'EndTime', title: '结束时间', width: 150 },
                     { field: 'Amount', title: '支付金额(元)', width: 100 },
                     { field: 'PayTime', title: '支付时间', width: 150 },
                     { field: 'CreateTime', title: '创建时间', width: 150 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45]
    });
});