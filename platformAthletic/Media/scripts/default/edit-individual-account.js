function EditIndividualAccount() {
    var _this = this;

    this.daysToDisable = [1, 2, 3, 4, 5, 6];
    this.ajaxSetNextSeason = "/Account/SetNextSeason";
    this.ajaxUploadLogo = "/Account/UploadAvatar";

    this.init = function ()
    {
        var date = $("#MinNextSelectDay").val();
        $("#NextSeasonStartDay").datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: new Date(date),
            beforeShowDay: _this.disableSpecificWeekDays,
        });

        $("#primary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#PrimaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $("#secondary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#SecondaryColor").val(color);
                alert();
                _this.UpdateColors();
            }
        });

        $('.left .menu').css('height', $('.account-basic-wrp').height());

        $("#ConfirmNextSeasonButton").click(function () {
            _this.setNextSeason();
            return false;
        });

        var titlePreview = $("#UploadAvatar").text();
        InitUpload($("#UploadAvatar")[0],
            false,
            _this.ajaxUploadLogo,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    $("#AvatarPath").val(responseJSON.imagePath);
                    $("#AvatarImgWrapper").html("<img src='" + responseJSON.imagePath + "'/>");
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
            data: { startDay: $("#NextSeasonStartDay").val() }
        });
    }

    this.UpdateColors = function ()
    {
        colors.init($("#PrimaryColor").val(), $("#SecondaryColor").val());
    }
}

var editIndividualAccount;
$().ready(function () {
    editIndividualAccount = new EditIndividualAccount();
    editIndividualAccount.init();
});
