
// 初始化正文编辑插件
// 初始化百度编辑器
var editor = new UE.ui.Editor({
    autoHeightEnabled: true, //高度自动增长
    pasteplain: true, //纯文本粘贴
    minFrameHeight: 500, //最小高度
    maximumWords: 20000,
    toolbars: [
            ['bold', 'italic', 'underline', '|', 'insertorderedlist', 'insertunorderedlist', '|', 'insertimage', '|', 'removeformat', 'autotypeset'],
            ['forecolor', 'backcolor', 'fontsize', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']
        ]
});
$(function () {

    editor.render('frm_text');
    // 百度编辑器内容变化事件监听
    editor.addListener('contentchange', function () {
        $("#frm_text").html(editor.getContent());
        $(".activeitem").find(".mtext").val(editor.getContent());
    });
    // 点击添加图文
    $(".appmsg_add_inner").click(function () {
        if ($(".appmsg_item").length >= 8) {
            $.messager.alert("系统提示", "你最多只可以加入8条图文消息!");
            return;
        }
        else {
            var appmsgmodel = $(".appmsg_model").clone(true);
            appmsgmodel.removeClass("appmsg_model");
            appmsgmodel.addClass("appmsg_item");
            if ($(".appmsg_item").length > 0) {
                appmsgmodel.addClass("appmsg_simple");

                $(".appmsg_item").find(".appmsg_desc").hide();
            } else {
                appmsgmodel.addClass("appmsg_first");

            }
            $(".appmsg_content").append(appmsgmodel);
            appmsgnumchange();
        }
    });
    // 点击删除图文
    $(".appmsg_edit_mask").find("span:eq(1)").click(function () {
        var appmsgitem = $(this).closest(".appmsg_item");
        $.messager.confirm('系统提示', '是否确定删除当前图文[删除后点击下方“保存”按钮进行保存]？',
            function (r) {
                if (r) {
                    if (appmsgitem.hasClass("appmsg_first")) {
                        var nextfirst = appmsgitem.next(".appmsg_item");
                        if (nextfirst.length > 0) {
                            nextfirst.removeClass("appmsg_simple");
                            nextfirst.addClass("appmsg_first");
                        }
                    }
                    var delid = appmsgitem.find(".mid").val();
                    if (delid != 0) {
                        $("#delgids").val($("#delgids").val() + "," + delid);
                    }
                    appmsgitem.remove();
                    appmsgnumchange();
                    loadediteappmsg(0);
                }
            });
    });

    // 点击修改图文
    $(".appmsg_edit_mask").find("span:eq(0)").click(function () {
        var appmsgitemindex = $(this).closest(".appmsg_item").prevAll(".appmsg_item").length;
      
        loadediteappmsg(appmsgitemindex);
    });
    var groupId = $("#hiddGroupId").val();
    GetArticle(groupId);

});
function GetArticle(groupId) {
    $.ajax({
        url: '/w/WXArticle/GetArticle',
        data: "groupId=" + groupId + "",
        type: 'post',
        success: function (data) {
            if (data.result) {

                if (data.data.length == 0) {
                    $(".appmsg_add_inner").click();
                } else {
                    for (var i = 0; i < data.data.length; i++) {
                        var appmsgmodel = $(".appmsg_model").clone(true);
                        appmsgmodel.removeClass("appmsg_model");
                        appmsgmodel.addClass("appmsg_item");

                        if ($(".appmsg_item").length > 0) {
                            appmsgmodel.addClass("appmsg_simple");

                            $(".appmsg_item").find(".appmsg_desc").hide();
                        } else {
                            appmsgmodel.addClass("appmsg_first");

                        }
                        appmsgmodel.find(".mid").val(data.data[i].ID);
                        appmsgmodel.find(".mtitle").val(data.data[i].Title);
                        appmsgmodel.find(".mdesc").val(data.data[i].Description);
                        appmsgmodel.find(".mtext").val(data.data[i].Text);
                        var type = data.data[i].ArticleType.toString();
                        appmsgmodel.find(".mtype").val(type);
                        if (type == "0") {
                            appmsgmodel.find(".murl").val("");
                            appmsgmodel.find(".mmoduleid").val("");
                        }
                        if (type == "1") {
                            appmsgmodel.find(".murl").val(data.data[i].Url);
                            appmsgmodel.find(".mmoduleid").val("");
                        }
                        if (type == "2") {
                            appmsgmodel.find(".murl").val("");
                            appmsgmodel.find(".mmoduleid").val(data.data[i].Url);
                        }
                        appmsgmodel.find(".msort").val(data.data[i].Sort);

                        if (!appmsgmodel.hasClass("has_thumb"))
                            appmsgmodel.addClass("has_thumb");
                        appmsgmodel.find(".appmsg_thumb").attr("src", data.data[i].ImagePath);
                        appmsgmodel.find(".mpic").val(data.data[i].ImagePath);

                        appmsgmodel.find(".mgroupid").val(data.data[i].GroupID);

                        appmsgmodel.find(".appmsg_title span").text(data.data[i].Title);
                        appmsgmodel.find(".mtitle").val(data.data[i].Title);

                        appmsgmodel.find(".appmsg_desc").text(data.data[i].Title);



                        $(".appmsg_content").append(appmsgmodel);
                        appmsgnumchange();
                    }
                }
                loadediteappmsg(0);
            } else {
                $.messager.alert("系统提示", data.msg, "error");
                return;
            }
        }
    });
}
// 图文数改变
function appmsgnumchange() {
    if ($(".appmsg_item").length <= 1) {
        $(".appmsg").addClass("multi");
        $(".appmsg_edit_mask").find("span").eq(1).hide();
    } else {
        if (!$(".appmsg").hasClass("multi")) {
            $(".appmsg").addClass("multi");
        }
        $(".appmsg_edit_mask").find("span").eq(1).show();
    }
}
function btnSelectArticleCover() {
    $("#fileUpload_ArticleCover").click();
}
$(function () {
    $("#fileUpload_ArticleCover").fileupload({
        url: '/UploadFile/PostWeiXinLogo',
        done: function (e, result) {
            if (result.result.result) {
                $("#imgArticleCover").show().attr("src", result.result.data);
                $("#hiddImagePath").val(result.result.data);
                doshowforpic(result.result.data);
            } else {
                $("#imgArticleCover").hide();
                $("#hiddImagePath").val("");
                $.messager.alert('系统提示', result.result.msg, 'error');
            }
        }
    })
    BindComboboxData();
    BindUrlEvent();
    BindDescriptionEvent();
    BindTitleEvent();

});
function BindComboboxData() {
    $('#cmbArticleType').combobox({
        url: '/w/WXArticle/GetArticleTypeData',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            EditShowItem();
            if (record.EnumValue == "2") {
                var moduleId = $('#cmdModule').combobox("getValue");
                $(".activeitem").find(".mmoduleid").val(moduleId);
            }
        },
        onLoadSuccess: function () {
            EditShowItem();
        }
    });
    $('#cmdModule').combobox({
        url: '/w/WXArticle/GetWeiXinModuleData',
        valueField: 'EnumValue',
        textField: 'Description',
        onSelect: function (record) {
            $(".activeitem").find(".mmoduleid").val(record.EnumValue);
        }
    });
}
function BindUrlEvent() {
    $('#txtUrl').textbox('textbox').keyup(function (e) {
        if ($(this).val().length > 200) {
            $(this).val($(this).val().substring(0, 200));
            $.messager.alert('系统提示', "字数不能超过200个字", 'error');
        }
        else {
            $(".activeitem").find(".murl").val($(this).val());
        }
    });
    $('#txtUrl').textbox('textbox').blur(function (e) {
        if ($(this).val().length > 200) {
            $(this).val($(this).val().substring(0, 200));
            $.messager.alert('系统提示', "字数不能超过200个字", 'error');
        }
        else {
            $(".activeitem").find(".murl").val($(this).val());
        }
    });
}
function BindDescriptionEvent() {

    $('#txtDescription').textbox('textbox').keyup(function (e) {
        if ($(this).val().length > 54) {
            $(this).val($(this).val().substring(0, 54));
            $.messager.alert('系统提示', "字数不能超过54个字", 'error');
        }
        else {
            $(".activeitem").find(".mdesc").val($(this).val());
        }
    });
    $('#txtDescription').textbox('textbox').blur(function (e) {
        if ($(this).val().length > 54) {
            $(this).val($(this).val().substring(0, 54));
            $.messager.alert('系统提示', "字数不能超过54个字", 'error');
        }
        else {
            $(".activeitem").find(".mdesc").val($(this).val());
        }
    });
}
function BindTitleEvent() {
    $('#txtTitle').textbox('textbox').keyup(function () {
        if ($(this).val().length > 64) {
            $(this).val($(this).val().substring(0, 64));
            $.messager.alert('系统提示', "字数不能超过64个字", 'error');
        }
        else {
            $(".activeitem .appmsg_title span").text($(this).val());
            $(".activeitem .mtitle").val($(this).val());
        }
    });
    $('#txtTitle').textbox('textbox').blur(function () {
        if ($(this).val().length > 64) {
            $(this).val($(this).val().substring(0, 64));
            $.messager.alert('系统提示', "字数不能超过64个字", 'error');
        }
        else {
            $(".activeitem .appmsg_title span").text($(this).val());
            $(".activeitem .mtitle").val($(this).val());
        }
    });
}
// 设置图片
function doshowforpic(picurl) {
    if ($.trim(picurl) != "") {
        var pimg = $(".upload_preview").find("img");
        if (pimg.length <= 0) {
            pimg = $("<img></img>");
        }
        pimg.attr("src", picurl);
        if (!$(".activeitem").hasClass("has_thumb"))
            $(".activeitem").addClass("has_thumb");
        $(".activeitem .appmsg_thumb").attr("src", picurl);
        $(".activeitem").find(".mpic").val(picurl);
        $(".upload_preview").prepend(pimg);
        $(".upload_preview").show();
    } else {
        doremoveforpic();
    }
}
// 移除图片
function doremoveforpic() {
    $("#deletepreviewimg").closest(".upload_preview").find("img").remove();
    $(".upload_preview").hide();
    $(".activeitem").find(".mpic").val("");
    $(".activeitem").removeClass("has_thumb");
}
function EditShowItem() {
    var type = $('#cmbArticleType').combobox("getValue");
    $(".activeitem").find(".mtype").val(type);
    switch (type.toString()) {
        case "0":
            {
                $("#divFeatures").hide();
                $("#divUrl").hide();
                $("#divText").show();
                break;
            }
        case "1":
            {
                $("#divFeatures").hide();
                $("#divUrl").show();
                $("#divText").hide();
                break;
            }
        case "2":
            {
                $("#divFeatures").show();
                $("#divUrl").hide();
                $("#divText").hide();
                break;
            }
    }
}
// 载入图文
function loadediteappmsg(itemindex) {
    $(".activeitem").removeClass("activeitem");
    // 判断是否存在
    var loaditem = $(".appmsg_content").find(".appmsg_item:eq(" + itemindex + ")");
    if (loaditem.length <= 0) {
        $(".appmsg_add_inner").click();
        return;
    } else {
        $(".media_edit_area").show();
        loaditem.addClass("activeitem");
    }
    var pimg = $(".upload_preview").find("img");
    if (pimg.length <= 0) {
        pimg = $("<img></img>");
        $(".upload_preview").prepend(pimg);
    }
    // 设置位置
    var marginpx = 0;
    if (itemindex > 0) {
        $("#spanImageDes").html("小图片建议尺寸：200像素 * 200像素");
        $(".upload_preview").find("img").css("max-height", "100px").css("max-width", "100px");
    } else {
        $("#spanImageDes").html("大图片建议尺寸：360像素 * 200像素");
        $(".upload_preview").find("img").css("max-height", "100px").css("max-width", "180px");
    }
   
    BindArticleEditItem();
}
// 载入图文数据
function BindArticleEditItem() {
    var loaditem = $(".activeitem");
    $("#hiddEditId").val(loaditem.find(".mid").val());
    $("#txtTitle").textbox("setValue", loaditem.find(".mtitle").val());
    $("#hiddEditSort").val(loaditem.find(".msort").val());
    doshowforpic(loaditem.find(".mpic").val());
    $("#txtDescription").textbox("setValue", loaditem.find(".mdesc").val());
    // 类型
    var type = loaditem.find(".mtype").val();
    $("#cmbArticleType").combobox("setValue", type);

    EditShowItem();
    // 正文

    $("#txtUrl").textbox("setValue", loaditem.find(".murl").val());
    $("#txtDescription").textbox("setValue", loaditem.find(".mdesc").val());
    $("#cmdModule").combobox("setValue", loaditem.find(".mmoduleid").val());
    var becontent = loaditem.find(".mtext").val();
    $("#frm_text").html(becontent);
    editor.setContent(becontent);
}
function btnSubmitArticleData() {

    var pdata = {};
    pdata.groupId = $("#hiddGroupId").val();
    var jsonData = [];
    var itemnum = 0;
    $(".appmsg_item").each(function () {
        var item = {};
        item.id = $(this).find(".mid").val();
        item.title = $(this).find(".mtitle").val();
        item.desc = $(this).find(".mdesc").val();
        item.url = $(this).find(".murl").val();
        item.pic = $(this).find(".mpic").val();
        item.text = $(this).find(".mtext").val();
        item.moduleid = $(this).find(".mmoduleid").val();
        item.type = $(this).find(".mtype").val();
        item.sort = itemnum;
        jsonData.push(item);
        itemnum++;
    });
    if (itemnum == 0) {
        $.messager.alert('系统提示', "请添加图文", 'error');
        return;
    }
    pdata.itemnum = itemnum;
    pdata.jsonData = JSON.stringify(jsonData);

    $.post('/w/WXArticle/SaveArticle', pdata, function (data) {
        if (data.result) {
            $.messager.alert('系统提示', "保存成功", 'success');
        } else {
            $.messager.alert('系统提示', data.msg, 'error');

        }
    });

}