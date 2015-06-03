function Thanks() 
{
    var _this = this;

    this.init = function () {
        $.ajax({
            type: "GET",
            url: "/generate-team",
            success: function ()
            {
                window.location.href = "/dashboard";
            }
        })
    }
}

var thanks;

$(function () {
    thanks = new Thanks();
    thanks.init();
});
