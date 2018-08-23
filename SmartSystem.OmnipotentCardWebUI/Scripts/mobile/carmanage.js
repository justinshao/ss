//function btnAddMyCar() {
//    $("#spanPlateNumberError").text("");
//    var city = $(".spanProvinceDescription").text();
//    var area = $(".spanCityDescription").text();
//    if (city == "" || area == "") {
//        $(".select_plate_cell_city").show();
//        $("#spanPlateNumberError").text("请选择省份");
//        return false;
//    }
//    var licensePlate = $("#txtUserPlateNumber").val();
//    if ($.trim(licensePlate) == "") {
//        $("#spanPlateNumberError").text("请输入车牌号");
//        return false;
//    }

//    if (licensePlate.replace(/\s+/g, "").length != 5 && licensePlate.replace(/\s+/g, "").length != 6) {
//        $("#spanPlateNumberError").text("车牌号格式不正确");
//        return false;
//    }
//    var value = $.trim(city) + $.trim(area) + $.trim(licensePlate);

//    var sdata = {};
//    sdata.licenseplate = value;
//    sdata.r = Math.random();
//    $.post("/CarManage/AddMyCar", sdata, function (data) {
//        if (data.result) {
//            GetMyLicenseplate();
//        } else {
//            $("#spanPlateNumberError").text(data.msg);
//        }
//    });
//}

//function btnPlatePay(licenseplate) {
//    // location.href = "/ParkingPayment/ComputeParkingFee?licensePlate=" + licenseplate + "&parkingId=" + parkid; 
//    var parkid = "";
//    var message = "您确定要交费【" + licenseplate + "】车牌吗？";
//    WxConfirm("系统提醒", message, "MyLicensePlates('" + licenseplate + "'),('" + parkid + "')");
//}
//function MyLicensePlates(licenseplate, parkid) {
//    location.href = "/ParkingPayment/ComputeParkingFee?licensePlate=" + licenseplate + "&parkingId=" + parkid;
//}
$('.black_overlay').on("click", function () {
    $('.black_overlay').hide();
    $("#deleteone").hide();
    $("#deletetwo").hide();
    $("#deletethree").hide();
}); 

function btnjiebangone() {
    $("#deleteone").show();
    $(".black_overlay").show();
}
function btnDeleteMyLicenseplateone() {

    $("#WxConfirmone").show();
    $("#deleteone").hide();


}
function btncancelone() {
    $("#WxConfirmone").hide();
    $(".black_overlay").hide();
}

function btnconfirmone(obj, carid,plateNo) {

    $("#WxConfirmone").hide();
    $(".black_overlay").hide();
    var cid = carid;
    $.ajax({
        type: "Post",
        url: "/CarManage/DeleteMyLicensePlate",
        data: "cid=" + cid + "&plateNo=" + plateNo,
        success: function (msg) {
            if (msg.Status == "解绑车辆成功") {
                window.location.href = "/CarManage/Index";
            }
            else {
                alert(msg.Status);
                window.location.href = "/CarManage/Index";
            }

        }
    });
}

///////
function btnjiebangtwo() {
    $("#deletetwo").show();
    $(".black_overlay").show();
}
function btnDeleteMyLicenseplatetwo() {

    $("#WxConfirmtwo").show();
    $("#deletetwo").hide();


}
function btncanceltwo() {
    $("#WxConfirmtwo").hide();
    $(".black_overlay").hide();
}

function btnconfirmtwo(obj, carid, plateNo) {

    $("#WxConfirmtwo").hide();
    $(".black_overlay").hide();
    var cid = carid;
    $.ajax({
        type: "Post",
        url: "/CarManage/DeleteMyLicensePlate",
        data: "cid=" + cid + "&plateNo=" + plateNo,
        success: function (msg) {
            if (msg.Status == "解绑车辆成功") {
                window.location.href = "/CarManage/Index";
            }
            else {
                alert(msg.Status);
                window.location.href = "/CarManage/Index";
            }
        }
    });
}

///////
function btnjiebangthree() {
    $("#deletethree").show();
    $(".black_overlay").show();
}
function btnDeleteMyLicenseplatethree() {

    $("#WxConfirmthree").show();
    $("#deletethree").hide();


}
function btncancelthree() {
    $("#WxConfirmthree").hide();
    $(".black_overlay").hide();
}

function btnconfirmthree(obj, carid, plateNo) {

    $("#WxConfirmthree").hide();
    $(".black_overlay").hide();
    var cid = carid;
    $.ajax({
        type: "Post",
        url: "/CarManage/DeleteMyLicensePlate",
        data: "cid=" + cid + "&plateNo=" + plateNo,
        success: function (msg) {
            if (msg.Status == "解绑车辆成功") {
                window.location.href = "/CarManage/Index";
            }
            else {
                alert(msg.Status);
                window.location.href = "/CarManage/Index";
            }
        }
    });
}

