function TeamPlayerInfo()
{
    var _this = this;

    this.init = function (userID)
    {
        $("#playerTabs").tab();

        var ctx = $("#PerformanceChart").get(0).getContext("2d");
        // This will get the first returned node in the jQuery collection.
        var data = null;
        $.ajax({
            url: "/User/Last12WeekPerformance",
            data :  {
                id: userID
            },
            success: function (result) {
                data = result;
            },
            async: false
        });
        var myLineChart = new Chart(ctx).Line(data, {
            animation: false,
            bezierCurve: false,
        });
    }
}

var teamPlayerInfo = null;
$(function () {
    teamPlayerInfo = new TeamPlayerInfo();
});