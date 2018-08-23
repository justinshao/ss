function btnSelectFile() {
    $("#fileupload_input").click();

}
$(function () {
    var config = $("#hiddHasConfigData").val();
    if (config == "0") {
        $.messager.alert('系统提示', '请先配置微信接口信息!', 'error');
        window.setTimeout(HrefWXApiConfig, 2000); 
        
    }
    $("#fileupload_input").fileupload({
        url: '/UploadFile/PostCretData',
        done: function (e, result) {
            if (result.result.result) {
                $("#spanUploadFileResult").text("获取文件成功").css("color", "#000000");
                $("#hiddCertPath").val(result.result.data);
            } else {
                $("#hiddCertPath").val("");
                $("#spanUploadFileResult").text(result.result.msg).css("color", "Red");
            }
        }
    })
    GetWXPaymentConfig();
});
function HrefWXApiConfig() {
    location.href = "/w/WXApiConfig/Index";
}
function GetWXPaymentConfig() {
    $.ajax({
        type: "post",
        url: '/w/WXPaymentConfig/GetWXPaymentConfigData',
        error: function () {
            $.messager.alert('系统提示', '获取配置信息失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                if (data.data != null) {
                    $("#txtPartnerKey").textbox("setValue",data.data.PartnerKey);
                    $("#txtPartnerId").textbox("setValue", data.data.PartnerId);
                    $("#txtCertPath").textbox("setValue", data.data.CertPath);
                    $("#txtCertPwd").textbox("setValue", data.data.CertPwd);
                    $("#hiddID").val(data.data.ID);
                }
            }
        }
    });
}
function btnSubmitForm() {
    var partnerKey = $("#txtPartnerKey").val();
    if ($.trim(partnerKey) == "") {
        $.messager.alert('系统提示', "商户支付密钥不能空", 'error');
        return false;
    }
    var cert = $("#hiddCertPath").val();
    if ($.trim(cert) == "") {
        $.messager.alert('系统提示', "请上传证书文件", 'error');
        return false;
    }

    var partnerId = $("#txtPartnerId").val();
    if ($.trim(partnerId) == "") {
        $.messager.alert('系统提示', "商户号不能为空", 'error');
        return false;
    }
    var pwd = $("#txtCertPwd").val();
    if ($.trim(pwd) == "") {
        $.messager.alert('系统提示', "证书文件密码不能为空", 'error');
        return false;
    }
  
    $.ajax({
        type: "post",
        url: '/w/WXPaymentConfig/AddOrUpdate',
        data: $("#wxPaymentConfigBox").serialize(),
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            GetWXPaymentConfig();
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