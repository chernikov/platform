function ManagePlayer() {
    var _this = this;

    this.init = function () {
        $("#Create").click(function () {
            _this.showCreatePopup();
        });

        $(".editUser").click(function () {
            _this.showEditPopup($(this).data("id"));
        });


        $(".sendActivation").click(function () {
            var id = $(this).data("id");
            _this.sendActivation(id);
        });

        $(".removeUser").click(function () {
            _this.showRemovePopup($(this).data("id"));
        });
        $(document).on("click", "#SubmitEditPlayer", function () {
            $.ajax({
                type: "POST",
                url: "/ManagePlayer/Edit",
                data: $("#PlayerForm").serialize(),
                success: function (data) {
                    $("#ModalEditPlayerWrapper").html(data);
                }
            })
        });

        $(document).on("click", "#SubmitRemovePlayer", function () {
            $.ajax({
                type: "POST",
                url: "/ManagePlayer/Delete",
                data: $("#PlayerForm").serialize(),
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
            url: "/ManagePlayer/Create",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalEditPlayer").modal();
                _this.onEdit();
            }
        });
    }

    this.showEditPopup = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManagePlayer/Edit",
            data : {id : id},
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalEditPlayer").modal();
                _this.onEdit();
            }
        });
    }

    this.showRemovePopup = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManagePlayer/Delete",
            data: { id: id },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalRemovePlayer").modal();
            }
        });
    }

    this.sendActivation = function (id) {
        $.ajax({
            type: "GET",
            url: "/ManagePlayer/SendActivation",
            data: { id: id },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalActivationPlayer").modal();
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

var managePlayer = null;

$(function () {
    managePlayer = new ManagePlayer();
    managePlayer.init();
});