function EquipmentEdit() {
    var _this = this;

    this.ajaxUploadImageFile = "/admin/File/UploadEquipment";


    this.init = function () {
        var titlePreview = $("#UploadImage").text();
        InitUpload($("#UploadImage")[0],
            true,
            _this.ajaxUploadImageFile,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#ImagePreviewWrapper").show();
                    $("#ImagePath").val(responseJSON.data);
                    $("#ImagePreview").attr("src", responseJSON.data);
                }
            },
            null, titlePreview);
    }
}

var equipmentEdit;
$().ready(function () {
    equipmentEdit = new EquipmentEdit();
    equipmentEdit.init();
});