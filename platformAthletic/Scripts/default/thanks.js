function Thanks() 
{
    var _this = this;

    this.init = function () {
        $.ajax({
            type: "GET",
            url: "/generate-team",
            beforeSend : function() {
                $.blockUI({ message: '<h1>Thank you!</h1><h2>We are preparing test team for you...</h2>' });
            },
            success: function ()
            {
               // $.unblockUI();
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
