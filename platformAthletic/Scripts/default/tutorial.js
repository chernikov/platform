function Tutorial() {
    var _this = this;

    this.init = function ()
    {
        _this.showTutorial(0);
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

        $(document).on("click", "#SubmitAddPlayerInfoBtn", function () {
            _this.addPlayerInfo();
        });
       
        $(document).on("click", "#TermAndConditionChk", function () {
            if ($("#TermAndConditionChk:checked").length > 0)
            {
                $(".nextBtn").removeAttr("disabled");
            } else {
                $(".nextBtn").attr("disabled", "disabled");
            }
        });
    }

    this.showTutorial = function ()
    {
        $.ajax({
            type: "GET",
            cache: false,
            url: "/Tutorial",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalTutorial").modal({
                    backdrop: 'static',
                    keyboard: false
                });
                if ($("#TermAndConditionChk").length > 0) {
                    $(".nextBtn").attr("disabled", "disabled");
                }
            }
        })
    }

    this.stepOn = function (id) {
        
        $("#modalTutorial").modal('hide');
        $(".modal-backdrop").remove();
        common.clearOnBoarding();
        $.ajax({
            type: "GET",
            cache: false,
            url: "/Tutorial/Step",
            data: { id: id },
            success: function (data)
            {
                _this.showTutorial();
            },
            async : false
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

    this.updateAddPlayerInfo = function () {
        var data = $("#AddPlayerUserInfoForm").serialize();
        $.ajax({
            type: "POST",
            url: "/User/UpdatePlayerFormUserInfo",
            data: data,
            success: function (data) {
                $("#AddPlayerInfoWrapper").html(data);
            }
        });
    }

    this.addPlayerInfo = function () {
        var data = $("#AddPlayerUserInfoForm").serialize();
        $.ajax({
            type: "POST",
            url: "/User/AddPlayerUserInfo",
            data: data,
            success: function (data) {
                $("#AddPlayerInfoWrapper").html(data);
            }
        });
    }
}

var tutorial = null;
$(function () {

    tutorial = new Tutorial();
    tutorial.init();
});