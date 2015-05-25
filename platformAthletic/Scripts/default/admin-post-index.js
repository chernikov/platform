function AdminPostIndex()
{
    var _this = this;
    this.init = function ()
    {
        CKEDITOR.replace('Text');
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
    /*
   

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
    */
}

var adminPostIndex = null;
$(function () {
    adminPostIndex = new AdminPostIndex();
    adminPostIndex.init();
});