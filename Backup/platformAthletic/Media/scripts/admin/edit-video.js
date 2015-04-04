function EditVideo() {

    var _this = this;

    this.ajaxProcessUrl = "/admin/Video/ProcessUrl";


    this.init = function () {
        $("#processVideoUrl").click(function () {
            _this.ProcessVideoUrl();
        });
    };

    this.ProcessVideoUrl = function () {
        var ajaxData = {
            url: $("#VideoUrl").val()
        };

        $.ajax({
            type: "POST",
            url: _this.ajaxProcessUrl,
            data: ajaxData,
            success: function (data) {
                if (data.result == "ok") {
                    $("#VideoCode").val(data.VideoCode);
                    $("#VideoCodeWrapper").html(data.VideoCode);
                } else {
                    $("#VideoUrl").val("");
                    $("#VideoCode").val("");
                    $("#VideoCodeWrapper").empty();
                }
            },
            error: function () {
                alert("Error");
            }
        });
    }
}

var editVideo = null;
$().ready(function () {
    editVideo = new EditVideo();
    editVideo.init();
});