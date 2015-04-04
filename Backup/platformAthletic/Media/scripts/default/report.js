function Report()
{
    var _this = this;

    this.ajaxReportResetAttendance = "/Report/ResetAttendance";
    this.ajaxReportResetProgress = "/Report/ResetProgress";

    this.init = function ()
    {
        $("#ResetProgress").click(function () {
            $.ajax({
                type: "GET",
                url: _this.ajaxReportResetProgress,
                success: function () {
                    window.location.reload();
                }
            });
        });

        $("#ResetAttendance").click(function () {
            $.ajax({
                type: "GET",
                url: _this.ajaxReportResetAttendance,
                success: function () {
                    window.location.reload();
                }
            });
        });

    }
}

var report = null;
$().ready(function ()
{
    report = new Report();
    report.init();
});