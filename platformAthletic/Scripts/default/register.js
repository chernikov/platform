function TeamRegister()
{
    var _this = this;

    this.init = function () {
        $(document).on("click", "#ApplyReferralCode", function () {
            _this.ApplyReferrerCode();
            return false;
        });
    };


    this.ApplyReferrerCode = function () {
        $.ajax({
            type: "GET",
            url: "/Account/ApplyReferrerCode",
            data: {
                target: $("#Target").val(),
                code: $("#ReferralCodeValue").val()
            },
            beforeSend: function () {
            },
            success: function (data) {
                $("#TotalSum").val(data.sum);
                if (data.result == "error")
                {
                    $("#PromoCodeID").val("");
                    $("#PromoCodeError").show();
                } else {
                    $("#PromoCodeID").val(data.promoCode);
                }
            }
        });
    }
}

var teamRegister = null;

$(function () {
    teamRegister = new TeamRegister();
    teamRegister.init();
});