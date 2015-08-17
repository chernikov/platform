
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
                var checkExpDateResult = _this.checkExpirationDate();
                _this.tuggleExpirationDateError(checkExpDateResult.expirationDate);
                if (!checkExpDateResult.result) {
                    event.preventDefault();
                }
            }
            //if team registration page
            else if (document.location.pathname === "/team-registration") {
              
            }
        })
    };

    this.checkExpirationDate = function () {
        var year = parseInt( $("#BillingInfo_ExpirationYear").val() );
        var month = parseInt( $("#BillingInfo_ExpirationMonth").val() ) - 1;
        var currentDate = new Date();
        var expirationDate = new Date(year, month);
        var result = {
            result: false,
            expirationDate: expirationDate
        }
        if (expirationDate > currentDate) {
            result.result = true;
        }
        return result;
    }

    this.tuggleExpirationDateError = function (expirationDate) {
        var currentDate = new Date();
        var expirationMonth = expirationDate.getMonth();
        var expirationYear = expirationDate.getFullYear();

        $("#BillingInfo_ExpirationMonth-error").remove();
        $("#BillingInfo_ExpirationMonth").removeClass("input-validation-error");
        $("#BillingInfo_ExpirationMonth").addClass("valid");
        $("[data-valmsg-for='BillingInfo.ExpirationMonth']").removeClass("field-validation-error");
        $("[data-valmsg-for='BillingInfo.ExpirationMonth']").addClass("field-validation-valid");

        if ( expirationYear < currentDate.getFullYear() ) {
        }
        else if (expirationYear >= currentDate.getFullYear() && expirationMonth <= currentDate.getMonth()) {
            $("[data-valmsg-for='BillingInfo.ExpirationMonth']").append("<span id='BillingInfo_ExpirationMonth-error' class=''>Expiration Date not valid</span>");
            $("[data-valmsg-for='BillingInfo.ExpirationMonth']").removeClass("field-validation-valid");
            $("[data-valmsg-for='BillingInfo.ExpirationMonth']").addClass("field-validation-error");
            $("#BillingInfo_ExpirationMonth").addClass("input-validation-error");
            $("#BillingInfo_ExpirationMonth").removeClass("valid");
        }

    }

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