function ManagePlayer()
{
    var _this = this;

    this.ajaxEditPlayer = "/manage-editplayer";
    this.ajaxSendActivation = "/resend-activation";
    
    this.init = function ()
    {
     
        $('.btn-edit').click(function () {
            var id = $(this).data("id");
            _this.showEditPopup(id);
        });

        $(".btn-send").click(function () {
            var id = $(this).data("id");
            _this.sendActivation(id);
        });
    }


    this.showEditPopup = function (id)
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxEditPlayer,
            data : {id : id},
            success: function (data)
            {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.sendActivation = function (id) {
        $.ajax({
            type: "GET",
            url: _this.ajaxSendActivation,
            data: { id: id },
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.initPopup = function () {
            var winWidth = $(window).width();
            var winHeight = $(window).height();

            var leftVal = winWidth / 2 - 447 / 2;
            var topVal = winHeight / 2 - 407 / 2;

            $('.popup-roster-wrp').css({
                top: topVal + 'px',
                left: leftVal + 'px'
            });
            $('.popup-roster-wrp').show();
            $('.popup-blur-bg').show();
            common.StyleDropDown($('#PopupWrapper .dropdown-styled'));

            $("#EditPlayerBtn").click(function () {
                _this.submitEditPlayerForm();
                return false;
            });
    }

    this.closePopup = function () {
        $("#PopupWrapper").empty();
        $('.popup-blur-bg').hide();
    }

    this.submitEditPlayerForm = function () {
        var ajaxData = $("#EditPlayerForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxEditPlayer,
            data : ajaxData,
            success: function (data)
            {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }
}

var managerPlayer = null;
$().ready(function () {
    managerPlayer = new ManagePlayer();
    managerPlayer.init();
});