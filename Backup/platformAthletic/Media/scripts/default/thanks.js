function Thanks() {
    var _this = this;

    this.init = function () {
        $("#buttonBackHome").click(function () {
            window.location = "/";
            return false;
        });
        $("#buttonEnterPlayer").click(function () {
            window.location = "/team";
            return false;
        });

        $("#buttonPersonalPage").click(function () {
            window.location = "/my-page";
            return false;
        });
    };
}

var thanks;
$().ready(function () {
    thanks = new Thanks();
    thanks.init();
});