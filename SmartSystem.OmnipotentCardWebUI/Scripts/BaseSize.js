    window.onresize = function () {
        calcRam()
    }
        function calcRam() {
            var html = document.querySelector("html");
            var w = document.documentElement.clientWidth;
            // 字体大小设置为12px一下，有些浏览器会转换为12px
            html.style.fontSize = w / 37.5 + "px";
        }
    calcRam()
