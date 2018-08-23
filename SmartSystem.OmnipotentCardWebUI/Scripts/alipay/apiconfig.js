$(function () {
    $('#treeCompany').tree({
        url: '/CompanyData/GetCompanyTree',
        onSelect: function (record) {
            GetAlipayApiConfig(record.id);
            $("#hiddCompanyID").val(record.id);
        }
    });
});
function GetAlipayApiConfig(companyId) {
    $('#AliPayApiConfigBox').form("clear");
    $("#hiddCompanyID").val(companyId);
    $.messager.progress({ text: '加载中....', interval: 100 });
    $.ajax({
        type: "post",
        url: '/a/AliPayApiConfig/GetAlipayApiConfig?companyId=' + companyId,
        error: function () {
            $.messager.alert('系统提示', '获取配置信息失败!', 'error');
        },
        success: function (data) {
            $.messager.progress("close");
            if (data.result) {
                if (data.data != null) {
                    $("#hiddRecordId").val(data.data.RecordId);
                    $("#hiddCompanyID").val(data.data.CompanyID);
                    $("#txtPrivateKey").textbox("setValue", data.data.PrivateKey);
                    $("#txtPublicKey").textbox("setValue", data.data.PublicKey);
                    $("#txtPayeeAccount").textbox("setValue", data.data.PayeeAccount);
                    $("#txtAppId").textbox("setValue", data.data.AppId);
                    $("#txtSystemDomain").textbox("setValue", data.data.SystemDomain);
                    $("#txtSystemName").textbox("setValue", data.data.SystemName);
                    $("#chkStatus").prop("checked", data.data.Status);
                    $("#chkSupportSuperiorPay").prop("checked", data.data.SupportSuperiorPay);
                    $("#sltAliPaySignType").val(data.data.AliPaySignType);
                }
            }
        }
    });
}
var urlReg = /^(http|https)\:///;
function btnSubmitForm() {
    var systemname = $("#txtSystemName").val();
    if ($.trim(systemname) == "") {
        $.messager.alert('系统提示', "系统名称不能空", 'error');
        return false;
    }
    var domain = $("#txtSystemDomain").val();
    if ($.trim(domain) == "") {
        $.messager.alert('系统提示', "域名不能为空", 'error');
        return false;
    }
    if (!$.trim(domain).match(urlReg)) {
        $.messager.alert('系统提示', "域名必须以http://或https://开头", 'error');
        return false;
    }
    var appid = $("#txtAppId").val();
    if ($.trim(appid) == "") {
        $.messager.alert('系统提示', "AppId不能为空", 'error');
        return false;
    }
    var payeeAccount = $("#txtPayeeAccount").val();
    if ($.trim(payeeAccount) == "") {
        $.messager.alert('系统提示', "收款账号不能为空", 'error');
        return false;
    }
    var publicKey = $("#txtPublicKey").val();
    if ($.trim(publicKey) == "") {
        $.messager.alert('系统提示', "公钥不能为空", 'error');
        return false;
    }
    var privateKey = $("#txtPrivateKey").val();
    if ($.trim(privateKey) == "") {
        $.messager.alert('系统提示', "私钥不能为空", 'error');
        return false;
    }
    $.ajax({
        type: "post",
        url: '/a/AliPayApiConfig/AddOrUpdate',
        data: $("#AliPayApiConfigBox").serialize(),
        error: function () {
            $.messager.progress("close");
            $.messager.alert('系统提示', '提交数据到服务器失败!', 'error');
        },
        success: function (data) {
            if (data.result) {
                $("#hiddRecordId").val(data.data.toString());
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