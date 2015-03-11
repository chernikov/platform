function Video() {
    var _this = this;

    this.ajaxVideoMenu = "/video-menu";
    this.ajaxVideoCode = "/video-code";
    this.ajaxVideoPillarCode = "/video-pillar-code";
    this.init = function ()
    {
        $('.scroll-pane').jScrollPane({ showArrows: true });
        _this.initMenu();
       
    };

    this.initMenu = function ()
    {
        $("#SortByExerciseBtn").click(function () {
            $("#SortType").val(1);
            $("#SearchString").val("");
            _this.UpdateMenu();
            return false;
        });

        $("#SortByPillarBtn").click(function () {
            $("#SortType").val(2);
            $("#SearchString").val("");
            _this.UpdateMenu();
            return false;
        });

        $("#SearchStringBtn").click(function () {
            _this.UpdateMenu();
            return false;
        });
        $("#SearchStringForm").submit(function () {
            _this.UpdateMenu();
            return false;
        });

        $(".video-link").live("click", function () {
            var id = $(this).data("id");
            var type = $(this).data("type");
            _this.ShowVideo(id, type);
            return false;
        });
    }

    this.UpdateMenu = function()
    {
        var ajaxData = {
            SearchString: $("#SearchString").val(),
            SortType: $("#SortType").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxVideoMenu,
            data: ajaxData,
            success: function (data)
            {
                $("#VideoMenuWrapper").html(data);
                $('.scroll-pane').jScrollPane({ showArrows: true });
                _this.initMenu();
                colors.update();
            }
        });
    }

    this.ShowVideo = function (id, type)
    {
        if (type == "Training") {
            $.ajax({
                type: "POST",
                url: _this.ajaxVideoCode,
                data: { id: id },
                success: function (data) {
                    $("#VideoWrapper").html(data);
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: _this.ajaxVideoPillarCode,
                data: { id: id },
                success: function (data) {
                    $("#VideoWrapper").html(data);
                }
            });
        }
    }
}

var video;
$().ready(function () {
    video = new Video();
    video.init();
});