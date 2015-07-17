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
            if (_this.checkEmail() === false | _this.checkFirstName() === false | _this.checkLastName() === false)
                return false;

            $("#modalEditPlayer").modal('hide');
            $(".modal-backdrop").remove();
            $.ajax({
                type: "POST",
                url: "/ManagePlayer/Edit",
                data: $("#PlayerForm").serialize(),
                success: function (data) {
                    $("#ModalWrapper").html(data);
                    $("#modalEditPlayer").modal();
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

    $(document).on("input", "#Email", function () {
        _this.checkEmail();
    })
    .on("input", "#FirstName", function () {
        _this.checkFirstName();
    })
    .on("input", "#LastName", function () {
        _this.checkLastName();
    });

    this.checkEmail = function () {
        var val = $("#Email").val();
        var error = "";

        if (val.length === 0)
            error = "Enter Email";
        if (val.length > 150)
            error = "The email can not exceed 150 characters"

        $("#emai-error-message").remove();
        $("#Email").parent().removeClass("has-error");
        if (error.length !== 0) {
            $("#Email").after('<div class="error" id="emai-error-message">' + error + '</div>');
            $("#Email").parent().addClass("has-error");
            return false;
        }
        return true;
    }

    this.checkFirstName = function () {
        var val = $("#FirstName").val();
        var error = "";

        if (val.length === 0)
            error = "The first name field is required";
        if (val.length > 500)
            error = "The first name can not exceed 500 characters"

        $("#firstname-error-message").remove();
        $("#FirstName").parent().removeClass("has-error");
        if (error.length !== 0) {
            $("#FirstName").after('<div class="error" id="firstname-error-message">' + error + '</div>');
            $("#FirstName").parent().addClass("has-error");
            return false;
        }
        return true;
    }

    this.checkLastName = function () {
        var val = $("#LastName").val();
        var error = "";

        if (val.length === 0)
            error = "The last name field is required";
        if (val.length > 500)
            error = "The last name can not exceed 500 characters"

        $("#lastname-error-message").remove();
        $("#LastName").parent().removeClass("has-error");
        if (error.length !== 0) {
            $("#LastName").after('<div class="error" id="lastname-error-message">' + error + '</div>');
            $("#LastName").parent().addClass("has-error");
            return false;
        }
        return true;
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
                $("#modalSendActivation").modal();
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