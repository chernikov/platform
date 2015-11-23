function ManageAssistant() {
    var _this = this;

    this.init = function () {
        $("#Create").click(function ()
        {
            if (typeof (testmode) == "undefined") {
                _this.showCreatePopup();
            }
        });


        $(".editUser").click(function () {
            _this.showEditPopup($(this).data("id"));
        });

        $(".removeUser").click(function () {
            _this.showRemovePopup($(this).data("id"));
        });

        $(".sendActivation").click(function () {
            var id = $(this).data("id");
            _this.sendActivation(id);
        });

        $(document).on("click", "#SubmitEditAssistant", function () {
            $.ajax({
                type: "POST",
                url: "/ManageAssistant/Edit",
                data: $("#AssistantForm").serialize(),
                success: function (data) {
                    $("#ModalEditAssistantWrapper").html(data);
                    _this.onEdit();
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
                ga('send', 'show_modal', 'modal', 'show', 'create_coach_modal', 1);
                hj('show_create_coach_modal');
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
                ga('send', 'show_modal', 'modal', 'show', 'edit_coach_modal', 1);
                hj('show_edit_coach_modal');
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
                ga('send', 'show_modal', 'modal', 'show', 'remove_coach_modal', 1);
                hj('show_remove_coach_modal');
            }
        });
    }


    this.sendActivation = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManageAssistant/SendActivation",
            data: { id: id },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalSendActivation").modal();
            }
        });
    }

    this.onEdit = function ()
    {
        if ($("#UploadImage").length > 0) {
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
}

var manageAssistant = null;

$(function () {
    manageAssistant = new ManageAssistant();
    manageAssistant.init();
});