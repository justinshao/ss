$(function () {
    $("#loadingToast").show();
    setTimeout("PageRedirect()", 200);
});
function PageRedirect() {
    location.href = "/R/" + $("#hiddRequestId").val();
}