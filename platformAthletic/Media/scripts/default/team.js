function Team()
{
    var _this = this;

    this.ajaxAddPlayer = "/team-addplayer";
    this.ajaxMaxPlayer = "/team-maxplayers";
    this.ajaxRemovePlayer = "/team-removeplayer";
    this.ajaxSetSbc = "/set-sbc";
    this.ajaxSetAttendance = "/set-attendance";

    this.init = function ()
    {
        $('.my-dropdown').querySelect({
            padding: '0 30px 0 4px'
        });

        $('.add-button').click(function () {
            _this.showAddPopup();
        });


        $('.max-player-button').click(function () {
            _this.showMaxPlayersPopup();
        });

        $('.delete-button').click(function () {
            _this.loadConfirmation();
        });

        $('.popup-blur-bg, .btn-gray').live('click', function () {
            _this.closePopup();
            return false;
        });

        $('.SbcTextBox').blur(function () {
            _this.setValue($(this));
        });

        $(".attendanceSet").click(function () {
            var id = $(this).closest(".playerItem").data("id");
            $(this).toggleClass('checked');
            _this.setAttendance(id, $(this).hasClass('checked'));
        });

        $("#printAll").click(function () {
            var data = "?";
            $(".playerItem").each(function () {
                var id = $(this).data("id");
                data += "idUsers=" + id + "&";
            });
            data = data.substring(0, data.length - 1);
            window.open("/team-table" + data, "_blank");

            return false;
        });

        $("#printSelected").click(function () {
            var data = "?";
            $(".selectCheckBox:checked").each(function () {
                var id = $(this).closest(".playerItem").data("id");
                data += "idUsers=" + id + "&";
            });
            data = data.substring(0, data.length - 1);
            if (data.length > 0) {
                window.open("/team-table" + data, "_blank");
            }
            $('.checkbox-wrp').each(function () {
                var $this = $(this);
                var checkbox = $this.find('input');
                var checkboxImage = $this.find('.checkbox-image');
                checkbox.attr('checked', false);
                checkboxImage.css('background-position', '0 0');
            });
            return false;
        });

        $("#SubmitSearchBtn").click(function () {
            $("#SearchForm").submit();
        });

        $("#groupId").change(function () {
            $("#SearchForm").submit();
        });
    }

    this.loadConfirmation = function() {
        $('#PopupWrapper').append('<div class="popup-confirm-wrp"><div class="inner-wrp"><div class="popup-title">Confirm delete</div><div class="button-wrp"><button class="btn-active" id="ConfirmDelete">Confirm</button><button class="btn-gray">Cancel</button></div></div></div>');
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

        $('#ConfirmDelete').click(function() {
            _this.deleteSelectedPlayers();
            _this.closePopup();
            return false;
        });
    };

    this.showAddPopup = function ()
    {
        $.ajax({
            type: "GET",
            url: _this.ajaxAddPlayer,
            success: function (data)
            {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.showMaxPlayersPopup = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxMaxPlayer,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initMaxPlayersPopup();
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

            $("#AddPlayerBtn").click(function () {
                _this.submitAddPlayerForm();
                return false;
            });
    }

    this.initMaxPlayersPopup = function () {
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
    }

    this.closePopup = function () {
        $("#PopupWrapper").empty();
        $('.popup-blur-bg').hide();
    }

    this.submitAddPlayerForm = function () {
        var ajaxData = $("#AddPlayerForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxAddPlayer,
            data : ajaxData,
            success: function (data)
            {
                $("#PopupWrapper").html(data);
                _this.initPopup();
            }
        });
    }

    this.deleteSelectedPlayers = function () {
        $(".selectCheckBox:checked").each(function () {
            var id = $(this).closest(".playerItem").data("id");
            $.ajax({
                type: "POST",
                url: _this.ajaxRemovePlayer,
                data: { id: id },
                success: function (data) {
                    if (data.result == "ok")
                    {
                        $(".playerItem[data-id='" + data.data + "']").remove();
                    }
                }
            });
        });
    }

    this.setSbc = function (id, value, name)
    {
        var ajaxData = {
            id: id,
            value: value,
            type : name
        };
        $.ajax({
            type: "POST",
            url: _this.ajaxSetSbc,
            data: ajaxData,
            success: function () {
                
            }
        });
    }

    this.setAttendance = function (id, hasAttendance)
    {
        $.ajax({
            type : "POST",
            url: _this.ajaxSetAttendance,
            data: { id: id, attendance: hasAttendance },
            success: function (data) {
            }
        });
    }

    this.setValue = function (item)
    {
        item.removeClass('input-validation-error');
        var name = item.attr("name");
        var id = item.closest(".playerItem").data("id");
        var value = item.val();
        var val = parseFloat(value);
        if (val != 0 || value == "0")
        {
            _this.setSbc(id, value, name);
        } else {
            item.addClass('input-validation-error');
        }
    }
}

var team = null;

$().ready(function () {
    team = new Team();
    team.init();
});