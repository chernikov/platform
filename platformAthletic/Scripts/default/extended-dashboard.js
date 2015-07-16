function ExtendedDashboard() {
    var _this = this;

    this.init = function () {
        if ($("#DateHidden").val() != $("#CurrentDateHidden").val()) {
            $('#CalendarButton').tooltipster({
                content: $('<span><span class="glyphicon glyphicon-info-sign"></span> Editing past dates!</span>'),
                trigger: "custom"
            });
            $('#CalendarButton').tooltipster("show");
        }

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
        }).bind('typeahead:selected', function (obj, selected, name) {
            window.location = "/dashboard/extended?searchString=" + selected.value.name;
        });

        $("#SearchBtn").click(function () {
            window.location = "/dashboard/extended?searchString=" + $("#SearchAthlete").val();
        });

        $("#GroupId").change(function () {
            window.location = "/dashboard/extended?groupId=" + $("#GroupId").val();
        });

        $(document).on("click", ".sbc-change", function () {
            var parent = $(this).closest(".value-wrapper");
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
                    value: value
                },
                success: function (data) {
                    valueWrapper.text(data.value);
                    _this.onSbcChange();
                }
            });
        });

        $(document).on("click", ".AttendanceCheck", function () {
            var ajaxData = {
                id: $(this).data("id"),
                date: $("#CurrentDate").data("date"),
                attendance: $(this).prop('checked')
            };
            $.ajax({
                type: "POST",
                url: "/dashboard/SetAttendance",
                data: ajaxData,
                success: function (data) {
                }
            });
        });

        $(document).on("click", ".sbcChange", function () {
            var id = $(this).closest(".value-wrapper").data("id");
            userSbc.showModal(id);
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

    this.onSbcChange = function () {

    }
}

var extendedDashboard = new ExtendedDashboard();
$(function () {
    extendedDashboard.init();
});
