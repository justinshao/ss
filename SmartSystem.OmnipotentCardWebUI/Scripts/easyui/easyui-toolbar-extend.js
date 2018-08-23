$(function () {
    $.extend($.fn.datagrid.methods, {
        addToolbarItem: function (jq, items) {
            return jq.each(function () {
                var dpanel = $(this).datagrid('getPanel');

                var toolbar = dpanel.children("div.datagrid-toolbar");
                if (toolbar.length > 0) {
                    return;
                }
                if (!toolbar.length) {
                    toolbar = $("<div class=\"datagrid-toolbar\"><table cellspacing=\"0\" cellpadding=\"0\"><tr></tr></table></div>").prependTo(dpanel);
                    $(this).datagrid('resize');
                }
                var tr = toolbar.find("tr");
                for (var i = 0; i < items.length; i++) {
                    var btn = items[i];
                    if (btn == "-") {
                        $("<td><div class=\"datagrid-btn-separator\"></div></td>").appendTo(tr);
                    } else {
                        var td = $("<td></td>").appendTo(tr);
                        var b = $("<a href=\"javascript:void(0)\"></a>").appendTo(td);
                        b[0].onclick = eval(btn.handler || function () { });
                        b.linkbutton($.extend({}, btn, {
                            plain: true
                        }));
                    }
                }

            });

        },
        removeToolbarItem: function (jq, param) {
            return jq.each(function () {
                var dpanel = $(this).datagrid('getPanel');
                var toolbar = dpanel.children("div.datagrid-toolbar");
                var cbtn = null;
                if (typeof param == "number") {
                    cbtn = toolbar.find("td").eq(param).find('span.l-btn-text');
                } else if (typeof param == "string") {
                    cbtn = toolbar.find("span.l-btn-text:contains('" + param + "')");
                }
                if (cbtn && cbtn.length > 0) {
                    cbtn.closest('td').remove();
                    cbtn = null;
                }
            });
        }
    });
});