$(function () {
    $(function () {
        $('#VillageTree').tree({
            url: '/VillageData/CreateVillageTreeOnLineStatus',
            onSelect: function (node) {
                if (node.attributes.type == 1) {
                    CurrentSelectVillage(node.id);
                }
            }
        });
    });
    function CurrentSelectVillage(villageid) {
        BindDeviceStatus(villageid);
    }
});
function BindDeviceStatus(villageid) {
    $('#tableDeviceStatus').treegrid({
        rownumbers: false,
        animate: true,
        collapsible: false,
        fitColumns: true,
        url: '/S/Device/GetDeviceDetectionData',
        queryParams : {villageid: villageid},
        idField: 'id',
        treeField: 'DeviceName',
        columns: [[
                     { field: 'id', title: 'ID', width: 0, hidden: true },
                     { title: 'HasChildMenu', field: 'HasChildMenu', width: 80, hidden: true },
                     { title: 'MasterID', field: 'MasterID', width: 80, hidden: true },
                     { field: 'DeviceName', title: '设备名称', width: 100 },
                     { field: 'IsOnLine', title: '连接状态', width: 80 }
				]],
        pagination: false,
        rownumbers: false
    });
}
