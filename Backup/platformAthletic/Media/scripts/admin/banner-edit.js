function BannerEdit() {
    var _this = this;

    this.ajaxUploadNewFile = "/admin/File/UploadBannerSource";
    this.ajaxMakePreview = "/admin/Banner/MakePreview";

    this.init = function () {
        var titlePreview = $("#UploadImage").text();
        InitUpload($("#UploadImage")[0],
            false,
            _this.ajaxUploadNewFile,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#SourcePath").val(responseJSON.data);
                    _this.MakeImage();
                }
            },
            null, titlePreview);

        $("#BannerPlaceID").change(function () {
            if ($("#SourcePath").val() != "") {
                _this.MakeImage();
            }
        });

        if ($("#SourcePath").val() != "") {
            _this.MakeImage();
        }
    }

    this.MakeImage = function () {
        var ajaxData = {
            sourcePath: $("#SourcePath").val(),
            bannerPlaceID: $("#BannerPlaceID").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxMakePreview,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    $("#ImagePreviewWrapper").show();
                    $("#ImagePath").val(data.data);
                    $("#ImagePreview").attr("src", data.data);
                }
            }

        });
    }

}

var bannerEdit;
$().ready(function () {
    bannerEdit = new BannerEdit();
    bannerEdit.init();
});