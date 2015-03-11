function Scheduling()
{
    var _this = this;

    this.ajaxCalendar = "/Schedule/Calendar";
    this.ajaxSetSchedule = "/Schedule/SetSchedule";
    this.ajaxResetSchedule = "/Schedule/ResetSchedule";

    this.ajaxCalendarPersonal = "/Schedule/PersonalCalendar";
    this.ajaxSetSchedulePersonal = "/Schedule/SetPersonalSchedule";
    this.ajaxResetSchedulePersonal = "/Schedule/ResetPersonalSchedule";

    this.number = null;
    this.date = null;
    this.init = function ()
    {
        $(document).on('click', '.left-arrow, .right-arrow', function () {
            var month = $(this).data("month");
            _this.loadCalendar(month, $("#TeamID").val(), $("#GroupID").val());
        });

        $(document).on('click', '.left-arrow-personal, .right-arrow-personal', function () {
            var month = $(this).data("month");
            _this.loadCalendarPersonal(month);
        });


        $(document).on('click', '.edit-week', function () {
            _this.number = $(this).data("id");
            _this.date = $(this).data("date");
            var selected = $(this).data("macrocycle");
            $(".calendar-drop-down-list").toggle();

            var offset = $(this).offset().top - 230;
            $(".calendar-drop-down-list").css({ top: offset + "px" });

            $(".calendar-drop-down-list .item").removeClass("selected");
            $(".calendar-drop-down-list .item[data-id='" + selected + "']").addClass("selected");
        });

        $(document).on('click', '.calendar-drop-down-list .team-item', function () {
            _this.setSelected(_this.number, $(this).data("id"), _this.date);
        });

        $(document).on('click', '.calendar-drop-down-list .personal-item', function () {
            _this.setSelectedPersonal(_this.number, $(this).data("id"), _this.date);
        });


        $(document).on("click", "#ResetShowBtn", function () {
            _this.showResetSchedule();
        });

        $(document).on("click", "#ResetBtn", function () {
            _this.resetSchedule();
        });

        $(document).on("click", "#ResetPersonalShowBtn", function () {
            _this.showResetSchedulePersonal();
        });

        $(document).on("click", "#ResetPersonalBtn", function () {
            _this.resetSchedulePersonal();
        });

        $(document).on("click", ".season", function () {
            var id = $(this).data("id");

            $(".season-data").hide();
            $(".season").removeClass("selected");
            $(this).addClass("selected");
            $(".season-data[data-id='" + id + "']").show();
        });
    }

    this.loadCalendar = function (month, teamID, groupID)
    {
        var ajaxData = {
            month : month,
            teamId : teamID,
            groupId : groupID
        };
        $.ajax({
            type: "GET",
            url: _this.ajaxCalendar,
            data: ajaxData,
            success: function (data)
            {
                $("#CalendarWrapper").html(data);
                $('.scroll-pane', $("#CalendarWrapper")).jScrollPane({ showArrows: true, autoReinitialise: true });
            }
        });
    }

    this.loadCalendarPersonal = function (month) {
        var ajaxData = {
            month: month
        };
        $.ajax({
            type: "GET",
            url: _this.ajaxCalendarPersonal,
            data: ajaxData,
            success: function (data) {
                $("#CalendarWrapper").html(data);
                $('.scroll-pane', $("#CalendarWrapper")).jScrollPane({ showArrows: true, autoReinitialise: true });
            }
        });
    }

    this.setSelected = function (number, id, date)
    {
        var ajaxData = {
            number : number,
            macrocycleId: id,
            date : date,
            teamId: $("#TeamID").val(),
            groupId : $("#GroupID").val()
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxSetSchedule,
            data: ajaxData,
            success: function (data)
            {
                _this.loadCalendar($("#Month").val(), $("#TeamID").val(), $("#GroupID").val());
            }
        });
    }

    this.setSelectedPersonal = function (number, id, date) {
        var ajaxData = {
            number: number,
            date : date,
            macrocycleId: id
        };

        $.ajax({
            type: "GET",
            url: _this.ajaxSetSchedulePersonal,
            data: ajaxData,
            success: function (data)
            {
                _this.loadCalendarPersonal($("#Month").val());
            }
        });
    }

    this.showResetSchedule = function ()
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxResetSchedule,
            data: { groupId: $("#GroupID").val() },
            success: function (data)
            {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.showResetSchedulePersonal = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxResetSchedulePersonal,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.initPopup = function () {
        var winWidth = $(window).width();
        var winHeight = $(window).height();

        var leftVal = winWidth / 2 - 447 / 2;
        var topVal = winHeight / 2 - 407 / 2;

        $('.popup-roster-wrp').css({
            top: topVal + 'px',
            left: leftVal + 'px'
        });
        $('.popup-roster-wrp').show();
        $('.popup-blur-bg').show();
        common.StyleDropDown($('#PopupWrapper .dropdown-styled'));
    }

    this.resetSchedule = function () {
        $.ajax({
            type: "POST",
            url: _this.ajaxResetSchedule,
            data: { groupId: $("#GroupID").val() },
            success: function (data) {
                $("#PopupWrapper").html(data);
            }
        });
    }

    this.resetSchedulePersonal = function () {
        $.ajax({
            type: "POST",
            url: _this.ajaxResetSchedulePersonal,
            success: function (data) {
                $("#PopupWrapper").html(data);
            }
        });
    }
}


var scheduling = null;

$().ready(function () {
    scheduling = new Scheduling();
    scheduling.init();
});