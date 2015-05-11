﻿function User() {
    var _this = this;

    this.init = function ()
    {
        _this.drawPerformance();

        $(".edit-profile").click(function () {
            if ($(this).data("mode") == "edit") {
                _this.stopEdit();
                $(this).text("EDIT PROFILE");
                $(this).data("mode", "no-edit");
            } else {
                _this.startEdit();
                $(this).text("STOP EDIT PROFILE");
                $(this).data("mode", "edit");
            }
        });

        $("#UserInfoWrapper").on("change", "select", function () {
            _this.updateEdit();
        });

        $("#UserInfoWrapper").on("blur", ".value-data-table input", function () {
            _this.save($(this));
        });

        $("#UserInfoWrapper").on("blur", ".info-table input", function () {
            _this.updateEdit();
        });
        
        $(document).on("click", "#SbcWrapper .plus,#SbcWrapper .minus", function () {
            _this.changeSbc($(this));
        });

        $("#UserVideoWrapper").mCustomScrollbar({ theme: "minimal-dark", axis: "x", setWidth: "100%" });

        $(".videoUpload").click(function () {
            _this.showUploadVideo();
        });

        $(document).on("click", "#SubmitUploadVideo", function () {
            _this.uploadVideo();
        });
    }

    this.drawPerformance = function () {
        
        var ctx = $("#PerformanceChart").get(0).getContext("2d");
        // This will get the first returned node in the jQuery collection.
        var data = null;
        $.ajax({
            url: "/User/Last12WeekPerformance",
            data: {
                id: $("#UserID").val()
            },
            success: function (result) {
                data = result;
            },
            async: false
        });
        var myLineChart = new Chart(ctx).Line(data, {
            animation: false,
            bezierCurve: false,
        });

       
    }

    this.stopEdit = function ()
    {
        console.log("Stop edit");
        $.ajax({
            type: "GET",
            url: "/User/UserInfo",
            data: { id: $("#UserID").val() },
            success: function (data) {
                $("#UserInfoWrapper").html(data);
            }
        });
    }

    this.startEdit = function () {
        console.log("Start edit");
        $.ajax({
            type: "GET",
            url: "/User/EditUserInfo",
            data: { id: $("#UserID").val() },
            success: function (data)
            {
                $("#UserInfoWrapper").html(data);
                _this.onEdit();
            }
        });
    }

    this.updateEdit = function ()
    {
        var data = $("#EditUserInfoForm").serialize();
        $.ajax({
            type: "POST",
            url: "/User/EditUserInfo",
            data: data,
            success: function (data) {
                $("#UserInfoWrapper").html(data);
                _this.onEdit();
            }
        });
    }

    this.onEdit = function () {
        $('#Birthday').mask("00/00/0000", { placeholder: "__/__/____" });

        var obj = new qq.FineUploader({
            element: $("#UploadImage")[0],
            multiple: false,
            request: {
                endpoint: "/User/UploadFile",
            },
            text: {
                uploadButton: ""
            },
            callbacks: {
                onComplete: function (id, fileName, responseJSON)
                {
                    if (responseJSON.success)
                    {
                        $("#FullAvatarPath").val(responseJSON.fileUrl);
                        $("#AvatarPath").val(responseJSON.fileUrl);
                        $("#ImagePreview").attr("src", responseJSON.fileUrl + "?w=200&h=200&mode=crop&scale=both");
                        _this.updateEdit();
                    }
                }
            },
            validation: {
                allowedExtensions: ["jpeg", "png", "jpg"]
            }
        });
    }

    this.save = function (item)
    {
        console.log("Save " + item.attr("name") + " " + item.val());
        $.ajax({
            type: "POST",
            url: "/User/SaveUserField",
            data: {
                id: $("#UserID").val(),
                prop: item.attr("name"),
                value: item.val()
            },
            success: function (data) {
                console.log(data);
            }
        });
    }

    this.changeSbc = function (item)
    {
        var id = $("#UserID").val();
        var type = item.data("type");
        var value = item.data("value");
        $.ajax({
            type: "GET",
            url: "/User/ChangeSbc",
            data: {
                id: id,
                type: type,
                value: value
            },
            success: function () {
                _this.updateSbc();
                _this.updateSchoolRank();
                _this.updateRank();
            }
        });
    }

    this.updateSbc = function () {
        $.ajax({
            type: "GET",
            url: "/User/SbcData",
            data: {
                id: $("#UserID").val()
            },
            success: function (data) {
                $("#SbcWrapper").html(data);
            }
        });
    }

    this.updateRank = function () {
        $.ajax({
            type: "GET",
            url: "/User/Rank",
            data: {
                id: $("#UserID").val()
            },
            success: function (data) {
                $("#RankWrapper").html(data);
            }
        });
    }

    this.updateSchoolRank = function () {
        $.ajax({
            type: "GET",
            url: "/User/SchoolRank",
            data: {
                id: $("#UserID").val()
            },
            success: function (data) {
                $("#SchoolRankWrapper").html(data);
            }
        });
    }

    this.showUploadVideo = function ()
    {
        $.ajax({
            type: "GET",
            url: "/user/UploadVideo",
            data : {id : $("#UserID").val()},
            success: function (data)
            {
                $("#ModalWrapper").html(data);
                $("#uploadVideoModal").modal();
            }
        })
    }

    this.uploadVideo = function () {

        $.ajax({
            type: "POST",
            url: "/user/UploadVideo",
            data: $("#UploadVideoForm").serialize(),
            success: function (data)
            {
                $("#UploadVideoBodyWrapper").html(data);
            }
        })
    }
}

var user = null;
$(function ()
{
    var user = new User();
    user.init();
});