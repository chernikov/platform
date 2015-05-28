function GettingStarted() {
    var _this = this;

    this.init = function () {
        $("#GettingStartedContent").mCustomScrollbar({ theme: "minimal-dark" });
    }
}
var gettingStarted = null;
$(function () {
    gettingStarted = new GettingStarted();
    gettingStarted.init();
});
