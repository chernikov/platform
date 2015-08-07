function TeamRegister()
{
    var _this = this;

    this.init = function () {
        $(document).on("click", "#ApplyReferralCode", function () {
            _this.ApplyReferrerCode();
            return false;
        });

        $(document).on("click", ".registration-button", function (event) {
            if (_this.checkEmail()) {
            }
            else {
                event.preventDefault();
            }


            //if individual registration page
            if (document.location.pathname === "/individual-registration") {

            }
            //if team registration page
            else if (document.location.pathname === "/team-registration") {

            }
        })
    };

    this.checkEmail = function () {
        $("#Email-error").remove();
        if (/^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/i.test($("#Email").val())) {
            $("#Email").removeClass("input-validation-error");
            $("#Email").addClass("valid");
            $("#Email").attr("aria-invalid", "false");
            $("[data-valmsg-for='Email']").removeClass("field-validation-error");
            $("[data-valmsg-for='Email']").addClass("field-validation-valid");

            return true;
        }
        else {
            $("#Email").removeClass("valid");
            $("#Email").addClass("input-validation-error");
            $("#Email").attr("aria-invalid", "true");
            $("[data-valmsg-for='Email']").removeClass("field-validation-valid");
            $("[data-valmsg-for='Email']").addClass("field-validation-error");
            $("[data-valmsg-for='Email']").append("<span id='Email-error' class=''>Enter Email</span>");

            return false;
        }
    }

    this.checkPhone = function () {
        return /^[\d|\+|\(]+[\)|\d|\s|-]*[\d]$/i.test($("#PhoneNumber").val());
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