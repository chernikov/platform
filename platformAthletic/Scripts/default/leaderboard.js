function Leaderboard() {
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

       /* $.ajax({
            url: $.param.querystring("/Leaderboard/JsonPlayers", currentFilter),
            success: function (result) {
                players = result.users;
            },
            async: false
        });
        */
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
               $.get('/Leaderboard/JsonPlayers' + filter, null, function (data)
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
           window.location = $.param.querystring("/Leaderboard/", params);
       });

        if ($("#StartID").val() != "") {
            $('html, body').animate({
                scrollTop: $(".item.selected").offset().top - 60
            }, 1000);
        }

        $(document).on('click', '.item, .all-item', function () {
            var id = $(this).data("id");
            _this.showPlayerInfo(id);
        })

        $(document).on("click", ".restrictAccess", function () {
            $(".privacyMessage").show();
        });

        $(document).on("click", ".clear-filters", function (e) {
            e.preventDefault();
            var parts = document.location.href.split(/\?/i);
            document.location.href = parts[0];
        })
       .on("click", ".clear-search", function (e) {
           e.preventDefault();

           if (document.location.search.length !== 0) {
               var loc = document.location;
               var oldPath = loc.href;
               var newPath = new Array();
               oldPath = oldPath.split("&");

               for (var i in oldPath) {
                   if (oldPath[i].indexOf("StartID") === -1 && oldPath[i].indexOf("SearchString") === -1) {
                       newPath.push(oldPath[i]);
                   }
               }
               newPath = newPath.join("&");
               oldPath = oldPath.join("&");

               if (newPath !== oldPath) {
                   loc.href = newPath;
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

    this.filterAll = function ()
    {
        var currentFilter = _this.getCurrentFilter();
        window.location = $.param.querystring(window.location.href, currentFilter);
    }

    this.getCurrentFilter = function () {
        var href = "";
        href = $.param.querystring(href, 'SportID=' + $("#SportID").val());
        href = $.param.querystring(href, 'StateID=' + $("#StateID").val());
        if ($("#SportID").val() != "") {
            href = $.param.querystring(href, 'FieldPositionID=' + $("#FieldPositionID").val());
        } else {
            href = $.param.querystring(href, 'FieldPositionID=');
        }
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

var leaderboard = null;

$(function () {
    leaderboard = new Leaderboard();
    leaderboard.init();
});