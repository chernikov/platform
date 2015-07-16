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

    this.clearOnBoarding = function ()
    {
        $(".tutorial-highlight > *").unwrap();
        $(".tm-coach-6").removeClass("tm-coach-6");
        $(".tm-individual-7").removeClass("tm-individual-7");
        $(".tm-player-5").removeClass("tm-player-5");
        $(".tm-player-7").removeClass("tm-player-7");
        $(".tm-player-8").removeClass("tm-player-8");
        $(".tm-individual-todo-16").removeClass("tm-individual-todo-16");
        $(".tm-individual-todo-32").removeClass("tm-individual-todo-32");
        $(".tm-player-todo-32").removeClass("tm-player-todo-32");
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