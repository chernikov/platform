function EditUser() {

    var _this = this;

    this.ajaxUploadImageFile = "/Account/UploadAvatar";


    this.init = function () {
        var titlePreview = $("#UploadImage").text();
        InitUpload($("#UploadImage")[0],
            true,
            _this.ajaxUploadImageFile,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok")
                {
                    $("#ImagePreviewWrapper").show();
                    $("#AvatarPath").val(responseJSON.imagePath);
                    $("#ImagePreview").attr("src", responseJSON.imagePath);
                }
            },
            null, titlePreview);
    }

}

var editUser = null;
$().ready(function () {
    editUser = new EditUser();
    editUser.init();
});