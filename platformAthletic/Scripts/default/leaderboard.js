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
    }

    this.filterAll = function ()
    {
        var href = window.location.href;
        href = $.param.querystring(href, 'SportID=' + $("#SportID").val());
        href = $.param.querystring(href, 'StateID=' + $("#StateID").val());
        href = $.param.querystring(href, 'FieldPositionID=' + $("#FieldPositionID").val());
        href = $.param.querystring(href, 'Gender=' + $("#Gender").val());
        href = $.param.querystring(href, 'Age=' + $("#Age").val());
        href = $.param.querystring(href, 'Level=' + $("#Level").val());
        href = $.param.querystring(href, 'Page=1');
        href = $.param.querystring(href, 'Sort=TotalDesc');
        console.log(href);
        window.location = href;
    }
}

var leaderboard = null;

$(function () {
    leaderboard = new Leaderboard();
    leaderboard.init();
});