function ExportTest() {

    var parkingid = $("#selectParks").combobox("getValue");

    var data = $('#tableListBox').datagrid('getExcelXml', { title: '进出场数量' });  //获取datagrid数据对应的excel需要的xml格式的内容 
    //用ajax发动到动态页动态写入xls文件中

    $.ajax({
        url: "/S/Statistics/DownLoadExcel",
        data: { data: data },
        type: 'POST',
        dataType: 'text',
        success: function (fn) {
            window.open(fn);
        },
        error: function (xhr) {
            $("#btnexport").html('动态页有问题\nstatus：' + xhr.status + '\nresponseText：' + xhr.responseText)
        }
    });
}
function BindGetParkTrees() {
    $.ajax({
        url: '/S/Statistics/GetParks',
        type: 'post',
        success: function (data) {
            var comdata = [{ 'text': '全部', 'id': '-1'}];
            for (var i = 0; i < data.length; i++) {
                comdata.push({ "text": data[i].PKName, "id": data[i].PKID });
            }

            $('#selectParks').combobox({
                data: comdata,
                valueField: 'id',
                textField: 'text'
            });
        }
    });
}

$(function () {
    $('#txtStartTime').datetimebox('setValue', currentdate00())
    $('#txtEndTime').datetimebox('setValue', currentdate23())
    //    GetParks();
    //    GetBoxes();
    //    GetOnDutys();
    $("#btnQueryData").click(function () {
        var parkingid = $("#selectParks").combobox("getValue");
       
        var starttime = $('#txtStartTime').datetimebox('getValue');
        var endtime = $('#txtEndTime').datetimebox('getValue');


        var options = $('#tableListBox').datagrid('options');
        options.url = "/S/Statistics/Search_ParkIO";
        $('#tableListBox').datagrid('load', { parkingid: parkingid, starttime: starttime, endtime: endtime });
    });
    //    $("#selectParks").change(function () {
    //        GetBoxes();
    //        GetOnDutys();
    //    })
    BindGetParkTrees();
    $('#tableListBox').datagrid({
        singleSelect: true,
        nowrap: false,
        striped: true,
        collapsible: false,
        remoteSort: false,
        columns: [[
                    { field: 'PKName', title: '车场名称', width: 150 },
                    { field: 'EntranceNumber', title: '车辆进场数', width: 150 },
                    { field: 'ExitNumber', title: '车辆出场数', width: 150 }

				]],
        onLoadSuccess: function (data) {
            $('#tableListBox').datagrid('fixRowHeight');
        },
        pagination: true,
        rownumbers: true,
        pageSize: 35,
        pageList: [35],
        toolbar: [{ id: 'btnexport', text: '导出', iconCls: 'icon-save', handler: function () { ExportTest() } }]
    });
});
function StatisticsChangeParking(parkingId) {
    if (typeof (parkingId) != 'undefined') {
        GetOnDutysByParkingId(parkingId);
        GetBoxesByParkingId(parkingId);
    }
}
