function EditUserSeason() {
    _this = this;

    this.ajaxUpdateByWeek = "/admin/UserSeason/UpdateByWeek";
    this.ajaxUpdateBySeasonAndDate = "/admin/UserSeason/UpdateBySeasonAndDate";

    this.daysToDisable = [1, 2, 3, 4, 5, 6];

    this.init = function ()
    {
        $("#WeekID").change(function () {
            _this.updateByWeek();
        });

        $("#SeasonID").change(function () {
            _this.updateBySeasonAndDate();
        });

        $("#StartDay").datepicker().on('changeDate', function(ev)
        {
            var date = ev.date.valueOf();
            date = date - 1000 * 60 * 60 * 24 * ev.date.getDay();
            var d = new Date(date);
            $("#StartDay").datepicker("setValue", d);
            _this.updateBySeasonAndDate();
        });
    }

    this.updateByWeek = function ()
    {
        var ajaxData =
        {
            WeekID: $("#WeekID").val(),
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxUpdateByWeek,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok")
                {
                    $("#StartDay").datepicker('setValue', data.startDay);
                    $("#SeasonID").val(data.seasonID);
                }
            }
        });
    }

    this.updateBySeasonAndDate = function ()
    {
        var ajaxData =
        {
            StartDay: $("#StartDay").val(),
            SeasonID: $("#SeasonID").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxUpdateBySeasonAndDate,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok")
                {
                    $("#WeekID").val(data.WeekID);
                }
            }
        });
    }
}

var editUserSeason = null;

$().ready(function ()
{
    editUserSeason = new EditUserSeason();
    editUserSeason.init();
});
