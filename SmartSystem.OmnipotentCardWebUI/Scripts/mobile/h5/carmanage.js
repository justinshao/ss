function btnAddMyCar() {
    $("#spanPlateNumberError").text("");
    var city = $(".spanProvinceDescription").text();
    var area = $(".spanCityDescription").text();
    if (city == "" || area == "") {
        $(".select_plate_cell_city").show();
        $("#spanPlateNumberError").text("请选择省份");
        return false;
    }
    var licensePlate = $("#txtUserPlateNumber").val();
    if ($.trim(licensePlate) == "") {
        $("#spanPlateNumberError").text("请输入车牌号");
        return false;
    }

    if (licensePlate.replace(/\s+/g, "").length != 5 && licensePlate.replace(/\s+/g, "").length != 6) {
        $("#spanPlateNumberError").text("车牌号格式不正确");
        return false;
    }
    var value = $.trim(city) + $.trim(area) + $.trim(licensePlate);

    var sdata = {};
    sdata.licenseplate = value;
    sdata.r = Math.random();
    $.post("/H5CarManage/AddMyCar", sdata, function (data) {
        if (data.result) {
            GetMyLicenseplate();
        } else {
            $("#spanPlateNumberError").text(data.msg);
        }
    });
}
function GetMyLicenseplate() {
    $(".carlist").html("");
    var sdata = {};
    sdata.r = Math.random();
    $.post("/H5CarManage/GetMyLicenseplate", sdata, function (data) {
        if (data.result) {
            var d = data.data;
            for (var i = 0; i < d.length; i++) {
                btnAddMyCarHtml(d[i].PlateNo, d[i].RecordID, d[i].Status);
            }
        }
        CheckShowNotData();
    });
}
function btnAddMyCarHtml(licenseplate, recordId, status) {
    var str = "<div class=\"weui_cell\">";
    str += "   <input type=\"hidden\" id=\"hiddCarId\" value=\"" + recordId + "\"/>";
    str += "  <div class=\"weui_cell_hd\"> <img src=\"/Content/mobile/images/car_icon.png?v=1\"  style=\"width: 20px;margin-right: 5px; display: block\" /></div>";
    str += " <div class=\"weui_cell_bd weui_cell_primary\"><p class=\"carname\">" + licenseplate + "</p></div>";
    str += " <div class=\"weui_cell_ft\">";
    if (status == "2") {
        str += "<a href=\"javascript:void(0);\"  onclick=\"return btnDeleteMyLicenseplate(this,'" + licenseplate + "')\"><img src=\"/Content/mobile/images/delete_icon.png\" style=\"width: 25px;\"/></a>";
    }
    str += " </div></div>";
    $(".carlist").prepend(str);
    CheckShowNotData();
}
var deleteid = "";
var deleteobj = null;
function btnDeleteMyLicenseplate(obj, licenseplate) {
    deleteobj = obj;
    deleteid = $(obj).parents(".weui_cell").eq(0).find("[id=hiddCarId]").val();
    var message = "您确定要删除【" + licenseplate + "】车牌吗？";
    WxConfirm("系统提醒", message, "DeleteMyLicensePlate");

}
function DeleteMyLicensePlate() {
    if (deleteid == "") {
        WxAlert("", "获取车牌失败,请退出后重试");
        return false;
    }
    var sdata = {};
    sdata.id = deleteid;
    sdata.r = Math.random();
    $.post("/H5CarManage/DeleteMyLicensePlate", sdata, function (data) {
        if (data.result) {
            $(deleteobj).parents(".weui_cell").eq(0).remove();
            CheckShowNotData();
        } else {
            WxAlert("", "删除失败");
        }
    });

}
$(function () {
    GetMyLicenseplate();
});
function CheckShowNotData() {
    var len = $(".carlist .weui_cell").length;
    if (len == 0) {
        $("#divNoCarDataBox").show();
    } else {
        $("#divNoCarDataBox").hide();
    }
}