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

    $(document).on("click", "#clear-filters", function (e) {
        e.preventDefault();
        var parts = document.location.href.split(/\?/i);
        //alert(parts[0]);
        document.location.href = parts[0];
        //$('#SearchAthlete').typeahead('val', '');
    })
    .on("click", "#clear-search", function (e) { 
        //e.preventDefault();
        //var loc = document.location;
        //var oldPath = loc.href;
        //var newPath = oldPath.split("&");
        //newPath = newPath.slice(0, newPath.length - 2);
        //alert(oldPath);
        //loc.href = newPath.join("&");
        e.preventDefault();

        if (document.location.search.length !== 0) {
            var loc = document.location;
            var oldPath = loc.href;
            var newPath = new Array();
            oldPath = oldPath.split("&");

            for (var i in oldPath) {
                if (oldPath[i].indexOf("StartID") === -1 && oldPath[i].indexOf("SearchString") === -1) {
                    newPath.push(oldPath[i]);
                }
            }
            newPath = newPath.join("&");
            oldPath = oldPath.join("&");

            if (newPath !== oldPath) {
                loc.href = newPath;
            }
            else {
                $("#SearchAthlete").val("");
            }
        }
        else {
            $("#SearchAthlete").val("");
        }
    });
    
});