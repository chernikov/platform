function Price() {

    var _this = this;

    this.ajaxUploadImage = "/admin/File/UploadPriceImage";

    this.init = function () {
        InitUpload(
            $("#ChangeTeamPriceImage")[0],
            false,
            _this.ajaxUploadImage,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#PreviewTeamPriceImage").attr("src", responseJSON.data);
                    $("#TeamPriceImagePath").val(responseJSON.data);
                }
            },
            [],
            $("#ChangeTeamPriceImage").text()
        );

        InitUpload(
            $("#ChangeIndividualPriceImage")[0],
            false,
            _this.ajaxUploadImage,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#PreviewIndividualPriceImage").attr("src", responseJSON.data);
                    $("#IndividualPriceImagePath").val(responseJSON.data);
                }
            },
            [],
            $("#ChangeIndividualPriceImage").text());
    };

    this.ChangePreview = function (previewPath) {

    }
}

var price = null;
$().ready(function () {
    price = new Price();
    price.init();
});