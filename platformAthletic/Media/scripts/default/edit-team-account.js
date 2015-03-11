function EditTeamAccount() {
    var _this = this;

    this.daysToDisable = [1, 2, 3, 4, 5, 6];
    this.ajaxSetNextSeason = "/Account/SetNextSeason";
    this.ajaxUploadLogo = "/Account/UploadTeamLogo";

    this.init = function () {
        var date = $("#MinNextSelectDay").val();
        $("#NextSeasonStartDay").datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: new Date(date),
            beforeShowDay: _this.disableSpecificWeekDays,
        });

        $("#primary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#Team_PrimaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $("#secondary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#Team_SecondaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $('.left .menu').css('height', $('.account-basic-wrp').height());

        $("#ConfirmNextSeasonButton").click(function () {
            _this.setNextSeason();
            return false;
        });

        $('.popup-blur-bg, .btn-gray').live('click', function () {
            $("#PopupWrapper").empty();
            $('.popup-blur-bg').hide();
            return false;
        });

        var titlePreview = $("#UploadLogo").text();
        InitUpload($("#UploadLogo")[0],
            false,
            _this.ajaxUploadLogo,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#Team_LogoPath").val(responseJSON.imagePath);
                    $("#LogoImgWrapper").html("<img src='" + responseJSON.imagePath + "'/>");
                } else {
                    alert(responseJSON.error);
                }
            },
            null, titlePreview);
    };

    this.disableSpecificWeekDays = function (date) {
        var day = date.getDay();
        for (i = 0; i < _this.daysToDisable.length; i++) {
            if ($.inArray(day, _this.daysToDisable) != -1) {
                return [false];
            }
        }
        return [true];
    }

    this.setNextSeason = function () {
        $.ajax({
            type: "POST",
            url: _this.ajaxSetNextSeason,
            data: { startDay: $("#NextSeasonStartDay").val() },
            success: function (data)
            {
                $('#PopupWrapper').append('<div class="popup-confirm-wrp"><div class="inner-wrp"><div class="popup-title">Your changes were successful</div><div class="button-wrp"><button class="btn-gray center-block">OK</button></div></div></div>');
                var winWidth = $(window).width();
                var winHeight = $(window).height();

                var leftVal = winWidth / 2 - 447 / 2;
                var topVal = winHeight / 2 - 407 / 2;

                $('.popup-confirm-wrp').css({
                    top: topVal + 'px',
                    left: leftVal + 'px'
                });
                $('.popup-confirm-wrp').show();
                $('.popup-blur-bg').show();
            }
        });
    }

    this.UpdateColors = function () {
        $(".primaryColor").css("color", $("#Team_PrimaryColor").val());
        $(".primaryColorBg").css("background-color", $("#Team_PrimaryColor").val());
        $(".secondaryColor").css("color", $("#Team_SecondaryColor").val());
        $(".secondaryColorBg").css("background-color", $("#Team_SecondaryColor").val());
    }
}

var editTeamAccount;
$().ready(function () {
    editTeamAccount = new EditTeamAccount();
    editTeamAccount.init();
});
