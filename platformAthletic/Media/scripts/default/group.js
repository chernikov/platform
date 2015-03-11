function Group()
{
    var _this = this;

    this.ajaxCreateGroup = "/create-group";
    this.ajaxEditGroup = "/edit-group";
    this.ajaxRemoveGroup = "/remove-group";
    this.ajaxAssignPlayers = "/assign-players";

    this.init = function ()
    {


        $(".scheduling .header .drop-down").click(function (e)
        {
            var header = $(this).closest(".header");
            var group = $(this).closest(".group");
            $(".drop-down", header).toggleClass("selected");
            $(".body", group).toggle();
            e.stopPropagation();
        });

        $(".scheduling .header").click(function () {
            if ($(this).hasClass("header-group"))
            {
                scheduling.loadCalendar($("#Month").val(), $("#TeamID").val(), $(this).data("id"));
            } else {
                scheduling.loadCalendar($("#Month").val(), $(this).data("id"), null);
            }
        });

        $('.btn-green').click(function () {
            _this.showCreateGroup();
        });

        $('.edit-group').click(function () {
            _this.showEditGroup($(this).data("id"));
        });

        $('.remove-group').click(function () {
            _this.showRemoveGroup($(this).data("id"));
        });

        $('.checkbox-wrp').click(function () {
            var exist = $('.checkbox-wrp input:checked').length > 0;
            var existGroup = $(".edit-group").length > 0;

            if (exist && existGroup) {
                $(".btn-drop-down").removeAttr("disabled");
            } else {
                $(".btn-drop-down").attr("disabled", "disabled");
            }
        });

        $("#AssignBtn").click(function () {
            $("#AssignList").toggle();
        });

        $("#AssignList .item").click(function () {
            _this.assignPlayers($(this).data("id"));
        });

        $('.scroll-pane').jScrollPane({ showArrows: true, autoReinitialise: true });
    }

    this.showCreateGroup = function () {
        $.ajax({
            type: "GET",
            url: _this.ajaxCreateGroup,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
                $("#EditGroupBtn").click(function () {
                    _this.submitEditGroupForm();
                    return false;
                });
            }
        });
    }

    this.showEditGroup = function (id) {
        $.ajax({
            type: "GET",
            data : {id : id},
            url: _this.ajaxEditGroup,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
                $("#EditGroupBtn").click(function () {
                    _this.submitEditGroupForm();
                    return false;
                });
            }
        });
    }

    this.showRemoveGroup = function (id) {
        $.ajax({
            type: "GET",
            data: { id: id },
            url: _this.ajaxRemoveGroup,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
                $("#RemoveGroupBtn").click(function () {
                    _this.submitRemoveGroupForm();
                    return false;
                });
            }
        });
    }

    this.initPopup = function ()
    {
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
    }

    this.submitEditGroupForm = function () {
        var ajaxData = $("#EditGroupForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxEditGroup,
            data: ajaxData,
            success: function (data) {
                $("#PopupWrapper").html(data);
                _this.initPopup();
                $("#EditGroupBtn").click(function () {
                    _this.submitEditGroupForm();
                    return false;
                });
            }
        });
    }

    this.submitRemoveGroupForm = function () {
        var ajaxData = $("#RemoveGroupForm").serialize();

        $.ajax({
            type: "POST",
            url: _this.ajaxRemoveGroup,
            data: ajaxData,
            success: function (data) {
                $("#PopupWrapper").html(data);
            }
        });
    }

    this.assignPlayers = function (groupID)
    {
        var data = "?groupId=" + groupID +"&";
        $(".selectCheckBox:checked").each(function () {
            var id = $(this).closest(".item").data("id");
            data += "idUsers=" + id + "&";
        });
        data = data.substring(0, data.length - 1);
        if (data.length > 0) {
            $.ajax({
                type: "GET",
                url: _this.ajaxAssignPlayers + data,
                success: function () {
                    window.location.reload();
                }
            });
        }
    }
}


var group = null;
$().ready(function ()
{
    group = new Group();
    group.init();
});