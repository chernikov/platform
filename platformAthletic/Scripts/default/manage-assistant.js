function ManageAssistant() {
    var _this = this;

    this.init = function () {
        $("#Create").click(function ()
        {
            if (typeof (todo) == "undefinded") {
                _this.showCreatePopup();
            }
        });


        $(".editUser").click(function () {
            _this.showEditPopup($(this).data("id"));
        });

        $(".removeUser").click(function () {
            _this.showRemovePopup($(this).data("id"));
        });
        $(document).on("click", "#SubmitEditAssistant", function () {
            $.ajax({
                type: "POST",
                url: "/ManageAssistant/Edit",
                data: $("#AssistantForm").serialize(),
                success: function (data) {
                    $("#ModalEditAssistantWrapper").html(data);
                }
            })
        });

        $(document).on("click", "#SubmitRemoveAssistant", function () {
            $.ajax({
                type: "POST",
                url: "/ManageAssistant/Delete",
                data: $("#AssistantForm").serialize(),
                success: function (data)
                {
                    window.location.reload();
                }
            })
        });
    }

    this.showCreatePopup = function ()
    {
        $.ajax({
            type : "GET",
            url: "/ManageAssistant/Create",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalEditAssistant").modal();
                _this.onEdit();
            }
        });
    }

    this.showEditPopup = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManageAssistant/Edit",
            data : {id : id},
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalEditAssistant").modal();
                _this.onEdit();
            }
        });
    }

    this.showRemovePopup = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManageAssistant/Delete",
            data: { id: id },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalRemoveAssistant").modal();
            }
        });
    }


    this.onEdit = function ()
    {
        var obj = new qq.FineUploader({
            element: $("#UploadImage")[0],
            multiple: false,
            request: {
                endpoint: "/User/UploadFile",
            },
            text: {
                uploadButton: "Upload"
            },
            callbacks: {
                onComplete: function (id, fileName, responseJSON) {
                    if (responseJSON.success) {
                        $("#AvatarPath").val(responseJSON.fileUrl);
                        $("#ImagePreviewWrapper").show();
                        $("#ImagePreview").attr("src", responseJSON.fileUrl + "?w=200&h=200&mode=crop&scale=both");
                        _this.updateEdit();
                    }
                }
            },
            validation: {
                allowedExtensions: ["jpeg", "png", "jpg"]
            }
        });
    }
}

var manageAssistant = null;

$(function () {
    manageAssistant = new ManageAssistant();
    manageAssistant.init();
});