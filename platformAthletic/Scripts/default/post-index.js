function PostIndex()
{
    var _this = this;
    this.init = function ()
    {
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
            success: function (data) {
                $("#AddPostWrapper").html(data);
                CKEDITOR.replace('Text');
                $("#AddPostBtn").hide();
                _this.onEdit();
            }
        });
    }

    this.SubmitPost = function () {
        $("#Text").val(CKEDITOR.instances.Text.getData());
        $.ajax({
            type: "POST",
            data : $("#EditPostForm").serialize(),
            url: "/Post/Edit",
            success: function (data) {
                $("#AddPostWrapper").html(data);
                CKEDITOR.replace('Text');
                $("#AddPostBtn").hide();
                _this.onEdit();
            }
        });
    }

    this.onEdit = function () {
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
    }
}

var postIndex = null;
$(function () {
    postIndex = new PostIndex();
    postIndex.init();
});