function Colors() {
    var _this = this;

    this.primaryColor = "";
    this.secondaryColor = "";

    this.init = function (primaryColor, secondaryColor) {
        _this.primaryColor = primaryColor;
        _this.secondaryColor = secondaryColor;
        if (_this.primaryColor == "") {
            _this.primaryColor = "#2b333c";
        }
        if (_this.secondaryColor == "") {
            _this.secondaryColor = "#000000";
        }
        _this.update();
    };

    this.update = function () {
        $(".primaryColor").css("color", _this.primaryColor);
        $(".primaryColorBg").css("background-color", _this.primaryColor);
        $(".primaryColorBg").css("background-image", "none");
        $(".primaryColorBorder").css("border-color", _this.primaryColor);
        $(".secondaryColor").css("color", _this.secondaryColor);
        $(".secondaryColorBg").css("background-color", _this.secondaryColor);
        $(".secondaryColorBg").css("background-image", "none");
        $(".secondaryColorBorder").css("border-color", _this.secondaryColor);
    }

    this.setCurrentDate = function (date) {
        $.ajax({
            type: "POST",
            url: _this.ajaxSetDate,
            data: { dateTime: date },
            success: function (data) {
                if (data.result == "ok") {
                    window.location.reload();
                }
            }
        });
    }
}