function Common() {
    var _this = this;

    this.init = function () {
        $(".go-up").click(function (e) {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        });
    }
}

var common = null;
$(function () {
    common = new Common();
    common.init();
});
