
function ParkingOverlay(point_lng, point_lat, text, type, pointid, address, quantity) {
    
    this._point = new BMap.Point(point_lng, point_lat);
    this._text = text;
    this._type = type;
    this._pointid = pointid;
    this._point_lng = point_lng;
    this._point_lat = point_lat;
    this._address = address;
    this._quantity = quantity;
    this._point_lng = point_lng;
    this._point_lat = point_lat;
}
ParkingOverlay.prototype = new BMap.Overlay();

ParkingOverlay.prototype.initialize = function (map) {
    this._map = map;
    var div = this._div = document.createElement("div");
    var ids = "map-1_" + this._pointid;
    div.setAttribute("id", ids);
    div.style.position = "absolute";
    div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
    div.style.height = "32px";
    div.style.padding = "2px 8px 0px 30px";
    div.style.cursor = "pointer";
    div.style.lineHeight = "32px";
    div.style.borderRadius = '32px';
    div.style.whiteSpace = "nowrap";
    div.style.MozUserSelect = "none";
    div.style.fontSize = "17px";

    var that = this;
    var arrow = this._arrow = document.createElement("div");
    var cids = "map-2_" + this._pointid;
    arrow.setAttribute("id", cids);
    if (this._type == 1) {
        arrow.style.background = "url(/Content/mobile/images/parking_lan.png)";
    } else {
        if (this._quantity < 10) {
            arrow.style.background = "url(/Content/mobile/images/parking_hong.png)";
            var span = this._span = document.createElement("span");
            span.style.zIndex = 999999;
            span.style.position = "absolute";
            span.style.margin = "0px 0px 0px -28px";
            span.style.color = "#d81e06";
            span.style.fontWeight = "bold";
            div.appendChild(span);
            //显示剩余车位数
            span.appendChild(document.createTextNode(this._quantity));
        } else {
            arrow.style.background = "url(/Content/mobile/images/parking_lv.png)";
        }
    }
    arrow.style.position = "absolute";
    arrow.style.width = "32px";
    arrow.style.height = "32px";
    arrow.style.top = "7px";
    arrow.style.left = "-10px";
    arrow.style.overflow = "auto";
    div.appendChild(arrow);

    map.getPanes().labelPane.appendChild(div);
    return div;
}
ParkingOverlay.prototype.draw = function () {
    var map = this._map;
    var pixel = map.pointToOverlayPixel(this._point);
    this._div.style.left = pixel.x - parseInt(this._arrow.style.left)-15 + "px";
    this._div.style.top = pixel.y - 40 + "px";
}
//6、自定义覆盖物添加事件方法
ParkingOverlay.prototype.addEventListener = function (event, fun) {
    this._div['on' + event] = fun;
}

function MakeParkingPoint(lng, lat, text, type, pointid, address, quantity) {
    var myCompOverlay = new ParkingOverlay(lng, lat, text, type, pointid, address, quantity);
    map.addOverlay(myCompOverlay);
    myCompOverlay.addEventListener('touchstart', function (e) {
        ShowParkingDetail(pointid, type, lng, lat, text, address);
    });
}