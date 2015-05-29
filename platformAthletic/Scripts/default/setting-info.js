function SettingInfo() {
    var _this = this;

    this.init = function ()
    {

        $("#primary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#PrimaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $("#secondary_color").ImageColorPicker({
            afterColorSelected: function (event, color) {
                $("#SecondaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $(document).on("click", "#UpdatePasswordBtn", function () {
            _this.updatePassword();
            return false;
        });

        var obj = new qq.FineUploader({
            element: $("#UploadLogoBtn")[0],
            multiple: false,
            request: {
                endpoint: "/Setting/UploadFile",
            },
            text: {
                uploadButton: "UPLOAD LOGO"
            },
            callbacks: {
                onComplete: function (id, fileName, responseJSON) {
                    if (responseJSON.success) {
                        $("#LogoPath").val(responseJSON.fileUrl);
                        $("#LogoPathImage").attr("src", responseJSON.fileUrl + "?w=250&h=166&mode=crop&scale=both");
                    }
                }
            },
            validation: {
                allowedExtensions: ["jpeg", "png", "jpg"]
            }
        });
    }

    this.updatePassword = function ()
    {
        
        $.ajax({
            type: "POST",
            url: "/Setting/UpdatePassword",
            data: {
                ID : $("#PasswordID").val(),
                Password: $("#PasswordPassword").val(),
                NewPassword: $("#PasswordNewPassword").val(),
                ConfirmPassword: $("#PasswordConfirmPassword").val()
            },
            success: function (data) {
                $("#UpdatePasswordWrapper").html(data);
            }
        });
    }

    this.UpdateColors = function ()
    {
        $(".primaryColor").css("color", $("#PrimaryColor").val());
        $(".primaryColorBg").css("background-color", $("#PrimaryColor").val());
        $(".primaryColorBg").css("background-image", "none");
        $(".primaryColorBorder").css("border-color", $("#PrimaryColor").val());
        $(".secondaryColor").css("color", $("#SecondaryColor").val());
        $(".secondaryColorBg").css("background-color", $("#SecondaryColor").val());
        $(".secondaryColorBg").css("background-image", "none");
        $(".secondaryColorBorder").css("border-color", $("#SecondaryColor").val());
    }
}

var settingInfo = null;
$(function () {
    settingInfo = new SettingInfo();
    settingInfo.init();
});