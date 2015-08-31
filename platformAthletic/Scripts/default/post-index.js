function PostIndex()
{
    var _this = this;
    this.init = function ()
    {
        if (common.isMobile()) {
            $(".video-wrapper").each(function () {
                $("iframe").attr("width", "100%");
                $("iframe").attr("height", 320);
            });
        }
        $(document).on("click", "#ListPostPagination a", function () {
            var href = $(this).attr("href");
            _this.ChangeListPost(href);
            return false;
        });

        $("#AddPostBtn").click(function () {
            _this.ShowAddPost();
        });

        $(document).on("click", "#CancelEditPostBtn", function () {
            $("#AddPostWrapper").html("");
            $("#AddPostBtn").show();
            return false;
        });

       

        $(document).on("click", "#SubmitEditPostBtn", function () {
            _this.SubmitPost();
            return false;
        });

        $(document).on("click", "#AddVideo", function () {
            $("#VideoCodeWrapper").show();
        });

        $(document).on("click", "#RemoveTitleImage", function () {
            $("#RemoveTitleImage").hide();
            $("#TitleImagePath").val("");
            $("#ImagePreview").attr("src", "");
            $("#ImagePreview").hide();
        });

        $(document).on("blur", "#VideoUrl", function () {
            _this.ToggleUrlVideoError()
        });

        $(document).on("input", "#Header", function () {
            _this.CheckHeader();
        });

        $(document).on("click", ".editPost", function () {
            var wrapper = $(this).closest(".user-post");
            $.ajax({
                type: "GET",
                url: "/Post/Edit",
                data : {id : $(this).data("id")},
                success: function (data) {
                    wrapper.html(data);
                    if (!common.isMobile()) {
                        CKEDITOR.replace('Text');
                    }
                    _this.onEdit();
                }
            });
        });

        $(document).on("click", ".removePost", function () {
            var wrapper = $(this).closest(".user-post");
            $.ajax({
                type: "GET",
                url: "/Post/Remove",
                data: { id: $(this).data("id") },
                success: function (data) {
                    $("#ModalWrapper").html(data);
                    $("#modalRemove").modal();
                }
            });
        });

        $(document).on("click", ".removePostBtn", function () {
            $.ajax({
                type: "POST",
                url: "/Post/Remove",
                data: { ID: $(this).data("id") },
                success: function (data) {
                    window.location.reload();
                }
            });
        });


        $(document).on("click", "#BackEditPostBtn", function () {
            var wrapper = $(this).closest(".user-post");
            $.ajax({
                type: "GET",
                url: "/Post/Get",
                data: { id: $(this).data("id") },
                success: function (data) {
                    wrapper.replaceWith(data);
                }
            });
            return false;
        });
    }

    this.ChangeListPost = function (href) {

        $.ajax({
            type: "GET",
            url: href,
            success: function (data) {
                $("#PostListWrapper").html(data);
            }
        });
    }

    this.ShowAddPost = function ()
    {
        $.ajax({
            type: "GET",
            url: "/Post/Create",
            success: function (data)
            {
                $("#AddPostWrapper").html(data);
                if (!common.isMobile()) {
                    CKEDITOR.replace('Text');
                }
                $("#AddPostBtn").hide();
                _this.onEdit();
            }
        });
    }

    this.CheckHeader = function () {
        var value = $("#Header").val();
        var error = "";
        $("#header-error-message").remove();
        if (value.length == 0) {
            error = "The Title is required.";
        }
        else if (value.length > 50) {
            error = "The length of the Title should not exceed 50 characters";
        }
        if (error.length > 0) {
            $("#Header").parent().addClass("has-error");
            $("#Header").after('<div id="header-error-message" class="error">' + error + '</div>');
            return false;
        }
        else {
            $("#Header").parent().removeClass("has-error");
            return true;
        }
    }

    this.CheckVideoURL = function (url) {
        var parser = document.createElement("a");
        parser.href = url;
        if (parser.hostname === "youtu.be" || parser.hostname === "www.youtube.com" || parser.hostname === "youtube.com" ||
            parser.hostname === "vimeo.com" || parser.hostname === "www.vimeo.com" || parser.hostname === "www.hudl.com")
            return true;
        else
            return false;

    }

    this.ToggleUrlVideoError = function () {
        var videoURL = $("#VideoUrl").val().trim();
        var error_msg = ""
        $("#VideoUrl").parent().removeClass('has-error');
        $("#url-error-message").remove();
        //if (videoURL.length > 0) {
        //    if (_this.CheckVideoURL(videoURL)) {
        //        $("#url-error-message").remove();
        //    }
        //    else {
        //        $("#url-error-message").remove();
        //        $("#VideoUrl").after('<div id="url-error-message" class="error">This source is not supported</div>');
        //        $("#VideoUrl").parent().addClass('has-error');
        //        return false;
        //    }
        //}
        if (videoURL.length > 0) {
            if (!_this.CheckVideoURL(videoURL)) {
                error_msg += "This source is not supported. ";
            }
            if (videoURL.length > 500) {
                error_msg += "Video link should not exceed 500 characters.";
            }
            if (error_msg.length > 0) {
                $("#VideoUrl").after('<div id="url-error-message" class="error">' + error_msg + '</div>');
                $("#VideoUrl").parent().addClass('has-error');
                return false;
            }
        }
        return true;
    }

    this.SubmitPost = function () {
        if (!common.isMobile()) {
            $("#Text").val(CKEDITOR.instances.Text.getData());
        }
        if (_this.ToggleUrlVideoError() === false || _this.CheckHeader() === false)
            return false

        $.ajax({
            type: "POST",
            data : $("#EditPostForm").serialize(),
            url: "/Post/EditPost",
            success: function (data) {
                $("#AddPostWrapper").html(data);
                if (!common.isMobile()) {
                    CKEDITOR.replace('Text');
                }
                $("#AddPostBtn").hide();
                _this.onEdit();
            }
        });
    }

    this.onEdit = function ()
    {
        //if (!common.isMobile()) {
            var obj = new qq.FineUploader({
                element: $("#AddImage")[0],
                multiple: false,
                request: {
                    endpoint: "/Post/UploadFile",
                },
                text: {
                    uploadButton: "ADD TITLE IMAGE"
                },
                callbacks: {
                    onComplete: function (id, fileName, responseJSON) {
                        if (responseJSON.success) {
                            $("#TitleImagePath").val(responseJSON.fileUrl);
                            $("#ImagePreview").attr("src", responseJSON.fileUrl + "?w=400&h=300&mode=crop&scale=both");
                            $("#ImagePreview").show();
                            $("#RemoveTitleImage").show();
                        }
                    }
                },
                validation: {
                    allowedExtensions: ["jpeg", "png", "jpg"]
                }
            });
        //} else {
        //    $("#AddImage").hide();
        //}
    }
}

var postIndex = null;
$(function () {
    postIndex = new PostIndex();
    postIndex.init();
});