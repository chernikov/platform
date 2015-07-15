function Schedule()
{
    var _this = this;

    this.init = function () {
        $("#toggleSlideButton").click(function () {
            $("#GroupPart").toggleClass("collapsed");
            $("#toggleSlideButton .switch-button").toggleClass("menu-left");
            $("#toggleSlideButton .switch-button").toggleClass("menu-right");
            $("#TablePart").toggleClass("col-lg-12");
            $("#TablePart").toggleClass("col-lg-8");
        });

        $(".group-list .header .drop-down").click(function (e) {
            var body = $(".body", $(this).closest(".team, .group"));
            body.toggle();
            return false;
        });

        $(".styled-checkbox input").change(function () {
            _this.enableAssignBtn();
        });

        _this.enableAssignBtn();

        $("#AssignPlayersBtn").click(function() {
            $("#AssignList").toggle();
        });

        $(".assign-list .item").click(function () {
            _this.assignPlayers($(this).data("id"));

        });

        $('#CreateGroupBtn').click(function () {
            _this.showCreateGroup();
        });

        $('.edit-group').click(function () {
            _this.showEditGroup($(this).data("id"));
        });

        $('.remove-group').click(function () {
            _this.showRemoveGroup($(this).data("id"));
        });

        $(document).on("click", "#EditGroupBtn", function () {
            _this.submitEditGroupForm();
            return false;
        });

        $(document).on("click", "#RemoveGroupBtn", function () {
            _this.submitRemoveGroupForm();
            return false;
        });

        $(".group-list .header").click(function () {
            if ($(this).hasClass("header-group")) {
                _this.loadCalendar($("#Month").val(), $("#TeamID").val(), $(this).data("id"));
            } else {
                _this.loadCalendar($("#Month").val(), $(this).data("id"), null);
            }
        });

        $(document).on('click', '.scheduleMonth', function () {
            var month = $(this).data("month");
            _this.loadCalendar(month, $("#TeamID").val(), $("#GroupID").val());
        });

        $(document).on('click', '.personal-scheduleMonth', function () {
            var month = $(this).data("month");
            _this.loadCalendarPersonal(month);
        });


        $(document).on('click', '.edit-week', function () {
            _this.number = $(this).data("id");
            _this.date = $(this).data("date");
            var selected = $(this).data("macrocycle");
            $(".calendar-drop-down-list").toggle();

            var offset = $(this).offset().top - 55;
            $(".calendar-drop-down-list").css({ top: offset + "px" });

            $(".calendar-drop-down-list .item").removeClass("selected");
            $(".calendar-drop-down-list .item[data-id='" + selected + "']").addClass("selected");
        });

        $(document).on('click', '.calendar-drop-down-list .team-item', function () {
            _this.setSelected(_this.number, $(this).data("id"), _this.date);
        });

        $(document).on('click', '.calendar-drop-down-list .season-item', function () {
            _this.startSeason($(this).data("id"), _this.date);
        });

        $(document).on('click', '.calendar-drop-down-list .cycle-item', function () {
            _this.startCycle(_this.number, $(this).data("id"), _this.date);
        });

        $(document).on('click', '.calendar-drop-down-list .personal-item', function () {
            _this.setSelectedPersonal(_this.number, $(this).data("id"), _this.date);
        });
        $(document).on('click', '.calendar-drop-down-list .personal-season-item', function () {
            _this.startSeasonPersonal($(this).data("id"), _this.date);
        });

        $(document).on('click', '.calendar-drop-down-list .personal-cycle-item', function () {
            _this.startCyclePersonal(_this.number, $(this).data("id"), _this.date);
        });

       
        $(document).on("click", ".season", function () {
            var id = $(this).data("id");

            $(".season-data").hide();
            $(".season").removeClass("selected");
            $(this).addClass("selected");
            $(".season-data[data-id='" + id + "']").show();
        });

        $(".styled-checkbox input").removeAttr("checked");


    }
    this.enableAssignBtn = function () {
        if ($(".styled-checkbox input:checked").length > 0) {
            $("#AssignPlayersBtn").removeAttr("disabled");
        } else {
            $("#AssignPlayersBtn").attr("disabled", "disabled");
        }
    }

    this.assignPlayers = function (groupID)
    {
        var data = "?groupId=" + groupID + "&";
        $(".styled-checkbox input:checked").each(function () {
            var id = $(this).data("id");
            data += "idUsers=" + id + "&";
        });
        data = data.substring(0, data.length - 1);
        if (data.length > 0)
        {
            $.ajax({
                type: "GET",
                url: "/assign-players" + data,
                success: function () {
                    window.location.reload();
                }
            });
        }
    }

    this.showCreateGroup = function () {
        $.ajax({
            type: "GET",
            url: "/create-group",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalGroup").modal();
                
            }
        });
    }

    this.showEditGroup = function (id) {
        $.ajax({
            type: "GET",
            data: { id: id },
            url: "/edit-group",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalGroup").modal();
            }
        });
    }

    this.showRemoveGroup = function (id) {
        $.ajax({
            type: "GET",
            data: { id: id },
            url: "/remove-group",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalRemoveGroup").modal();
            }
        });
    }

    this.submitEditGroupForm = function () {
        var ajaxData = $("#EditGroupForm").serialize();
        $('#ModalWrapper').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $.ajax({
            type: "POST",
            url: "/edit-group",
            data: ajaxData,
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalGroup").modal();
            }
        });
    }

    this.submitRemoveGroupForm = function () {
        var ajaxData = $("#RemoveGroupForm").serialize();

        $.ajax({
            type: "POST",
            url: "/remove-group",
            data: ajaxData,
            success: function (data) {
                $("#ModalWrapper").html(data);
            }
        });
    }

    this.loadCalendar = function (month, teamID, groupID) {
        var ajaxData = {
            month: month,
            teamId: teamID,
            groupId: groupID
        };
        $.ajax({
            type: "GET",
            url: "/Schedule/Calendar",
            data: ajaxData,
            success: function (data) {
                $("#CalendarWrapper").html(data);
            }
        });
    }


    this.loadCalendarPersonal = function (month, teamID, groupID) {
        var ajaxData = {
            month: month,
            teamId: teamID,
            groupId: groupID
        };
        $.ajax({
            type: "GET",
            url: "/Schedule/PersonalCalendar",
            data: ajaxData,
            success: function (data) {
                $("#CalendarWrapper").html(data);
            }
        });
    }
    this.setSelected = function (number, id, date) {
        var ajaxData = {
            number: number,
            macrocycleId: id,
            date: date,
            teamId: $("#TeamID").val(),
            groupId: $("#GroupID").val()
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/SetSchedule",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendar($("#Month").val(), $("#TeamID").val(), $("#GroupID").val());
                _this.onSetSelect();
            }
        });
    }

    this.startCycle = function (number, id, date) {
        var ajaxData = {
            number: number,
            cycleId: id,
            date: date,
            teamId: $("#TeamID").val(),
            groupId: $("#GroupID").val()
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/StartCycle",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendar($("#Month").val(), $("#TeamID").val(), $("#GroupID").val());
                _this.onSetSelect();
            }
        });
    }

    this.startSeason = function (id, date) {
        var ajaxData = {
            seasonId: id,
            date: date,
            teamId: $("#TeamID").val(),
            groupId: $("#GroupID").val()
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/StartSeason",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendar($("#Month").val(), $("#TeamID").val(), $("#GroupID").val());
                _this.onSetSelect();
            }
        });
    }

    this.setSelectedPersonal = function (number, id, date) {
        var ajaxData = {
            number: number,
            date: date,
            macrocycleId: id
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/SetPersonalSchedule",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendarPersonal($("#Month").val());
                _this.onSetSelect();
            }
        });
    }


    this.startCyclePersonal = function (number, id, date) {
        var ajaxData = {
            number: number,
            cycleId: id,
            date: date,
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/StartPersonalCycle",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendarPersonal($("#Month").val());
                _this.onSetSelect();
            }
        });
    }

    this.startSeasonPersonal = function (id, date) {
        var ajaxData = {
            seasonId: id,
            date: date
        };

        $.ajax({
            type: "GET",
            url: "/Schedule/StartPersonalSeason",
            data: ajaxData,
            success: function (data) {
                _this.loadCalendarPersonal($("#Month").val(), $("#TeamID").val(), $("#GroupID").val());
                _this.onSetSelect();
            }
        });
    }

    this.onSetSelect = function () {

    }

}

var schedule = new Schedule();

$(function () {
    schedule.init();
})