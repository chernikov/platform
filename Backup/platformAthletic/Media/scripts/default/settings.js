function Settings() {
    var _this = this;
    
    var ajaxSettings = "/ajax-settings";

    this.init = function () {
        $(".radio, .label").click(function () {
            var radio = $(this).closest(".item").find(".radio");
            $.ajax({
                type: "POST",
                url: ajaxSettings,
                data: { control: radio.data("type") },
                success: function ()
                {
                    $(".radio").removeClass("selected");
                    radio.addClass("selected");
                }
            });
        });
    };
}

var settings = null;
$().ready(function () {
    settings = new Settings();
    settings.init();
});