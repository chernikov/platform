function IntextSearch() {
    var _this = this;

    this.init = function ()
    {
        $("#FindWord").click(function () {
            $(".content-box").removeHighlight().highlight($("#SearchText").val());
        });

        $("#SearchText").keypress(function (event) {
            if (event.which == 13) {
                $(".content-box").removeHighlight().highlight($("#SearchText").val());
                event.preventDefault();
            }
        });
    };
}

var intextSearch = null;
$().ready(function () {
    intextSearch = new IntextSearch();
    intextSearch.init();
});