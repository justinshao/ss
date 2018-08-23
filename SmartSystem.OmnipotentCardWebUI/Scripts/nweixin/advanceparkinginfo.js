$(function () {
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            var plateNo = $("#txtPlateNo").val();
            var start = $("#txtStartTime").datetimebox("getValue");
            var end = $("#txtEndTime").datetimebox("getValue");

            $('#tableAdvanceParking').datagrid('load', { PlateNo: plateNo, StartTime: start, EndTime: end, CompanyId: record.id });
        }
    });
});
$(function () {
    $("#btnQueryData").click(function () {
        var plateNo = $("#txtPlateNo").val();
        var start = $("#txtStartTime").datetimebox("getValue");
        var end = $("#txtEndTime").datetimebox("getValue");
        var selectRow = $('#treeCompany').tree('getSelected');
        if (selectRow == null) {
            $.messager.alert("系统提示", "请先选择单位!");
            return;
        }
        $('#tableAdvanceParking').datagrid('load', { PlateNo: plateNo, StartTime: start, EndTime: end, CompanyId: selectRow.id });
    });

    $('#tableAdvanceParking').datagrid({
        url: '/nwx/AdvanceParkingInfo/GetAdvanceParkingInfoData',
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
                     { field: 'StartTimeToString', title: '开始时间', width: 150 },
                     { field: 'EndTimeToString', title: '结束时间', width: 150 },
                     { field: 'Amount', title: '支付金额(元)', width: 100 },
                     { field: 'PayTimeToString', title: '支付时间', width: 150 },
                     { field: 'CreateTimeToString', title: '创建时间', width: 150 }
				]],
        pagination: true,
        rownumbers: true,
        pageSize: 15,
        pageList: [15, 25, 35, 45]
    });
});