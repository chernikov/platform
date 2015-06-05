function Tutorial() {
    var _this = this;

    this.init = function () {
        _this.showTutorial();
        $(document).on("click", ".nextBtn", function () {
            var id = $(this).data("step");
            if (id == "end") {
                _this.endTutorial();
            } else {
                _this.stepOn(id);
            }
        });


        $(document).on("click", "#SubmitAddInfoBtn", function () {
            _this.addInfo();
        });
    }

    this.showTutorial = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Tutorial",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
        })
    }

    this.stepOn = function (id) {
        $("#modalTutorial").modal('hide');
        $(".modal-backdrop").remove();
        $(".tutorial-highlight > *").unwrap();
        $(".tutorial-manipulate").removeClass("tutorial-manipulate");
        $.ajax({
            type: "GET",
            url: "/Tutorial/Step",
            data: { id: id },
            success: function (data)
            {
                _this.showTutorial();
            }
        })
    }

    this.endTutorial = function () {
        $("#modalTutorial").modal('hide');
        $(".modal-backdrop").remove();
        $.ajax({
            type: "GET",
            url: "/Tutorial/EndTutorial",
            success: function (data) {
                window.location.reload();
            }
        })
    }

    this.updateAddInfo = function () {
        var data = $("#AddUserInfoForm").serialize();
        $.ajax({
            type: "POST",
            url: "/User/UpdateFormUserInfo",
            data: data,
            success: function (data) {
                $("#AddInfoWrapper").html(data);
            }
        });
    }

    this.addInfo = function () {
        var data = $("#AddUserInfoForm").serialize();
        $.ajax({
            type: "POST",
            url: "/User/AddUserInfo",
            data: data,
            success: function (data) {
                $("#AddInfoWrapper").html(data);
            }
        });
    }
}

var tutorial = null;
$(function () {

    tutorial = new Tutorial();
    tutorial.init();
});