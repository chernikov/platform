﻿function SchoolLeaderboard() {
    var _this = this;

    this.init = function ()
    {
        $(".leaderboard-header .sortable").click(function ()
        {
            var value = $(".sort", $(this)).data("type");
            if ($(".sort", $(this)).hasClass("desc"))
            {
                value = value + "Asc";
            } else {
                value = value + "Desc";
            }
            window.location = $.param.querystring(window.location.href, 'Sort=' + value);
        });

        $(".leaderboard-header .reverse-sortable").click(function () {
            var value = $(".sort", $(this)).data("type");
            if ($(".sort", $(this)).hasClass("asc")) {
                value = value + "Desc";
            } else {
                value = value + "Asc";
            }
            window.location = $.param.querystring(window.location.href, 'Sort=' + value);
        });

        $(".filter").change(function () {
            
            _this.filterAll();
        });
        var currentFilter = _this.getCurrentFilter();
        var players = null;

        $('#SearchAthlete').typeahead({
            hint: true,
            highlight: function () {
                return true;
            },
            minLength: 2
        },
       {
           name: 'searchString',
           displayKey: function (data) {
               return data.name;
           },
           source: function (query, process) 
           {
               var filter = _this.getCurrentFilter();
               filter = $.param.querystring(filter, 'SearchString=' + query);
               $.get('/Leaderboard/JsonSchoolPlayers' + filter, null, function (data)
               {
                   //return _this.substringMatcher(data.users);
                   return process(data.users);
               });
           },
           templates: {
               suggestion: function (data) {
                   return '<li class="search-suggestion"><img src="' + data.avatar + '?w=38&h=38&mode=max"><div class="name">' + data.name + '</div><div class="state">' + data.state + '</div></li>';
               }
           },
           engine: Hogan
       }).bind('typeahead:selected', function (obj, selected, name)
       {
           var params = _this.getCurrentFilter();
           params = $.param.querystring(params, 'StartID=' + selected.id);
           params = $.param.querystring(params, 'SearchString=' + selected.name);
           window.location = $.param.querystring("/Leaderboard/School", params);
       });

        if ($("#StartID").val() != "") {
            $('html, body').animate({
                scrollTop: $(".item.selected").offset().top
            }, 1000);
        }

        $(".item").click(function () {
            var id = $(this).data("id");
            _this.showPlayerInfo(id);
        })
    }

    this.filterAll = function ()
    {
        var currentFilter = _this.getCurrentFilter();
        window.location = $.param.querystring(window.location.href, currentFilter);
    }

    this.getCurrentFilter = function ()
    {
        var href = "";
        href = $.param.querystring(href, 'SportID=' + $("#SportID").val());
        href = $.param.querystring(href, 'FieldPositionID=' + $("#FieldPositionID").val());
        href = $.param.querystring(href, 'Gender=' + $("#Gender").val());
        href = $.param.querystring(href, 'Age=' + $("#Age").val());
        href = $.param.querystring(href, 'LevelID=' + $("#LevelID").val());
        if ($("#GradYear").length > 0)
        {
            href = $.param.querystring(href, 'GradYear=' + $("#GradYear").val());
        }
        href = $.param.querystring(href, 'Page=1');
        href = $.param.querystring(href, 'Sort=TotalDesc');
        console.log(href);
        return href;
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

    this.showPlayerInfo = function (id) {
        $.ajax({
            type: "GET",
            url: "/Leaderboard/PlayerInfo",
            data : {id : id},
            success: function (data)
            {
                $("#ModalWrapper").html(data);
                $("#modalPlayerInfo").modal();
            }
        });
    }
}

var schoolLeaderboard = null;
$(function () {
    schoolLeaderboard = new SchoolLeaderboard();
    schoolLeaderboard.init();
});