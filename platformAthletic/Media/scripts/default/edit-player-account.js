function EditPlayerAccount() {
    var _this = this;

    this.ajaxUploadLogo = "/Account/UploadAvatar";

    this.init = function () {
        $('.left .menu').css('height', $('.account-basic-wrp').height());


        var titlePreview = $("#UploadAvatar").text();
        InitUpload($("#UploadAvatar")[0],
            false,
            _this.ajaxUploadLogo,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#AvatarPath").val(responseJSON.imagePath);
                    $("#AvatarImgWrapper").html("<img src='" + responseJSON.imagePath + "'/>");
                } else {
                    alert(responseJSON.error);
                }
            },
            null, titlePreview);
    };
}

var editPlayerAccount;
$().ready(function () {
    editPlayerAccount = new EditPlayerAccount();
    editPlayerAccount.init();
});
