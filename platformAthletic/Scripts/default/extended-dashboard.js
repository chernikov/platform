function ExtendedDashboard() {
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
            window.location = "/dashboard/extended?searchString=" + selected.value.name;
        });

        $("#SearchBtn").click(function () {
            window.location = "/dashboard/extended?searchString=" + $("#SearchAthlete").val();
        });

        $("#GroupId").change(function () {
            window.location = "/dashboard/extended?groupId=" + $("#GroupId").val();
        });
    }


    this.substringMatcher = function (strs)
    {
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
}

var extendedDashboard = null;

$(function () {
    extendedDashboard = new ExtendedDashboard();
    extendedDashboard.init();
});
