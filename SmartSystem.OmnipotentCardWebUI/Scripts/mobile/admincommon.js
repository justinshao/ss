function GetParks() {
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetParks',
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectParks").append("<option value='" + item.PKID + "'>" + item.PKName + "</option>");
                })
            }
        }
    });
}
function GetEntranceCardType() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetCardEntranceType',
        data: "parkingid=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectCardType").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectCardType").append("<option value='" + item.CarTypeID + "'>" + item.CarTypeName + "</option>");
                })
            }
        }
    });
}
function GetEntranceGates() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetEntranceGates',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectGateIn").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectGateIn").append("<option value='" + item.GateID + "'>" + item.GateName + "</option>");
                })
            }
        }
    });
}

function GetGates() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetGates',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectGate").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectGate").append("<option value='" + item.GateID + "'>" + item.GateName + "</option>");
                })
            }
        }
    });
}



function GetExitGates() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetExitGates',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectGateOut").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectGateOut").append("<option value='" + item.CouponID + "'>" + item.CouponType + "</option>");
                })
            }
        }
    });
}

function GetCarTypes() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetCarTypes',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectCarType").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectCarType").append("<option value='" + item.CarModelID + "'>" + item.CarModelName + "</option>");
                })
            }
        }
    });
}
function GetAreasNotAll() {
    GetAreaData(false);
}
function GetAreas() {
    GetAreaData(true);
}
function GetAreaData(all) {
    $("#selectArea option").remove();
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetAreas',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                if (all) {
                    $("#selectArea").html("<option value='-1'>全部</option>");
                }
               
                $.each(data, function (index, item) {
                    $("#selectArea").append("<option value='" + item.AreaID + "'>" + item.AreaName + "</option>");
                })


            }
        }
    });
}

function GetBoxes() {
    var parkingid = $("#selectParks").val();
    $("#selectBox").html("<option value='-1'>全部</option>");
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetBoxes',
        data: "ParkingID=" + parkingid,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectBox").append("<option value='" + item.BoxID + "'>" + item.BoxName + "</option>");
                })
            }
        }
    });
}

function GetOnDutys() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetOnDutys',
        data: "parkingid=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectOnDutys").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectOnDutys").append("<option value='" + item.RecordID + "'>" + item.UserName + "</option>");
                })
            }
        }
    });
}

function GetEventType() {
    $.ajax({
        type: "post",
        url: '/ParkCommonData/GetEventType',
        success: function (data) {
            if (data != null) {
                $("#selectEvent").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectEvent").append("<option value='" + item.EnumValue + "'>" + item.Description + "</option>");
                })
            }
        }
    });
}
