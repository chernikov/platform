function Console() {
    this.log = function (message) {
    };
}

var console = new Console();

function UpdateOnline() {
    SendUpdateOnline();
}

function SendUpdateOnline() {
    $.getJSON("/online", function (data) { });
    setTimeout(UpdateOnline, 60000);
}

function Common() {
    var _this = this;
    this.init = function ()
    {
        $.ajaxSetup({ cache: false });
    };

    this.isMobile = function () {
        var ua = navigator.userAgent.toLowerCase();
        return ua.indexOf("mobile") > -1; //&& ;
    }
}
var common = null;

$(function () {
    common = new Common();
    common.init();
    SendUpdateOnline();
    $('#SideMenuToggle').click(function () {
        $('.side-menu-container .navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    });
});