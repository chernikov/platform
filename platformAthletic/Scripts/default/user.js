function User() {
    var _this = this;

    this.init = function () {
        var ctx = $("#PerformanceChart").get(0).getContext("2d");
        // This will get the first returned node in the jQuery collection.
        var data = null;
        $.ajax({
            url: "/User/Last12WeekPerformance",
            data: {
                id: $("#UserID").val()
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

var user = null;
$(function ()
{
    var user = new User();
    user.init();
});