function Billing() {
    var _this = this;

    this.init = function () {
        $("#CancelAutoDebit").click(function () {
            if (typeof (testmode) == "undefined") {
                _this.showCancelDebit();
            }
        });

        $(document).on("click", "#SubmitConfirmAutoDebit", function () {
            $.ajax({
                type: "POST",
                url: "/Billing/CancelAutoDebit",
                data: { id: 0 },
                success: function () {
                    window.location.reload();
                }
            });
            return false;
        });

        $(document).on("click", "#ApplyReferralCode", function () {
            _this.ApplyReferrerCode();
            return false;
        });

        $("#PayForSubscription").click(function () {
            if (typeof (testmode) == "undefined") {
                $("#PaymentWrapper").show();
                $('.left .menu').css('height', $('.account-billing-wrp').height());
                return false;
            }
        });

        $("#ViewInvoice").click(function () {
            window.open("/invoice", "_blank");
            return false;
        });
    }


    this.ApplyReferrerCode = function () {
        $.ajax({
            type: "GET",
            url: "/Billing/ApplyReferrerCode",
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
                } 
            }
        });
    }

    this.showCancelDebit = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Billing/CancelAutoDebit",
            success: function (data) {
                $("#ModalWrapper").html(data);
                $("#modalCancelAutoDebit").modal();
            }
        });
    }
}

var billing = null;
$(function () {
    billing = new Billing();
    billing.init();
})