function currentdate00() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " 00:00:00"
    } catch (ex) {
        return "";
    }
}
function currentdate23() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " 23:59:59"
    } catch (ex) {
        return "";
    }
}
function currentdatenow() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
    } catch (ex) {
        return "";
    }
}

function currentdateDay() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + day;
    } catch (ex) {
        return "";
    }
}

function CurrentMonthFirstDay() {
    try {
        var date = new Date();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

        return date.getFullYear() + "-" + month + "-01";
    } catch (ex) {
        return "";
    }
}
function GetParks() {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetParks',
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

function GetTgPerson() {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetTgPerson',
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#selectParks").append("<option value='" + item.id + "'>" + item.name + "</option>");
                })
            }
        }
    });
}

function BindGetParkTree() {
    $.ajax({
        url: '/S/Statistics/GetParks',
        type: 'post',
        success: function (data) {
            $("#selectParks").combobox({
                data: data,
                valueField: 'PKID',
                textField: 'PKName',
                onSelect: function (record) {
                    StatisticsChangeParking(record.PKID);
                }, onLoadSuccess: function () {
                    var data = $('#selectParks').combobox('getData');
                    if (data.length > 0) {
                        $('#selectParks').combobox('select', data[0].PKID);
                        //StatisticsChangeParking(data[0].PKID);
                    } 
                   
                }
            });
        }
    });
}
function GetEntranceCardType() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetCardEntranceType',
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
        url: '/S/Statistics/GetEntranceGates',
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
        url: '/S/Statistics/GetGates',
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
function GetGatesByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetGates',
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
        url: '/S/Statistics/GetExitGates',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectGateOut").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectGateOut").append("<option value='" + item.GateID + "'>" + item.GateName + "</option>");
                })
            }
        }
    });
}
function GetExitGatesByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetExitGates',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectGateOut").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectGateOut").append("<option value='" + item.GateID + "'>" + item.GateName + "</option>");
                })
            }
        }
    });
}
function GetCarTypes() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetCarTypes',
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
function GetCarTypes(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetCarTypes',
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
function GetCarTypesByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetCarTypes',
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
function GetAreas() {
    var parkingid = $("#selectParks").val();
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetAreas',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectArea").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectArea").append("<option value='" + item.AreaID + "'>" + item.AreaName + "</option>");
                })


            }
        }
    });
}
function GetAreasByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetAreas',
        data: "ParkingID=" + parkingid + "",
        success: function (data) {
            if (data != null) {
                $("#selectArea").html("<option value='-1'>全部</option>");
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
        url: '/S/Statistics/GetBoxes',
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
function GetBoxesByParkingId(parkingid) {
    $("#selectBox").html("<option value='-1'>全部</option>");
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetBoxes',
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
        url: '/S/Statistics/GetOnDutys',
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
function GetOnDutysByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetOnDutys',
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
        url: '/S/Statistics/GetEventType',
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
function GetEventType() {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetEventType',
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
function GetSellers() {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetSellers',
        success: function (data) {
            if (data != null) {
                $("#selectSeller").html("<option value='-1'>全部</option>");
                $.each(data, function (index, item) {
                    $("#selectSeller").append("<option value='" + item.SellerID + "'>" + item.SellerName + "</option>");
                })
            }
        }
    });
}

function ShowImage(entranceimage, exitimage) {
    //进场大图与小图
    if (entranceimage != null && entranceimage.length > 0) {
        $("#EntranceImage").prop("src", "/Pic/" + entranceimage);
        if (entranceimage.indexOf("HandGate") == -1) {
            //非手动抬杆(有小图)
            $("#EntranceSmallImage").prop("src", "/Pic/" + entranceimage.replace("Big", "Small"));
        }
        else {
            $("#EntranceSmallImage").prop("src", "/Content/images/default_not_image_small.png");
        }
       
    }
    else {
        $("#EntranceImage").prop("src", "/Content/images/default_not_image_big.png?v=1");
        $("#EntranceSmallImage").prop("src", "/Content/images/default_not_image_small.png");
    }

    if (exitimage != null && exitimage.length > 0) {
        $("#ExitImage").prop("src", "/Pic/" + exitimage);
        if (exitimage.indexOf("HandGate") == -1) {
            //非手动抬杆(有小图)
            $("#ExitSmallImage").prop("src", "/Pic/" + exitimage.replace("Big", "Small"));
        }
        else {
            $("#ExitSmallImage").prop("src", "/Content/images/default_not_image_small.png");
        }
    }
    else {
        $("#ExitImage").prop("src", "/Content/images/default_not_image_big.png?v=1");
        $("#ExitSmallImage").prop("src", "/Content/images/default_not_image_small.png");
    }
}

function GetEntranceCardTypeByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetCardEntranceType',
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
function GetEntranceGatesByParkingId(parkingid) {
    $.ajax({
        type: "post",
        url: '/S/Statistics/GetEntranceGates',
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

