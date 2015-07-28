'use strict';
function Dashboard() {
    var _this = this;

    this.locals = null;
    this.init = function ()
    {
        if ($("#DateHidden").val() != $("#CurrentDateHidden").val()) {
            $('#CalendarButton').tooltipster({
                content: $('<span><span class="glyphicon glyphicon-info-sign"></span> Editing past dates!</span>'),
                trigger : "custom"
            });
            $('#CalendarButton').tooltipster("show");
        }

        $.ajax({
            url: "/Dashboard/JsonPlayers",
            success: function (result) {
                _this.locals = result.team;
                $('#SearchAthlete').typeahead({
                    hint: true,
                    highlight: function () {
                        return true;
                    },
                    minLength: 1
                },
               {
                   name: 'searchString',
                   displayKey: function (data) {
                       return data.value.name;
                   },
                   source: _this.substringMatcher(_this.locals),
                   templates: {
                       suggestion: function (data) {
                           return '<li class="search-suggestion"><img src="' + data.value.avatar + '?w=38&h=38&mode=max"><div>' + data.value.name + '</div><div class="state">' + data.value.state + '</div></li>';
                       }
                   },
                   engine: Hogan
               }).bind('typeahead:selected', function (obj, selected, name) {
                   window.location = "/dashboard?searchString=" + selected.value.name;
               });
            }
        });

        $("#SearchBtn").click(function () {
            window.location = "/dashboard?searchString=" + $("#SearchAthlete").val();
        });

        $("#ClearBtn").click(function () {
            window.location = "/dashboard";
        });

        $("#SearchAthlete").keyup(function (e) {
            var code = e.which; 
            if (code == 13) {
                window.location = "/dashboard?searchString=" + $("#SearchAthlete").val();
            }
        });

        $("#GroupId").change(function () {
            window.location = "/dashboard?groupId=" + $("#GroupId").val();
        });

        $(".user-item .user-name").click(function () {
            _this.showUserInfo($(this).data("id"));
        });

        $(document).on("click", ".attendanceMonth", function () {
            var date = $(this).data("date");
            var id = $(this).data("id");
            $.ajax({
                url: "/dashboard/AttendanceCalendar",
                type: "GET",
                data: { id: id, date: date },
                success: function (data) {
                    $("#AttendanceMonthWrapper").html(data);
                }
            });
        });

        $(document).on("click", ".sbc-player-change", function () {
            var parent = $(this).closest(".item");
            var valueWrapper = $(".value", parent);
            var id = $(this).data("id");
            var type = $(this).data("type");
            var value = $(this).data("value");
            $.ajax({
                type: "GET",
                url: "/User/ChangeSbc",
                data: {
                    id: id,
                    type: type,
                    value : value
                },
                success: function (data)
                {
                    valueWrapper.text(data.value);
                }
            });
        });

        $(document).on("click", ".AttendanceCheck", function () {
            var ajaxData = {
                id : $(this).data("id"),
                date : $("#CurrentDate").data("date"),
                attendance: $(this).prop('checked')
            };
            var id = $(this).data("id");
            $.ajax({
                type: "POST",
                url: "/dashboard/SetAttendance",
                data: ajaxData,
                success: function (data) {
                    _this.showUserInfo(id);
                }
            });
        });
        if ($(".playerItem").length > 0) {
            this.showUserInfo($(".playerItem").first().data("id"));
        }

        $(document).on("click", ".sbcChange", function () {
            var id = $(this).data("id");
            userSbc.showModal(id, function () {
                _this.showUserInfo(id);
            });
        });

        $(document).on("click", ".clear-search", function (e) {
            e.preventDefault();

            if (document.location.search.length !== 0) {
                var loc = document.location;
                var oldPath = loc.search;
                var newPath = new Array();
                oldPath = oldPath.split("&");
                for (var i in oldPath) {
                    if (oldPath[i].indexOf("searchString") === -1) {
                        newPath.push(oldPath[i]);
                    }
                }
                newPath = newPath.join("&");
                oldPath = oldPath.join("&");

                if (newPath !== oldPath) {
                    loc.href = loc.origin + loc.pathname + newPath;
                }
                else {
                    $("#SearchAthlete").val("");
                }
            }
            else {
                $("#SearchAthlete").val("");
            }
        });
    }

    this.substringMatcher = function (strs) {
        return function findMatches(q, cb) {
            var matches, substrRegex;

            // an array that will be populated with substring matches
            matches = [];

            // regex used to determine if a string contains the substring `q`
            substrRegex = new RegExp(q, 'i');

            // iterate through the pool of strings and for any string that
            // contains the substring `q`, add it to the `matches` array
            $.each(strs, function (i, str) {
                if (substrRegex.test(str.name)) {
                    // the typeahead jQuery plugin expects suggestions to a
                    // JavaScript object, refer to typeahead docs for more info
                    matches.push({ value: str });
                }
            });

            cb(matches);
        };
    };

    this.showUserInfo = function (id) {
        if (typeof (id) == "undefined") {
            return;
        }
        $.ajax({
            url: "/dashboard/UserInfo",
            type: "GET",
            data: { id: id },
            success: function (data)
            {
                $("#UserInfoWrapper").html(data);
                teamPlayerInfo.init(id);
                _this.onCompleteUserInfo();
            }
        });
    }

    this.onCompleteUserInfo = function () {

    }
}

var dashboard = new Dashboard();
$(function () {
    dashboard.init();
});
