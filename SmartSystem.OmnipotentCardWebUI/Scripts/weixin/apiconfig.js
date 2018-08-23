
function btnSelectWeiXin() {
    $("#fileupload_weixinlogo").click();
}
function btnSelectVerification() {
    $("#fileupload_verification").click();
}
$(function () {
    $("#fileupload_weixinlogo").fileupload({
        url: '/UploadFile/PostWeiXinLogo',
        done: function (e, result) {
            if (result.result.result) {
                $("#imgLogo").show().attr("src", result.result.data);
                $("#hiddWxSystemLogo").val(result.result.data);
            } else {
                $("#imgLogo").hide();
                $("#hiddWxSystemLogo").val("");
                $.messager.alert('系统提示', result.result.msg, 'error');
            }
        }
    })
    $("#fileupload_verification").fileupload({
        url: '/UploadFile/PostWeiXinVerificationFile',
        done: function (e, result) {
            if (result.result.result) {
                $("#spanverificationresult").text("上传成功");
            } else {
                $("#spanverificationresult").text(result.result.msg);
            }
        }
    })
    GetWXApiConfig();
});
function GetWXApiConfig() {
    $.ajax({
        type: "post",
        url: '/w/WXApiConfig/GetWeiXinApiConfig',
        error: function () {
            $.messager.alert('系统提示', '获取配置信息失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                if (data.data != null) {
                    $("#txtSystemName").textbox("setValue",data.data.SystemName);
                    $("#txtAppId").textbox("setValue", data.data.AppId);
                    $("#txtToken").textbox("setValue", data.data.Token);
                    $("#txtAppSecret").textbox("setValue", data.data.AppSecret);
                    $("#txtDomain").textbox("setValue", data.data.Domain);
                    $("#txtServerIP").textbox("setValue", data.data.ServerIP);
                    $("#hiddID").val(data.data.ID);

                    if ($.trim(data.data.SystemLogo) != "") {
                        $("#imgLogo").show().attr("src", data.data.SystemLogo)
                        $("#hiddSystemLogo").val(data.data.SystemLogo);
                    } else {
                        $("#imgLogo").hide().attr("src", data.data.SystemLogo)
                        $("#hiddSystemLogo").val(data.data.SystemLogo);
                    }
                }
            }
        }
    });
}
var urlReg = /^(http|https)\:///;
var ipReg = /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/;
function btnSubmitForm() {
    var systemname = $("#txtSystemName").val();
    if ($.trim(systemname) == "") {
        $.messager.alert('系统提示', "微信公众号名称不能空", 'error');
        return false;
    }
    var logo = $("#imgLogo").attr("src");
    if ($.trim(logo) == "") {
        $.messager.alert('系统提示', "请上传微信LOGO", 'error');
        return false;
    }
    $("#hiddSystemLogo").val(logo);
    var domain = $("#txtDomain").val();
    if ($.trim(domain) == "") {
        $.messager.alert('系统提示', "微信域名不能为空", 'error');
        return false;
    }
    if (!$.trim(domain).match(urlReg)) {
        $.messager.alert('系统提示', "微信域名必须以http://或https://开头", 'error');
        return false;
    }
    var serverIP = $("#txtServerIP").val();
    if ($.trim(serverIP) == "") {
        $.messager.alert('系统提示', "微信服务器IP不能为空", 'error');
        return false;
    }
    if (!$.trim(serverIP).match(ipReg)) {
        $.messager.alert('系统提示', "微信服务器IP格式不正确", 'error');
        return false;
    }
    var appid = $("#txtAppId").val();
    if ($.trim(appid) == "") {
        $.messager.alert('系统提示', "微信开发者AppId不能为空", 'error');
        return false;
    }
    var token = $("#txtToken").val();
    if ($.trim(token) == "") {
        $.messager.alert('系统提示', "微信开发者Token不能为空", 'error');
        return false;
    }
    var appsecret = $("#txtAppSecret").val();
    if ($.trim(appsecret) == "") {
        $.messager.alert('系统提示', "微信开发者AppSecret不能为空", 'error');
        return false;
    }

    $.ajax({
        type: "post",
        url: '/w/WXApiConfig/AddOrUpdate',
        data: $("#wxApiConfigBox").serialize(),
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            GetWXApiConfig();
            if (data.result) {
                $.messager.show({
                    width: 200,
                    height: 100,
                    msg: '保存成功!',
                    title: "系统提醒"
                });
            } else {
                $.messager.progress("close");
                $.messager.alert('系统提示', data.msg, 'error');

            }
        }
    });
}