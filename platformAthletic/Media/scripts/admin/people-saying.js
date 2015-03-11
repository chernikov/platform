function PeopleSaying() {
    var _this = this;

    this.ajaxUploadImageFile = "/admin/File/UploadFile";


    this.init = function () {
        var titlePreview = $("#UploadImage").text();
        InitUpload($("#UploadImage")[0],
            true,
            _this.ajaxUploadImageFile,
            function (id, fileName, responseJSON) {
                if (responseJSON.success) {
                    $("#ImagePreviewWrapper").show();
                    $("#ImagePath").val(responseJSON.fileUrl);
                    $("#ImagePreview").attr("src", responseJSON.fileUrl + ".ashx?w=135&h=135&mode=crop");
                }
            },
            null, titlePreview);
    }
}

var peopleSaying;
$().ready(function () {
    peopleSaying = new PeopleSaying();
    peopleSaying.init();
});