
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
    var val = $("#Email").val();
    var pattern = new RegExp(param, "i");
    return pattern.test(val);
    //return /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/.test(val);
});

$.validator.unobtrusive.adapters.add("validemail", ["regex"], function (options) {
    options.rules["validemail"] = options.params.regex;
    options.messages["validemail"] = options.message;
});

$.validator.addMethod("validphone", function (value, element, param) {
    var val = $("#PhoneNumber").val();
    var pattern = new RegExp(param, "i");
    return pattern.test(val);
});

$.validator.unobtrusive.adapters.add("validphone", ["regex"], function (options) {
    options.rules["validphone"] = options.params.regex;
    options.messages["validphone"] = options.message;
});