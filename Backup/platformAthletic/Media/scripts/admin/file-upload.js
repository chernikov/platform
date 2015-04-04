function FileUpload() {
    var page = 0;

    var _this = this;

    this.ajaxUploadNewFile = "/admin/File/Upload";
    this.ajaxRemoveFile = "/admin/File/Remove";
    this.ajaxShowMore = "/admin/File/Index";

    this.init = function () {
        var titlePreview = $("#UploadImage").text();

        InitUpload($("#UploadImage")[0],
            true,
            _this.ajaxUploadNewFile,
            function (id, fileName, responseJSON) {
                if (responseJSON.result == "ok") {
                    _this.AddImage(responseJSON.data);
                }
            },
            null, titlePreview);

        $(".icon-remove").click(function (e)
        {
            _this.RemoveFile($(this));
            e.stopPropagation();
        });

        $("#ShowMore").live("click", function () {
            _this.ShowMore($(this));
        });

        $(".thumbnail").live("click", function () {
            _this.InsertImage($(this).data("path"));
        });
    }

    this.AddImage = function (image) {
        $(".thumbnails").prepend(
        $("<li>").addClass("span2").append(
        $("<a>").addClass("thumbnail").data("path", image.Path).append(
        $("<img>").attr("src", image.PreviewUrl)
        ).append($("<i>").addClass("icon-remove").data("id", image.ID)
        .click(function (e) {
            _this.RemoveFile($(this));
            e.stopPropagation();
        }))));

    }

    this.RemoveFile = function (item) {
        var parent = item.closest("li");
        var id = item.data("id");

        $.ajax({
            type: "GET",
            url: _this.ajaxRemoveFile,
            data: { id: id },
            success: function (data) {
                if (data.result == "ok") {
                    parent.remove();
                }
            }
        });
    }

    this.ShowMore = function (item) {
        var page = item.data("page");

        $.ajax({
            type: "GET",
            url: _this.ajaxShowMore,
            data: { page: page },
            success: function (data) {
                var obj = $("<div>").html(data);
                item.remove();
                $.each($("li", obj), function (i, item) {
                    $(".thumbnails").append($(this));

                    $(".icon-remove", $(this)).click(function (e) {
                        _this.RemoveFile($(this));
                        e.stopPropagation();
                    });

                });
                obj.remove();
            }
        });
    }

    this.InsertImage = function (path)
    {
        tinymce.activeEditor.execCommand('mceInsertContent', true, '<img src="' + path + '" alt="image">');
    }
}



var fileUpload = null;
$().ready(function () {
    fileUpload = new FileUpload();
    fileUpload.init();

});