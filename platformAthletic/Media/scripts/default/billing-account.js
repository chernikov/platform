function BillingTeamAccount() {
    var _this = this;

    this.ajaxApplyReferrerCode = "/Account/ApplyReferrerCode";
    this.ajaxCancelAutoDebit = "/Account/CancelAutoDebit";

    this.creditNum = null;
    this.init = function ()
    {
        this.creditNum = $("#BillingInfo_CreditCardNumber").val();

        $('.left .menu').css('height', $('.account-billing-wrp').height());

        $("#ApplyReferralCode").live("click", function () {
            _this.ApplyReferrerCode();
            return false;
        });

        $("#PayForSubscription").click(function () {
            $("#PaymentWrapper").show();
            $('.left .menu').css('height', $('.account-billing-wrp').height());
            return false;
        });

        $("#CancelPayment").click(function () {
            $("#PaymentWrapper").hide();
            $('.left .menu').css('height', $('.account-billing-wrp').height());
            return false;
        });

        $("#CancelAutoDebit").click(function () {
            $("#ConfirmAutoDebit").show();
            $('.left .menu').css('height', $('.account-billing-wrp').height());
            return false;
        });

        $("#SubmitConfirmAutoDebit").click(function () {
            $.ajax({
                type: "POST",
                url: _this.ajaxCancelAutoDebit,
                success: function () {
                    window.location.reload();
                }
            });
            return false;
        });

        $("#CancelConfirmAutoDebit").click(function () {
            $("#ConfirmAutoDebit").hide();
            $('.left .menu').css('height', $('.account-billing-wrp').height());
            return false;
        });


        $("#ViewInvoice").click(function () {
            window.open("/invoice", "_blank");
            return false;
        });

        $("#BillingInfo_CreditCardNumber").focus(function () {
            if (_this.creditNum != "")
            {
                $("#BillingInfo_CreditCardNumber").val("");
            }
            
        });

        $("#BillingInfo_CreditCardNumber").blur(function () {
            if ($("#BillingInfo_CreditCardNumber").val() == "") {
                $("#BillingInfo_CreditCardNumber").val(_this.creditNum);
            }
            else {
                _this.creditNum = "";
            }
        });
    };

    this.ApplyReferrerCode = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxApplyReferrerCode,
            data: {
                target: $("#Target").val(),
                code: $("#ReferralCodeValue").val()
            },
            beforeSend: function () {
                $("#PromoCodeError").hide();
                $("#ReferralCodeValidation").remove();
            },
            success: function (data) {
                $("#TotalPayment").text(data.sum);
                if (data.result == "error") {
                    $("#PromoCodeError").show();
                    $("#PromoCodeID").val("");

                } else {
                    $("#PromoCodeID").val(data.promoCode);
                }
            }
        });
    }
}

var billingTeamAccount;
$().ready(function () {
    billingTeamAccount = new BillingTeamAccount();
    billingTeamAccount.init();
});
