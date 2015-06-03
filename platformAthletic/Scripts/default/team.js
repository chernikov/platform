function Team()
{
    var _this = this;

    this.init = function ()
    {
        $("#PrintAll").click(function () {
            var data = "?";
            $(".playerItem").each(function () {
                var id = $(this).data("id");
                data += "idUsers=" + id + "&";
            });
            data = data.substring(0, data.length - 1);
            window.open("/team-table" + data, "_blank");
        });

        $("#PrintSelected").click(function () {
            var data = "?";
            $(".forPrint:checked").each(function () {
                var id = $(this).data("id");
                data += "idUsers=" + id + "&";
            });
            data = data.substring(0, data.length - 1);
            if (data.length > 0) {
                window.open("/team-table" + data, "_blank");
            }
            return false;
        });
    }
}

var team = null;

$(function () {
    team = new Team();
    team.init();
});