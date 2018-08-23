// 百度地图API功能
var map = new BMap.Map("parkingmap");
map.enableScrollWheelZoom(true);
map.addControl(new BMap.ScaleControl());
var top_right_navigation = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_SMALL });
map.addControl(top_right_navigation);

// 添加定位控件
//var geolocationControl = new BMap.GeolocationControl();
//geolocationControl.addEventListener("locationSuccess", function (e) {
//    var latlng = $("#hiddWeiXinLocation").val();
//    var ls = latlng.split(",");
//    map.centerAndZoom(new BMap.Point(ls[1], ls[0]), 18);
//});
//geolocationControl.addEventListener("locationError", function (e) {
//    var latlng = $("#hiddWeiXinLocation").val();
//    var ls = latlng.split(",");
//    map.centerAndZoom(new BMap.Point(ls[1], ls[0]), 18);
//});
//map.addControl(geolocationControl);

map.disableDoubleClickZoom();
$(function () {
    var latlng = $("#hiddWeiXinLocation").val();
    var ls = latlng.split(",");
    map.centerAndZoom(new BMap.Point(ls[1], ls[0]), 18);
    var wxloaction = $("#hiddIsWeiXinLocation").val();
    if (wxloaction == "1") {
        GetChargingPileLocation();
    } else {
        var geolocation = new BMap.Geolocation();
        geolocation.getCurrentPosition(function (r) {
            if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                var latlng = $("#hiddWeiXinLocation").val();
                var ls = latlng.split(",");
                var zoom = map.getZoom();
                map.centerAndZoom(new BMap.Point(r.point.lng, r.point.lat), zoom);
                $("#hiddWeiXinLocation").val(r.point.lat + "," + r.point.lng);
                GetChargingPileLocation();
            }
            else {
                GetChargingPileLocation();
            }

        }, { enableHighAccuracy: true })
    }
    var ctrl = new BMapLib.TrafficControl({
        showPanel: false //是否显示路况提示面板
    });
    map.addControl(ctrl);
    
});

map.addEventListener("dragend", function (e) {
    var pt = map.getBounds().getCenter();
    var loaction = pt.lat + "," + pt.lng;
    $("#hiddSearchLocation").val(loaction);
    GetChargingPileLocation();
    var zoom = map.getZoom();
    map.centerAndZoom(new BMap.Point(pt.lng, pt.lat), zoom);
});
function GetChargingPileLocation() {
    map.clearOverlays();
    var sdata = {};
    var latlng = $("#hiddWeiXinLocation").val();
    var querylatlng = $("#hiddSearchLocation").val();
    if ($.trim(querylatlng) != "") {
        latlng = querylatlng;
    }
    var ls = latlng.split(",");
    sdata.lat = ls[0];
    sdata.lng = ls[1];
    sdata.r = Math.random();

    $.post("/FindParking/GeParkingLocation", sdata, function (data) {
        if (data.result) {
            var result = data.data;
            for (var i = 0; i < result.length; i++) {
//                if (i == result.length - 1) {
//                    ShowParkingDetail(result[i].id, result[i].type, result[i].lng, result[i].lat, result[i].name, result[i].address);
//                }
                MakeParkingPoint(result[i].lng, result[i].lat, result[i].name, result[i].type, result[i].id, result[i].address, result[i].quantity);
            }

        }
    });
}
function ShowParkingDetail(pointid, type, lng, lat, name, address) {
    var url = 'http://api.map.baidu.com/marker?location=' + lat + ',' + lng + '&title=' + name + '&content=' + address + '&mode=driving&output=html';
    $("#anavigation").attr("href", url);
    var sdata = {};
    var latlng = $("#hiddWeiXinLocation").val();
    var ls = latlng.split(",");
    sdata.formlat = ls[0];
    sdata.formlng = ls[1];
    sdata.tolat = lat;
    sdata.tolng = lng;
    sdata.id = pointid;
    sdata.type = type;
    sdata.city = $("#hiddCityName").val();
    sdata.r = Math.random();
    $.post("/FindParking/ParkingDetail", sdata, function (data) {
        if (data.result) {
            ShowParkingDetailBox();
            $("#imgParking").attr("src", data.data.ImageUrl);
            $("#spanParkingName").text(data.data.ParkName);
            $("#spanDistanceDes").text(data.data.DistanceDes);
            $("#spanAddress").text(data.data.ParkAddress);
            $("#spanTotalCarBitNum").text(data.data.TotalParkQuantity);
            $("#spanSpaceBitNum").text(data.data.SurplusParkQuantity);
            $("#spanParkTypeDes").text(data.data.ParkType);
            $("#spanPriceInfoDes").text(data.data.PriceInfo);
        }
    });
}
$(function () {
    $("#search_input").focus(function () {
        $("#search_cancel").show();
        $("#search_clear").show();
        $("#search_bar").addClass("weui_search_focusing");
        $("#actionsheet_cancel").click();
    });
    $("#search_input").keyup(function () {
        var value = $(this).val();
        if ($.trim(value) != "") {
            GetGetSuggestionInfo();
        } else {
            $("#search_show").hide()
        }
    });
    $("#search_input").blur(function () {
        var v = $(this).val();
        if ($.trim(v) != "") {
            return false;
        }
        $("#search_cancel").hide();
        $("#search_bar").removeClass("weui_search_focusing");
    });
    $("#search_clear").click(function () {
        $("#search_input").val("").focus();
    });
    $("#search_cancel").click(function () {
        $("#search_input").val("");
        $("#search_show").hide();
    });

})
function btnSelectQueryAddress(obj) {
    var loaction = $(obj).find("[id=hiddQueryLocation]").val();
    var name = $(obj).find("[id=pname]").text();
    $("#search_input").val(name);
    $("#hiddSearchLocation").val(loaction);
    $("#search_clear").hide();
    $("#search_cancel").hide();
    GetChargingPileLocation();
    $("#search_show").hide();
    $("#parkingmap").show();
    var result = loaction.split(',');
    var zoom = map.getZoom();
    map.centerAndZoom(new BMap.Point(result[1], result[0]), zoom);
}
function GetGetSuggestionInfo() {
    $("#search_show").find(".weui_cell").remove();
    var sdata = {};
    var latlng = $("#hiddWeiXinLocation").val();
    var ls = latlng.split(",");
    sdata.query = $("#search_input").val();
    sdata.lat = ls[0];
    sdata.lng = ls[1];
    sdata.city = $("#hiddCityName").val();
    sdata.r = Math.random();
    $.post("/FindParking/GetParkingSuggestion", sdata, function (data) {
        if (data.result) {
            var result = data.data;
            for (var i = 0; i < result.length; i++) {
                if ($("#search_show").find(".weui_cell").length >= 8) {
                    return false;
                }
                var location = result[i].location.lat + "," + result[i].location.lng;
                var str = "<div class=\"weui_cell\" onclick=\"return btnSelectQueryAddress(this)\">";
                str += "       <div class=\"weui_cell_bd weui_cell_primary\">";
                str += "           <p id=\"pname\"> " + result[i].name + "</p>";
                str += "     </div>";
                str += " <input type=\"hidden\" id=\"hiddQueryLocation\" value=\"" + location + "\" />";
                str += "</div>";

                $("#search_show").append(str);
            }
            if (result.length > 0) {
                $("#search_show").show();
            } else {
                $("#search_show").hide();

            }
        }
    });
}
function ShowParkingDetailBox() {
    var mask = $('#mask');
    var weuiActionsheet = $('#weui_actionsheet');
    weuiActionsheet.addClass('weui_actionsheet_toggle');
    mask.show().addClass('weui_fade_toggle').one('click', function () {
        hideActionSheet(weuiActionsheet, mask);
    });
    $('#actionsheet_cancel').one('click', function () {
        hideActionSheet(weuiActionsheet, mask);
    });
    weuiActionsheet.unbind('transitionend').unbind('webkitTransitionEnd');

    function hideActionSheet(weuiActionsheet, mask) {
        weuiActionsheet.removeClass('weui_actionsheet_toggle');
        mask.removeClass('weui_fade_toggle');
        weuiActionsheet.on('transitionend', function () {
            mask.hide();
        }).on('webkitTransitionEnd', function () {
            mask.hide();
        })
    }
}