function Login() {
    var _this = this;

    this.ajaxCheckActivation = "/is-activated";

    this.form = null;
    this.init = function ()
    {
        $(".LoginBtn").click(function () {
            _this.form = $(this).closest(".LoginForm");
            var ajaxData = _this.form.serialize();

            $.ajax({
                type: "POST",
                url: _this.ajaxCheckActivation,
                data: ajaxData,
                success: function (data) {
                    if (data.result == "error") {
                        _this.loadConfirmation();
                    } else {
                        _this.login();
                    }

                }
            });
            return false;
        });

        $(document).on('click', '.popup-blur-bg, .btn-gray', function () {
            _this.closePopup();
            return false;
        });
    }

    this.loadConfirmation = function () {
        $('#PopupWrapper').append('<div class="popup-confirm-wrp"><div class="inner-wrp"><div class="popup-title">Before entering the site please review and agree to our <a href="/term-and-condition" target="_blank">Terms and Conditions</a> and <a href="/privacy" target="_blank">Privacy Policy</a> below.</div><div class="button-wrp"><button class="btn-active" id="Confirm">Agree</button><button class="btn-gray">Cancel</button></div></div></div>');
        var winWidth = $(window).width();
        var winHeight = $(window).height();

        var leftVal = winWidth / 2 - 447 / 2;
        var topVal = winHeight / 2 - 407 / 2;

        $('.popup-confirm-wrp').css({
            top: topVal + 'px',
            left: leftVal + 'px'
        });
        $('.popup-confirm-wrp').show();
        $('.popup-blur-bg').show();

        $('#Confirm').click(function () {
            _this.login();
            _this.closePopup();
            return false;
        });
    };

    this.login = function () {
        _this.form.submit();
    }

    this.closePopup = function () {
        $("#PopupWrapper").empty();
        $('.popup-blur-bg').hide();
    }
}

var login = null;
$().ready(function () {
    login = new Login();
    login.init();
});