function Dashboard() {
    var _this = this;

    this.init = function () {
        $.ajax({
            url: "/Dashboard/JsonPlayers",
            success: function (result) {
                locals = result.team;
            },
            async: false
        });
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
            source: _this.substringMatcher(locals),
            templates: {
                suggestion: function (data) {
                    return '<li class="search-suggestion"><img src="' + data.value.avatar + '?w=38&h=38&mode=max"><div>' + data.value.name + '</div><div class="state">' + data.value.state + '</div></li>';
                }
            },
            engine: Hogan
        }).bind('typeahead:selected', function(obj, selected, name) {
            window.location = "/dashboard?searchString=" + selected.value.name;
        });

        $("#SearchBtn").click(function () {
            window.location = "/dashboard?searchString=" + $("#SearchAthlete").val();
        });

        $("#GroupId").change(function () {
            window.location = "/dashboard?groupId=" + $("#GroupId").val();
        });

        $(".user-item .user-name").click(function () {
            _this.showUserInfo($(this).data("id"));
        });

        $("#FullTable").click(function () {
            $("#UserInfoWrapper").empty();
            $("#UserInfoWrapper").hide();
            $("#TablePart").removeClass("col-lg-6");
            $("#TablePart").removeClass("col-lg-height");
            $("#TablePart").addClass("col-lg-12");
            $("#TablePart").addClass("col-lg-height");
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
        $.ajax({
            url: "/dashboard/UserInfo",
            type: "GET",
            data: { id: id },
            success: function (data)
            {
        
                $("#TablePart").removeClass("col-lg-12");
                $("#TablePart").removeClass("col-lg-height");
                $("#UserInfoWrapper").removeClass("col-lg-height");
                $("#UserInfoWrapper").html(data);
                $("#UserInfoWrapper").show();
                $("#TablePart").addClass("col-lg-6");
                $("#TablePart").addClass("col-sm-12");
                $("#TablePart").addClass("col-lg-height");
                $("#UserInfoWrapper").addClass("col-lg-height");
                teamPlayerInfo.init();
            }
        });
    }
}

var dashboard = null;

$(function () {
    dashboard = new Dashboard();
    dashboard.init();
});
