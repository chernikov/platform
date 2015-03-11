function SelectSeason() {
    var _this = this;

    this.daysToDisable = [1, 2, 3, 4, 5, 6];
    this.init = function () {
        $('.checkbox-wrp').each(function () {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');

            if (checkbox.is(':checked')) {
                checkboxImage.css('background-position', '0 0');
            } else {
                checkboxImage.css('background-position', '0 -93px');
            }
        });

        $('.checkbox-wrp').click(function (e) {
            var $this = $(this);
            var checkbox = $this.find('input');
            var checkboxImage = $this.find('.checkbox-image');

            $("input[type=checkbox]").removeAttr('checked');
            $('.checkbox-image').css('background-position', '0 -93px');

            checkbox.attr('checked', "checked");
            checkboxImage.css('background-position', '0 0');
            return false;
        });

        $("#datepicker").datepicker({
            showOn: "focus",
            buttonImageOnly: false,
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: 0,
            beforeShowDay: _this.disableSpecificWeekDays,
        });

        $("#ConfirmSeasonDate").click(function () {
            $("#buttonConfirm").show();
        });

        $("#InSeasonCheck").click(function () {
            $("#SeasonID").val(2);
        });

        $("#OffSeasonCheck").click(function () {
            $("#SeasonID").val(1);
        });

        $("#ConfirmBtn").click(function ()
        {
            if ($("#datepicker").val() != "")
            {
                $(".start-date-wrapper input").removeClass("input-validation-error");
                $.ajax({
                    type: "POST",
                    url: "/Account/SetSeason",
                    data: {
                        SeasonID: $("#SeasonID").val(),
                        StartDay: $("#datepicker").val(),
                    },
                    beginSend : function() {
                        $("#DateError").hide();
                    },
                    success: function (data) {
                        if (data.result == "ok") {
                            window.location = "/";
                        } else {
                            $("#DateError").show();
                        }
                    }
                });
            } else {
                $(".start-date-wrapper input").addClass("input-validation-error");
            }
        });
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
}

var selectSeason;
$().ready(function () {
    selectSeason = new SelectSeason();
    selectSeason.init();
});