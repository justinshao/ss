function LeftMenuExpandOption(src) {
    $(".list").each(function () {
        if ($(this).attr("src") == src) {
            var sel_obj = $(this).parents(".panel").eq(0).find(".panel-header");
            if (!$(sel_obj).hasClass("accordion-header-selected")) {
                $(this).parents(".panel").eq(0).find(".panel-header").click();
            }
            return true;
        }
    });
}
$(function () {
    //顶部导航切换
    $(".nav2 li a").click(function () {
        $(".nav2 li a.selected").removeClass("selected")
        $(this).addClass("selected");
        var title = $(this).attr('title');
        var tab = $("#tbmain").tabs('getTab', title);
        var hef = $(this).attr('src');
        if (tab == null) {
            var content = createFrame(hef);
            $('#tbmain').tabs('add', {
                id: hef,
                content: content,
                title: title,
                closable: true,
                selected: true
            });


        } else {
            $("#tbmain").tabs('select', title);

        }
        LeftMenuExpandOption(hef);
        tabClose();
    })
    //            //顶部导航切换
    //            $(".nav li a").click(function () {
    //                $(".nav li a.selected").removeClass("selected")
    //                $(this).addClass("selected");
    //            })

    //导航切换
    $(".menuson .header").click(function () {
        var $parent = $(this).parent();
        $(".menuson>li.active").not($parent).removeClass("active open").find('.sub-menus').hide();

        $parent.addClass("active");
        if (!!$(this).next('.sub-menus').size()) {
            if ($parent.hasClass("open")) {
                $parent.removeClass("open").find('.sub-menus').hide();
            } else {
                $parent.addClass("open").find('.sub-menus').show();
            }
        }
    });


    $('.title').click(function () {
        var $ul = $(this).next('ul');
        $('dd').find('.menuson').slideUp();
        if ($ul.is(':visible')) {
            $(this).next('.menuson').slideUp();
        } else {
            $(this).next('.menuson').slideDown();
        }
    });

    function createFrame(url) {
        var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
        return s;
    }
    tabCloseEven();
    //顶部导航切换
    $(".list").click(function () {

        var title = $(this).attr('title');
        var tab = $("#tbmain").tabs('getTab', title);
        var hef = $(this).attr('src');
        if (tab == null) {
            var content = createFrame(hef);
            $('#tbmain').tabs('add', {
                id: hef,
                content: content,
                title: title,
                closable: true,
                selected: true
            });

        } else {
            $("#tbmain").tabs('select', title);

        }
        tabClose();
    })


})

function tabClose() {

    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();

        $('#tbmain').tabs('close', subtitle);
    })
    /*为选项卡绑定右键*/
    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children(".tabs-closable").text();

        $('#mm').data("currtab", subtitle);
        $('#tbmain').tabs('select', subtitle);
        return false;
    });
}
//绑定右键菜单事件
function tabCloseEven() {
    //            //刷新
    //            $('#mm-tabupdate').click(function () {
    //                var currTab = $('#tbmain').tabs('getSelected');
    //                var url = $(currTab.panel('options').content).attr('src');
    //                if (url != undefined ) {
    //                    $('#tbmain').tabs('update', {
    //                        tab: currTab,
    //                        options: {
    //                            content: createFrame(url)
    //                        }
    //                    })
    //                }
    //            })
    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('#tbmain').tabs('close', currtab_title);
    })
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t != '车场首页') {
                $('#tbmain').tabs('close', t);
            }
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        var nextall = $('.tabs-selected').nextAll();
        if (prevall.length > 0) {
            prevall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != '车场首页') {
                    $('#tbmain').tabs('close', t);
                }
            });
        }
        if (nextall.length > 0) {
            nextall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != '车场首页') {
                    $('#tbmain').tabs('close', t);
                }
            });
        }
        return false;
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            //msgShow('系统提示','后边没有啦~~','error');

            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tbmain').tabs('close', t);
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            //                    alert('到头了，前边没有啦~~');
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tbmain').tabs('close', t);
        });
        return false;
    });
}
function UpdateUserLoginPwd() {
    var userAccount = $("hiddCurrLoginUserName").val();
    $("#spanCurrLoginUserName").text(userAccount);

    $('#divUpdatePwdBoxForm').form('clear');

    $('#divUpdatePwdBox').show().dialog({
        title: "修改登录密码",
        width: 360,
        height: 250,
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        buttons: [{
            text: '取消',
            iconCls: 'icon-status-no',
            handler: function () {
                $('#divUpdatePwdBox').dialog('close');
            }
        }, {
            text: '确定',
            iconCls: 'icon-status-yes',
            handler: function () {
                if ($('#divUpdatePwdBoxForm').form('validate')) {
                    $.messager.progress({
                        text: '正在保存....',
                        interval: 100
                    });
                    $.ajax({
                        type: "post",
                        url: '/home/UpdateCurrLoginPwd',
                        data: $("#divUpdatePwdBoxForm").serialize(),
                        error: function () { $.messager.alert('系统提示', '提交数据到服务器失败!', 'error'); },
                        success: function (data) {
                            if (data.result) {
                                $.messager.progress("close");
                                $('#divUpdatePwdBox').dialog('close');
                                $.messager.alert('系统提示', "修改成功", 'success');
                            } else {
                                $.messager.progress("close");
                                $.messager.alert('系统提示', data.msg, 'error');

                            }
                        }
                    });
                }
            }
        }]

    });
}