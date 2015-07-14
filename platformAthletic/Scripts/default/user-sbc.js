function UserSbc() {
    var _this = this;

    this.callback = function () {
        window.location.reload();
    }
    this.init = function () {

        $(document).on("click", "#UserSbcSaveBtn", function () {
            $.ajax({
                type: "POST",
                url: "/user/UserSbc",
                data: {
                    id: $("#UserSbcID").val(),
                    squat: $("#UserSbcSquat").val(),
                    bench: $("#UserSbcBench").val(),
                    clean: $("#UserSbcClean").val(),
                },
                success: function (data) {
                    if (data.result = "ok") {
                        $("#modalUserSbc").modal("hide");
                        _this.callback();
                    } else {
                        $.each(data.errors, function (i, item) {
                            if (item == "squat") {
                                $("#UserSbcSquat").addClass("field-input-error");
                            }
                            if (item == "bench") {
                                $("#UserSbcBench").addClass("field-input-error");
                            }
                            if (item == "clean") {
                                $("#UserSbcClean").addClass("field-input-error");
                            }
                        })
                    }

                }
            });
            return false;
        });
    }

    this.showModal = function (id, callback) {
        if (callback != null) {
            _this.callback = callback;
        }
        $.ajax({
            type: "GET",
            url: "/user/UserSbc",
            data: {
                id: id
            },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalUserSbc").modal();
                $("#modalUserSbc .form-control").mask("000");
                $("#modalUserSbc .form-control").blur(function () {
                    var val = $(this).val();
                    if (val % 5 != 0) {
                        val = parseInt(val / 5) * 5;
                        $(this).val(val);
                    }
                })
            }
        });
    }
}

var userSbc;
$(function () {
    userSbc = new UserSbc();
    userSbc.init();
});