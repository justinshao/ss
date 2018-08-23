function DGReflush() {
    var parkingid = $("#selectParks").combobox("getValue");
    if ($.trim(parkingid) == "") {
        $.messager.alert('系统提示', '请选择车场!', 'error');
        return;
    }
    var options = $('#tableListBox').datagrid('options');
    options.url = "/p/ParkSettle/Search_ApprovalFlows";
    $('#tableListBox').datagrid('load', { parkingid: parkingid });
}
$(function () {
    BindGetParkTree();
    $("#btnQueryData").click(function () {
        DGReflush();
    });
    $("#btnSave").click(function () {
        var pkid = $("#selectParks").combobox("getValue");
        if ($.trim(pkid) == "") {
            $.messager.alert('系统提示', '请选择车场!', 'error');
            return;
        }
        var userid = $("#selectUsers").val();
        var selectRow = $('#tableListBox').datagrid('getSelected');
        var flowid = selectRow.FlowID;
        $.ajax({
            type: "post",
            url: '/p/ParkSettle/SaveFlowOperator',
            data: "pkid=" + pkid + "&userid=" + userid + "&flowid=" + flowid,
            async: false,
            success: function (data) {
                $('#divAuditing').dialog('close');
                if (data) {
                    DGReflush();
                }
                else {
                    $.messager.alert('系统提示', '保存失败!', 'error');
                }
            }
        });
    })
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'RecordID', title: '记录编号', width: 150, hidden: true },
                    { field: 'PKID', title: '车场编号', width: 150, hidden: true },
                    { field: 'FlowUser', title: '用户编号', width: 150, hidden: true },
                    { field: 'FlowID', title: '流程状态值', width: 80 },
                    { field: 'FlowName', title: '流程状态名称', width: 150 },
                    { field: 'FlowUserName', title: '流程审批人', width: 400 },
                    { field: 'Remark', title: '流程备注', width: 300 },
				]],
        onLoadSuccess: function (data) {
            $('.editcls').linkbutton({ plain: true, iconCls: 'icon-large-picture' });
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: false,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{
            iconCls: 'icon-edit',
            text: '流程审批人',
            handler: function () {
                var selectRow = $('#tableListBox').datagrid('getSelected');
                if (selectRow == null) {
                    $.messager.alert("系统提示", "请选择需要编辑的行!");
                    return;
                }
                $("#txtAuditingRemark").html("流程状态: " + selectRow.FlowName + "(" + selectRow.Remark + ")");
                BindUser(selectRow.PKID)
                $('#divAuditing').show().dialog({ title: "选择流程审批人", width: 400, height: 200, modal: true, collapsible: false, minimizable: false, maximizable: false });
            }
        }]
    });
});

function BindUser(pkid) {
    if ($.trim(pkid) == "") {
        return;
    }
    $("#selectUsers").html("");
    $.ajax({
        type: "post",
        url: '/p/ParkSettle/GetUsers',
        data: "pkid=" + pkid + "",
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectUsers").append("<option value='" + item.RecordID + "'>" + item.UserName + "</option>");
                })
            }
        }
    });
}
function StatisticsChangeParking(parkingId) {
    if (typeof(parkingId) != 'undefined') {
        GetPriod();
        BindUser();
    }
}