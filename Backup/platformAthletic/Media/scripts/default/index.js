function Index() {
    var _this = this;
    
    this.ajaxCheckActivation = "/is-activated";

    this.init = function ()
    {
        var array = [];
        $('.query-gallery').queryGallery();

        $('.query-gallery .item').each(function () {
            var elemHeight = $(this).height();

            array.push(elemHeight);
        });
        var biggestItem = Math.max.apply(null, array);
        $('.query-gallery .content').css('height', biggestItem);

        $("#LoginBtn").click(function () {
            var ajaxData = $("#LoginForm").serialize();

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
    }

    this.loadConfirmation = function () {
        $('#PopupWrapper').append('<div class="popup-confirm-wrp"><div class="inner-wrp"><div class="popup-title">Before entering the site please review and agree to our <a href="/term-and-condition" target="_blank">Terms and Conditions</a> and <a href="/privacy">Privacy Policy</a> below.</div><div class="button-wrp"><button class="btn-active" id="Confirm">Agree</button><button class="btn-gray">Cancel</button></div></div></div>');
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
        $("#LoginForm").submit();
    }
}

var index = null;
$().ready(function () {
    index = new Index();
    index.init();
});