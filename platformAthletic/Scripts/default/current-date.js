function CurrentDate() {
    var _this = this;
    this.ajaxSetDate = "/Setting/SetCurrentDate";

    this.init = function () {
        $("#currentDate").datepicker()
            .on("hide", function() {
                _this.setCurrentDate($("#currentDate").val());
        });
    };

    this.setCurrentDate = function (date) {
        $.ajax({
            type: "POST",
            url: _this.ajaxSetDate,
            data: { dateTime: date },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
            }
        });
    }
}

var currentDate = null;
$().ready(function () {
    currentDate = new CurrentDate();
    currentDate.init();
});