
function TeamRegister()
{
    var _this = this;

    this.init = function () {

       

        $(document).on("click", "#ApplyReferralCode", function () {
            _this.ApplyReferrerCode();
            return false;
        });

        $(document).on("click", ".registration-button", function (event) {


            //if individual registration page
            if (document.location.pathname === "/individual-registration") {

            }
            //if team registration page
            else if (document.location.pathname === "/team-registration") {

            }
        })
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

$.validator.addMethod("validemail", function (value, element, param) {
    alert(value);
    alert(element);
    alert(param);
    var val = $("#Email").val();
    return /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(val);
    //return param.test(val);
});

$.validator.unobtrusive.adapters.add("validemail", ["regex"], function (options) {
    options.rules["validemail"] = new RegExp(options.params.regex, "i");
    options.messages["validemail"] = options.message;
});
