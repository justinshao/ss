function btnUnLockOrLockCar(obj, parkingid,platenumber, villageid) {
    var status = $(obj).parents(".weui_cell_ft").eq(0).find("[id=hiddStatus]").val();
    var sdata = {};
    sdata.parkingid = parkingid;
    sdata.platenumber = platenumber;
    sdata.villageid = villageid;
    sdata.status = status;
    sdata.r = Math.random();
    $.post("/LockCar/LockOrUnLockCar", sdata, function (data) {
        if (data.result) {
            if (status == "1") {
                //解锁成功
                $(obj).parents(".weui_cell_ft").eq(0).find("[id=hiddStatus]").val("0");
                $(obj).text("锁车").removeClass("weui_btn_warn").addClass("weui_btn_primary");

            } else {
                //锁定成功
                $(obj).parents(".weui_cell_ft").eq(0).find("[id=hiddStatus]").val("1");
                $(obj).text("解锁").removeClass("weui_btn_primary").addClass("weui_btn_warn");
            }
        } else {
            WxAlert("", data.msg);
        }
    });
}