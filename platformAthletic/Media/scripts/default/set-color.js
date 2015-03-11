function SetColors() {
    var _this = this;

    this.init = function () {
        
        $("#primary_color").ImageColorPicker({
             afterColorSelected: function (event, color)
            {
                $("#primaryColor").val(color);
                _this.UpdateColors();
            }
        });

        $("#secondary_color").ImageColorPicker({
            afterColorSelected: function (event, color)
            {
                $("#secondaryColor").val(color);
                _this.UpdateColors();
            }
        });
    };

    this.UpdateColors = function ()
    {
        var ajaxData = {
            primaryColor: $("#primaryColor").val(),
            secondaryColor: $("#secondaryColor").val(),
        }

        $.ajax({
            type: "POST",
            url: "/Account/SetColors",
            data: ajaxData,
            success: function (data) {
                colors.init($("#primaryColor").val(), $("#secondaryColor").val());
            }
        });
    }
}

var setColors = null;
$(function () {
    setColors = new SetColors();
    setColors.init();
});