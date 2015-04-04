function Join() {
    var _this = this;

    this.init = function () {
        $("#TeamRegister").click(function () {
            window.location = "/register?type=1";
        });
        $("#IndividualRegister").click(function ()
        {
            window.location = "/register?type=2";
        });
    };
}

var join;
$().ready(function () {
    join = new Join();
    join.init();
});
