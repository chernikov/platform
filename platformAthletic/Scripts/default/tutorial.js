function Tutorial() {
    var _this = this;

    this.init = function ()
    {
        _this.showTutorial(0);
        $(document).on("click", ".nextBtn, .backBtn", function () {
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
        $('.summary-message-errors').hide();
        if (_this.checkDateValid() === false | _this.checkSportId() === false |
            _this.checkPositionId() === false | _this.checkGradYear() === false) {
            $('.summary-message-errors').show();
            return false
        }

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

    this.checkGradYear = function () {
        var birth_year = parseInt($("#BirthdayYear").val());
        var val = parseInt($("#GradYear").val());

        if (!$(".grad-year").hasClass("hidden") && !(val > birth_year)) {
            $(".grad-year-message-error").show();
            return false;
        }
        $(".grad-year-message-error").hide();
        return true;
    }

    this.checkSportId = function () {
        var val = $("#SportID").val();
        if (val.length === 0) {
            $(".sportid-message-error").show();
            return false;
        }
        else {
            $('.sportid-message-error').hide();
            return true;
        }
    }

    this.checkPositionId = function () {
        var val = $("#FieldPositionID").val();
        if (val.length === 0) {
            $(".positionid-message-error").show();
            return false;
        }
        else {
            $('.positionid-message-error').hide();
            return true;
        }
    }

    this.checkDateValid = function () {
        //Since JavaScripts counts months starting from 0 (January is 0), 
        //subtract 1 from the month integer submitted by the user

        var day = $("#BirthdayDay").val();
        var month = $("#BirthdayMonth").val() - 1;
        var year = $("#BirthdayYear").val();
        var testDate = new Date(year, month, day, 0, 0, 0, 0);
        if (day != testDate.getDate() || month != testDate.getMonth() || year != testDate.getFullYear()) {
            $(".age-message-error").show();
            return false
        }
        $(".age-message-error").hide();
        return true;

    }

    this.addPlayerInfo = function () {
        var data = $("#AddPlayerUserInfoForm").serialize();
        $('.summary-message-errors').hide();
        if (_this.checkDateValid() === false | _this.checkSportId() === false |
            _this.checkPositionId() === false | _this.checkGradYear() === false) {
            $('.summary-message-errors').show();
            return false
        }

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