function ProgressReport() {
    var _this = this;

    this.init = function () {
        Chart.defaults.global.responsive = true;

        //$(".side-menu-container .nav.navbar-nav li:nth-of-type(3)").addClass("active");
        $('.datetime').mask("00/00/0000", { placeholder: "__/__/____" });

        $.ajax({
            url: "/Report/JsonPlayers",
            success: function (result) {
                locals = result.team;
            },
            async: false
        });

        $(".report-table .sortable").click(function () {
            var value = $(".sort", $(this)).data("type");
            if ($(".sort", $(this)).hasClass("desc")) {
                value = value + "Asc";
            } else {
                value = value + "Desc";
            }
            window.location = $.param.querystring(window.location.href, 'Sort=' + value);
        });

        $(document).on("click", ".report-table .item", function () {
            var id = $(this).data("id");
            $(".report-table .item").removeClass("selected");
            $(this).addClass("selected");
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
           var params = _this.getCurrentFilter();
           params = $.param.querystring(params, 'StartID=' + selected.value.id);
           params = $.param.querystring(params, 'SearchString=' + selected.value.name);
           window.location = $.param.querystring("/report/progress/", params);
       });

        $(".filter").change(function () {
            _this.filterAll();
        });
        $(".datetime").change(function () {
            $("#StartPeriodError").hide();
            $("#EndPeriodError").hide();
            _this.filterAll();
        });

        $(".report-table .item .graph").click(function () {
            _this.showGraph($(this).data("id"));
        });

        $("#StartPeriod,#EndPeriod").datepicker({
            autoclose: true,
        });

        $(document).on("click", ".userName", function () {
            var id = $(this).data("id");
            _this.showPlayerInfo(id);
        });

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
               var newPath = [];
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
    };

    this.getCurrentFilter = function () {
        var href = "";
        href = $.param.querystring(href, 'SportID=' + $("#SportID").val());
        if ($("#SportID").val() !== "") {
            href = $.param.querystring(href, 'FieldPositionID=' + $("#FieldPositionID").val());
        } else {
            href = $.param.querystring(href, 'FieldPositionID=');
        }
        href = $.param.querystring(href, 'GroupID=' + $("#GroupID").val());
        href = $.param.querystring(href, 'GradYear=' + $("#GradYear").val());
        href = $.param.querystring(href, 'StartPeriod=' + $("#StartPeriod").val());
        href = $.param.querystring(href, 'EndPeriod=' + $("#EndPeriod").val());
        href = $.param.querystring(href, 'Page=1');
        href = $.param.querystring(href, 'Sort=TotalDesc');
        console.log(href);
        return href;
    };

    this.filterAll = function () {
        var currentFilter = _this.getCurrentFilter();
        if ($("#StartPeriod").val() === "") {
            $("#StartPeriodError").show();
            return;
        }
        if ($("#EndPeriod").val() === "") {
            $("#EndPeriodError").show();
            return;
        }
        window.location = $.param.querystring(window.location.href, currentFilter);
    };

    this.showGraph = function (id) {
        $.ajax({
            type: "GET",
            url: "/Report/ProgressGraph",
            data: {
                id: id,
                startDate: $("#StartDate").val(),
                endDate: $("#EndDate").val()
            },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalProgressGraph").modal();
                $("#modalProgressGraph").on("shown.bs.modal", function () {
                    _this.drawGraph(id);
                });

                $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                    if (e.currentTarget.hash === "#30-day") {
                        _this.drawGraph30(id);
                    }
                });
                ga('send', 'show_modal', 'modal', 'show', 'report_progress_graph', 1);
                hj('show_report_progress_graph_modal');
            }
        });
    };
    this.drawGraph = function (id) {
        // This will get the first returned node in the jQuery collection.
        var data = null;
        $.ajax({
            url: "/Report/Performance",
            data: {
                id: id,
                startDate: $("#StartDate").val(),
                endDate: $("#EndDate").val()
            },
            success: function (result) {
                data = result;
                var ctx = $("#PerformanceChart")[0].getContext("2d");
                var myLineChart = new Chart(ctx).Line(data, {
                    animation: false,
                    bezierCurve: false,
                    scaleShowVerticalLines: false,
                    maintainAspectRatio: false,
                    responsive: true
                });

            },
        });
    };

    
    this.drawGraph30 = function (id) {
        // This will get the first returned node in the jQuery collection.
        var data = null;
        $.ajax({
            url: "/Report/Performance30",
            data: {
                id: id,
                startDate: $("#StartDate").val(),
                endDate: $("#EndDate").val()
            },
            success: function (result) {
                data = result;
                var ctx = $("#PerformanceChart30").get(0).getContext("2d");
                var myLineChart = new Chart(ctx).Line(data, {
                    animation: false,
                    bezierCurve: false,
                    scaleShowVerticalLines: false,
                    maintainAspectRatio: false,
                    responsive: true
                });
            },
        });
    };

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
            data: { id: id },
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalPlayerInfo").modal();
                ga('send', 'show_modal', 'modal', 'show', 'report_progress_player_profile', 1);
                hj('show_report_progress_player_profile_modal');
            }
        });
    };

}

var progressReport = null;
$(function () {
    progressReport = new ProgressReport();
    progressReport.init();
});