function Register() {
    var _this = this;

    this.init = function ()
    {
        $("#SubmitFormButton").click(function () {
            /*$("#RegisterForm").submit();*/
        });

        $(document).on("click", "#ApplyReferralCode", function () {
            _this.ApplyReferrerCode();
            return false;
        });
    };


    this.ApplyReferrerCode = function ()
    {
        $.ajax({
            type: "GET",
            url:  "/Account/ApplyReferrerCode",
            data : {
                target : $("#Target").val(), 
                code : $("#ReferralCode").val()
            },
            beforeSend : function() 
            {
            },
            success: function (data)
            {
                $("#TotalSum").val(data.sum);
                if (data.result == "error") {
                    $("#PromoCodeID").val("");
                    
                } else {
                    $("#PromoCodeID").val(data.promoCode);
                }
            }
        });
    }
}

var register;
$(function () {
    register = new Register();
    register.init();
});