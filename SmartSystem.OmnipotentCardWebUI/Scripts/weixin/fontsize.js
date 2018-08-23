window.onresize = function () {
  calcRam()
}
function calcRam() {
  var html = document.querySelector("html");
  var w = document.documentElement.clientWidth;
  // 字体大小设置为10px一下，谷歌浏览器会转换为12px
  html.style.fontSize = w / 37.5 + "px";
}
calcRam()


//alert去除网址
var wAlert = window.alert;
window.alert = function (message) {
  try {
    var iframe = document.createElement("IFRAME");
    iframe.style.display = "none";
    // iframe.setAttribute("src", 'data:text/plain,');
    document.documentElement.appendChild(iframe);
    var alertFrame = window.frames[0];
    var iwindow = alertFrame.window;
    if (iwindow == undefined) {
      iwindow = alertFrame.contentWindow;
    }
    iwindow.alert(message);
    iframe.parentNode.removeChild(iframe);
  } catch (exc) {
    return wAlert(message);
  }
}
