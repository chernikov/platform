function VideoLibrary() {
    var _this = this;

    this.init = function ()
    {
        var url = "";

        if ($("#SortType").val() == "1") {
            url = "/Video/JsonVideos";
        } else {
            url = "/Video/JsonPillars";
        }

        $.ajax({
            url: url,
            success: function (result) {
                locals = result.data;
            },
            async: false
        });

        $('#Search').typeahead({
            hint: true,
            highlight: function () {
                return true;
            },
            minLength: 1
        },
        {
            name: 'searchString',
            displayKey: function (data) {
                return data.value.header;
            },
            source: _this.substringMatcher(locals),
            templates: {
                suggestion: function (data) {
                    return '<li class="search-suggestion"><img src="' + data.value.preview + '?w=38&h=38&mode=max"><div>' + data.value.header + '</div></li>';
                }
            },
            engine: Hogan
        }).bind('typeahead:selected', function (obj, selected, name)
        {
            var sortType = selected.value.sortType;
            if (sortType == 1) {
                window.location = "/video?searchString=" + selected.value.header;
            } else {
                window.location = "/video-pillar?searchString=" + selected.value.header;
            }
        });

        $("#VideoContent").mCustomScrollbar({ theme: "minimal-dark" });

        $("#VideoContent .item").click(function () {
            $("#VideoContent .item").removeClass("selected");
            $(this).addClass("selected");
            _this.showVideo($(this));
        });

        _this.showVideo($("#VideoContent .item.selected"), function () {
            var item = $("#VideoContent .inside .item.selected");
            $("#VideoContent").mCustomScrollbar("scrollTo", item);
        });

    }


    this.showVideo = function (item, callback)
    {
        var url = item.data("url");
        var id = item.data("id");

        $.ajax({
            type: "GET",
            url: url,
            data: { id: id },
            success: function (data)
            {
                $("#VideoWrapper").html(data);
                youtubeResizer.init({width: "", height: ""});
                $("#VideoScroll").mCustomScrollbar({ theme: "minimal-dark" });
                if (callback != null) {
                    callback();
                }
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
                if (substrRegex.test(str.header)) {
                    // the typeahead jQuery plugin expects suggestions to a
                    // JavaScript object, refer to typeahead docs for more info
                    matches.push({ value: str });
                }
            });

            cb(matches);
        };
    };
}

var videoLibrary = null;
$(function () {
    videoLibrary = new VideoLibrary();
    videoLibrary.init();
});